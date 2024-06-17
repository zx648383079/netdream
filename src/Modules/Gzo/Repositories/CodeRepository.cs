using NetDream.Core.Helpers;
using NetDream.Modules.Gzo.Writers;
using System;
using System.Text.RegularExpressions;

namespace NetDream.Modules.Gzo.Repositories
{
    public class CodeRepository
    {

        public IWriter Exchange(string content, string source = "", string target = "")
        {
            var writer = new MemoryWriter();
            writer.Write(target, PhpToCSharp(content));
            return writer;
        }

        private string PhpToCSharp(string content) 
        {
            content = Regex.Replace(content, @"function([^\(]*)\(([^\)]*)\)([^\{]*)\{", match => {
                var paramerters = string.Empty;
                if (!string.IsNullOrWhiteSpace(match.Groups[2].Value))
                {
                    paramerters = string.Join(',', 
                        match.Groups[2].Value.Split(',').Select(item => {
                            var key = item.Trim();
                            if (key[0] == '$')
                            {
                                return "object " + key[1..];
                            }
                            return key.Replace("$", string.Empty);
                        }));
                }
                if (string.IsNullOrWhiteSpace(match.Groups[1].Value))
                {
                    return $"({paramerters}) => {{";
                }
                var returnType = "void";
                if (!string.IsNullOrWhiteSpace(match.Groups[3].Value))
                {
                    returnType = match.Groups[3].Value.Replace(":", string.Empty).Trim();
                }
                return $"{returnType} {Studly(match.Groups[1].Value)}({paramerters}) {{";
            });
            content = Regex.Replace(content, @"(->|::)([^\(\)\s]+)", 
                match => "." + Studly(match.Groups[2].Value));
            content = content.Replace("$this.", string.Empty)
                .Replace('\'', '"').Replace("$", string.Empty)
                .Replace("\"\"", "string.Empty")
                .Replace(" extends ", " : ")
                .Replace("RoleRepository.NewPermission", "privilege.AddPermission")
                .Replace("RoleRepository.NewRole", "privilege.AddRole")
                .Replace("Option.Group", "option.AddGroup")
                .Replace("Model.CreatedAt", "MigrationTable.COLUMN_CREATED_AT")
                .Replace("\"created_at\"", "MigrationTable.COLUMN_CREATED_AT")
                .Replace("\"updated_at\"", "MigrationTable.COLUMN_UPDATED_AT");
            content = Regex.Replace(content, @"Append\((\w+)\.TableName\(\),.+?\{", match => {
                var entity = match.Groups[1].Value.Replace("Model", "Entity");
                return $"Append<{entity}>(table => {{";
            });

            return content;
        }

        /// <summary>
        /// 转驼峰写法
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private static string Studly(string content)
        {
            content = content.Trim();
            if (string.IsNullOrEmpty(content))
            {
                return content;
            }
            if (!content.Contains('_'))
            {
                return content[0..1].ToUpper() + content[1..];
            }
            if (content.ToUpper().Equals(content, StringComparison.Ordinal))
            {
                return content;
            }
            return StrHelper.Studly(content);
        }
    }
}
