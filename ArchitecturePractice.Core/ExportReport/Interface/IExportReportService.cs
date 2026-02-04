using ArchitecturePractice.Core.CommonFormat;
using ArchitecturePractice.Core.ExportReport.DropdownModel;

namespace ArchitecturePractice.Core.ExportReport.Interface
{
    /// <summary>
    /// Export頁面Service。
    /// </summary>
    public interface IExportReportService
    {
        /// <summary>
        /// 取得公司選單所需資料。
        /// </summary>
        /// <returns>ServiceResult<IEnumerable<CompanyForDropDownDto>>。</returns>
        Task<ServiceResult<IEnumerable<CompanyForDropDownModel>>> GetCompanyListAsync();

        /// <summary>
        /// 依據CompanyId取得該公司下的一級單位選單所需資料。
        /// </summary>
        /// <param name="companyId">公司ID。</param>
        /// <returns>ServiceResult<IEnumerable<RoleForDropDownDto>>>。</returns>
        Task<ServiceResult<IEnumerable<RoleForDropDownModel>>> GetRoleListByCompanyIdAsync(int companyId);
    }
}
