using ArchitecturePractice.Core.ExportReport.ReportSetting;

namespace ArchitecturePractice.Core.ExportReport.ExportModel
{
    /// <summary>
    /// 各報表匯出基本欄位資料基底類別。
    /// </summary>
    public class ExportBaseModel
    {
        /// <summary>
        /// 健檢查詢起始日期。
        /// </summary>
        public DateTime ExamineStartDate { get; set; }

        /// <summary>
        /// 健檢查詢結束日期。
        /// </summary>
        public DateTime ExamineEndDate { get; set; }

        /// <summary>
        /// 選擇的公司選單ID。
        /// </summary>
        public int? SelectedCompanyId { get; set; }

        /// <summary>
        /// 選擇的一級單位選單ID。
        /// </summary>
        public int? SelectedRoleId { get; set; }

        /// <summary>
        /// 選擇的報表匯出格式。
        /// </summary>
        public ReportFormat ReportFormat { get; set; }
    }
}
