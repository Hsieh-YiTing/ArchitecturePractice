using ArchitecturePractice.Core.CommonFormat;
using Microsoft.Extensions.Logging;

namespace ArchitecturePractice.Common.Logger
{
    /// <summary>
    /// Logger擴充方法，定義LoggerMessage模板，以及Logger紀錄方法。
    /// </summary>
    public static partial class LoggerExtension
    {
        /// <summary>
        /// 紀錄例外的程式錯誤，包含TraceId和例外訊息。
        /// </summary>
        [LoggerMessage(
            EventId = 1001,
            Level = LogLevel.Error, 
            Message = "應用程式執行失敗 | TraceId為: {TraceId}")]
        public static partial void AppErrorLog(this ILogger logger, string? traceId, Exception? ex);

        /// <summary>
        /// 紀錄Service層的業務邏輯錯誤，包含錯誤訊息。
        /// </summary>
        [LoggerMessage(
            EventId = 1002,
            Level = LogLevel.Warning,
            Message = "應用程式業務邏輯錯誤 | TraceId為: {TraceId} | 錯誤訊息為: {ErrorMessage}")]
        public static partial void AppBusinessErrorLog(this ILogger logger, string? traceId, string errorMessage);

        /// <summary>
        /// 紀錄Model驗證失敗的訊息。
        /// </summary>
        [LoggerMessage(
            EventId = 1003,
            Level = LogLevel.Warning,
            Message = "Model驗證失敗 | TraceId為: {TraceId} | 錯誤訊息為: {validationErrors}")]
        public static partial void ValidationFailureLog(this ILogger logger, string? traceId, IEnumerable<ValidationErrorResult> validationErrors);
    }
}
