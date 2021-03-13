using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SwaggerDoc.Controllers
{
    /// <summary>
    /// SwaggerController
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SwaggerController : ControllerBase
    {
        /// <summary>
        /// API文档导出
        /// </summary>
        [HttpGet]
        public async Task<object> Export([FromServices] SwaggerHelper swaggerHelper, [FromServices] IWebHostEnvironment environment)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var md = swaggerHelper.GeneratorMarkDown();
            var file = Path.Combine(environment.ContentRootPath, "Swagger.md");
            using var fileStream = new FileStream(file, FileMode.OpenOrCreate);
            using var sw = new StreamWriter(fileStream);
            await sw.WriteLineAsync(md);
            stopwatch.Stop();
            return Ok("Swagger文档导出成功，耗时" + stopwatch.ElapsedMilliseconds + "ms");
        }
    }
}
