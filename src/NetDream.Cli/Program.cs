using NetDream.Cli.Models;
using NetDream.Modules.Gzo.Repositories;
using NPoco;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace NetDream.Cli
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                RunWidthArgs(args);
                return;
            }
            // Console.WriteLine("-g Generate Database Entities");
            Console.WriteLine("Please select mode:");
            Console.WriteLine("1. Generate Database Entities");
            Console.Write("input number:");
            var index = ReadInt();
            if (index != 1)
            {
                Console.WriteLine("Input error");
                Console.ReadKey();
                return;
            }
            label:
            Console.WriteLine("Please select save folder:");
            var i = 0;
            var items = LoadModuleFolder();
            foreach (var item in items)
            {
                Console.WriteLine(string.Format("{0}. {1}/Entities", ++ i, Path.GetFileName(item)));
            }
            Console.Write("input number or folder:");
            var workspace = Console.ReadLine();
            if (int.TryParse(workspace, out index) && index > 0 && index <= items.Count)
            {
                workspace = Path.Combine(items[index - 1], "Entities");
            }
            if (string.IsNullOrWhiteSpace(workspace))
            {
                Console.WriteLine("Input error");
                Console.ReadKey();
                return;
            }
            Console.Write("Please input table prefix:");
            var prefix = Console.ReadLine();
            GenerateEntity(prefix is null ? string.Empty : prefix, workspace);
            Console.WriteLine("Generate finished!");
            Console.Write("Continue Generate other: Y/N");
            if (Console.ReadKey().Key == ConsoleKey.Y)
            {
                goto label;
            }

        }

        static int ReadInt()
        {
            var line = Console.ReadLine();
            if (!int.TryParse(line, out var index))
            {
                return 0;
            }
            return index;
        }

        static void RunWidthArgs(string[] args)
        {
            switch (args[0])
            {
                case "-g":
                    GenerateEntity(args.Length > 1 ? args[1] : string.Empty, Environment.CurrentDirectory);
                    break;
            }
        }

        static IList<string> LoadModuleFolder()
        {
            return Directory.GetDirectories(
                Path.Combine(ProjectRootFolder, "Modules")
            );
        }

        static string ProjectRootFolder => Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../../"));
        static string ModuleRootFolder => Path.Combine(ProjectRootFolder, "Modules");

        static void GenerateEntity(string dbPrefix, string workspace)
        {
            var configFile = Path.Combine(ProjectRootFolder, "NetDream.Web/appsettings.json");
            var config = JsonSerializer.Deserialize<Configuration>(File.ReadAllText(configFile));
            if (string.IsNullOrEmpty(config?.ConnectionStrings.Default))
            {
                Console.WriteLine($"config is error [{configFile}]");
                return;
            }
            string connectString = config.ConnectionStrings.Default;
            using var db = new Database(connectString, DatabaseType.MySQL, MySql.Data.MySqlClient.MySqlClientFactory.Instance);

            //Console.WriteLine("bin folder: " + binFolder);
            Console.WriteLine("workspace: " + workspace);
            //Console.WriteLine("config: " + configFile);
            Console.WriteLine("config: " + config.ConnectionStrings?.Default);
            Console.WriteLine("Generate Model ...");
            var repository = new GzoRepository(db)
            {
                Schema = connectString
            };
            Console.WriteLine($"schema: {repository.Schema}");
            repository.BatchGenerateModel(dbPrefix, workspace);
            Console.WriteLine("Generate Model Finished");
        }
    }
}