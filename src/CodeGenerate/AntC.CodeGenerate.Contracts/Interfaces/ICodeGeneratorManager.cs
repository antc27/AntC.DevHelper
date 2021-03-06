﻿using System;
using System.Collections.Generic;
using System.Reflection;
using AntC.CodeGenerate.Model;

namespace AntC.CodeGenerate.Interfaces
{
    /// <summary>
    /// 代码创建器管理
    /// </summary>
    public interface ICodeGeneratorManager : IDbInfoProvider, ICodeConverter, ICodeGeneratorContainer, ICodeConverterContainer
    {
        IServiceProvider ServiceProvider { get; set; }

        event Action<ICodeWriter> OnCodeWriterCreated;

        /// <summary>
        /// 执行代码创建
        /// </summary>
        /// <param name="codeGenerateInfo"></param>
        void ExecCodeGenerate(CodeGenerateInfo codeGenerateInfo);

        /// <summary>
        /// 设置代码输出器类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void SetCodeWriterType<T>() where T : ICodeWriter;
    }
}
