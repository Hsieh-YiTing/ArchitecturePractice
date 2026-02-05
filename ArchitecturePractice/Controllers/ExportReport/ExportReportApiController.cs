using ArchitecturePractice.Core.ExportReport.Interface;
using ArchitecturePractice.Common.Logger;
using ArchitecturePractice.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ArchitecturePractice.Controllers.ExportReport
{
    [Route("api/export")]
    [ApiController]
    public class ExportReportApiController(ILogger<ExportReportApiController> logger, IExportReportService exportReportService) : ControllerBase
    {
        private readonly ILogger<ExportReportApiController> _logger = logger;

        private readonly IExportReportService _exportReportService = exportReportService;

        /// <summary>
        /// 每次請求的TraceId。
        /// </summary>
        private string CurrentTraceId => Activity.Current?.TraceId.ToString() ?? HttpContext.TraceIdentifier;

        /// <summary>
        /// 根據請求的CompanyId取得對應的單位清單API。
        /// </summary>
        [HttpGet("roles")]
        public async Task<IActionResult> GetRolesByCompanyId([FromQuery] int companyId)
        {
            var result = await _exportReportService.GetRoleListByCompanyIdAsync(companyId);

            // 處理服務失敗的情況
            if (!result.IsSuccess)
            {
                string errorMessage = string.IsNullOrEmpty(result.Message) ? "一級單位選單載入失敗。" : result.Message;
                _logger.AppBusinessErrorLog(errorMessage);

                return StatusCode(500, ServiceResultExtensions.ToApiResult(result, CurrentTraceId));
            }

            return Ok(ServiceResultExtensions.ToApiResult(result));
        }
    }
}
