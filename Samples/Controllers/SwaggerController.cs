using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace SwaggerDoc.Controllers
{
    /// <summary>
    /// SwaggerController
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SwaggerController : ControllerBase
    {
        /// <summary>
        /// API文档导出
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> SwaggerDoc([FromServices] ISwaggerDocGenerator swaggerDocGenerator, [FromServices] IWebHostEnvironment environment)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var stream = await swaggerDocGenerator.GetSwaggerDocStreamAsync("v1");
            stopwatch.Stop();
            var log = "Swagger文档导出成功，耗时" + stopwatch.ElapsedMilliseconds + "ms";
            Debug.WriteLine(log);
            var mime = "application/octet-stream";
            var name = "SwaggerDoc.md";
            return File(stream.ToArray(), mime,name);
        }
    }
}
