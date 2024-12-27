using NetDream.Shared.Helpers;
using NetDream.Modules.Gzo.Writers;
using System;
using System.Text.RegularExpressions;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Reflection.Metadata;

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
                var parameters = string.Empty;
                if (!string.IsNullOrWhiteSpace(match.Groups[2].Value))
                {
                    parameters = string.Join(',', 
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
                    return $"({parameters}) => {{";
                }
                var returnType = "void";
                if (!string.IsNullOrWhiteSpace(match.Groups[3].Value))
                {
                    returnType = match.Groups[3].Value.Replace(":", string.Empty).Replace("?", string.Empty).Trim();
                }
                var func = Studly(match.Groups[1].Value);
                if (returnType == "void" && (func == "Up" || func == "Seed"))
                {
                    // Migration 实现继承
                    returnType = "override " + returnType;
                }
                return $"{returnType} {func}({parameters}) {{";
            });
            content = Regex.Replace(content, @"(->|::)([^\(\)\s]+)", 
                match => "." + Studly(match.Groups[2].Value));
            content = content.Replace("$this.", string.Empty)
                .Replace('\'', '"').Replace("$", string.Empty)
                .Replace("\"\"", "string.Empty")
                .Replace("RoleRepository.NewPermission", "privilege.AddPermission")
                .Replace("RoleRepository.NewRole", "privilege.AddRole")
                .Replace("Option.Group", "option.AddGroup")
                .Replace("Model.CREATED_AT", "MigrationTable.COLUMN_CREATED_AT")
                .Replace("\"created_at\"", "MigrationTable.COLUMN_CREATED_AT")
                .Replace("\"updated_at\"", "MigrationTable.COLUMN_UPDATED_AT");

            content = Regex.Replace(content, @"class\s+(\w+)([^\{]*)", match => {
                var name = Studly(match.Groups[1].Value);
                var impl = match.Groups[2].Value.Trim();
                if (string.IsNullOrEmpty(impl))
                {
                    return "class " + name;
                }
                var isFirst = true;
                if (impl.Contains("extends"))
                {
                    isFirst = false;
                    impl.Replace("extends", ":");
                }
                if (impl.Contains("implements"))
                {
                    impl.Replace("implements", isFirst ? ":" : ",");
                }
                if (impl.Contains("Migration"))
                {
                    var service = "IDatabase db";
                    if (content.Contains("privilege."))
                    {
                        service += ", IPrivilegeManager privilege";
                    }
                    if (content.Contains("option."))
                    {
                        service += ", IGlobeOption option";
                    }
                    return $"class {name}({service}) : Migration(db)";
                }
                return $"class {name} {impl}";
            });

            content = Regex.Replace(content, @"Append\((\w+)\.TableName\(\),.+?\{", match => {
                var entity = match.Groups[1].Value;
                if (entity.EndsWith("Model"))
                {
                    entity = entity[0..^5] + "Entity";
                }
                return $"Append<{entity}>(table => {{";
            });

            content = Regex.Replace(content, @"const\s+([^;]+);", match => {
                if (match.Groups[1].Value.Contains('"'))
                {
                    return $"public const string {match.Groups[1].Value};";
                }
                return $"public const int {match.Groups[1].Value};";
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

        public void UpgradeEntity(string projectRootFolder)
        {
            foreach (var item in Directory.GetDirectories(projectRootFolder))
            {
                if (Path.GetFileName(item) == "Gzo")
                {
                    continue;
                }
                UpgradeModuleEntity(item);
            }
        }

        private void UpgradeModuleEntity(string moduleFolder)
        {
            var encoding = new UTF8Encoding(false);
            var moduleName = Path.GetFileName(moduleFolder);
            var tableRegex = new Regex(@"internal const string ND_TABLE_NAME.+?;");
            var columnRegex = new Regex(@"\[Column.+?\]");
            var tableItems = new List<string>();
            foreach(var item in Directory.GetFiles(moduleFolder, "*Entity.cs", SearchOption.AllDirectories))
            {
                var content = File.ReadAllText(item);
                content = content.Replace("using NPoco;", string.Empty)
                    .Replace("[TableName(ND_TABLE_NAME)]", string.Empty);
                content = tableRegex.Replace(content, string.Empty);
                content = columnRegex.Replace(content, string.Empty);
                File.WriteAllText(item, content, encoding);
                tableItems.Add(Path.GetFileName(item)[..^9]);
            }
            if (tableItems.Count == 0)
            {
                return;
            }
            var contextName = moduleName + "Context";
            var sb  = new StringBuilder();
            sb.AppendLine("using Microsoft.EntityFrameworkCore;")
               .AppendLine($"using NetDream.Modules.{moduleName}.Entities;")
               .AppendLine($"using NetDream.Modules.{moduleName}.Migrations;")
               .AppendLine()
               .AppendLine($"namespace NetDream.Modules.{moduleName}")
               .AppendLine("{")
               .AppendLine($"    public class {contextName}(DbContextOptions<{contextName}> options): DbContext(options)")
               .AppendLine("    {");
            foreach (var item in tableItems)
            {
                sb.AppendLine($"        public DbSet<{item}Entity> {item}s {{get; set; }}");
            }
            sb.AppendLine("        protected override void OnModelCreating(ModelBuilder modelBuilder)")
                .AppendLine("        {");
            foreach (var item in tableItems)
            {
                sb.AppendLine($"            modelBuilder.ApplyConfiguration(new {item}EntityTypeConfiguration());");
            }
            sb.AppendLine("            base.OnModelCreating(modelBuilder);")
                .AppendLine("        }").
                AppendLine("    }")
               .AppendLine("}");
            File.WriteAllText(Path.Combine(moduleFolder, contextName + ".cs"), sb.ToString(), encoding);
            foreach (var item in tableItems)
            {
                sb.Clear();
                sb.AppendLine("using Microsoft.EntityFrameworkCore;")
               .AppendLine("using Microsoft.EntityFrameworkCore.Metadata.Builders;")
               .AppendLine($"using NetDream.Modules.{moduleName}.Entities;")
               .AppendLine()
               .AppendLine($"namespace NetDream.Modules.{moduleName}.Migrations")
               .AppendLine("{")
               .AppendLine($"    public class {item}EntityTypeConfiguration : IEntityTypeConfiguration<{item}Entity>")
               .AppendLine("    {")
               .AppendLine($"        public void Configure(EntityTypeBuilder<{item}Entity> builder)")
               .AppendLine("        {")
               .AppendLine($"            builder.ToTable(\"{item}\", table => table.HasComment(\"\"));")
               .AppendLine($"            builder.HasKey(table => table.Id);")
                .AppendLine("        }")
                .AppendLine("    }")
                .AppendLine("}");
                File.WriteAllText(Path.Combine(moduleFolder, "Migrations", $"{item}EntityTypeConfiguration.cs"), sb.ToString(), encoding);
            }
        }
    }
}
