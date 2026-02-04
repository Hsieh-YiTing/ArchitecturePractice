namespace ArchitecturePractice.Core.ExportReport.ReportSetting
{
    // 總表匯出選項
    public enum SummaryReportOption
    {
        勞檢總表 = 1,
        一般總表 = 2,
        一般總表含加做項 = 3,
        一高統計表 = 4,
        三高統計表 = 5,
        代謝症候群統計表 = 6,
        肌肉骨骼症狀調查統計表 = 7,
        高血壓統計表 = 8
    }

    // 異常分析表匯出選項
    public enum ExceptionReportOption
    {
        異常總表 = 1,
        異常明細表 = 2
    }

    // 個人健檢報告匯出選項
    public enum PersonalHealthExamOption
    {
        不含前次 = 1,
        包含前次 = 2
    }

    // 公司報告匯出選項
    public enum CompanyReportOption
    {
        員工資料表 = 1,
        檢查項目及代號對照表 = 2,
        健檢項目參考值院內檢核表 = 3,
        健檢資料表 = 4
    }

    // 問卷報告匯出選項，實際沒有這個，為了符合泛型條件設置
    public enum QuestionnaireReportOption
    {
        None = 0
    }

    // 報告匯出格式
    public enum ReportFormat
    {
        Excel = 1,
        PDF = 2
    }
}
