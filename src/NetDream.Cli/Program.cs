using NetDream.Cli.Models;
using NetDream.Modules.Gzo.Repositories;
using NPoco;
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
            var line = Console.ReadLine();
            if (!int.TryParse(line, out var mode) || mode != 1)
            {
                Console.WriteLine("Input error");
                Console.ReadKey();
                return;
            }
            Console.Write("Please select save folder:");
            var workspace = Console.ReadLine();
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
            Console.ReadKey();
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

        static void GenerateEntity(string dbPrefix, string workspace)
        {
            var binFolder = AppDomain.CurrentDomain.BaseDirectory;
            var configFile = Path.GetFullPath(Path.Combine(binFolder, "../../../../NetDream.Web/appsettings.json"));
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