namespace ArchitecturePractice.Core.ExportReport.ExportModel.PersonalHealthExam
{
    /// <summary>
    /// 查詢個人健檢報告資料結構。
    /// </summary>
    public class QueryHealthReportItemModel
    {
        /// <summary>
        /// RV01。
        /// </summary>
        public int 預約序號 { get; set; }

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
        public string? 病歷號碼 { get; set; }

        /// <summary>
        /// PAT05。
        /// </summary>
        public string? 電話 { get; set; }

        /// <summary>
        /// PAT06。
        /// </summary>
        public string? 地址 { get; set; }

        /// <summary>
        /// RV02。
        /// </summary>
        public DateTime? 健檢日期 { get; set; }

        /// <summary>
        /// RI01。
        /// </summary>
        public int 預約細項序號 { get; set; }

        /// <summary>
        /// RI08。
        /// </summary>
        public string? 檢查結果 { get; set; }

        /// <summary>
        /// RI09。
        /// </summary>
        public byte? 檢驗評值 { get; set; }

        /// <summary>
        /// 依據DI19判斷取DI09或RI29。
        /// </summary>
        public string? 單位 { get; set; }

        /// <summary>
        /// DI02。
        /// </summary>
        public string? 細項名稱 { get; set; }

        /// <summary>
        /// 若DI19為0，則為DI09；若DI19為1、2、3，用PAT08判斷，為RI27或RI28。
        /// </summary>
        public string? 正常值_中 { get; set; }

        /// <summary>
        /// DI05，1為數字型，2為代碼型，3為文字型，4為文件型。
        /// </summary>
        public byte 報告類型 { get; set; }

        /// <summary>
        /// IC01。
        /// </summary>
        public int 細項類別序號 { get; set; }

        /// <summary>
        /// IC02。
        /// </summary>
        public string? 細項類別名稱 { get; set; }

        /// <summary>
        /// RC05，這裡用來判斷是否讓細項類別名稱顯示正常值。
        /// </summary>
        public byte 是否列印細項參考值 { get; set; }

        /// <summary>
        /// IO02。
        /// </summary>
        public string? 器官名稱 { get; set; }

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
        public byte 醫師章授權 { get; set; }

        /// <summary>
        /// 日期排序，當日期都是同一天就是同一序號。
        /// </summary>
        public long DateRank { get; set; }
    }
}
