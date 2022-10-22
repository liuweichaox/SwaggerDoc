using Microsoft.AspNetCore.Mvc;
using Samples.Models;

namespace Samples.Controllers
{
    /// <summary>
    /// 示例控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        /// <summary>
        /// Get方法
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public Output Get(int id, Input input)
        {
            return new Output();
        }
        
        /// <summary>
        /// Post方法
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }
        
        /// <summary>
        /// Put方法
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }
        
        /// <summary>
        /// Delete方法
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
