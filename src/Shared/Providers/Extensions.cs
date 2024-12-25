using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;

namespace NetDream.Shared.Providers
{
    public static class QueryExtensions
    {

        public static T CopyTo<T>(this object data)
            where T : class, new()
        {
            var res = new T();
            var type = typeof(T);
            foreach (var item in data.GetType().GetProperties())
            {
                var property = type.GetProperty(item.Name);
                if (property is null)
                {
                    continue;
                }
                property.SetValue(res, item.GetValue(data));
            }
            return res;
        }
        public static TSource[] CopyTo<FSource, TSource>(this IEnumerable<FSource> data)
            where FSource : class
            where TSource : class, new()
        {
            return data.Select(i => i.CopyTo<TSource>()).ToArray();
        }

        public static IPage<TSource> CopyTo<FSource, TSource>(this IPage<FSource> data)
            where FSource : class
            where TSource : class, new()
        {
            return new Page<TSource>(data.TotalItems, data.CurrentPage, data.ItemsPerPage)
            {
                Items = data.Items.Select(i => i.CopyTo<TSource>()).ToArray()
            };
        }

        public static IPage<TSource> ConvertTo<FSource, TSource>(this IPage<FSource> data)
            where FSource : TSource
        {
            return new Page<TSource>(data.TotalItems, data.CurrentPage, data.ItemsPerPage)
            {
                Items = Array.ConvertAll(data.Items, i => (TSource)i)
            };
        }

        public static PropertyBuilder<TProperty> Column<TEntity, TProperty>(this EntityTypeBuilder<TEntity> builder, string key)
            where TEntity : class
        {
            var type = typeof(TEntity);
            var memberProperty = type.GetProperty(StrHelper.Studly(key))!;
            var thisArg = Expression.Parameter(type);
            var lambda = Expression.Lambda<Func<TEntity, TProperty>>(Expression.Property(thisArg, memberProperty), thisArg);
            return builder.Property(lambda).HasColumnName(key);
        }

        public static void Save<TSource>(this DbSet<TSource> db, TSource model)
            where TSource : class, IIdEntity
        {
            if (model.Id > 0)
            {
                db.Update(model);
            } else
            {
                db.Add(model);
            }
        }

        public static void Save<TSource>(this DbSet<TSource> db, TSource model, int timestamp)
            where TSource : class, IIdEntity, ICreatedEntity
        {
            if (model is ITimestampEntity o)
            {
                o.UpdatedAt = timestamp;
            }
            if (model.Id > 0)
            {
                db.Update(model);
            }
            else
            {
                model.CreatedAt = timestamp;
                db.Add(model);
            }
        }

        public static IQueryable<TSource> Select<TSource, TProperty>(this IQueryable<TSource> source, [MinLength(1)] params string[] keys)
        {
            var type = typeof(TSource);
            var thisArg = Expression.Parameter(type);
            var lambda = Expression.Lambda<Func<TSource, TSource>>(Expression.MemberInit(
                    Expression.New(type.GetConstructor([])),
                    keys.Select(key => {
                        var property = type.GetProperty(StrHelper.Studly(key));
                        return Expression.Bind(
                            property,
                            Expression.Property(thisArg, property)
                        );
                    })
                ), thisArg);
            return source.Select(lambda);
        }

        public static IQueryable<TTarget> Select<TSource, TTarget>(this IQueryable<TSource> source)
            where TTarget : class, TSource
        {
            var type = typeof(TSource);
            var target = typeof(TTarget);
            var thisArg = Expression.Parameter(type);
            var lambda = Expression.Lambda<Func<TSource, TTarget>>(Expression.MemberInit(
                    Expression.New(target.GetConstructor([])),
                    type.GetProperties()
                    .Where(i => !i.GetType().IsArray && !i.GetType().IsClass)
                    .Select(i => {
                        return Expression.Bind(
                            i,
                            Expression.Property(thisArg, i)
                        );
                    })
                ), thisArg);
            return source.Select(lambda);
        }

        public static IQueryable<TSource> OrderBy<TSource, TProperty>(this IQueryable<TSource> source, string sort, string desc)
        {
            return source.OrderBy<TSource, TProperty>(sort, desc.Equals("desc", StringComparison.InvariantCultureIgnoreCase));
        }

