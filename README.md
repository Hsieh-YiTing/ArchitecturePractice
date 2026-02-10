
# ArchitecturePractice

使用 **ASP.NET Core 10** 的 Web 應用程式。
主要目的為驗證 **QuestPDF** 套件的可行性，並實作 **分層架構**、**Log紀錄**、**模型驗證** 。

## 系統架構

本解決方案由多個專案組成，以下為各自職責說明

*   **Web (Main Project)**
    *   採用 MVC 模式，內部區分為 MVC Controller 與 API Controller。
    *   Controller 僅負責接收與回應 HTTP 請求，畫面呈現交由 View 處理。

*   **Core**
    *   定義系統核心介面 (Interfaces)、資料模型 (Models)、通用回傳格式及報表資訊。
    *   **特點**：不依賴任何第三方套件，保持架構核心的純淨性。

*   **Services**
    *   封裝業務邏輯。
    *   負責協調 Core、Repository 與 Common 層的運作，並將最終結果返回給 Controller。
    
*   **Repositories**
    *   負責資料庫存取操作。
    *   使用 **Dapper** 與 SQL Server 互動。
    *   複雜的 SQL 查詢語句以「嵌入式資源」方式管理，存放在 `SQL Queries` 資料夾中。

*   **Common**
    *   提供各層級皆可使用的通用工具與輔助類別。

## 專案依賴關係

遵循**單向依賴原則**，確保架構清晰且避免循環引用。

### 引用階層
*   **Web**
    *   依賴：`Services`, `Repositories`, `Core`, `Common`
    *   *備註：Web 層引用所有專案主要是為了在 `Program.cs` 進行 DI 的集中註冊。Controller 原則上僅注入 Service 層介面。*
*   **Services**
    *   依賴：`Repositories`, `Core`, `Common`
    *   *職責：調用 Repository 獲取資料，並使用 Core 定義的介面進行處理。*
*   **Repositories**
    *   依賴：`Core`, `Common`
    *   *職責：實作 Core 定義的介面，並使用 Common 的 SQL 讀取工具。*
*   **Core**
    *   依賴：無
    *   *職責：最底層的合約定義，不依賴任何專案。*
*   **Common**
    *   依賴：無
    *   *職責：獨立的工具庫。*

---

## 技術堆疊與套件
### 全域依賴
*   **Microsoft.Extensions.DependencyInjection.Abstractions**：提供介面定義，允許各層設計獨立的依賴注入註冊方法，最終於 `Program.cs` 進行集中管理。

---

## 各層級實作細節
### 1. Core 層
核心層專注於定義合約與資料結構，維持無依賴的乾淨架構。

*   **CommonFormat**：
    *   設計抽象基底類別 `BaseResult<T>`，統一全系統的資料回傳格式。
    *   繼承給 `ServiceResult<T>` (Service 層使用) 與 `ApiResult<T>` (Controller 層使用)。

### 2. Web 層
負責處理 HTTP 請求、驗證與回應。

*   **使用套件**：
    *   **FluentValidation**：針對 View 傳入的模型資料進行強型別驗證。
    *   **Serilog**：替換內建的 `ILogger`，並將日誌輸出至 **Seq** 平台進行集中管理。
*   **Controller 設計**：
    *   **職責分離**：明確區分 API Controller 與非 API (View) Controller。
    *   **基底類別 (Base Controller)**：負責注入 `ILogger` 及取得當前請求的 `Trace Id` (用於追蹤)。
    *   **錯誤處理**：設計專屬的錯誤處理 Controller，並透過 Middleware 進行全域錯誤分流。
*   **Extensions**：
    *   擴充功能，將 `ServiceResult` 轉換為 `ApiResult`，確保回傳給 View 的資料格式一致。

### 3. Service 層
負責業務邏輯與報表產出。

*   **PDF 報表引擎 (QuestPDF)**：
    *   使用 **Factory Pattern (工廠模式)** 進行報表匯出：
        1.  Service 傳入 Model 給 Factory。
        2.  Factory 根據 Model 資訊決定並回傳對應的 Generator。
        3.  Service 調用 Generator 方法產出 PDF。
    *   **PDF Documents**：實作 QuestPDF 的 `IDocument` 介面，以物件導向方式建構報表。
    *   **PDF Tools**：集中管理 PDF 頁面設定與共用樣式。

### 4. Repository 層
負責資料存取。

*   **使用套件**：
    *   **Dapper**：用於執行 SQL 並將結果 Mapping 為 C# 物件。
    *   **Microsoft.Data.SqlClient**：SQL Server 驅動程式。
*   **SQL 管理 (SQL Queries)**：
    *   將 SQL 語句存為嵌入式資源檔案。
    *   檔案編碼強制設定為 **UTF-8 (Codepage 65001)**，解決中文亂碼問題。
*   **Extensions**：
    *   擴充匯出欄位的基底類別，生成 View 共用欄位的 SQL 條件語法。

### 5. Common 層
提供跨層級的通用功能。

*   **SQL Resource Reader**：
    *   設計讀取嵌入式 SQL 資源的方法，並透過 **快取機制** 以提升效能。
*   **Debug Tools**：
    *   設計物件具現化方法，方便 Debug 查看資料結構。
    *   機制：僅在 Debug 模式下輸出，正式環境自動略過。
*   **高效能Logger**：
    *   擴充 `ILogger` 方法，利用 `partial` 關鍵字配合 `[LoggerMessage]` 屬性，實現高效能的 Logging 。