namespace ArchitecturePractice.Core.ExportReport.DropdownModel
{
    /// <summary>
    /// 查詢後返回的公司選單資料容器。
    /// </summary>
    public class CompanyForDropDownModel
    {
        /// <summary>
        /// CM01，公司序號。
        /// </summary>
        public required int Id { get; set; }

        /// <summary>
        /// CM02，公司名稱。
        /// </summary>
        public required string CompanyName { get; set; }
    }
}
