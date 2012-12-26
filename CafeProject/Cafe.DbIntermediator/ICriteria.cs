using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate;

namespace Cafe.DbIntermediator
{
    public interface ICriteria
    {
        IList<SubCriteria> SubCriteria { get; set; }

        ICriteria Asc(string propertyName);
        ICriteria And(ICriteria contained);
        ICriteria Asc(IEnumerable<string> propertyNames);
        ICriteria Between(string propertyName, object lo, object hi);
        ICriteria Count(string propertyName);
        ICriteria CountDistinct(string propertyName);
        ICriteria Desc(string propertyName);
        ICriteria Desc(IEnumerable<string> propertyNames);
        ICriteria Distinct();
        bool Contains(Expression expressionToEvaluate);
        IList ExecuteUsing(ISession session);
        IList<T> ExecuteUsing<T>(ISession session);
        string[] GetOrderList();
        ICriteria GetProperty(string propertyName);
        ICriteria GroupProperty(string propertyName);
        Type PersistentType { get; }
        ICriteria Eq(string propertyName, object val);
        ICriteria Ne(string propertyName, object val);
        ICriteria Le(string propertyName, object val);
        ICriteria Ge(string propertyName, object val);
        ICriteria Like(string propertyName, object val);
        ICriteria Or(ICriteria contained);
        ICriteria EqOr(IList val);
        ICriteria In(string propertyName, ICollection values);
        ICriteria IsNull(string field);
        ICriteria NotBetween(string propertyName, object lo, object hi);
        ICriteria NotIn(string propertyName, ICollection values);
        ICriteria RowCount();
        ICriteria SetFirstResult(int firstResult);
        ICriteria SetMaxResult(int maxResult);
        ICriteria Lt(string propertyName, object val);
        ICriteria Gt(string propertyName, object val);
        string ToString();
        ICriteria Transform(NHibernate.Transform.IResultTransformer transformer);
    }
}