        public static IQueryable<TSource> OrderBy<TSource, TProperty>(this IQueryable<TSource> source, string sort, bool isDesc)
        {
            var methodName = isDesc ? "OrderByDescending" : "OrderBy";
            var type = typeof(TSource);
            var memberProperty = type.GetProperty(StrHelper.Studly(sort))!;
            if (memberProperty is null)
            {
                return source;
            }
            var thisArg = Expression.Parameter(type);
            var lambda = Expression.Lambda<Func<TSource, TProperty>>(Expression.Property(thisArg, memberProperty), thisArg);
            var method = typeof(Queryable).GetMethod(methodName)!;
            return (IOrderedQueryable<TSource>)method.Invoke(null, [source, lambda])!;
        }

        public static IQueryable<TSource> Search<TSource>(this IQueryable<TSource> source, string keywords, [MinLength(1)] params string[] keys)
        {
            if (string.IsNullOrWhiteSpace(keywords))
            {
                return source;
            }
            var wordItems = SearchHelper.Split(keywords).Select(Expression.Constant).ToArray();
            var type = typeof(TSource);
            var func = typeof(string).GetMethod("Contains");
            var thisArg = Expression.Parameter(type);
            Expression? expression = null;
            foreach (var key in keys)
            {
                var property = type.GetProperty(StrHelper.Studly(key));
                if (property is null)
                {
                    continue;
                }
                var propertyEx = Expression.Property(thisArg, property);
                foreach (var item in wordItems)
                {
                    var exp = Expression.Call(propertyEx, func, item);
                    if (expression is null)
                    {
                        expression = exp;
                    } else
                    {
                        expression = Expression.OrElse(expression, exp);
                    }
                }
            }
            if (expression is null)
            {
                return source;
            }
            var lambda = Expression.Lambda<Func<TSource, bool>>(expression, thisArg);
            return source.Where(lambda);
        }
        public static IQueryable<TSource> Where<TSource>(this IQueryable<TSource> source, string key, object value)
        {
            var type = typeof(TSource);
            var thisArg = Expression.Parameter(type);
            var property = type.GetProperty(StrHelper.Studly(key));
            if (property is null)
            {
                return source;
            }
            var propertyEx = Expression.Property(thisArg, property);
            var lambda = Expression.Lambda<Func<TSource, bool>>(Expression.Equal(propertyEx, Expression.Constant(value)), thisArg);
            return source.Where(lambda);
        }

        public static IQueryable<TSource> WhereIn<TSource, TProperty>(this IQueryable<TSource> source, string key, IEnumerable<TProperty> items)
        {
            var type = typeof(TSource);
            var thisArg = Expression.Parameter(type);
            var property = type.GetProperty(StrHelper.Studly(key));
            if (property is null)
            {
                return source;
            }
            var propertyEx = Expression.Property(thisArg, property);
            var func = typeof(Enumerable).GetMethod("Contains");
            var lambda = Expression.Lambda<Func<TSource, bool>>(Expression.Call(Expression.Constant(items), func, propertyEx), thisArg);
            return source.Where(lambda);
        }
        public static IQueryable<TSource> When<TSource>(this IQueryable<TSource> source, bool isTure, Expression<Func<TSource, bool>> predicate)
        {
            if (!isTure)
            {
                return source;
            }
            return source.Where(predicate);
        }

        public static IQueryable<TSource> When<TSource>(this IQueryable<TSource> source, bool isTure, Expression<Func<TSource, bool>> truePredicate, Expression<Func<TSource, bool>> falsePredicate)
        {
            return source.Where(isTure ? truePredicate : falsePredicate);
        }

        public static IQueryable<TSource> When<TSource>(this IQueryable<TSource> source, string isTure, Expression<Func<TSource, bool>> predicate)
        {
            return source.When(!string.IsNullOrWhiteSpace(isTure), predicate);
        }
        public static IPage<TSource> ToPage<TSource>(this IQueryable<TSource> source, int page)
        {
            return source.ToPage(new PaginationForm(page));
        }
        public static IPage<TSource> ToPage<TSource>(this IQueryable<TSource> source, int page, Func<IQueryable<TSource>, IQueryable<TSource>> cb)
        {
            return source.ToPage(new PaginationForm(page), cb);
        }

        public static IPage<TSource> ToPage<TSource>(this IQueryable<TSource> source, PaginationForm pagination)
        {
            var res = new Page<TSource>(source.Count(), pagination);
            if (!res.IsEmpty)
            {
                res.Items = source.Skip(res.ItemsOffset).Take(res.ItemsPerPage)
                    .ToArray();
            }
            return res;
        }

