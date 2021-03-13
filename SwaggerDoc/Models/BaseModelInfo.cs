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

    /// <summary>
    /// RequestModelInfo
    /// </summary>
    public class RequestModelInfo : BaseModelInfo
    {
        /// <summary>
        /// 是否必传
        /// </summary>
        public bool 是否必传 { get; set; }
    }
    /// <summary>
    /// ResponseModelInfo
    /// </summary>
    public class ResponseModelInfo : BaseModelInfo
    {

    }
    /// <summary>
    /// ModelExample
    /// </summary>
    public class ModelExample : Dictionary<string, object>
    {

    }
}
