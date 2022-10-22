using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwaggerDoc.Models
{
    /// <summary>
    /// 基础模型
    /// </summary>
    public abstract class BaseModelInfo
    {
        /// <summary>
        /// 参数类型
        /// </summary>
        public object 参数类型 { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string 描述 { get; set; }
        /// <summary>
        /// 可空类型
        /// </summary>
        public bool 可空类型 { get; set; }
    }
}
