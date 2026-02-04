using ArchitecturePractice.Core.CommonFormat;
using System.Diagnostics;

namespace ArchitecturePractice.CommonFormat
{
    /// <summary>
    /// 統一回傳API執行的結果。
    /// </summary>
    public class ApiResult<T> : BaseResult<T>
    {
        /// <summary>
        /// API請求的追蹤識別碼。
        /// </summary>
        public string? TraceId { get; set; }

        /// <summary>
        /// 取得API回傳結果，由Service結果與追蹤識別碼組成。
        /// </summary>
        /// <param name="source">Service結果。</param>
        /// <param name="traceId">API請求的追蹤識別碼。</param>
        /// <returns>ApiResult<T>。</returns>
        public static ApiResult<T> GetApiResult(BaseResult<T> source, string? traceId)
        {
            return new ApiResult<T>
            {
                IsSuccess = source.IsSuccess,
                Message = source.Message,
                Data = source.Data,
                Errors = source.Errors,
                TraceId = traceId
            };
        }
    }
}
