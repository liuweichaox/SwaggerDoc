using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwaggerDoc.Models
{
    /// <summary>
    /// Model
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
    }



}
