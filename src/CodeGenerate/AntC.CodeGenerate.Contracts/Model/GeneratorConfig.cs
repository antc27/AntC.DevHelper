﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AntC.CodeGenerate.Model
{
    /// <summary>
    /// 执行器配置
    /// </summary>
    public class GeneratorConfig
    {
        /// <summary>
        /// 文件保存相对路径
        /// </summary>
        public string FileRelativePath { get; set; }

        /// <summary>
        /// 文件保存编码
        /// </summary>
        public Encoding FileEncoding { get; set; } = Encoding.UTF8;
    }
}
