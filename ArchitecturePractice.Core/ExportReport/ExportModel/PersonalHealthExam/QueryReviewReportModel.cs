namespace ArchitecturePractice.Core.ExportReport.ExportModel.PersonalHealthExam
{
    /// <summary>
    /// 查詢醫師總評資料結構。
    /// </summary>
    public class QueryReviewReportModel
    {
        /// <summary>
        /// IC01。
        /// </summary>
        public int 細項類別序號 { get; set; }

        /// <summary>
        /// IO02。
        /// </summary>
        public string? 器官名稱 { get; set; }

        /// <summary>
        /// RR02。
        /// </summary>
        public string? 診斷 { get; set; }

        /// <summary>
        /// RR03。
        /// </summary>
        public string? 建議 { get; set; }
    }
}
