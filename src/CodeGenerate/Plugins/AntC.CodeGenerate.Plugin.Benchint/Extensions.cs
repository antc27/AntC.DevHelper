﻿using AntC.CodeGenerate.Extension;
using AntC.CodeGenerate.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AntC.CodeGenerate.Plugin.Benchint
{
    public static class Extensions
    {
        public static void AddBenchintCodeGenerateImpl(this ICodeGeneratorManager codeGeneratorManager)
        {
            codeGeneratorManager.AddCodeGenerator(typeof(Extensions).Assembly);
            codeGeneratorManager.AddPropertyTypeConverter(typeof(Extensions).Assembly);
        }

        /// <summary>
        /// 注入 代码生成执行器<see cref="ITableCodeGenerator"/>
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection UseBenchintCodeGenerateImpl(this IServiceCollection services)
        {
            services.UseCodeGenerateExecutor(typeof(Extensions).Assembly);
            services.UsePropertyTypeConverter(typeof(Extensions).Assembly);
            return services;
        }

    }
}
