using System.Collections.Concurrent;
using System.Text;

namespace ArchitecturePractice.Common.Helper
{
    public static class EmbeddedResourceHelper
    {
        // 使用執行緒安全的字典來做快取
        private static readonly ConcurrentDictionary<string, string> _cache = new();

        /// <summary>
        /// 取得嵌入式資源的內容 (含快取機制)。
        /// </summary>
        /// <param name="namespacePath">資源所在的命名空間路徑。</param>
        /// <param name="fileName">檔案名稱。</param>
        /// <returns>SQL字串。</returns>
        public static string GetSqlContent<T>(string namespacePath, string fileName)
        {
            // 組合完整的資源路徑
            string resourceKey = $"{namespacePath}.{fileName}";

            // 檢查快取，如果有就直接回傳，沒有才執行後面的讀取邏輯
            return _cache.GetOrAdd(resourceKey, key =>
            {
                // 取得Assembly(.dll檔案或.exe檔案)
                var assembly = typeof(T).Assembly;

                // 去Assembly內的資源清單取得資料流
                using (Stream? stream = assembly.GetManifestResourceStream(key))
                {
                    // 判斷是否為空，空值就直接拋出錯誤，中介軟體會捕捉
                    if (stream == null)
                    {
                        // 方便除錯，列出所有可用的資源名稱
                        var existingResources = string.Join(Environment.NewLine, assembly.GetManifestResourceNames());
                        throw new FileNotFoundException($"找不到嵌入式資源: '{key}'。{Environment.NewLine}目前可用的資源如下:{Environment.NewLine}{existingResources}");
                    }

                    // 開始讀取資料流，使用Encoding.UTF8確保不會亂碼
                    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        return reader.ReadToEnd();
                    }
                }
            });
        }
    }
}
