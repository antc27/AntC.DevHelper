﻿using AntC.CodeGenerate.CodeConverters;
using AntC.CodeGenerate.Interfaces;
using AntC.CodeGenerate.Model;
using AntC.CodeGenerate.Mysql;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AntC.CodeGenerate.Cmd.Benchint.Libra.PropertyTypeConverters;
using AntC.CodeGenerate.Extension;

namespace AntC.CodeGenerate.Cmd
{
    class Program
    {
        static void Main(string[] args)
        {
            //var dbConnectionString = "server=10.4.1.248;port=3310;database=information_schema;User ID=root;Password=123456;";
            var dbConnectionString = "server=localhost;port=3306;database=information_schema;User ID=root;Password=123456;";
            var dbName = "libra.kpidb";
            var tableNames = new List<string>() { "kpi_define_base", "kpi_define_ext" };

            var serviceProvider = Init(services =>
            {
            });

            var codeGeneratorManager = InitGeneratorManager(serviceProvider, dbConnectionString);

            codeGeneratorManager.AddCodeGenerator(typeof(Program).Assembly);
            codeGeneratorManager.AddPropertyTypeConverter(typeof(Program).Assembly);

            tableNames = codeGeneratorManager.GetTables(dbName).Select(x => x.TableName).ToList();
            var codeGenerateInfo = new CodeGenerateInfo()
            {
                //OutPutRootPath = AppDomain.CurrentDomain.BaseDirectory,
                DbName = dbName,
                CodeGenerateTableInfos = tableNames.Select(x => new CodeGenerateTableInfo()
                {
                    TableName = x,
                    GroupName = GetGroupName(x)
                })
            };
            codeGeneratorManager.ExecCodeGenerate(codeGenerateInfo);

            var totalMemory = GC.GetTotalMemory(true);

            Console.WriteLine($"完成代码创建,共使用内存{GetMemorySizeWithUnit(totalMemory)}...");

            //Process p = new Process();
            //p.StartInfo.FileName = "explorer.exe";
            //p.StartInfo.Arguments = codeGenerateInfo.OutPutRootPath;
            //p.Start();
        }

        private static string GetGroupName(string tableName)
        {
            if (tableName.StartsWith("kpi_stat"))
            {
                return "Stat";
            }
            if (tableName.StartsWith("kpi_run"))
            {
                return "Run";
            }
            if (tableName.StartsWith("kpi_define"))
            {
                return "Define";
            }

            return "Basic";
        }

        private static string GetMemorySizeWithUnit(decimal byteSize, int jz = 0)
        {
            if (byteSize > 1024)
            {
                return GetMemorySizeWithUnit(byteSize / 1024, jz + 1);
            }

            var unit = jz switch
            {
                0 => "B",
                1 => "KB",
                2 => "MB",
                3 => "GB",
                4 => "TB",
                5 => "PB",
            };
            return $"{Math.Round(byteSize, 2)}{unit}";
        }

        private static IServiceProvider Init(Action<IServiceCollection> action = null)
        {
            IServiceCollection services = new ServiceCollection();
            //注入
            //services.AddTransient<EntityCodeGenerateExecutor, EntityCodeGenerateExecutor>();
            services.AddTransient<MysqlDbInfoProvider, MysqlDbInfoProvider>();
            services.AddTransient<ICodeConverter, DefaultCodeConverter>();

            services.UseCodeGenerateExecutor(typeof(Program).Assembly);
            services.UsePropertyTypeConverter(typeof(Program).Assembly);

            action?.Invoke(services);

            //构建容器
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }

        private static ICodeGeneratorManager InitGeneratorManager(IServiceProvider serviceProvider, string dbConnectionString)
        {
            var codeGeneratorManager = new CodeGeneratorManager(serviceProvider.GetService<MysqlDbInfoProvider>())
            {
                ServiceProvider = serviceProvider,
                CodeConverter = serviceProvider.GetService<ICodeConverter>(),
                DbConnectionString = dbConnectionString,
            };
            return codeGeneratorManager;
        }
    }
}
