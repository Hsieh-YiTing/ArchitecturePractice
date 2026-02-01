using ArchitecturePractice.Core.ExportReport.DropdownModel;

namespace ArchitecturePractice.Core.ExportReport.Interface
{
    /// <summary>
    /// ExportReport頁面下拉選單資料存取介面
    /// </summary>
    public interface IExportReportDropdownRepository
    {
        /// <summary>
        /// 取得公司選單所需資料。
        /// </summary>
        /// <returns>CompanyForDropDownModel集合。</returns>
        Task<IEnumerable<CompanyForDropDownModel>> GetCompanyItemsAsync();

        /// <summary>
        /// 依據CompanyId取得該公司下的單位選單所需資料。
        /// </summary>
        /// <param name="companyId">公司ID。</param>
        /// <returns>RoleForDropDownModel集合。</returns>
        Task<IEnumerable<RoleForDropDownModel>> GetRoleItemsByCompanyIdAsync(int companyId);
    }
}
