using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwaggerDoc
{
    /// <summary>
    /// ISwaggerDocGenerator
    /// </summary>
    public interface ISwaggerDocGenerator
    {
        /// <summary>
        /// 获取Swagger流文件
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<MemoryStream> GetSwaggerDocStreamAsync(string name);
        /// <summary>
        /// 获取Swagger MarkDown源代码
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        string GetSwaggerDoc(string name);
    }
}
