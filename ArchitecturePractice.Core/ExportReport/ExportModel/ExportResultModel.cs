namespace ArchitecturePractice.Core.ExportReport.ExportModel
{
    /// <summary>
    /// 回傳的檔案結果類別。
    /// </summary>
    public class ExportResultModel
    {
        /// <summary>
        /// 檔案內容。
        /// </summary>
        public byte[]? FileContent { get; set; }

        /// <summary>
        /// 檔案名稱。
        /// </summary>
        public string? FileName { get; set; }

        /// <summary>
        /// 檔案類型。
        /// </summary>
        public string? ContentType { get; set; }
    }
}
