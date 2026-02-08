using ArchitecturePractice.Controllers.Base;
using ArchitecturePractice.Common.Logger;
using ArchitecturePractice.Core.ExportReport.Interface;
using ArchitecturePractice.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArchitecturePractice.Controllers.ExportReport
{
    public class ExportReportController(IExportReportService exportReportService) : ViewBaseController<ExportReportController>
    {
        private readonly IExportReportService _exportReportService = exportReportService;

        /// <summary>
        /// 初始化ExportPage。
        /// </summary>
        /// <returns>帶有CompanyForDropDownVm的ExportPage View。</returns>
        public async Task<IActionResult> ExportPage()
        {
            var result = await _exportReportService.GetCompanyListAsync();

            // 處理服務失敗的情況
            if (!result.IsSuccess)
            {
                string errorMessage = string.IsNullOrEmpty(result.Message) ? "公司選單載入失敗。" : result.Message;
                Logger.AppBusinessErrorLog(errorMessage, CurrentTraceId);

                // 將錯誤訊息加入ModelState
                ModelState.AddModelError(string.Empty, errorMessage);

                // 回傳一個空的CompanyForDropDownVm，以避免View因為缺少資料而崩潰
                var emptyCompanyListVm = new CompanyForDropDownVm
                {
                    CompanyItemList = new List<SelectListItem>()
                };

                return View(emptyCompanyListVm);
            }

            var companyListVm = new CompanyForDropDownVm
            {
                CompanyItemList = result.Data?.Select(c => new SelectListItem
                {
                    Text = c.CompanyName,
                    Value = c.Id.ToString()
                }) ?? new List<SelectListItem>()
            };

            return View(companyListVm);
        }
    }
}
