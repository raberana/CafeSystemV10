using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using log4net;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Type;

namespace Cafe.DbIntermediator
{
    public abstract class Repository
    {
        protected readonly static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    }

    public class Repository<T> : Repository, IRepository<T>
    {
        public int Count(string queryString, params object[] args)
        {
            using (NHSessionScope scope = NHSessionScope.Start())
            {
                // construct the IQuery...
                NHibernate.IQuery query = scope.Session.CreateQuery("select count(*) " + queryString);
                if (args != null && args.Length > 0)
                {
                    for (int index = 0; index < args.Length; index++)
                        query.SetParameter(index, args[index]);
                }

                // execute the IQuery...
                IList resultset = query.List();
                return resultset == null || resultset.Count == 0 ? 0 : Convert.ToInt32(resultset[0]);
            }
        }

        public void Delete(IEnumerable<T> entities)
        {
            using (NHSessionScope scope = NHSessionScope.Start(true))
            {
                foreach (T entity in entities)
                    scope.Session.Delete(entity);

                scope.Session.Flush();
                scope.Commit();
            }
        }

        public void Delete(T persistentObject)
        {
            if (Logger.IsDebugEnabled)
                Logger.DebugFormat("Delete: {0}", persistentObject);
            using (NHSessionScope scope = NHSessionScope.Start(true))
            {
                try
                {
                    scope.Session.Delete(persistentObject);
                    scope.Session.Flush();
                    scope.Commit();
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                    throw;
                }
            }
        }

        public void Delete(string queryString)
        {
            this.Delete(queryString, null, null);
        }

        public void Delete(string queryString, object[] args, IType[] types)
        {
            using (NHSessionScope scope = NHSessionScope.Start(true))
            {
                try
                {
                    if (args == null && types == null)
                        scope.Session.Delete(queryString);
                    else
                        scope.Session.Delete(queryString, args, types);
                    scope.Commit();
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                    throw;
                }
            }
        }

        public void DeleteAll(Type typeOfPersistentObject)
        {
            if (Logger.IsDebugEnabled)
                Logger.DebugFormat(CultureInfo.InvariantCulture, "DeleteAll: {0}", typeOfPersistentObject.FullName);
            using (NHSessionScope scope = NHSessionScope.Start(true))
            {
                try
                {
                    scope.Session.Delete("from " + typeOfPersistentObject.Name);
                    scope.Commit();
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                    throw;
                }
            }
        }

        public void DeleteAll(Type typeOfPersistentObject, string condition)
        {
            if (Logger.IsDebugEnabled)
                Logger.DebugFormat("DeleteAll: {0} where {1}", typeOfPersistentObject.FullName, condition);
            using (NHSessionScope scope = NHSessionScope.Start(true))
            {
                try
                {
                    scope.Session.Delete("from " + typeOfPersistentObject.Name + " where " + condition);
                    scope.Commit();
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                    throw;
                }
            }
        }

        public IList<T> Find(string queryString, params object[] args)
        {
            IList<T> result;
            using (NHSessionScope scope = NHSessionScope.Start())
            {
                try
                {
                    NHibernate.IQuery query = scope.Session.CreateQuery(queryString);
                    for (int index = 0; index < args.Length; index++)
                        query.SetParameter(index, args[index]);
                    IList<T> list = query.List<T>();
                    result = list;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                    throw;
                }
            }
            return result;
        }

