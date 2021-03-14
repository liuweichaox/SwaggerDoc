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
        Task<MemoryStream> GetSwaggerDocStreamAsync(string name);
        string GetSwaggerDoc(string name);
    }
}
