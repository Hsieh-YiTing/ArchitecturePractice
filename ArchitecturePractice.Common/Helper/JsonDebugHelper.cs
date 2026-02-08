using System.Diagnostics;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ArchitecturePractice.Common.Helper
{
    public static class JsonDebugHelper
    {
        // 初始化序列化選項
        private static readonly JsonSerializerOptions _options = new JsonSerializerOptions()
        {
            // 讓JSON輸出有換行以及縮排
            WriteIndented = true,

            // 避免循環參考導致序列化失敗
            ReferenceHandler = ReferenceHandler.IgnoreCycles,

            // 可以讓中文正常顯示
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        /// <summary>
        /// 在Debug模式下，將物件序列化為JSON並輸出到Debug視窗。
        /// </summary>
        /// <param name="obj">實際要輸出的物件。</param>
        [Conditional("DEBUG")]
        public static void ObjectValuePrint(object obj)
        {
            try
            {
                string objName = obj?.GetType().Name ?? "Unknown";
                var json = JsonSerializer.Serialize(obj, _options);

                Debug.WriteLine($"=====以下為{objName}資料內容輸出=====");
                Debug.WriteLine(json);
                Debug.WriteLine("=====輸出結束=====");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"序列化失敗: {ex.Message}");
            }
        }
    }
}
