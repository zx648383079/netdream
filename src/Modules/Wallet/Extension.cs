using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.Wallet.Entities;
using NetDream.Modules.Wallet.Models;
using NetDream.Modules.Wallet.Repositories;
using NetDream.Shared.Interfaces;
using System.Linq;

namespace NetDream.Modules.Wallet
{
    public static class Extension
    {
        public static void ProvideWalletRepositories(this IServiceCollection service)
        {
            service.AddScoped<IWallet, WalletRepository>();
        }


        internal static IQueryable<AccountLogListItem> SelectAs(this IQueryable<AccountLogEntity> query)
        {
            return query.Select(i => new AccountLogListItem()
            {
                Id = i.Id,
                UserId = i.UserId,
                ItemId = i.ItemId,
                Type = i.Type,
                Money = i.Money,
                TotalMoney = i.TotalMoney,
                Credits = i.Credits,
                TotalCredits = i.TotalCredits,
                Status = i.Status,
                Remark = i.Remark,
                CreatedAt = i.CreatedAt,
            });
        }
    }
}
