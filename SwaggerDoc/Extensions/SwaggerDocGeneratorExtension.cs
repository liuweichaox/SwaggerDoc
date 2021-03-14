using Microsoft.Extensions.DependencyInjection;
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
            services.AddScoped<ISwaggerDocGenerator,SwaggerDocGenerator>();
            return services;
        }
    }
}
