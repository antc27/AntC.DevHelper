﻿using System;
using System.IO;
using System.Linq;
using AntC.CodeGenerate.CodeGenerateExecutors;
using AntC.CodeGenerate.Model;

namespace AntC.CodeGenerate.Plugin.Benchint.Libra.CodeGenerators.Application.Contracts
{
    public class PagedAndSortedResultRequestDtoGenerator : BaseTableCodeGenerator
    {
        public override void PreExecCodeGenerate(CodeGenerateTableContext context)
        {
            var outPutPath = Path.Combine("Application.Contracts",
                context.ClassInfo.GroupName ?? string.Empty,
                "Dto",
                context.ClassInfo.ClassName,
                $"{context.ClassInfo.ClassName}PagedAndSortedResultRequestDto.cs");
            SetRelativePath(context, outPutPath);
        }

        public override void ExecutingCodeGenerate(CodeGenerateTableContext context)
        {
            context.AppendLine("using System;");
            context.AppendLine("using Volo.Abp.Application.Dtos;");
            context.AppendLine("");
            context.AppendLine($"namespace {context.GetNameSpace()}");
            context.AppendLine("{");
            context.AppendLine($"    /// <summary>");
            context.AppendLine($"    /// {context.ClassInfo.Annotation} 分页排序数据传输对象");
            context.AppendLine($"    /// </summary>");
            context.AppendLine($"    public class {context.ClassInfo.ClassName}PagedAndSortedResultRequestDto : PagedAndSortedResultRequestDto");
            context.AppendLine("    {");

            context.AppendLine($"        public {context.ClassInfo.ClassName}PagedAndSortedResultRequestDto()");
            context.AppendLine("        {");

            context.AppendLine($"            if (this.Sorting.IsNullOrWhiteSpace())");
            context.AppendLine($"            {{");
            context.AppendLine($"                Sorting = \"{GetSortPropertyName(context.ClassInfo)} Asc\";");
            context.AppendLine($"            }}");

            context.AppendLine("        }");
            context.AppendLine("    }");
            context.AppendLine("}");
        }

        private string GetSortPropertyName(ClassModel classModel)
        {
            var sortNo = classModel.Properties.FirstOrDefault(x => "SortNo".Equals(x.PropertyName, StringComparison.CurrentCultureIgnoreCase));
            if (sortNo != null)
            {
                return sortNo.PropertyName;
            }

            return classModel.Properties.FirstOrDefault(x => x.DbColumnInfo.Key)?.PropertyName;
        }
    }
}
