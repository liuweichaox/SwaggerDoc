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
    public class Input
    {
        /// <summary>
        ///  Status Type XML
        /// </summary>
        public StatusType Type { get; set; }
        /// <summary>
        ///  Code XML
        /// </summary>
        public string Code { get; set; }
    }
}
