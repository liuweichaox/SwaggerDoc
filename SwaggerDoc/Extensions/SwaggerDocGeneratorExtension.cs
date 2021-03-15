using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using SwaggerDoc.Helpers;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwaggerDoc.Extensions
{
    /// <summary>
    /// <see cref="ISwaggerDocGenerator"/>服务
    /// </summary>
    public static class SwaggerDocGeneratorExtension
    {
        /// <summary>
        /// 注册<see cref="ISwaggerDocGenerator"/>服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSwaggerDoc(this IServiceCollection services)
        {
            services.AddScoped<SwaggerGenerator>();
            services.AddScoped<ISwaggerDocGenerator, SwaggerDocGenerator>();
            return services;
        }
        /// <summary>
        /// 判断是否为 Object 类型
        /// </summary>
        /// <param name="openApiSchema"></param>
        /// <returns></returns>
        public static bool IsObject(this OpenApiSchema openApiSchema, IDictionary<string, OpenApiSchema> schemas)
        {
            return openApiSchema.Type == null && openApiSchema.Reference != null && schemas.FirstOrDefault(x => x.Key == openApiSchema.Reference.Id).Value.Enum.Count == 0;
        }
        /// <summary>
        /// 判断是否为枚举类型
        /// </summary>
        /// <param name="openApiSchema"></param>
        /// <param name="schemas"></param>
        /// <returns></returns>
        public static bool IsEnum(this OpenApiSchema openApiSchema, IDictionary<string, OpenApiSchema> schemas)
        {
            return openApiSchema.Reference != null && schemas.FirstOrDefault(x => x.Key == openApiSchema.Reference.Id).Value.Enum.Count != 0;
        }
        /// <summary>
        /// 判断是否为数组类型
        /// </summary>
        /// <param name="openApiSchema"></param>
        /// <returns></returns>
        public static bool IsArray(this OpenApiSchema openApiSchema)
        {
            return openApiSchema.Type == "array" && openApiSchema.Items != null;
        }
        /// <summary>
        /// 判断是否为基础数组类型
        /// </summary>
        /// <param name="openApiSchema"></param>
        /// <returns></returns>
        public static bool IsBaseTypeArray(this OpenApiSchema openApiSchema)
        {
            return openApiSchema.Type == "array" && openApiSchema.Items != null && openApiSchema.Items.Type != null && openApiSchema.Items.Reference == null;
        }

        /// <summary>
        /// 判断是否为基本类型
        /// </summary>
        /// <param name="openApiSchema"></param>
        public static bool IsBaseType(this OpenApiSchema openApiSchema)
        {
            return openApiSchema.Type != null;
        }
    }
}
