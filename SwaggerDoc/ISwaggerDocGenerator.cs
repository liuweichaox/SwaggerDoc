using System;
using System.Collections.Generic;
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
        string GetSwaggerDoc(string name);
    }
}
