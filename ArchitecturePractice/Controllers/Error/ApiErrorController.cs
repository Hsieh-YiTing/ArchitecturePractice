using ArchitecturePractice.Common.Logger;
using ArchitecturePractice.Controllers.Base;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ArchitecturePractice.Core.CommonFormat;

namespace ArchitecturePractice.Controllers.Error
{
    [Route("api/error")]
    public class ApiErrorController : ApiBaseController<ApiErrorController>
    {
        /// <summary>
        /// 當前請求的錯誤物件。
        /// </summary>
        private Exception? Exception => HttpContext.Features.Get<IExceptionHandlerPathFeature>()?.Error;

        /// <summary>
        /// 未預期API請求錯誤處理。
        /// </summary>
        /// <returns>500以及ApiResult物件。</returns>
        public IActionResult Error()
        {
            if (Exception is not null)
            {
                Logger.AppErrorLog(CurrentTraceId, Exception);
            }

            return StatusCode(500, ApiResult<object>.Failure("請求執行失敗，請稍後再試。", CurrentTraceId));
        }
    }
}
