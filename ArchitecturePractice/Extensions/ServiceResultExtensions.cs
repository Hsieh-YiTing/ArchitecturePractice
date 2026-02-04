using ArchitecturePractice.CommonFormat;
using ArchitecturePractice.Core.CommonFormat;

namespace ArchitecturePractice.Extensions
{
    /// <summary>
    /// 擴充ServiceResult的功能。
    /// </summary>
    public static class ServiceResultExtensions
    {
        /// <summary>
        /// ServiceResult轉換為ApiResult並回傳。
        /// </summary>
        /// <param name="serviceResult">擴充ServiceResult。</param>
        /// <param name="traceId">API請求的追蹤識別碼，可空。</param>
        /// <returns>ApiResult<T>。</returns>
        public static ApiResult<T> ToApiResult<T>(this ServiceResult<T> serviceResult, string? traceId = null)
        {
            return ApiResult<T>.GetApiResult(serviceResult, traceId);
        }
    }
}
