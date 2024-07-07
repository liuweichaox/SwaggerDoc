using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace SwaggerDoc
{
    /// <summary>
    /// Swagger 控制器
    /// </summary>
    [ApiController]
    [AllowAnonymous]
    public class SwaggerController : ControllerBase
    {
        /// <summary>
        /// API文档导出
        /// </summary>
        /// <param name="swaggerDocGenerator"></param>
        /// <param name="swaggerVersion"></param>
        /// <returns></returns>
        [HttpGet("/doc")]
        public async Task<IActionResult> Doc([FromServices] ISwaggerDocGenerator swaggerDocGenerator, string swaggerVersion = "v1")
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var stream = await swaggerDocGenerator.GetSwaggerDocStreamAsync(swaggerVersion);
            stopwatch.Stop();
            var log = "Swagger文档导出成功，耗时" + stopwatch.ElapsedMilliseconds + "ms";
            Debug.WriteLine(log);
            const string mime = "application/octet-stream";
            const string name = "SwaggerDoc.md";
            return File(stream.ToArray(), mime, name);
        }
    }
}
