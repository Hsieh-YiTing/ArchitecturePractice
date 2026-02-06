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
 * @returns {Promise<byte[]>} - 回傳Promise，解析後會得到檔案的byte陣列。
 * @throws {Error} - 若API回傳錯誤則拋出Error物件。
 */
const fetchExportReport = async (reportTab) => {
    const endpoint = REPORT_ENDPOINTS[reportTab];

    if (!endpoint) {
        throw new Error("無效的API路徑");
    }

    const params = collectReportTabParameters(reportTab);

    const response = await fetch(endpoint, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(params)
    });

    //const result = await response.json();

    //if (!response.ok) {
    //    const customError = new Error(result.message || `Http錯誤: ${response.status}`);
    //    customError.data = result;
    //    throw customError;
    //}

    //return result;

    // 待回傳後下載檔案
    try {
        const response = await fetch(endpoint, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(params)
        });

        // 1. 先檢查回應是否成功
        if (!response.ok) {
            // 如果失敗 (例如 400 或 500)，後端通常回傳 JSON 錯誤訊息
            // 嘗試讀取 JSON 錯誤內容
            let errorMsg = `Http錯誤: ${response.status}`;
            try {
                const errorResult = await response.json();
                if (errorResult.message) errorMsg = errorResult.message;
            } catch (e) {
                // 如果無法解析 JSON，就維持原本的 HTTP Status 錯誤訊息
            }
            throw new Error(errorMsg);
        }

        // 2. 嘗試從 Header 取得後端設定的檔名 (Content-Disposition)
        // 後端範例: return File(bytes, "app/pdf", "HealthReport_2024.pdf");
        let filename = "download.pdf"; // 預設檔名
        const disposition = response.headers.get('Content-Disposition');
        if (disposition && disposition.indexOf('attachment') !== -1) {
            const filenameRegex = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/;
            const matches = filenameRegex.exec(disposition);
            if (matches != null && matches[1]) {
                filename = matches[1].replace(/['"]/g, '');
                // 解碼中文檔名 (如果後端有做 URL Encode)
                filename = decodeURIComponent(filename);
            }
        }

        // 3. 將回應轉為 Blob (二進位物件)
        const blob = await response.blob();

        // 4. 建立下載連結並觸發點擊
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = filename; // 設定下載的檔名
        document.body.appendChild(a); // Firefox 需要將元素加入 body 才能觸發點擊
        a.click();

        // 5. 清理資源
        a.remove();
        window.URL.revokeObjectURL(url);

        return true; // 表示下載成功

    } catch (error) {
        console.error("下載失敗:", error);
        throw error; // 往外拋出讓 UI 層處理 (例如顯示 SweetAlert)
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

    if (error.data.errors && error.data.errors.length > 0) {
        htmlContent += `<p class="mb-1">詳細錯誤：</p>`;
        htmlContent += `<ul class="list-group list-group-flush">`;

        error.data.errors.forEach(error => {
            htmlContent += `<li class="list-group-item list-group-item-danger py-1 small">${error.errorMessage}</li>`;
        });

        htmlContent += `</ul>`;
    }

    return htmlContent;
};