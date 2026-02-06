using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ArchitecturePractice.Controllers.Base
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiBaseController<T> : ControllerBase
        where T : ApiBaseController<T>
    {
        // 快取變數，儲存ILogger物件
        private ILogger<T>? _logger;

        /// <summary>
        /// Service Locator模式，呼叫Logger時候才會取得ILogger物件。
        /// </summary>
        protected ILogger<T> Logger => _logger ??= HttpContext.RequestServices.GetService<ILogger<T>>() ?? throw new InvalidOperationException($"無法取得{typeof(T).Name}的ILogger物件。");

        /// <summary>
        /// 目前請求的TraceId。
        /// </summary>
        protected string CurrentTraceId => Activity.Current?.TraceId.ToString() ?? HttpContext.TraceIdentifier;

    }
}
