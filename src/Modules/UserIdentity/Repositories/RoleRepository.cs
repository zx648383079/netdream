using MediatR;
using Microsoft.EntityFrameworkCore;
using NetDream.Modules.UserIdentity.Models;
using NetDream.Modules.UserIdentity.Entities;
using NetDream.Modules.UserIdentity.Forms;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Providers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NetDream.Shared.Repositories;
using NetDream.Shared.Notifications;
using NetDream.Shared.Models;

namespace NetDream.Modules.UserIdentity.Repositories
{
    public class RoleRepository(IdentityContext db, IClientContext client, IMediator mediator)
    {
        public IPage<RoleEntity> RoleList(QueryForm form)
        {
            return db.Roles.Search(form.Keywords, "name")
                .OrderByDescending(i => i.Id)
                .ToPage(form);
        }

        public IOperationResult<RoleModel> RoleGet(int id)
        {
            var model = db.Roles.Where(i => i.Id == id).SingleOrDefault();
            if (model == null)
            {
                return OperationResult.Fail<RoleModel>("数据错误");
            }
            var res = model.CopyTo<RoleModel>();
            res.Permissions = db.RolePermissions.Where(i => i.RoleId == model.Id)
                .Pluck(i => i.PermissionId);
            return OperationResult.Ok(res);
        }

        public void BindingUser(int user, int[] roles)
        {
            var (add, _, remove) = ModelHelper.SplitId(roles,
                db.UserRoles.Where(i => i.UserId == user)
                .Select(i => i.RoleId).ToArray()
            );
            if (remove.Count > 0)
            {
                db.UserRoles.Where(i => i.UserId == user && remove.Contains(i.RoleId)).ExecuteDelete();
            }
            if (add.Count > 0)
            {
                db.UserRoles.AddRange(add.Select(i => new UserRoleEntity()
                {
                    RoleId = i,
                    UserId = user,
                }));
                db.SaveChanges();
            }
        }
        public IOperationResult<RoleEntity> RoleSave(RoleForm data)
        {
            if (string.IsNullOrWhiteSpace(data.Name))
            {
                return OperationResult<RoleEntity>.Fail("请输入角色名");
            }
            var count = db.Roles.Where(i => i.Id != data.Id && i.Name == data.Name).Any();
            if (count)
            {
                return OperationResult<RoleEntity>.Fail("已存在角色");
            }
            var model = new RoleEntity
            {
                Name = data.Name,
                Id = data.Id,
                Description = data.Description,
                DisplayName = data.DisplayName
            };
            db.Roles.Save(model);
            BindRolePermission(model.Id, data.Permissions);
            mediator.Publish(ManageAction.Create(client, 
                "role_edit", 
                string.Empty, ModuleTargetType.Role, model.Id));
            return OperationResult.Ok(model);
        }

        private void BindRolePermission(int roleId, params int[] permissions)
        {
            var (add, _, remove) = ModelHelper.SplitId(permissions,
                db.RolePermissions.Where(i => i.RoleId == roleId).Select(i => i.PermissionId).ToArray());
            if (remove.Count > 0)
            {
                db.RolePermissions.Where(i => i.RoleId == roleId && remove.Contains(i.PermissionId)).ExecuteDelete();
            }
            if (add.Count > 0)
            {
                db.RolePermissions.AddRange(add.Select(i => new RolePermissionEntity()
                {
                    PermissionId = i,
                    RoleId = roleId,
                }));
                db.SaveChanges();
            }
        }


        /// <summary>
        /// 新增角色和权限
        /// </summary>
        /// <param name="name"></param>
        /// <param name="display_name"></param>
        /// <param name="permission"></param>
        /// <returns></returns>
        public int NewRole(string name, string display_name,
            Dictionary<string, string> permission)
        {
            var id = db.Roles.Where(i => i.Name == name).Select(i => i.Id).Single();
            var existBind = Array.Empty<int>();
            if (id < 1)
            {
                var role = new RoleEntity()
                {
                    Name = name,
                    DisplayName = display_name,
                };
                db.Roles.Save(role, client.Now);
                db.SaveChanges();
                id = role.Id;
            }
            else if(permission.Count > 0) 
            {
                existBind = db.RolePermissions.Where(i => i.RoleId == id)
                    .Select(i => i.PermissionId).ToArray();
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
                db.RolePermissions.AddRange(bindId);
                db.SaveChanges();
            }
            return id;
        }

