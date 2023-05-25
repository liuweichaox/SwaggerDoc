using System.IO;
using System.Threading.Tasks;

namespace SwaggerDoc
{
    /// <summary>
    /// Swagger 文档生成器接口
    /// </summary>
    public interface ISwaggerDocGenerator
    {
        /// <summary>
        /// 获取 Swagger 流文件
        /// </summary>
        /// <param name="name"></param>E
        /// <returns></returns>
        Task<MemoryStream> GetSwaggerDocStreamAsync(string name);
        /// <summary>
        /// 获取 Swagger MarkDown 源代码
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        string GetSwaggerDoc(string name);
    }
}
