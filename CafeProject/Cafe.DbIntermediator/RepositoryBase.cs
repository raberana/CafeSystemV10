using System.Collections.Generic;
using NHibernate;

namespace Cafe.DbIntermediator
{
    /// <summary>
    /// Base class for all data access classes. This class must handle on data access operations 
    /// and must not implement any business logic.
    /// </summary>
    public abstract class RepositoryBase<T> where T : class
    {
        public virtual string CacheRegion
        {
            get { return typeof(T).FullName; }
        }

        /// <summary>
        /// Clears the 2nd level cache of all confidence ratings.
        /// </summary>
        public virtual void ClearCache()
        {
            ISessionFactory sessionFactory = NHSessionScope.GetSessionFactory();
            sessionFactory.Evict(typeof(T));
            sessionFactory.EvictQueries(this.CacheRegion);
        }

        /// <summary>
        /// Deletes the persistent instance from the database.
        /// </summary>
        /// <param name="obj">The persistent instance to delete from the database.</param>
        public virtual void Delete(T obj)
        {
            using (NHSessionScope scope = NHSessionScope.Start(true))
            {
                scope.Session.Delete(obj);
                scope.Commit();
            }
        }

        /// <summary>
        /// Deletes the given persistent instances.
        /// </summary>
        /// <param name="list"></param>
        public virtual void Delete(IEnumerable<T> list)
        {
            using (NHSessionScope scope = NHSessionScope.Start(true))
            {
                foreach (T obj in list)
                    this.Delete(obj);
                scope.Commit();
            }
        }

        /// <summary>
        /// Returns the persistent instance of <see cref="T"/> based on the specified id.
        /// </summary>
        /// <param name="id">The identifier of the persistent instance to get.</param>
        /// <returns>The persistent instance or null if there is no such persistent instance.</returns>
        public virtual T GetByID(int id)
        {
            using (NHSessionScope scope = NHSessionScope.Start())
            {
                return scope.Session.Get<T>(id);
            }
        }

        /// <summary>
        /// Returns all the entities of the given type.
        /// </summary>
        /// <returns></returns>
        public virtual IList<T> List()
        {
            using (NHSessionScope scope = NHSessionScope.Start())
            {
                return scope.Session.QueryOver<T>().List();
            }
        }

        /// <summary>
        /// Saves or updates the given instance, depending on the value of its identifier property.
        /// </summary>
        /// <param name="obj">The instance to persist.</param>
        public virtual void Save(T obj)
        {
            using (NHSessionScope scope = NHSessionScope.Start(true))
            {
                scope.Session.SaveOrUpdate(obj);
                scope.Commit();
            }
        }

        /// <summary>
        /// Saves or updates the given list of persistent objects.
        /// </summary>
        /// <param name="list"></param>
        public virtual void Save(IEnumerable<T> list)
        {
            using (var scope = NHSessionScope.Start(true))
            {
                foreach (T obj in list)
                    this.Save(obj);
                scope.Commit();
            }
        }
    }
}
