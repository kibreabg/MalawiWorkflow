﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;

namespace Chai.WorkflowManagment.CoreDomain.DataAccess
{
    public static class Dao
    {
        private static readonly IDictionary<Type, IDictionary<int, ArrayList>> Cache = new Dictionary<Type, IDictionary<int, ArrayList>>();

        private static void AddToCache(Type t, int key, object item)
        {
            if (item == null) return;
            if (!Cache.ContainsKey(t))
                Cache.Add(t, new Dictionary<int, ArrayList>());
            if (!Cache[t].ContainsKey(key))
                Cache[t].Add(key, new ArrayList());
            Cache[t][key].Add(item);
        }

        private static T GetFromCache<T>(Expression<Func<T, bool>> predictate, int key)
        {
            if (Cache.ContainsKey(typeof(T)))
                if (Cache[typeof(T)].ContainsKey(key))
                    return Cache[typeof(T)][key].Cast<T>().SingleOrDefault(predictate.Compile());
            return default(T);
        }

        public static void ResetCache()
        {
            foreach (var arrayList in Cache.Values)
            {
                arrayList.Clear();
            }
            Cache.Clear();
        }

        public static TResult Single<TSource, TResult>(int id, Expression<Func<TSource, TResult>> expression) where TSource : class,IEntity
        {
            using (var workspace = WorkspaceFactory.CreateReadOnly())
            {
                return workspace.Single(id, expression);
            }
        }

        public static T Single<T>(Expression<Func<T, bool>> predictate, params Expression<Func<T, object>>[] includes) where T : class
        {
            using (var workspace = WorkspaceFactory.CreateReadOnly())
            {
                var result = workspace.Single(predictate, includes);
                //AddToCache(typeof(T), ObjectCloner.DataHash(includes), result);
                return result;
            }
        }

        //public static T SingleWithCache<T>(Expression<Func<T, bool>> predictate, params Expression<Func<T, object>>[] includes) where T : class
        //{
        //    var ci = GetFromCache(predictate, ObjectCloner.DataHash(includes));
        //    if (ci != null) return ci;

        //    using (var workspace = WorkspaceFactory.CreateReadOnly())
        //    {
        //        var result = workspace.Single(predictate, includes);
        //        AddToCache(typeof(T), ObjectCloner.DataHash(includes), result);
        //        return result;
        //    }
        //}

        public static IEnumerable<string> Distinct<T>(Expression<Func<T, string>> expression) where T : class
        {
            using (var workspace = WorkspaceFactory.CreateReadOnly())
            {
                return workspace.Distinct(expression).ToList();
            }
        }

        public static IEnumerable<T> Query<T>(Expression<Func<T, bool>> predictate, params Expression<Func<T, object>>[] includes) where T : class
        {
            using (var workspace = WorkspaceFactory.CreateReadOnly())
            {
                return workspace.Query(predictate, includes).ToList();
            }
        }

        public static IEnumerable<T> Query<T>(params Expression<Func<T, object>>[] includes) where T : class
        {
            using (var workspace = WorkspaceFactory.CreateReadOnly())
            {
                return workspace.Query(null, includes).ToList();
            }
        }

        public static IEnumerable<TResult> Select<TSource, TResult>(Expression<Func<TSource, TResult>> expression,
                                                  Expression<Func<TSource, bool>> predictate) where TSource : class
        {
            using (var workspace = WorkspaceFactory.CreateReadOnly())
            {
                return workspace.Select(expression, predictate).ToList();
            }
        }

        public static IDictionary<int, T> BuildDictionary<T>() where T : class,IEntity
        {
            IDictionary<int, T> result = new Dictionary<int, T>();
            using (var workspace = WorkspaceFactory.CreateReadOnly())
            {
                var values = workspace.Query<T>(null);
                foreach (var value in values)
                {
                    result.Add(value.Id, value);
                }
            }

            return result;
        }

        public static int Count<T>(Expression<Func<T, bool>> predictate) where T : class
        {
            using (var workspace = WorkspaceFactory.CreateReadOnly())
            {
                return workspace.Count(predictate);
            }
        }

        public static decimal Sum<T>(Expression<Func<T, decimal>> func, Expression<Func<T, bool>> predictate) where T : class
        {
            using (var workspace = WorkspaceFactory.CreateReadOnly())
            {
                return workspace.Sum(func, predictate);
            }
        }

        public static T Last<T>(Expression<Func<T, bool>> predictate, params Expression<Func<T, object>>[] includes) where T : class ,IEntity
        {
            using (var workspace = WorkspaceFactory.CreateReadOnly())
            {
                return workspace.Last(predictate, includes);
            }
        }

        //public static IEnumerable<T> Last<T>(int recordCount) where T : class,IEntity
        //{
        //    using (var workspace = WorkspaceFactory.CreateReadOnly())
        //    {
        //        return workspace.Last<T>(recordCount).OrderBy(x => x.Id);
        //    }
        //}

        public static int ExecuteFunction(string functionName, params ObjectParameter[] parameters)
        {
            using (var workspace = WorkspaceFactory.CreateReadOnly())
            {
                return workspace.ExecuteFunction(functionName, parameters);
            }
        }
    }
}
