using ArchitecturePractice.Core.CommonFormat;
using ArchitecturePractice.Core.ExportReport.DropdownModel;
using ArchitecturePractice.Core.ExportReport.Interface;

namespace ArchitecturePractice.Services.ExportReport
{
    public class ExportReportService(IExportReportDropdownRepository exportReportDropdownRepository) : IExportReportService
    {
        private readonly IExportReportDropdownRepository _exportReportDropdownRepository = exportReportDropdownRepository;

        /// <inheritdoc />
        public async Task<ServiceResult<IEnumerable<CompanyForDropDownModel>>> GetCompanyListAsync()
        {
            var queryResult = await _exportReportDropdownRepository.GetCompanyItemsAsync();

            return ServiceResult<IEnumerable<CompanyForDropDownModel>>.Success(queryResult);
        }

        /// <inheritdoc />
        public async Task<ServiceResult<IEnumerable<RoleForDropDownModel>>> GetRoleListByCompanyIdAsync(int companyId)
        {
            var queryResult = await _exportReportDropdownRepository.GetRoleItemsByCompanyIdAsync(companyId);

            return ServiceResult<IEnumerable<RoleForDropDownModel>>.Success(queryResult);
        }
    }
}
