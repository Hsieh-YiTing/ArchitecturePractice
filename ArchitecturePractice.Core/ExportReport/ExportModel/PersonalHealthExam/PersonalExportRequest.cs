using ArchitecturePractice.Core.ExportReport.ReportSetting;

namespace ArchitecturePractice.Core.ExportReport.ExportModel.PersonalHealthExam
{
    /// <summary>
    /// View傳來的個人健檢匯出資料。
    /// </summary>
    public class PersonalExportRequest : ExportBaseModel
    {
        /// <summary>
        /// 套餐代碼。
        /// </summary>
        public string? PackageCode { get; set; }

        /// <summary>
        /// 身分證或病歷號。
        /// </summary>
        public string? IdOrChartNumber { get; set; }

        /// <summary>
        /// 個人健檢匯出選項。
        /// </summary>
        public PersonalHealthExamOption ReportOption { get; set; }
    }
}
