using NetDream.Modules.Auth.Entities;
using NetDream.Modules.Auth.Models;
using NetDream.Shared.Extensions;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Migrations;
using NPoco;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Auth.Repositories
{
    public class RelationshipRepository(IDatabase db, IClientContext client)
    {
        public const byte TYPE_FOLLOWING = 1; // 关注
        public const byte TYPE_BLOCKING = 5; // 屏蔽用户

        /**
         * 判断关系是
         * @param int me
         * @param int user
         * @param int type
         * @return bool
         */
        public bool Is(int me, int user, int type)
        {
            return db.FindCount<UserRelationshipEntity>("user_id=@0 and link_id=@1 and type=@2",
                me, user, type) > 0;
        }

        public int Count(int user, int type)
        {
            return db.FindCount<UserRelationshipEntity>("user_id=@0 and type=@1",
                user, type);
        }

        public int FollowingCount(int user)
        {
            return Count(user, TYPE_FOLLOWING);
        }

        public int FollowerCount(int user)
        {
            return db.FindCount<UserRelationshipEntity>("link_id=@0 and type=@1",
                user, TYPE_FOLLOWING);
        }

        /**
         * 添加
         * @param int user
         * @param int type
         * @return void
         * @throws \Exception
         */
        public void Bind(int user, int type)
        {
            var me = client.UserId;
            if (me == user)
            {
                throw new Exception("can link self");
            }
            var log = db.First<UserRelationshipEntity>("user_id=@0 and link_id=@1",
                me, user);
            if (log is null)
            {
                db.Insert(new UserRelationshipEntity()
                {
                    UserId = me,
                    LinkId = user,
                    Type = type,
                    CreatedAt = client.Now
                });
                return;
            }
            if (log.Type == type)
            {
                return;
            }
            db.Update<UserRelationshipEntity>(
                new Sql().Where("user_id=@0 and link_id=@1",
                me, user),
                new()
                {
                    {"type", type},
                    {MigrationTable.COLUMN_CREATED_AT, client.Now}
                }
            );
        }

        /**
         * 取消操作
         * @param int user
         * @param int type
         * @return void
         * @throws \Exception
         */
        public void Unbind(int user, int type)
        {
            var me = client.UserId;
            if (me == user)
            {
                return;
            }
            db.DeleteWhere<UserRelationshipEntity>("user_id=@0 and link_id=@1 and type=@2",
                me, user, type);
        }

        /**
         * 变更为
         * @param int user
         * @param int type
         * @return int // {0: 取消，1: 更新为，2：新增}
         * @throws \Exception
         */
        public int Toggle(int user, int type)
        {
            var me = client.UserId;
            if (me == user)
            {
                throw new Exception("can link self");
            }
            var log = db.First<UserRelationshipEntity>("user_id=@0 and link_id=@1",
                 me, user);
            if (log is null)
            {
                db.Insert(new UserRelationshipEntity()
                {
                    UserId = me,
                    LinkId = user,
                    Type = type,
                    CreatedAt = client.Now
                });
                return 2;
            }
            if (log.Type == type)
            {
                db.DeleteWhere<UserRelationshipEntity>("user_id=@0 and link_id=@1",
                me, user);
                return 0;
            }
            db.Update<UserRelationshipEntity>(
                new Sql().Where("user_id=@0 and link_id=@1",
                me, user),
                new()
                {
                    {"type", type},
                    {MigrationTable.COLUMN_CREATED_AT, client.Now}
                }
            );
            return 1;
        }

        /**
         * 获取某一类型的双方关注状态
         * @param int user
         * @param int type
         * @return int  0 未关注 1 已关注，当对方未关注 2 已互相关注
         * @throws \Exception
         */
        public int TypeStatus(int user, int type)
        {
            var me = client.UserId;
            if (me == user)
            {
                return 0;
            }
            if (!Is(me, user, type))
            {
                return 0;
            }
            return UserAlsoIs(me, user, type) ? 2 : 1;
        }

        public bool UserAlsoIs(int me, int user, int type)
        {
            return Is(user, me, type);
        }

        /**
         * 获取用户的所有关系
         * @param int me
         * @param int user
         * @return int[]
         */
        public UserRelationship Relationship(int me, int user)
        {
            var res = new UserRelationship();
            if (me == user)
            {
                return res;
            }
            var log = db.First<UserRelationshipEntity>("user_id=@0 and link_id=@1",
                 me, user); 
            if (log is null)
            {
                return res;
            }
            if (log.Type == TYPE_BLOCKING)
            {
                res.MarkStatus = 1;
                return res;
            }
            if (log.Type == TYPE_FOLLOWING)
            {
                res.FollowStatus = UserAlsoIs(me, user, log.Type) ? 2 : 1;
                return res;
            }
            return res;
        }

        public bool ContainsRelationship(string[] keys)
        {
            return keys.Contains("follow_status") || keys.Contains("mark_status");
        }
    }
}