        /// <summary>
        /// 新增权限
        /// </summary>
        /// <param name="permission"></param>
        /// <returns></returns>
        public int[] NewPermission(Dictionary<string, string> permission)
        {
            if (permission.Count == 0)
            {
                return [];
            }
            var idItems = new List<int>();
            var items = db.Permissions.Where(i => permission.Keys.Contains(i.Name))
                .Select(i => new {i.Name, i.Id})
                .ToDictionary(i => i.Name, i => i.Id);
            var pid = 0;
            foreach (var item in permission)
            {
                if (!items.ContainsKey(item.Key))
                {
                    var model = new PermissionEntity()
                    {
                        Name = item.Key,
                        DisplayName = item.Value,
                    };
                    db.Permissions.Save(model, client.Now);
                    db.SaveChanges();
                    pid = model.Id;
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

        /// <summary>
        /// 获取用户的角色和权限
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserRoleResult UserRolePermission(int userId)
        {
            var roleId = db.UserRoles.Where(i => i.UserId == userId)
                .Select(i => i.RoleId).ToArray();
            if (roleId.Length == 0)
            {
                return new UserRoleResult();
            }

            var roles = db.Roles.Where(i => roleId.Contains(i.Id))
                .Select(i => i.Name).ToArray();
            var role = db.Roles.Where(i => i.Id == roleId.Min()).Single();
            if (roles.Contains(IdentityRepository.Administrator))
            {
                var permissions = db.Permissions.Select(i => i.Name).ToArray();
                return new UserRoleResult()
                {
                    Role = role,
                    Roles = [..roles],
                    Permissions = [..permissions]
                };
            }
            var permissionId = db.RolePermissions.Where(i => roleId.Contains(i.RoleId))
                .Select(i => i.PermissionId).ToArray();
            if (permissionId.Length > 0)
            {
                var permissions = db.Permissions.Where(i => permissionId.Contains(i.Id))
                    .Select(i => i.Name).ToArray();
                return new UserRoleResult()
                {
                    Role = role,
                    Roles = [.. roles],
                    Permissions = [.. permissions]
                };
            }
            return new UserRoleResult()
            {
                Role = role,
                Roles = [.. roles],
            };
        }

        public PermissionEntity[] RolePermissions(int roleId)
        {
            var permissionId = db.RolePermissions.Where(i => i.RoleId == roleId)
                .Select(i => i.PermissionId).ToArray();
            if (permissionId.Length > 0)
            {
                return db.Permissions.Where(i => permissionId.Contains(i.Id))
                    .ToArray();
            }
            return [];
        }

        public IOperationResult<PermissionEntity> PermissionSave(PermissionForm data)
        {
            if (string.IsNullOrWhiteSpace(data.Name))
            {
                return OperationResult<PermissionEntity>.Fail("请输入权限名");
            }
            var count = db.Permissions.Where(i => i.Id != data.Id && i.Name == data.Name).Any();

            if (count)
            {
                return OperationResult<PermissionEntity>.Fail("已存在权限");
            }
            var model = new PermissionEntity()
            {
                Id = data.Id,
                Name = data.Name,
                DisplayName = data.DisplayName,
                Description = data.Description,
            };
            db.Permissions.Save(model, client.Now);
            db.SaveChanges();
            mediator.Publish(ManageAction.Create(client, 
                "permission_edit", model.Name, ModuleTargetType.RolePermission, model.Id));
            return OperationResult.Ok(model);
        }

        public void RoleRemove(int id)
        {
            var model = db.Roles.Where(i => i.Id == id).Single();
            if (model is null)
            {
                return;
            }
            db.Roles.Where(i => i.Id == id).ExecuteDelete();
            db.UserRoles.Where(i => i.RoleId == id).ExecuteDelete();
            db.RolePermissions.Where(i => i.RoleId == id).ExecuteDelete();
            db.SaveChanges();
            mediator.Publish(ManageAction.Create(client, "role_remove", model.Name, 
               ModuleTargetType.Role, model.Id));
        }

        public RoleEntity[] RoleAll()
        {
            return db.Roles.Select(i => new RoleEntity()
            {
                Id = i.Id,
                Name = i.Name,
                DisplayName = i.DisplayName,
            }).OrderBy(i => i.Id).ToArray();
        }

        public IPage<PermissionEntity> PermissionList(QueryForm form)
        {
            return db.Permissions.Search(form.Keywords, "name")
                .OrderByDescending(i => i.Id)
                .ToPage(form);
        }

        public IOperationResult<PermissionEntity> PermissionGet(int id)
        {
            var model = db.Permissions.Where(i => i.Id == id).SingleOrDefault();
            return OperationResult.OkOrFail(model, "数据错误");
        }

        public void PermissionRemove(int id)
        {
            db.Permissions.Where(i => i.Id == id).ExecuteDelete();
            db.SaveChanges();
        }

        public PermissionEntity[] PermissionAll()
        {
            return db.Permissions.Select(i => new PermissionEntity()
            {
                Id = i.Id,
                Name = i.Name,
                DisplayName = i.DisplayName,
            }).OrderBy(i => i.Id).ToArray();
        }

        public int[] GetByUser(int user)
        {
            return db.UserRoles.Where(i => i.UserId == user).Pluck(i => i.RoleId);
        }

    }
}
