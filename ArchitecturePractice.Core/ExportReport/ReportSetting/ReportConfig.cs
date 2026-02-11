namespace ArchitecturePractice.Core.ExportReport.ReportSetting
{
    /// <summary>
    /// PDF報告匯出的相關資訊，對應appsettings.json。
    /// </summary>
    public class ReportConfig
    {
        /// <summary>
        /// 醫院資訊區域顯示設定。
        /// </summary>
        public Organization? Organization { get; set; }

        /// <summary>
        /// 醫師區域顯示設定。
        /// </summary>
        public DoctorDisplay? DoctorDisplay { get; set; }

        /// <summary>
        /// 註記區域顯示設定。
        /// </summary>
        public RiskNote? RiskNote { get; set; }

        /// <summary>
        /// 異常評值判斷規則。
        /// </summary>
        public Rules? Rules { get; set; }
    }

    public class Organization
    {
        /// <summary>
        /// 醫院名稱。
        /// </summary>
        public string? HospitalName { get; set; }

        /// <summary>
        /// 醫院聯絡資訊。
        /// </summary>
        public string? ContactInfo { get; set; }
    }

    public class DoctorDisplay
    {
        /// <summary>
        /// 負責醫師，格式為"負責醫師 : {0}"。
        /// </summary>
        public string? NameFormat { get; set; }

        /// <summary>
        /// 醫師證號，格式為"醫字 {0} 號"。
        /// </summary>
        public string? LicenseFormat { get; set; }
    }

    public class RiskNote
    {
        /// <summary>
        ///【註】
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// 心血管疾病風險計算參考網址。
        /// </summary>
        public string? Content { get; set; }
    }

    public class Rules
    {
        /// <summary>
        /// 判斷評值是否異常的集合。
        /// </summary>
        public string[]? AbnormalCodes { get; set; }
    }
}
