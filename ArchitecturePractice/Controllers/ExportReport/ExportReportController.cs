using ArchitecturePractice.Core.ExportReport.Interface;
using ArchitecturePractice.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArchitecturePractice.Controllers.ExportReport
{
    public class ExportReportController(IExportReportService exportReportService) : Controller
    {
        private readonly IExportReportService _exportReportService = exportReportService;

        /// <summary>
        /// 初始化ExportPage。
        /// </summary>
        /// <returns>帶有CompanyForDropDownVm的ExportPage View。</returns>
        public async Task<IActionResult> ExportPage()
        {
            var result = await _exportReportService.GetCompanyListAsync();

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
