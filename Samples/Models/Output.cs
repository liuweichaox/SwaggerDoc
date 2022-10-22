using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Samples.Models
{
    /// <summary>
    /// 输入模型 <see>
    ///     <cref>HomeController.Index</cref>
    /// </see>
    /// action.
    /// </summary>
    public class Output
    {
        /// <summary>
        /// Status Type XML
        /// </summary>
        public StatusType Status { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }
    }
}
