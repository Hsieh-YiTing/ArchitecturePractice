using ArchitecturePractice.Core.CommonFormat;
using ArchitecturePractice.Core.ExportReport.DropdownModel;
using ArchitecturePractice.Core.ExportReport.ExportModel;
using ArchitecturePractice.Core.ExportReport.Interface;
using System.Reflection;

namespace ArchitecturePractice.Services.ExportReport
{
    public class ExportReportService(IExportReportDropdownRepository exportReportDropdownRepository, IReportGeneratorFactory reportGeneratorFactory) : IExportReportService
    {
        private readonly IExportReportDropdownRepository _exportReportDropdownRepository = exportReportDropdownRepository;

        private readonly IReportGeneratorFactory _reportGeneratorFactory = reportGeneratorFactory;

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

        /// <inheritdoc />
        public async Task<ServiceResult<ExportResultModel>> ExportPersonalHealthExamAsync(ExportRequestBaseModel exportRequestModel)
        {
            // 將Model傳入工廠，取得對應的報表產生器
            var reportGenerator = _reportGeneratorFactory.GetReportGenerator(exportRequestModel);

            // 使用報表產生器生成報表
            var result = await reportGenerator.GenerateReportAsync(exportRequestModel);

            return ServiceResult<ExportResultModel>.Success(result, "報表匯出成功");
        }
    }
}
