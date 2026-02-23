namespace ArchitecturePractice.Services.ExportReport.ReportModels
{
    internal abstract class PersonalReportBaseModel<TItem>
    {
        /// <summary>
        /// 病患基本資料，包含頁首。
        /// </summary>
        internal required PatientBasicInfo PatientBasicInfo { get; set; }

        /// <summary>
        /// 使用泛型，有檢查大項，理學檢查、X光檢查...等，內部包含對應的細項。
        /// </summary>
        internal required IEnumerable<TItem> HealthReportItems { get; set; }

        /// <summary>
        /// 醫師總評，包含各器官的診斷與建議。
        /// </summary>
        internal required IEnumerable<OrganReviewReportModel> OrganReviewReports { get; set; }

        /// <summary>
        /// 醫師資訊，頁尾。
        /// </summary>
        internal required DoctorInfo DoctorInfo { get; set; }
    }

    internal class PatientBasicInfo
    {
        /// <summary>
        /// PD02。
        /// </summary>
        internal string? 套餐名稱 { get; set; }

        /// <summary>
        /// PAT02。
        /// </summary>
        internal string? 姓名 { get; set; }

        /// <summary>
        /// PAT08。
        /// </summary>
        internal string? 性別 { get; set; }

        /// <summary>
        /// 格式化的PAT04。
        /// </summary>
        internal string? 出生日期 { get; set; }

        /// <summary>
        /// PAT03。
        /// </summary>
        internal string? 身份證字號 { get; set; }

        /// <summary>
        /// PAT07。
        /// </summary>
        internal string? 病歷號 { get; set; }

        /// <summary>
        /// RV02。
        /// </summary>
        internal string? 檢查日期 { get; set; }

        /// <summary>
        /// RV02。
        /// </summary>
        internal string? 前次檢查日期 { get; set; }

        /// <summary>
        /// PAT05。
        /// </summary>
        internal string? 電話 { get; set; }

        /// <summary>
        /// PAT06。
        /// </summary>
        internal string? 住址 { get; set; }
    }

    internal class OrganReviewReportModel
    {
        /// <summary>
        /// IO02。
        /// </summary>
        internal string? 器官 { get; set; }

        /// <summary>
        /// RR02。
        /// </summary>
        internal string? 診斷 { get; set; }

        /// <summary>
        /// RR03。
        /// </summary>
        internal string? 建議 { get; set; }
    }

    internal class DoctorInfo
    {
        /// <summary>
        /// AD05。
        /// </summary>
        internal string? 負責醫師 { get; set; }

        /// <summary>
        /// AD07。
        /// </summary>
        internal string? 醫師證號 { get; set; }

        /// <summary>
        /// AD10，0:不顯示醫師章，1:顯示醫師章。
        /// </summary>
        internal byte? 醫師章授權 { get; set; }
    }
}
