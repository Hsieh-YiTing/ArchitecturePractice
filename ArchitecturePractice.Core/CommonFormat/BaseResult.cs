namespace ArchitecturePractice.Core.CommonFormat
{
    /// <summary>
    /// 抽象類別，繼承給ServiceResult及ApiResult使用的共同屬性。
    /// </summary>
    public abstract class BaseResult<T>
    {
        /// <summary>
        /// 是否成功。
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 成功/失敗的訊息。
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// 執行成功取得的資料。
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// 執行失敗時的詳細錯誤資訊集合。
        /// </summary>
        public IEnumerable<ValidationErrorResult>? Errors { get; set; }
    }
}
