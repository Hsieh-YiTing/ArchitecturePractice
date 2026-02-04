using ArchitecturePractice.Core.ExportReport.Interface;
using ArchitecturePractice.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ArchitecturePractice.Controllers.ExportReport
{
    [Route("api/export")]
    [ApiController]
    public class ExportReportApiController(IExportReportService exportReportService) : ControllerBase
    {
        private readonly IExportReportService _exportReportService = exportReportService;

        /// <summary>
        /// 根據請求的CompanyId取得對應的單位清單API。
        /// </summary>
        [HttpGet("roles")]
        public async Task<IActionResult> GetRolesByCompanyId([FromQuery] int companyId)
        {
            var result = await _exportReportService.GetRoleListByCompanyIdAsync(companyId);

            return Ok(ServiceResultExtensions.ToApiResult(result));
        }
    }
}
