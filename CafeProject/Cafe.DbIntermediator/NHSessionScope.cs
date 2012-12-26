using System;
using System.Data;
using System.Reflection;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Connection;
using NHibernate.Context;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Cafe.Business.Mapping;
using NHibernate.Tool.hbm2ddl;

namespace Cafe.DbIntermediator
{
    public class NHSessionScope : IDisposable
    {
        #region NHSessionScope : Private Fields.
        private static ISessionFactory _sessionFactory;
        private readonly bool _isSessionOwner;
        private bool _isTransactionOwner;
        #endregion

        #region NHSessionScope : Constructors.
        static NHSessionScope()
        {
            if (_sessionFactory == null)
                InitializeSessionFactory();
            // _sessionFactory = new Configuration().Configure().BuildSessionFactory();
        }

        private static void InitializeSessionFactory()
        {
            _sessionFactory = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008
                  .ConnectionString(
                  @"Server=localhost;initial catalog=cafe_system_10; user=cafev10;password=cafev10123;") // Modify your ConnectionString
                              .ShowSql()
                )
                .Mappings(m =>m.FluentMappings
                              .AddFromAssemblyOf<EmployeeMap>()
                              .AddFromAssemblyOf<CustomerMap>()
                              .AddFromAssemblyOf<BranchMap>()
                              .AddFromAssemblyOf<ProductMap>()
                              .AddFromAssemblyOf<ReferenceDataMap>()
                              .AddFromAssemblyOf<HistoryMap>()
                              .AddFromAssemblyOf<InventoryMap>()
                              .AddFromAssemblyOf<SalesMap>())
                .ExposeConfiguration(cfg => new SchemaExport(cfg)
                                                .Create(true, true))
                .BuildSessionFactory();
        }

        private NHSessionScope(ISession session, bool isSessionOwner)
        {
            if (session == null) throw new ArgumentNullException("session");

            this.Session = session;
            this._isSessionOwner = isSessionOwner;
        }
        #endregion

        public bool IsDisposed { get; private set; }

        public bool IsSessionOwner
        {
            get
            {
                return this._isSessionOwner;
            }
        }

        public ISession Session { get; private set; }

        #region NHSessionScope : Methods.
        /// <summary>
        /// Commits the current NHibernate session if this <see cref="NHSessionScope"/> started the transaction.
        /// </summary>
        public void Commit()
        {
            if (this._isTransactionOwner)
            {
                var session = GetCurrentSession();
                if (session.Transaction != null && session.Transaction.IsActive)
                    session.Transaction.Commit();
            }
        }

        private void EndSession()
        {
            if (this._isSessionOwner)
            {
                var session = CurrentSessionContext.Unbind(_sessionFactory);
                if (session.Transaction != null)
                    session.Transaction.Dispose();
                session.Dispose();
            }
        }

        /// <summary>
        /// Removes the specified object from the current session's cache. If no session is associated with the current context, do nothing.
        /// </summary>
        /// <param name="entity"></param>
        public static void Evict(object entity)
        {
            var session = GetCurrentSession();
            if (session != null)
                session.Evict(entity);
        }


        /// <summary>
        /// Gets the NHibernate session bound to the current context.
        /// </summary>
        /// <returns></returns>
        public static ISession GetCurrentSession()
        {
            return CurrentSessionContext.HasBind(_sessionFactory) ? _sessionFactory.GetCurrentSession() : null;
        }

        public static ISessionFactory GetSessionFactory()
        {
            return _sessionFactory;
        }

        public void Rollback()
        {
            if (this._isTransactionOwner)
            {
                var session = GetCurrentSession();
                if (session.Transaction != null && session.Transaction.IsActive)
                    session.Transaction.Rollback();
            }
        }

        /// <summary>
        /// Opens a new stateless session.
        /// </summary>
        /// <returns></returns>
        public static IStatelessSession OpenStatelessSession()
        {
            return _sessionFactory.OpenStatelessSession();
        }

        public static IStatelessSession OpenStatelessSession(IDbConnection connection)
        {
            return _sessionFactory.OpenStatelessSession(connection);
        }

        /// <summary>
        /// Creates and initializes a new <see cref="NHSessionScope"/> instance. Opens a new <see cref="ISession"/>
        /// instance if the current context has no opened session , otherwise the returned <see cref="NHSessionScope"/>
        /// instance will use the session of the current context.
        /// </summary>
        /// <param name="enableTransactions">A value indicating whether to begin a new transaction
        /// if the current context has no opened transaction yet.</param>
        /// <param name="isolationLevel"> </param>
        /// <returns></returns>
        public static NHSessionScope Start(bool enableTransactions = false, IsolationLevel isolationLevel = IsolationLevel.Unspecified)
        {
            ISession session = GetCurrentSession();
            NHSessionScope scope;
            if (session == null)
            {
                session = _sessionFactory.OpenSession();
                CurrentSessionContext.Bind(session);
                scope = new NHSessionScope(session, true);
            }
            else
                scope = new NHSessionScope(session, false);

            if (enableTransactions)
            {
                if (session.Transaction != null && !session.Transaction.IsActive)
                {
                    if (isolationLevel == IsolationLevel.Unspecified)
                        session.BeginTransaction();
                    else
                        session.BeginTransaction(isolationLevel);
                    scope._isTransactionOwner = true;
                }
            }
            return scope;
        }

        /// <summary>
        /// Creates and initializes a new <see cref="NHSessionScope"/> instance using the <see cref="ISession"/>
        /// of the created <see cref="NHSessionScope"/>.
        /// </summary>
        /// <param name="scope"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">An NHibernate session has already been bound to the current context.</exception>
        public static NHSessionScope Start(NHSessionScope scope)
        {
            if (scope == null) throw new ArgumentNullException("scope");
            if (scope.Session == null) throw new ArgumentException("Scope has no session.", "scope");

            ISession currentSession = GetCurrentSession();
            if (currentSession != null && currentSession != scope.Session)
                throw new InvalidOperationException("A session has already been bound to the current context.");

            if (currentSession == null)
                CurrentSessionContext.Bind(scope.Session);

            return new NHSessionScope(scope.Session, false);
        }

        ~NHSessionScope()
        {
            this.Dispose(false);
        }
        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Rollback();
                this.EndSession();
                this.IsDisposed = true;
            }
        }
        #endregion
    }
}
