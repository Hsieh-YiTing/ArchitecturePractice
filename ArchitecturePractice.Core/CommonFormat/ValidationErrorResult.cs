namespace ArchitecturePractice.Core.CommonFormat
{
    /// <summary>
    /// 驗證錯誤結果類別。
    /// </summary>
    public class ValidationErrorResult
    {
        /// <summary>
        /// 驗證失敗的屬性名稱。
        /// </summary>
        public string? PropertyName { get; set; }

        /// <summary>
        /// 驗證失敗的錯誤訊息。
        /// </summary>
        public string? ErrorMessage { get; set; }
    }
}
