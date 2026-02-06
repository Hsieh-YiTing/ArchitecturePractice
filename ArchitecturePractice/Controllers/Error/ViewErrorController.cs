using ArchitecturePractice.Controllers.Base;
using ArchitecturePractice.Common.Logger;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ArchitecturePractice.ViewModel;

namespace ArchitecturePractice.Controllers.Error
{
    public class ViewErrorController : ViewBaseController<ViewErrorController>
    {
        /// <summary>
        /// 每次請求的錯誤物件。
        /// </summary>
        private Exception? Exception => HttpContext.Features.Get<IExceptionHandlerPathFeature>()?.Error;

        /// <summary>
        /// 500與404的錯誤頁面，並禁止快取頁面，確保每次錯誤都是最新的追蹤碼。
        /// </summary>
        /// <param name="statusCode">Http狀態碼。</param>
        /// <returns>對應的錯誤頁面。</returns>
        [Route("error/error")]
        [Route("error/error/{statusCode}")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? statusCode = null)
        {
            if (Exception is not null)
            {
                Logger.AppErrorLog(CurrentTraceId, Exception);
            }

            if (statusCode == 404)
            {
                return View("_NotFound");
            }

            var errorVm = new ErrorVm
            {
                RequestId = CurrentTraceId
            };

            return View("_ErrorPage", errorVm);
        }
    }
}
