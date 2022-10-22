using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SwaggerDoc;

namespace Samples.Controllers
{
    /// <summary>
    /// Swagger 控制器
    /// </summary>
    [ApiController]
    public class SwaggerController : ControllerBase
    {
        /// <summary>
        /// API文档导出
        /// </summary>
        [HttpGet("/doc")]
        public async Task<IActionResult> Doc([FromServices] ISwaggerDocGenerator swaggerDocGenerator, [FromServices] IWebHostEnvironment environment)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var stream = await swaggerDocGenerator.GetSwaggerDocStreamAsync("v1");
            stopwatch.Stop();
            var log = "Swagger文档导出成功，耗时" + stopwatch.ElapsedMilliseconds + "ms";
            Debug.WriteLine(log);
            var mime = "application/octet-stream";
            const string name = "SwaggerDoc.md";
            return File(stream.ToArray(), mime, name);
        }
    }
}
