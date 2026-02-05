using Microsoft.Extensions.Logging;

namespace ArchitecturePractice.Common.Logger
{
    public static partial class LoggerExtension
    {
        [LoggerMessage(
            EventId = 1001,
            Level = LogLevel.Error, 
            Message = "應用程式執行失敗 | TraceId為: {TraceId}")]
        public static partial void AppErrorLog(this ILogger logger, string? traceId, Exception? ex);

        [LoggerMessage(
            EventId = 1002,
            Level = LogLevel.Warning,
            Message = "應用程式業務邏輯錯誤 | 錯誤訊息為: {ErrorMessage}")]
        public static partial void AppBusinessErrorLog(this ILogger logger, string errorMessage);
    }
}
