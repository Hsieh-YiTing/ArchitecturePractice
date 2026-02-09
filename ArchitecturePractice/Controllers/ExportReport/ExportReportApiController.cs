using ArchitecturePractice.Common.Helper;
using ArchitecturePractice.Common.Logger;
using ArchitecturePractice.Controllers.Base;
using ArchitecturePractice.Core.CommonFormat;
using ArchitecturePractice.Core.ExportReport.ExportModel.PersonalHealthExam;
using ArchitecturePractice.Core.ExportReport.Interface;
using ArchitecturePractice.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace ArchitecturePractice.Controllers.ExportReport
{
    [Route("api/export")]
    public class ExportReportApiController(IExportReportService exportReportService) : ApiBaseController<ExportReportApiController>
    {
        private readonly IExportReportService _exportReportService = exportReportService;

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
                Logger.AppBusinessErrorLog(errorMessage, CurrentTraceId);

                return StatusCode(500, ServiceResultExtensions.ToApiResult(result, CurrentTraceId));
            }

            return Ok(ServiceResultExtensions.ToApiResult(result));
        }

        [HttpPost("personal-health-exam")]
        public async Task<IActionResult> ExportPersonalHealthExam([FromBody] PersonalExportRequest reportParams)
        {
            JsonDebugHelper.ObjectValuePrint(reportParams);

            var validator = HttpContext.RequestServices.GetService<IValidator<PersonalExportRequest>>();

            if (validator is null)
            {

                return StatusCode(500, ApiResult<PersonalExportRequest>.Failure("伺服器內部錯誤。", CurrentTraceId));
            }

            var validationResult = await validator.ValidateAsync(reportParams);

            if (!validationResult.IsValid)
            {
                var validationErrors = validationResult.Errors.Select(e => new ValidationErrorResult
                {
                    PropertyName = e.PropertyName,
                    ErrorMessage = e.ErrorMessage
                });

                // 記錄驗證失敗的Log，等級為Warning
                Logger.ValidationFailureLog(CurrentTraceId, validationErrors);

                return BadRequest(ApiResult<PersonalExportRequest>.DataValidationError("驗證失敗。", CurrentTraceId, validationErrors));
            }

            var exportResult = await _exportReportService.ExportPersonalHealthExamAsync(reportParams);

            if (!exportResult.IsSuccess)
            {
                string errorMessage = string.IsNullOrEmpty(exportResult.Message) ? "個人健檢報告匯出失敗。" : exportResult.Message;
                Logger.AppBusinessErrorLog(errorMessage, CurrentTraceId);

                return StatusCode(500, ServiceResultExtensions.ToApiResult(exportResult, CurrentTraceId));
            }

            return Ok(ServiceResultExtensions.ToApiResult(exportResult));
        }
    }
}
