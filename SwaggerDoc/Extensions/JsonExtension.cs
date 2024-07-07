using System.Text.Encodings.Web;
using System.Text.Json;

namespace SwaggerDoc.Extensions
{
    /// <summary>
    /// JSON 拓展
    /// </summary>
    public static class JsonExtension
    {
        /// <summary>
        /// 转换为 JSON 字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {
           var options = new JsonSerializerOptions
           {
               Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
               WriteIndented = true
           };
           return JsonSerializer.Serialize(obj, options);
        }
    }
}
