document.addEventListener("DOMContentLoaded", () => {
    initRoleSelect();
    initCheckboxToggle();
    bindCheckboxEvent();
    loadCompanyRoles();
    handleExportButtons();
});

/**
 * 各Tab對應的API路徑。
 */
const REPORT_ENDPOINTS = {
    "summary-report-tab": "/api/export/summary",
    "exception-report-tab": "",
    "personal-health-examination-tab": "/api/export/personal-health-exam",
    "company-report-tab": "",
    "questionnaire-report-tab": ""
};

/**
 * 初始化一級單位的顯示狀態，預設皆為隱藏。
 */
const initRoleSelect = () => {
    const roleSelectArea = document.querySelectorAll("div.role");

    roleSelectArea.forEach(area => {
        area.classList.add("d-none");
    });
};

/**
 * 初始化checkbox的顯示狀態，預設皆為未勾選。
 */
const initCheckboxToggle = () => {
    const checkboxes = document.querySelectorAll('input[type="checkbox"][data-target]');

    checkboxes.forEach(cb => {
        const target = document.querySelector(cb.dataset.target);

        if (!target) {
            return;
        }

        target.classList.toggle("d-none", !cb.checked);
    });
};

/**
 * 載入對應公司的一級單位選單。
 */
const loadCompanyRoles = () => {
    document.querySelectorAll(".company-select").forEach(companySelect => {
        companySelect.addEventListener("change", async (e) => {
            try {
                // 取得群組名稱及公司ID
                const group = e.target.dataset.group;
                const companyId = e.target.value;

                // 取得對應的一級單位區域及下拉選單
                const roleArea = document.querySelector(`div.role[data-group="${group}"]`);
                const roleSelect = document.querySelector(`select.role-select[data-group="${group}"]`);

                // 判斷是否有選擇公司，沒有就隱藏
                if (!companyId || companyId === "") {
                    roleSelect.innerHTML = "";
                    roleArea.classList.remove("d-flex");
                    roleArea.classList.add("d-none");
                    return;
                }

                // 發送API請求取得一級單位資料
                const roleData = await fetchCompanyRoles(companyId);

                if (roleData.isSuccess) {
                    // 一次累積所有項目，再一次性加入選單中
                    let roleSelectItemList = '<option value="">全部</option>';

                    if (roleData.data && Array.isArray(roleData.data)) {
                        roleData.data.forEach(role => {
                            roleSelectItemList += `<option value="${role.id}">${role.roleName}</option>`;
                        });
                    }

                    roleSelect.innerHTML = roleSelectItemList;

                    // 確認資料載入後顯示一級單位區域
                    roleArea.classList.remove("d-none");
                    roleArea.classList.add("d-flex");
                }
            } catch (error) {
                console.error('載入一級單位失敗:', error.message);

                Swal.fire({
                    icon: "error",
                    title: "發生錯誤",
                    html: formatErrorToHtml(error),
                });
            }
        });
    });
};

/**
 * 綁定checkbox的change事件。
 */
const bindCheckboxEvent = () => {
    document.addEventListener("change", (e) => {
        const checkbox = e.target.closest('input[type="checkbox"][data-target]');

        if (!checkbox) {
            return;
        }

        // 處理被勾選的checkbox
        const selfSelector = checkbox.dataset.target;
        const selfTarget = selfSelector ? document.querySelector(selfSelector) : null;

        if (selfTarget) {
            // 取消勾選就清空欄位
            if (!checkbox.checked) {
                clearFormElements(selfTarget);
            }

            selfTarget.classList.toggle("d-none", !checkbox.checked);
        }

        // 處理同群組中的其他checkbox
        const group = checkbox.dataset.group;

        if (!group) {
            return;
        }

        document.querySelectorAll(`input[type="checkbox"][data-group="${group}"]`).forEach(peer => {
            if (peer === checkbox) {
                return;
            }

            if (peer.checked) {
                peer.checked = false;

                const peerSelector = peer.dataset.target;
                const peerTarget = peerSelector ? document.querySelector(peerSelector) : null;

                if (peerTarget) {
                    // 清空欄位值並隱藏
                    clearFormElements(peerTarget);
                    peerTarget.classList.add("d-none");
                }
            }
        });
    });
};

/**
 * 清空對應target內的欄位值。
 * @param {HTMLElement} container - 要清空欄位值的容器。
 */
const clearFormElements = (container) => {
    const inputs = container.querySelectorAll('input[type="text"], input[type="date"]');
    const selects = container.querySelectorAll("select");

    inputs.forEach(input => {
        input.value = '';
    });

    selects.forEach(select => {
        select.selectedIndex = 0;
    });
};

/**
 * 取得一級單位API。
 * @param {any} companyId - 公司ID。
 * @returns {Promise<Array>} - 回傳Promise，解析後會得到一級單位陣列。
 * @throws {Error} - 若API回傳錯誤則拋出Error物件。
 */
