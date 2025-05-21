using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Finance.Entities;
using NetDream.Modules.Finance.Forms;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System.Linq;

namespace NetDream.Modules.Finance.Repositories
{
    public class ChannelRepository(FinanceContext db, IClientContext client)
    {
        public ConsumptionChannelEntity[] All()
        {
            return db.Channel.Where(i => i.UserId == client.UserId)
                .OrderByDescending(i => i.Id).ToArray();
        }

        /**
         * 获取
         * @param int id
         * @return ConsumptionChannelModel
         * @throws Exception
         */
        public IOperationResult<ConsumptionChannelEntity> Get(int id)
        {
            var model = db.Channel.Where(i => i.UserId == client.UserId && i.Id == id)
                .FirstOrDefault();
            if (model is null)
            {
                return OperationResult.Fail<ConsumptionChannelEntity>("渠道不存在");
            }
            return OperationResult.Ok(model);
        }

        /**
         * 保存
         * @param array data
         * @return ConsumptionChannelModel
         * @throws Exception
         */
        public IOperationResult<ConsumptionChannelEntity> Save(ChannelForm data)
        {
            ConsumptionChannelEntity? model;
            if (data.Id > 0)
            {
                model = db.Channel.Where(i => i.UserId == client.UserId && i.Id == data.Id)
                .FirstOrDefault();
            }
            else
            {
                model = new();
            }
            if (model is null)
            {
                return OperationResult.Fail<ConsumptionChannelEntity>("渠道不存在");
            }
            model.Name = data.Name;
            model.UserId = client.UserId;
            db.Channel.Save(model, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        /**
         * 删除产品
         * @param int id
         * @return mixed
         */
        public void Remove(int id)
        {
            db.Channel.Where(i => i.UserId == client.UserId && i.Id == id)
                .ExecuteDelete();
            db.SaveChanges();
        }
    }
}
