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
            if (args.Length == 0)
            {
                Console.WriteLine("-g Generate Database Entities");
                return;
            }
            switch (args[0])
            {
                case "-g":
                    GenerateEntity(args);
                    break;
            }
        }

        static void GenerateEntity(string[] args)
        {
            var binFolder = AppDomain.CurrentDomain.BaseDirectory;
            var workspace = Environment.CurrentDirectory;
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
            //Console.WriteLine("workspace: " + workspace);
            //Console.WriteLine("config: " + configFile);
            Console.WriteLine("config: " + config.ConnectionStrings?.Default);
            Console.WriteLine("Generate Model ...");
            var repository = new GzoRepository(db)
            {
                Schema = connectString
            };
            Console.WriteLine($"schema: {repository.Schema}");
            repository.BatchGenerateModel(args.Length > 1 ? args[1] : string.Empty, workspace);
            Console.WriteLine("Generate Model Finished");
        }
    }
}