        public IList<T> Find(ICriteria criteria)
        {
            try
            {
                if (Logger.IsDebugEnabled)
                    Logger.DebugFormat(CultureInfo.InvariantCulture, "Find({0})", criteria.ToString());

                IList<T> result;
                using (NHSessionScope scope = NHSessionScope.Start())
                {
                    result = criteria.ExecuteUsing<T>(scope.Session);
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

      
        public IList FindList(ICriteria criteria)
        {
            try
            {
                if (Logger.IsDebugEnabled)
                    Logger.DebugFormat(CultureInfo.InvariantCulture, "Find({0})", criteria.ToString());
                using (NHSessionScope scope = NHSessionScope.Start())
                {
                    return criteria.ExecuteUsing(scope.Session);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        public IList FindList(string queryString, params object[] args)
        {
            IList result;
            using (NHSessionScope scope = NHSessionScope.Start())
            {
                try
                {
                    NHibernate.IQuery query = scope.Session.CreateQuery(queryString);
                    for (int index = 0; index < args.Length; index++)
                        query.SetParameter(index, args[index]);
                    IList list = query.List();
                    result = list;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                    throw;
                }
            }
            return result;
        }

        public T FindOne(object id)
        {
            T result;
            if (Logger.IsDebugEnabled)
                Logger.DebugFormat(CultureInfo.InvariantCulture, "FindOne({0}, {1})", typeof(T).FullName, id);
            using (NHSessionScope scope = NHSessionScope.Start())
            {
                try
                {
                    result = scope.Session.Get<T>(id, LockMode.None);
                }
                catch (ObjectNotFoundException)
                {
                    result = default(T);
                }
            }
            return result;
        }

        public IList<T> Find(Type persistentType)
        {
            return Find(new Criteria(persistentType));
        }

        public T FindOne(string hql, params object[] args)
        {
            T result = default(T);
            IList<T> results = this.Find(hql, args);
            if (results != null && results.Count > 0)
            {
                if (results.Count == 1)
                    result = results[0];
                else
                    throw new ApplicationException(
                        string.Format(CultureInfo.InvariantCulture, "{0} is not unique.", hql));
            }
            return result;
        }

        public T FindOne(ICriteria criteria)
        {
            T result = default(T);
            IList<T> results = Find(criteria);
            if (results != null && results.Count > 0)
            {
                if (results.Count == 1)
                    result = results[0];
                else
                    throw new ApplicationException(
                        string.Format(CultureInfo.InvariantCulture, "{0} is not unique.", criteria.ToString()));
            }

            return result;
        }

        public IList<T> FindDistinct(string persistentTypeName, string propertyName)
        {
            string sql = string.Format(
                CultureInfo.InvariantCulture, "select distinct pt.{0} from {1} as pt", propertyName, persistentTypeName);

            if (Logger.IsDebugEnabled)
                Logger.DebugFormat(CultureInfo.InvariantCulture, "FindDistinct({0})", sql);

            using (NHSessionScope scope = NHSessionScope.Start())
            {
                return scope.Session.CreateQuery(sql).List<T>();
            }
        }


        public T FindFirst(ICriteria criteria)
        {
            IList<T> results = this.Find(criteria);
            if (results == null || results.Count == 0)
                return default(T);

            return results[0];
        }

        public T Get(object id, LockMode lockMode)
        {
            using (NHSessionScope scope = NHSessionScope.Start())
            {
                try
                {
                    return scope.Session.Get<T>(id, lockMode);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                    throw;
                }
            }
        }

        public IList<TReturn> List<TReturn>(DetachedCriteria detachedCriteria)
        {
            using (NHSessionScope scope = NHSessionScope.Start())
            {
                try
                {
                    return detachedCriteria.GetExecutableCriteria(scope.Session).List<TReturn>();
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                    throw;
                }
            }
        }

        public IList List(DetachedCriteria detachedCriteria)
        {
            using (NHSessionScope scope = NHSessionScope.Start())
            {
                try
                {
                    return detachedCriteria.GetExecutableCriteria(scope.Session).List();
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                    throw;
                }
            }
        }

        public T Refresh(T obj)
        {
            using (var scope = NHSessionScope.Start())
            {
                try
                {
                    scope.Session.Refresh(obj);
                    return obj;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                    throw;
                }
            }
        }

        public T Save(T persistentObject)
        {
            using (var scope = NHSessionScope.Start())
            {
                try
                {
                    scope.Session.SaveOrUpdate(persistentObject);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                    throw;
                }
            }
            return persistentObject;
        }

        public void SaveOnly(T persistentObject)
        {
            if (Logger.IsDebugEnabled)
                Logger.DebugFormat(CultureInfo.InvariantCulture, "SaveOnly: {0}", persistentObject);
            using (var scope = NHSessionScope.Start())
            {
                try
                {
                    scope.Session.Save(persistentObject);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                    throw;
                }
            }
        }

        public TReturn UniqueResult<TReturn>(DetachedCriteria criteria)
        {
            try
            {
                if (Logger.IsDebugEnabled)
                    Logger.DebugFormat(CultureInfo.InvariantCulture, "Find({0})", criteria);
                using (var scope = NHSessionScope.Start())
                {
                    return criteria.GetExecutableCriteria(scope.Session).UniqueResult<TReturn>();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }
    }
}