const fetchCompanyRoles = async (companyId) => {
    const response = await fetch(`/api/export/roles?companyId=${companyId}`);
    const result = await response.json();

    if (!response.ok) {
        const customError = new Error(result.message || `Http錯誤: ${response.status}`);
        customError.data = result;
        throw customError;
    }

    return result;
};

/**
 * 處理匯出按鈕的點擊事件。
 */
const handleExportButtons = () => {
    document.addEventListener("click", async (e) => {
        const exportButton = e.target.closest("button.export-btn");

        if (!exportButton) {
            return;
        }

        try {
            const reportTab = exportButton.dataset.reportTab;

            if (reportTab) {
                const response = await fetchExportReport(reportTab);
                console.log('檔案匯出成功:', response);

                Swal.fire({
                    icon: 'success',
                    title: '下載成功',
                    timer: 2000,
                    showConfirmButton: false
                });
            }
        } catch (error) {
            console.error('檔案匯出失敗:', error.message);

            Swal.fire({
                icon: "error",
                title: "發生錯誤",
                html: formatErrorToHtml(error),
            });
        }
    });
};

/**
 * 取得欄位資料後呼叫API進行報表匯出。
 * @param {any} reportTab - Tab的ID。
 * @throws {Error} - 若API回傳錯誤則拋出Error物件。
 */
const fetchExportReport = async (reportTab) => {
    const endpoint = REPORT_ENDPOINTS[reportTab];

    if (!endpoint) {
        throw new Error("無效的API路徑");
    }

    const params = collectReportTabParameters(reportTab);

    // 顯示匯出進度的Swal
    Swal.fire({
        title: '資料匯出中',
        html: `
            <div class="mb-2">正在從伺服器下載資料...</div>
            <div class="progress" style="height: 20px;">
                <div id="swal-progress-bar" 
                     class="progress-bar progress-bar-striped progress-bar-animated" 
                     role="progressbar" 
                     style="width: 100%; background-color: #3085d6;">
                </div>
            </div>
            <div id="swal-progress-text" style="margin-top: 10px; font-weight: bold; color: #555;">
                已下載: 0 Bytes
            </div>
        `,
        allowOutsideClick: false,
        allowEscapeKey: false,
        showConfirmButton: false,
        didOpen: () => {
            Swal.showLoading(); // 顯示轉圈圈
        }
    });

    try {
        const result = await fetchWithProgress(
            endpoint, 
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(params)
            },
            (loadedBytes) => {
                const progressText = document.getElementById('swal-progress-text');

                if (progressText) {
                    // 使用工具函式進行轉換
                    progressText.textContent = `已下載: ${formatBytes(loadedBytes)}`;
                }
            }
        );

        if (!result.isSuccess) {
            const customError = new Error(result.message || `Http錯誤: ${result.status}`);
            customError.data = result;
            throw customError;
        }

        if (result.data && result.data.fileContent) {
            // 下載完成後更新Swal的文字提示
            const progressText = document.getElementById('swal-progress-text');

            if (progressText) {
                progressText.textContent = "下載完成，正在處理檔案..."
            }

            downloadFileFromBase64(result.data.fileContent, result.data.fileName, result.data.contentType);
        } else {
             throw new Error("請求成功，但伺服器未回傳檔案內容");
        }
    } catch (error) {
        console.error("未預期下載錯誤:", error);
        throw error; 
    }
};

/**
 * 依據Tab收集需要的欄位集合，並依序傳入getFiledElementValue()內取得欄位值，最後與名稱組成物件後傳回。
 * @param {any} reportTab - Tab的ID。
 * @returns - 回傳欄位名稱及值的物件。
 */
const collectReportTabParameters = (reportTab) => {
    const tabElement = document.getElementById(reportTab);

    if (!tabElement) {
        throw new Error("無效的報表Tab");
    }

    const fields = tabElement.querySelectorAll("[data-field]");
    const params = {};

    fields.forEach(field => {
        const filedName = field.dataset.field;

        if (!filedName) {
            return;
        }

        const fieldValue = getFiledElementValue(field);

        if (fieldValue !== null) {
            params[filedName] = fieldValue;
        }
    });

    return params;
};

/**
 * 取得欄位值，若Select有值則會進行類型轉換，防止Controller出現模型綁定失敗。
 * @param {any} fieldElement - 欄位元素。
 * @returns - 回傳欄位值。
 */
const getFiledElementValue = (fieldElement) => {
    if (fieldElement.type === "date" || fieldElement.type === "text") {
        return fieldElement.value || null;
    }

    if (fieldElement.tagName === "SELECT") {
        if (!fieldElement.value) {
            return null;
        }

        return Number(fieldElement.value);
    }

    if (fieldElement.type === "checkbox") {
        return fieldElement.checked;
    }

    return null;
};

/**
 * 使用Fetch API搭配ReadableStream來實現下載進度回報。
 * @param {string} url - API的URL。
 * @param {object} options - Fetch的選項，例如method、headers、body等。
 * @param {function} onProgress - 下載進度回報的callback函式，會傳入已下載的bytes數量。
 * @returns {Promise<any>} - 回傳組合解析後的JSON資料。
 */
