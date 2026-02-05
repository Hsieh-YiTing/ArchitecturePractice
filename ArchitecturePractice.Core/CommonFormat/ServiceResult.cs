namespace ArchitecturePractice.Core.CommonFormat
{
    /// <summary>
    /// 統一回傳Service執行的結果。
    /// </summary>
    public class ServiceResult<T> : BaseResult<T>
    {
        /// <summary>
        /// Service執行成功後，回傳的資料結構。
        /// </summary>
        /// <param name="data">執行成功後的資料。</param>
        /// <param name="message">執行成功後的訊息，可空。</param>
        /// <returns>ServiceResult<T>。</returns>
        public static ServiceResult<T> Success(T data, string? message = null)
        {
            return new ServiceResult<T>
            {
                IsSuccess = false,
                Data = data,
                Message = "測試錯誤"
            };
        }

        /// <summary>
        /// Service執行失敗後，回傳的資料結構。
        /// </summary>
        /// <param name="message">執行失敗後的訊息。</param>
        /// <param name="errors">驗證失敗後的錯誤集合，可空。</param>
        /// <returns>ServiceResult<T>。</returns>
        public static ServiceResult<T> Fail(string message, IEnumerable<ValidationErrorResult>? errors = null)
        {
            return new ServiceResult<T>
            {
                IsSuccess = false,
                Message = message,
                Errors = errors
            };
        }
    }
}
