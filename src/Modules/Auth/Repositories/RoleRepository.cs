using MediatR;
using NetDream.Modules.Auth.Entities;
using NetDream.Modules.Auth.Events;
using NetDream.Modules.Auth.Forms;
using NetDream.Modules.Auth.Models;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace NetDream.Modules.Auth.Repositories
{
    public class RoleRepository(AuthContext db, IClientContext client, IMediator mediator)
    {
        /**
         * @param array data
         * @param array permission
         * @return RoleModel
         * @throws Exception
         */
        public RoleEntity SaveRole(RoleForm data, params int[] permissions)
        {
            if (string.IsNullOrWhiteSpace(data.Name))
            {
                throw new Exception("请输入角色名");
            }
            var count = db.FindCount<RoleEntity>("id<>@0 and name=@1", data.Id, data.Name);
            if (count > 0)
            {
                throw new Exception("已存在角色");
            }
            var model = new RoleEntity
            {
                Name = data.Name,
                Id = data.Id,
                Description = data.Description,
                DisplayName = data.DisplayName
            };
            db.TrySave(model);
            BindRolePermission(model.Id, permissions);
            mediator.Publish(ManageAction.Create(client, 
                "role_edit", 
                string.Empty, ModuleModelType.TYPE_ROLE, model.Id));
            return model;
        }

        private void BindRolePermission(int roleId, params int[] permissions)
        {
            var (add, _, remove) = ModelHelper.SplitId(permissions,
                db.Pluck<int>(new Sql().Select("permission_id").From<RolePermissionEntity>(db)
                .Where("role_id=@0", roleId)));
            if (remove.Count > 0)
            {
                db.DeleteWhere<RolePermissionEntity>(
                    $"role_id={roleId} AND permission_id IN ({string.Join(',', remove)})");
            }
            if (add.Count > 0)
            {
                db.InsertBatch<RolePermissionEntity>(add.Select(i => new Dictionary<string, object>()
                {
                    {"permission_id", i},
                    {"role_id", roleId}
                }));
            }
        }


        /**
         * 新增角色和权限
         * @param name
         * @param display_name
         * @param array permission [name => display_name]
         * @throws Exception
         * @return integer
         */
        public int NewRole(string name, string display_name,
            Dictionary<string, string> permission)
        {
            var id = db.FindScalar<int, RoleEntity>("id", "name=@0", name);
            var existBind = Array.Empty<int>();
            if (id < 1)
            {
                id = (int)db.Insert(new RoleEntity()
                {
                    Name = name,
                    DisplayName = display_name,
                });
            }
            else if(permission.Count > 0) 
            {
                existBind = db.Pluck<int>(new Sql().Select("permission_id").From<RolePermissionEntity>(db)
                    .Where("role_id=@0", id),
                    "permission_id"
                    ).ToArray();
            }
            if (permission.Count == 0 || id <= 0)
            {
                return id;
            }
            var bindId = new List<RolePermissionEntity>();
            var permissionId = NewPermission(permission);
            foreach (var pid in permissionId)
            {
                if (!existBind.Contains(pid))
                {
                    bindId.Add(new()
                    {
                        RoleId = id,
                        PermissionId = pid
                    });
                }
            }
            if (bindId.Count > 0)
            {
                db.InsertBatch(bindId);
            }
            return id;
        }

        /**
         * 新增权限
         * @param array permission
         * @return array
         * @throws Exception
         */
        public int[] NewPermission(Dictionary<string, string> permission)
        {
            if (permission.Count == 0)
            {
                return [];
            }
            var idItems = new List<int>();
            var items = db.Pluck<string, int>(
                new Sql().Select("name", "id")
                .From<PermissionEntity>(db)
                .WhereIn("name", permission.Keys.ToArray()), "name", "id", false);
            var pid = 0;
            foreach (var item in permission)
            {
                if (!items.ContainsKey(item.Key))
                {
                    pid = (int)db.Insert(new PermissionEntity()
                    {
                        Name = item.Key,
                        DisplayName = item.Value,
                    });
                }
                else
                {
                    pid = items[item.Key];
                }
                if (pid > 0)
                {
                    idItems.Add(pid);
                }
            }
            return [..idItems];
        }

        /**
         * 获取用户的角色和权限
         * @param user_id
         * @return array [role => array, roles => string[], permissions => string[]]
         */
        public UserRole UserRolePermission(int userId)
        {
            var roleId = db.Pluck<int>(
                new Sql().Select("role_id")
                .From<UserRoleEntity>(db).Where("user_id=@0", userId)
            );
            if (roleId.Count == 0)
            {
                return new UserRole();
            }

            var roles = db.Pluck<string>(
                new Sql().Select("name")
                .From<RoleEntity>(db).WhereIn("id", roleId.ToArray())
             );
            var role = db.SingleById<RoleEntity>(roleId.Min());
            if (roles.Contains("administrator"))
            {
                var permissions = db.Pluck<string>(
                    new Sql().Select("name")
                    .From<PermissionEntity>(db)
                 );
                return new UserRole()
                {
                    Role = role,
                    Roles = [..roles],
                    Permissions = [..permissions]
                };
            }
            var permissionId = db.Pluck<int>(
                new Sql().Select("permission_id")
                .From<RolePermissionEntity>(db).WhereIn("role_id", roleId.ToArray())
             );
            if (permissionId.Count > 0)
            {
                var permissions = db.Pluck<string>(
                    new Sql().Select("name")
                    .From<PermissionEntity>(db)
                    .WhereIn("id", permissionId.ToArray())
                 );
                return new UserRole()
                {
                    Role = role,
                    Roles = [.. roles],
                    Permissions = [.. permissions]
                };
            }
            return new UserRole()
            {
                Role = role,
                Roles = [.. roles],
            };
        }

        public PermissionEntity[] RolePermissions(int roleId)
        {
            var permissionId = db.Pluck<int>(
                new Sql().Select("permission_id")
                .From<RolePermissionEntity>(db).Where("role_id=@0", roleId)
             );
            if (permissionId.Count > 0)
            {
                return db.Fetch<PermissionEntity>(
                    new Sql().Select("name")
                    .From<PermissionEntity>(db)
                    .WhereIn("id", permissionId.ToArray())
                 ).ToArray();
            }
            return [];
        }

        public PermissionEntity SavePermission(PermissionForm data)
        {
            if (string.IsNullOrWhiteSpace(data.Name))
            {
                throw new Exception("请输入权限名");
            }
            var count = db.FindCount<PermissionEntity>("id<>@0 and name=@1", data.Id, data.Name);

            if (count > 0)
            {
                throw new Exception("已存在权限");
            }
            var model = new PermissionEntity()
            {
                Id = data.Id,
                Name = data.Name,
                DisplayName = data.DisplayName,
                Description = data.Description,
            };
            db.TrySave(model);

            mediator.Publish(ManageAction.Create(client, 
                "permission_edit", model.Name, ModuleModelType.TYPE_ROLE_PERMISSION, model.Id));
            return model;
        }

        public void RemoveRole(int id)
        {
            var model = db.SingleById<RoleEntity>(id);
            if (model is null)
            {
                return;
            }
            db.DeleteById<RoleEntity>(id);
            db.DeleteWhere<UserRoleEntity>("role_id", id);
            db.DeleteWhere<RolePermissionEntity>("role_id", id);
            mediator.Publish(ManageAction.Create(client, "role_remove", model.Name, ModuleModelType.TYPE_ROLE, model.Id));
        }
    }
}
