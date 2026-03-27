using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.Wallet.Repositories;
using NetDream.Shared.Interfaces;

namespace NetDream.Modules.Wallet
{
    public static class Extension
    {
        public static void ProvideWalletRepositories(this IServiceCollection service)
        {
            service.AddScoped<IWallet, WalletRepository>();
        }
    }
}
