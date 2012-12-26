using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Type;
using NHibernate;

namespace Cafe.DbIntermediator
{
    public interface IRepository<T>
    {
        int Count(string queryString, params object[] args);

        void Delete(T persistentObject);

        void Delete(IEnumerable<T> persistentObject);

        void Delete(string hql);

        void Delete(string hql, object[] args, IType[] types);

        void DeleteAll(Type typeOfPersistentObject);

        void DeleteAll(Type typeOfPersistentObject, string condition);

        T FindOne(ICriteria criteria);

        T FindOne(string hql, params object[] args);

        T FindOne(object id);

        IList<T> Find(ICriteria criteria);

        IList<T> Find(Type persistentType);

        IList<T> Find(string hql, params object[] args);

        T FindFirst(ICriteria criteria);

        IList FindList(ICriteria criteria);

        IList FindList(string hql, params object[] args);

        T Get(object id, LockMode lockMode);

        IList<TReturn> List<TReturn>(NHibernate.Criterion.DetachedCriteria criteria);

        IList List(NHibernate.Criterion.DetachedCriteria criteria);

        T Refresh(T obj);

        T Save(T persistentObject);

        void SaveOnly(T persistentObject);

        TReturn UniqueResult<TReturn>(NHibernate.Criterion.DetachedCriteria criteria);
    }
}
