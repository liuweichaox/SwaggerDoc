using Newtonsoft.Json;
using System.IO;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace SwaggerDoc.Extensions
{
    /// <summary>
    /// JSON 拓展
    /// </summary>
    public static class JsonExtension
    {
        /// <summary>
        /// 转换为JSON字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {
            return ConvertJsonString(JsonConvert.SerializeObject(obj));
        }
        
        /// <summary>
        /// 转换为JSON字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string ConvertJsonString(string str)
        {
            var serializer = new JsonSerializer();
            TextReader tr = new StringReader(str);
            var jtr = new JsonTextReader(tr);
            var obj = serializer.Deserialize(jtr);
            if (obj == null) return str;
            var textWriter = new StringWriter();
            var jsonWriter = new JsonTextWriter(textWriter)
            {
                Formatting = Formatting.Indented,
                Indentation = 4,
                IndentChar = ' '
            };
            serializer.Serialize(jsonWriter, obj);
            return textWriter.ToString();

        }
    }
}
