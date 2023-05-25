namespace SwaggerDoc.Models
{
    /// <summary>
    /// 请求参数
    /// </summary>
    public class RequestModelInfo
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

        /// <summary>
        /// 是否必传
        /// </summary>
        public bool 是否必传 { get; set; }
    }
}
