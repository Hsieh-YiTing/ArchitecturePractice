namespace ArchitecturePractice.Core.ExportReport.ExportModel.PersonalHealthExam
{
    /// <summary>
    /// 個人健檢PDF結構，拆成多個部分。
    /// </summary>
    public class PersonalHealthExamModel
    {
        /// <summary>
        /// 病患基本資料，包含頁首。
        /// </summary>
        public required PatientBasicInfo PatientBasicInfo { get; set; }

        /// <summary>
        /// 檢查大項，理學檢查、X光檢查...等，內部包含對應的細項。
        /// </summary>
        public required IEnumerable<HealthReportItemModel> HealthReportItems { get; set; }

        /// <summary>
        /// 醫師總評，包含各器官的診斷與建議。
        /// </summary>
        public required IEnumerable<OrganReviewReportModel> OrganReviewReports { get; set; }

        /// <summary>
        /// 醫師資訊，頁尾。
        /// </summary>
        public required DoctorInfo DoctorInfo { get; set; }
    }

    public class PatientBasicInfo
    {
        /// <summary>
        /// PD02。
        /// </summary>
        public string? 套餐名稱 { get; set; }

        /// <summary>
        /// PAT02。
        /// </summary>
        public string? 姓名 { get; set; }

        /// <summary>
        /// PAT08。
        /// </summary>
        public string? 性別 { get; set; }

        /// <summary>
        /// 格式化的PAT04。
        /// </summary>
        public string? 出生日期 { get; set; }

        /// <summary>
        /// PAT03。
        /// </summary>
        public string? 身份證字號 { get; set; }

        /// <summary>
        /// PAT07。
        /// </summary>
        public string? 病歷號 { get; set; }

        /// <summary>
        /// RV02。
        /// </summary>
        public string? 檢查日期 { get; set; }

        /// <summary>
        /// RV02。
        /// </summary>
        public string? 前次檢查日期 { get; set; }

        /// <summary>
        /// PAT05。
        /// </summary>
        public string? 電話 { get; set; }

        /// <summary>
        /// PAT06。
        /// </summary>
        public string? 住址 { get; set; }
    }

    public class HealthReportItemModel
    {
        /// <summary>
        /// IC02。
        /// </summary>
        public string? 檢查類別 { get; set; }

        /// <summary>
        /// DI02。
        /// </summary>
        public string? 項目 { get; set; }

        /// <summary>
        /// RI08。
        /// </summary>
        public string? 結果 { get; set; }

        /// <summary>
        /// 依據DI19判斷取DI09或RI29。
        /// </summary>
        public string? 單位 { get; set; }

        /// <summary>
        /// 若DI19為0，則為DI09；若DI19為1、2、3，用PAT08判斷，為RI27或RI28。
        /// </summary>
        public string? 正常值 { get; set; }

        /// <summary>
        /// RI09。
        /// </summary>
        public string? 檢驗評值 { get; set; }

        /// <summary>
        /// DI05，1為數字型，2為代碼型，3為文字型，4為文件型。
        /// </summary>
        public byte 報告類型 { get; set; }

        /// <summary>
        /// RC05，0:不顯示正常值欄位，1:顯示正常值欄位。
        /// </summary>
        public byte IsDisplay { get; set; }

        /// <summary>
        /// 日期排序，當日期都是同一天就是同一序號。
        /// </summary>
        public long DateRank { get; set; }
    }

    public class OrganReviewReportModel
    {
        /// <summary>
        /// IO02。
        /// </summary>
        public string? 器官 { get; set; }

        /// <summary>
        /// RR02。
        /// </summary>
        public string? 診斷 { get; set; }

        /// <summary>
        /// RR03。
        /// </summary>
        public string? 建議 { get; set; }
    }

    public class DoctorInfo
    {
        /// <summary>
        /// AD05。
        /// </summary>
        public string? 負責醫師 { get; set; }

        /// <summary>
        /// AD07。
        /// </summary>
        public string? 醫師證號 { get; set; }

        /// <summary>
        /// AD10，0:不顯示醫師章，1:顯示醫師章。
        /// </summary>
        public byte? 醫師章授權 { get; set; }
    }
}