        public static IPage<TSource> ToPage<TSource>(this IQueryable<TSource> source, PaginationForm pagination, Func<IQueryable<TSource>, IQueryable<TSource>> cb)
        {
            var res = new Page<TSource>(source.Count(), pagination);
            if (!res.IsEmpty)
            {
                res.Items = cb.Invoke(source).Skip(res.ItemsOffset).Take(res.ItemsPerPage)
                    .ToArray();
            }
            return res;
        }

        /// <summary>
        /// 批量更改数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db"></param>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <param name="allowCollumn"></param>
        /// <returns></returns>
        public static TSource? BatchToggle<TSource>(this DbSet<TSource> db, int id,
            string[] data, params string[] allowColumn)
            where TSource : class, IIdEntity
        {
            var model = db.Where(i => i.Id == id).Single();
            if (model is null)
            {
                return null;
            }
            var isUpdated = false;
            foreach (var key in data)
            {
                if (string.IsNullOrEmpty(key) || !allowColumn.Contains(key))
                {
                    continue;
                }
                var field = model.GetType().GetField(StrHelper.Studly(key));
                if (field is null)
                {
                    continue;
                }
                if (field.FieldType == typeof(bool))
                {
                    field.SetValue(model, !(bool)field.GetValue(model));
                }
                else
                {
                    field.SetValue(model, Convert.ChangeType(
                        (byte)field.GetValue(model) > 0 ? 0 : 1, field.FieldType));
                }
                isUpdated = true;
            }
            if (isUpdated)
            {
                db.Update(model);
            }
            return model;
        }
        /// <summary>
        /// 批量更改数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db"></param>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <param name="allowCollumn"></param>
        /// <returns></returns>
        public static TSource? BatchToggle<TSource>(this DbSet<TSource> db, int id,
            IDictionary<string, string> data, params string[] allowColumn)
            where TSource : class, IIdEntity
        {
            var model = db.Where(i => i.Id == id).Single();
            return BatchToggle(db, model, data, allowColumn);
        }

        public static TSource? BatchToggle<TSource>(this DbSet<TSource> db, TSource? model,
            IDictionary<string, string> data, params string[] allowColumn)
            where TSource : class, IIdEntity
        {
            if (model is null)
            {
                return null;
            }
            var isUpdated = false;
            foreach (var item in data)
            {
                if (Helpers.Validator.IsInt(item.Key))
                {
                    if (string.IsNullOrEmpty(item.Value) || !allowColumn.Contains(item.Value))
                    {
                        continue;
                    }
                    var field = model.GetType().GetField(StrHelper.Studly(item.Value));
                    if (field is null)
                    {
                        continue;
                    }
                    if (field.FieldType == typeof(bool))
                    {
                        field.SetValue(model, !(bool)field.GetValue(model));
                    }
                    else
                    {
                        field.SetValue(model, Convert.ChangeType(
                            (byte)field.GetValue(model) > 0 ? 0 : 1, field.FieldType));
                    }
                    isUpdated = true;
                }
                else if (string.IsNullOrEmpty(item.Key) || !allowColumn.Contains(item.Key))
                {
                    var field = model.GetType().GetField(StrHelper.Studly(item.Key));
                    if (field is null)
                    {
                        continue;
                    }
                    field.SetValue(model, Convert.ChangeType(item.Value, field.FieldType));
                    isUpdated = true;
                }
            }
            if (isUpdated)
            {
                db.Update(model);
            }
            return model;
        }

        /// <summary>
        /// 更新自增字段
        /// </summary>
        /// <param name="cb">(oldId, newId)</param>
        /// <param name="key"></param>
        public static void RefreshPk<TSource>(this DbSet<TSource> db, Action<int, int> cb)
            where TSource : class, IIdEntity
        {
            var data = db.OrderBy(i => i.Id).Select(i => i.Id).ToArray();
            var i = 1;
            foreach (var id in data)
            {
                if (id == i)
                {
                    i++;
                    continue;
                }
                db.Where(i => i.Id == id).ExecuteUpdate(setters => setters.SetProperty(i => i.Id, i));
                cb.Invoke(id, i);
                i++;
            }
            // db.Database.ExecuteSql($"ALTER TABLE [{nameof(TSource)}] AUTO_INCREMENT = {i};");
        }
    }
}
