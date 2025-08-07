using MediatR;
using NetDream.Modules.UserAccount.Forms;
using NetDream.Modules.UserAccount.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Notifications;
using NetDream.Shared.Providers;
using NetDream.Shared.Repositories;
using System;
using System.Linq;

namespace NetDream.Modules.UserAccount.Repositories
{
    public class AccountRepository(UserContext db, 
        IClientContext client,
        IUserRepository repository, IFundAccount account, IMediator mediator)
    {
        public IPage<AccountLogListItem> LogList(LogQueryForm form)
        {
            var items = db.AccountLogs.Search(form.Keywords, "remark")
                .When(form.User > 0, i => i.UserId == form.User)
                .When(form.Type > 0, i => i.Type == form.Type)
                .OrderByDescending(i => i.Id)
                .ToPage(form, i => i.SelectAs());
            repository.Include(items.Items);
            return items;
        }

        public IPage<ActionLogListItem> ActionLog(LogQueryForm form)
        {
            var items = db.ActionLogs
                .When(form.User > 0, i => i.UserId == form.User)
                .Search(form.Keywords, "ip")
                .OrderByDescending(i => i.Id)
                .ToPage(form, i => i.SelectAs());
            repository.Include(items.Items);
            return items;
        }

        public IPage<AdminLogListItem> AdminLog(LogQueryForm form)
        {
            var items = db.AdminLogs
                .Search(form.Keywords, "action", "remark")
                .When(form.User > 0, i => i.UserId == form.User)
                .OrderByDescending(i => i.Id)
                .ToPage(form, i => i.SelectAs());
            repository.Include(items.Items);
            return items;
        }

        /// <summary>
        /// 充值账户
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="money"></param>
        /// <param name="remark"></param>
        /// <param name="type"></param>
        /// <exception cref="Exception"></exception>
        public IOperationResult Recharge(RechargeForm data)
        {
            data.Money = Math.Abs(data.Money);
            if (data.Money <= 0)
            {
                return OperationResult.Fail("金额输入不正确");
            }
            var op = account.Trading(FundChangeRequest.Create(data.User,
                data.IsMinus ? -data.Money : data.Money,
                data.IsMinus ? "扣除" : "充值",
                FundOperateType.Recharge,
                client.UserId
                ));
            var res = op.Commit();
            if (!res.Succeeded)
            {
                return res;
            }
            mediator.Publish(ManageAction.Create(client, "user_recharge",
                string.Format("{0}金额：{1}", data.IsMinus ?  "扣除" : "充值", data.Money)
                , ModuleTargetType.UserRecharge, data.User));
            return OperationResult.Ok();
        }
    }
}
