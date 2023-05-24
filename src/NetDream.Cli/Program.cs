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
            var binFolder = AppDomain.CurrentDomain.BaseDirectory;
            var workspace = Environment.CurrentDirectory;

            var configFile = Path.GetFullPath(Path.Combine(binFolder, "../../../../NetDream.Web/appsettings.json"));
            var config = JsonSerializer.Deserialize<Configuration>(File.ReadAllText(configFile));
            if (config == null)
            {
                Console.WriteLine($"config is error [{configFile}]");
                return;
            }
            var db = new Database(config.ConnectionStrings.Default, DatabaseType.MySQL, MySql.Data.MySqlClient.MySqlClientFactory.Instance);

            //Console.WriteLine("bin folder: " + binFolder);
            //Console.WriteLine("workspace: " + workspace);
            //Console.WriteLine("config: " + configFile);
            Console.WriteLine("config: " + config?.ConnectionStrings?.Default);
            Console.WriteLine("Generate Model ...");
            var repository = new GzoRepository(db);
            repository.BatchGenerateModel(workspace);
            Console.WriteLine("Generate Model Finished");
            Console.ReadKey();
        }
    }
}