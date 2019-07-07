﻿using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;

namespace Chai.WorkflowManagment.CoreDomain
{
    public interface IReadOnlyWorkspace : IDisposable
    {
        IEnumerable<T> Query<T>(Expression<Func<T, bool>> predictate, params Expression<Func<T, object>>[] includes) where T : class;
        IEnumerable<string> Distinct<T>(Expression<Func<T, string>> expression) where T : class;
        TResult Single<TSource, TResult>(int id, Expression<Func<TSource, TResult>> expression) where TSource : class, IEntity;
        T Single<T>(Expression<Func<T, bool>> predictate, string[] includes) where T : class;
        T Single<T>(Expression<Func<T, bool>> predictate, params Expression<Func<T, object>>[] includes) where T : class;
        IEnumerable<TResult> Select<TSource, TResult>(Expression<Func<TSource, TResult>> expression, Expression<Func<TSource, bool>> predictate) where TSource : class;
        int Count<T>(Expression<Func<T, bool>> predictate) where T : class;
        decimal Sum<T>(Expression<Func<T, decimal>> selector, Expression<Func<T, bool>> predictate) where T : class;
        T Last<T>(Expression<Func<T, bool>> predictate, Expression<Func<T, object>>[] includes) where T : class, IEntity;
        //IEnumerable<T> Last<T>(int recordCount) where T : class,IEntity;
        IQueryable<T> Queryable<T>() where T : class;
        IEnumerable<T> Queryable<T>(string query) where T : class;
        int ExecuteFunction(string functionName, params ObjectParameter[] parameters);
    }
}