const fetchWithProgress = async (url, options, onProgress) => {
    const response = await fetch(url, options);

    if (!response.ok) {
        throw new Error(`Http錯誤: ${response.status}`);
    }

    // 取得ReadableStream的reader
    const reader = response.body.getReader();

    // 下載過程中累積已下載的bytes數量
    let receivedLength = 0;

    // 用來暫存下載的資料塊，最後可以組合成完整檔案
    let chunks = []; 

    // 持續讀取資料直到完成
    while (true) {
        // read()回傳後物件解構
        // done: 是否傳輸結束 (true/false)，value: 這一塊資料的內容 (Uint8Array 二進位陣列)
        const { done, value } = await reader.read();

        if (done) {
            break;
        }

        // 儲存資料，並記錄目前的長度
        chunks.push(value);
        receivedLength += value.length;

        // 檢查是否有傳入方法，有就回報目前已下載的bytes數量
        if (onProgress) {
            onProgress(receivedLength);
        }
    }

    // 開始組合資料
    const chunksAll = new Uint8Array(receivedLength); 
    let position = 0;

    // 把chunks裡面的每一個碎片資料貼到chunksAll裡面，最後就會得到完整的二進位資料
    for (let chunk of chunks) {
        chunksAll.set(chunk, position); // 把碎片資料貼上
        position += chunk.length;       // 索引往後移
    }

    // 把二進位資料 (Uint8Array) 解碼成文字 (string)
    const textDecoder = new TextDecoder("utf-8");
    const jsonString = textDecoder.decode(chunksAll);

    // 最後把 JSON 字串轉成物件並回傳
    return JSON.parse(jsonString);
};

/**
 * 將bytes進行轉換，最大寫到GB，長度為小數點後2位。
 * @param {number} bytes - 要轉換的bytes數字。
 * @param {number} decimals - 小數位數，預設為2。
 */
const formatBytes = (byte,  decimals = 2) => {
    // 確認傳入的bytes數字有效
    if (byte === 0) {
        return '0 Bytes';
    }

    // 定義出常數
    const k = 1024; // 1 KB = 1024 Bytes
    const sizes = ['Bytes', 'KB', 'MB', 'GB'];

    // 使用對數方式，算出傳入byte位於哪個size範圍
    const i = Math.floor(Math.log(byte) / Math.log(k));

    // 算出分母，並與bytes相除，最後使用toFixed()保留小數位數，組合單位後回傳
    return parseFloat((byte / Math.pow(k, i)).toFixed(decimals)) + ' ' + sizes[i];
};

/**
 * 將Base64字串轉換為Blob並觸發下載。
 * @param {string} base64String - Base64編碼的字串。
 * @param {string} fileName - 下載的檔案名稱。
 * @param {string} contentType - 檔案的MIME類型。
 */
const downloadFileFromBase64 = (base64String, fileName, contentType) => {
    // 瀏覽器內建方法，用來解碼Base64
    const bytes = atob(base64String);

    // 解碼後字串轉換為Uint8Array
    const byteNumbers = new Array(bytes.length);
    for (let i = 0; i < bytes.length; i++) {
        byteNumbers[i] = bytes.charCodeAt(i);
    }

    const byteArray = new Uint8Array(byteNumbers);

    // 建立Blob物件
    const blob = new Blob([byteArray], { type: contentType });

    // 建立下載連結並觸發點擊
    const url = window.URL.createObjectURL(blob);
    const a = document.createElement('a');

    a.href = url;
    a.download = fileName;

    document.body.appendChild(a);
    a.click();

    // 設置延遲100ms後清理資源，確保下載流程完成
    setTimeout(() => {
        document.body.removeChild(a); // 將剛剛加入的a元素移除
        window.URL.revokeObjectURL(url); // 釋放Blob URL資源，否則會一直緩存在瀏覽器記憶體
    }, 100);
};

/**
 * 格式化錯誤訊息為HTML內容。
 * @param {any} error - Error物件。
 * @returns - 物件有data屬性就組合並回傳HTML字串，否則就回傳錯誤訊息。
 */
const formatErrorToHtml = (error) => {
    // 沒有data屬性，直接回傳訊息
    if (!error.data) {
        return error.message;
    }

    // 組裝html內容
    let htmlContent = `<p class="fw-bold text-danger mb-2">${error.message}</p>`;

    if (error.data.traceId) {
        htmlContent += `<p class="text-muted small mb-3">錯誤碼: ${error.data.traceId}</p>`;
    }

    if (error.data.validationErrors && error.data.validationErrors.length > 0) {
        htmlContent += `<p class="mb-1">詳細錯誤：</p>`;
        htmlContent += `<ul class="list-group list-group-flush">`;

        error.data.validationErrors.forEach(error => {
            htmlContent += `<li class="list-group-item list-group-item-danger py-1 small">${error.errorMessage}</li>`;
        });

        htmlContent += `</ul>`;
    }

    return htmlContent;
};