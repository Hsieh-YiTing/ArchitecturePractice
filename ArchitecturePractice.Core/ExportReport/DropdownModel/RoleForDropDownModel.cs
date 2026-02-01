namespace ArchitecturePractice.Core.ExportReport.DropdownModel
{
    /// <summary>
    /// 查詢後返回的一級單位選單資料容器。
    /// </summary>
    public class RoleForDropDownModel
    {
        /// <summary>
        /// CR01，職位序號。
        /// </summary>
        public required int Id { get; set; }

        /// <summary>
        /// CR02，職位名稱。
        /// </summary>
        public required string RoleName { get; set; }
    }
}
