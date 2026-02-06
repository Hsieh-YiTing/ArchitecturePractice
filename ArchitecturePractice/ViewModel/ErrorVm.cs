namespace ArchitecturePractice.ViewModel
{
    /// <summary>
    /// 錯誤頁面的ViewModel。
    /// </summary>
    public class ErrorVm
    {
        /// <summary>
        /// 當前TraceId。
        /// </summary>
        public string? RequestId { get; set; }

        /// <summary>
        /// 方便頁面判斷，如果有RequestId就顯示。
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
