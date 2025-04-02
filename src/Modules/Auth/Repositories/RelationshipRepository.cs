using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Auth.Entities;
using NetDream.Modules.Auth.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using System;
using System.Linq;

namespace NetDream.Modules.Auth.Repositories
{
    public class RelationshipRepository(AuthContext db, IClientContext client)
    {
        public const byte TYPE_FOLLOWING = 1; // 关注
        public const byte TYPE_BLOCKING = 5; // 屏蔽用户


        /// <summary>
        /// 判断关系是
        /// </summary>
        /// <param name="me"></param>
        /// <param name="user"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool Is(int me, int user, int type)
        {
            return db.Relationships.Where(i => i.UserId == me && i.LinkId == user && i.Type == type)
                .Any();
        }

        public static bool Is(AuthContext db, int me, int user, int type)
        {
            return db.Relationships.Where(i => i.UserId == me && i.LinkId == user && i.Type == type)
                .Any();
        }

        public int Count(int user, int type)
        {
            return db.Relationships.Where(i => i.UserId == user && i.Type == type).Count();
        }

        public static int FollowingCount(AuthContext db, int user)
        {
            return db.Relationships.Where(i => i.UserId == user && i.Type == TYPE_FOLLOWING).Count();
        }

        public static int FollowerCount(AuthContext db, int user)
        {
            return db.Relationships.Where(i => i.LinkId == user && i.Type == TYPE_FOLLOWING).Count();
        }

        /**
         * 添加
         * @param int user
         * @param int type
         * @return void
         * @throws \Exception
         */
        public IOperationResult Bind(int user, int type)
        {
            var me = client.UserId;
            if (me == user)
            {
                return OperationResult.Fail("can link self");
            }
            var log = db.Relationships.Where(i => i.UserId == me && i.LinkId == user).Single();
            if (log is null)
            {
                db.Relationships.Add(new RelationshipEntity()
                {
                    UserId = me,
                    LinkId = user,
                    Type = type,
                    CreatedAt = client.Now
                });
                db.SaveChanges();
                return OperationResult.Ok();
            }
            if (log.Type == type)
            {
                return OperationResult.Ok();
            }
            db.Relationships.Where(i => i.UserId == me && i.LinkId == user)
                .ExecuteUpdate(setters => 
                    setters.SetProperty(i => i.Type, type)
                    .SetProperty(i => i.CreatedAt, client.Now));
            return OperationResult.Ok();
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
            db.Relationships.Where(i => i.UserId == me && i.LinkId == user 
                && i.Type == type).ExecuteDelete();
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
            var log = db.Relationships.Where(i => i.UserId == me && i.LinkId == user).Single();
            if (log is null)
            {
                db.Relationships.Add(new RelationshipEntity()
                {
                    UserId = me,
                    LinkId = user,
                    Type = type,
                    CreatedAt = client.Now
                });
                db.SaveChanges();
                return 2;
            }
            if (log.Type == type)
            {
                db.Relationships.Where(i => i.UserId == me && i.LinkId == user).ExecuteDelete();
                return 0;
            }
            db.Relationships.Where(i => i.UserId == me && i.LinkId == user)
             .ExecuteUpdate(setters =>
                 setters.SetProperty(i => i.Type, type)
                 .SetProperty(i => i.CreatedAt, client.Now));
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
            var log = db.Relationships.Where(i => i.UserId == me && i.LinkId == user).Single(); 
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
