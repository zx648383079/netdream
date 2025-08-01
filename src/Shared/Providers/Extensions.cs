﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Interfaces.Forms;
using NetDream.Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace NetDream.Shared.Providers
{
    public static class QueryExtensions
    {

        [DbFunction("DATE_FORMAT", "")]
        public static string DateFormat(DateTime date, string format)
            => throw new NotSupportedException();

        

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
            return [.. data.Select(i => i.CopyTo<TSource>())];
        }

        public static IPage<TSource> CopyTo<FSource, TSource>(this IPage<FSource> data)
            where FSource : class
            where TSource : class, new()
        {
            return new Page<TSource>(data)
            {
                Items = data.Items.Select(i => i.CopyTo<TSource>()).ToArray()
            };
        }

        public static IPage<TSource> ConvertTo<FSource, TSource>(this IPage<FSource> data)
            where FSource : TSource
        {
            return new Page<TSource>(data)
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

        public static IQueryable<TSource> Select<TSource, TProperty>(this IQueryable<TSource> source, string[] keys)
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
            where TTarget : class
        {
            var type = typeof(TSource);
            var target = typeof(TTarget);
            var srcKeys = type.GetProperties().ToDictionary(i => i.Name, i => i); 
            var thisArg = Expression.Parameter(type);
            var lambda = Expression.Lambda<Func<TSource, TTarget>>(Expression.MemberInit(
                    Expression.New(target.GetConstructor([])),
                    target.GetProperties()
                    .Where(i => srcKeys.ContainsKey(i.Name))
                    .Select(i => {
                        return Expression.Bind(
                            i,
                            Expression.Property(thisArg, srcKeys[i.Name])
                        );
                    })
                ), thisArg);
            return source.Select(lambda);
        }

        public static IOrderedQueryable<TSource> OrderBy<TSource, TProperty>(this IQueryable<TSource> source, string sort, string desc)
        {
            return source.OrderBy<TSource, TProperty>(sort, desc.Equals("desc", StringComparison.InvariantCultureIgnoreCase));
        }

        public static IOrderedQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> source, QueryForm form)
        {
            return OrderBy<TSource, int>(source, form.Sort, form.Order);
        }

        public static IOrderedQueryable<TSource> OrderBy<TSource, TProperty>(this IQueryable<TSource> source, string sort, bool isDesc)
        {
            var methodName = isDesc ? "OrderByDescending" : "OrderBy";
            var type = typeof(TSource);
            var memberProperty = type.GetProperty(StrHelper.Studly(sort))!;
            if (memberProperty is null)
            {
                throw new ArgumentNullException(nameof(memberProperty));
            }
            var thisArg = Expression.Parameter(type);
            var lambda = Expression.Lambda<Func<TSource, TProperty>>(Expression.Property(thisArg, memberProperty), thisArg);
            var method = GetTMethod(typeof(Queryable), methodName, 2, type, memberProperty.PropertyType);
            var res = method?.Invoke(null, [source, lambda]);
            return (IOrderedQueryable<TSource>)res!;
        }
        /// <summary>
        /// 获取泛型方法
        /// </summary>
        /// <param name="type">类</param>
        /// <param name="methodName">方法的名字</param>
        /// <param name="parameterCount">方法参数的个数</param>
        /// <param name="argsT">方法泛型的每个类型</param>
        /// <returns></returns>
        private static MethodInfo? GetTMethod(Type type, string methodName,
            int parameterCount, params Type[] argsT)
        {
            var methodInfo = type
                .GetMethods()
                .First(m => m.Name == methodName && m.GetParameters().Length == parameterCount);
            return methodInfo?.MakeGenericMethod(argsT);
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
            var func = GetTMethod(typeof(Enumerable), "Contains", 2, property.PropertyType);
            var lambda = Expression.Lambda<Func<TSource, bool>>(Expression.Call(null, func, Expression.Constant(items), propertyEx), thisArg);
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
        public static IPage<TTarget> ToPage<TSource, TTarget>(this IQueryable<TSource> source, int page, Func<IQueryable<TSource>, IQueryable<TTarget>> cb)
        {
            return source.ToPage(new PaginationForm(page), cb);
        }

        public static IPage<TSource> ToPage<TSource>(this IQueryable<TSource> source, IPaginationForm pagination)
        {
            var res = new Page<TSource>(source.Count(), pagination);
            if (!res.IsEmpty)
            {
                res.Items = source.Skip(res.ItemsOffset).Take(res.ItemsPerPage)
                    .ToArray();
            }
            return res;
        }

        public static IPage<TTarget> ToPage<TSource, TTarget>(this IQueryable<TSource> source, IPaginationForm pagination)
            where TSource : class
            where TTarget : class, new()
        {
            var res = new Page<TTarget>(source.Count(), pagination);
            if (!res.IsEmpty)
            {
                res.Items = source.Skip(res.ItemsOffset).Take(res.ItemsPerPage)
                    .ToArray().CopyTo<TSource, TTarget>();
            }
            return res;
        }

        public static IPage<TTarget> ToPage<TSource, TTarget>(this IQueryable<TSource> source, PaginationForm pagination, Func<IQueryable<TSource>, IQueryable<TTarget>> cb)
        {
            var res = new Page<TTarget>(source.Count(), pagination);
            if (!res.IsEmpty)
            {
                res.Items = cb.Invoke(source).Skip(res.ItemsOffset).Take(res.ItemsPerPage)
                    .ToArray();
            }
            return res;
        }

        public static TResult[] Pluck<TSource, TResult>(
            this IQueryable<TSource> source,
            Expression<Func<TSource, TResult>> selector)
        {
            return source.Select(selector).ToArray();
        }
        public static TResult? Value<TSource, TResult>(
            this IQueryable<TSource> source,
            Expression<Func<TSource, TResult>> selector)
        {
            return source.Select(selector).FirstOrDefault();
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
