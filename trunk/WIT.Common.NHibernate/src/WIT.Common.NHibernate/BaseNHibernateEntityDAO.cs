using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Expression;

namespace WIT.Common.NHibernate
{
    public abstract class BaseNHibernateEntityDao<T, IdT> : IEntityDAO<T, IdT>
    {
        /// <summary>
        /// Loads an instance of type TypeOfListItem from the DB based on its ID.
        /// </summary>
        public T GetById(IdT id, bool shouldLock)
        {
            T entity;

            if (shouldLock)
            {
                entity = (T)NHibernateSession.Load(persitentType, id, LockMode.Upgrade);
            }
            else
            {
                entity = (T)NHibernateSession.Load(persitentType, id);
            }

            return entity;
        }

        /// <summary>
        /// Loads every instance of the requested type with no filtering.
        /// </summary>
        public List<T> GetAll()
        {
            return GetByCriteria();
        }

        /// <summary>
        /// Loads every instance of the requested type using the conditions specified
        /// in the query param. This particular implementation uses HQL.
        /// </summary>
        /// <param name="query">The HQL of the query</param>
        /// <param name="parameters">A Dictionary containing key value pairs of param name - param value</param>
        /// <returns></returns>
        public List<T> GetByQuery(string query, Dictionary<string, object> parameters)
        {
            IQuery hql = NHibernateSession.CreateQuery(query);
            foreach (KeyValuePair<string, object> param in parameters)
            {
                hql.SetParameter(param.Key, param.Value);
            }
            return hql.List<T>() as List<T>;
        }
        /// <summary>
        /// Loads every instance of the requested type using the conditions specified
        /// in the query param. This particular implementation uses HQL.
        /// </summary>
        /// <param name="query">The HQL of the query</param>
        /// <param name="parameters">A Dictionary containing key value pairs of param name - param value</param>
        /// <returns></returns>
        public List<T> GetByQueryPerPage(string query, Dictionary<string, object> parameters, int pageNum, int maxRecordsPerPage)
        {
            IQuery hql = NHibernateSession.CreateQuery(query);
            hql.SetMaxResults(maxRecordsPerPage);
            hql.SetFirstResult(((pageNum - 1) * maxRecordsPerPage));
            foreach (KeyValuePair<string, object> param in parameters)
            {
                hql.SetParameter(param.Key, param.Value);
            }
            return hql.List<T>() as List<T>;
        }

        /// <summary>
        /// Loads an unique instance of the requested type using the conditions specified
        /// in the query param. This particular implementation uses HQL.
        /// </summary>
        /// <param name="query">The HQL of the query</param>
        /// <param name="parameters">A Dictionary containing key value pairs of param name - param value</param>
        /// <returns></returns>
        public T GetUniqueByQuery(string query, Dictionary<string, object> parameters)
        {
            IQuery hql = NHibernateSession.CreateQuery(query);

            foreach (KeyValuePair<string, object> param in parameters)
            {
                hql.SetParameter(param.Key, param.Value);
            }

            return hql.UniqueResult<T>();
        }

        /// <summary>
        /// Gets an scalar value from the database according to the specified query
        /// in the query param. This particular implementation uses HQL.
        /// </summary>
        /// <param name="query">The HQL of the query</param>
        /// <param name="parameters">A Dictionary containing key value pairs of param name - param value</param>
        /// <returns></returns>
        public int GetScalarByQuery(string query, Dictionary<string, object> parameters)
        {
            IQuery hql = NHibernateSession.CreateQuery(query);

            foreach (KeyValuePair<string, object> param in parameters)
            {
                hql.SetParameter(param.Key, param.Value);
            }

            int result;
            if (!int.TryParse(hql.UniqueResult().ToString(), out result))
            {
                //TODO: Cambiar a exception tipada
                throw new Exception("HQL GetScalar nulo");
            }

            return result;
        }

        /// <summary>
        /// Loads every instance of the requested type using the supplied <see cref="ICriterion" />.
        /// If no <see cref="ICriterion" /> is supplied, this behaves like <see cref="GetAll" />.
        /// </summary>
        public List<T> GetByCriteria(params ICriterion[] criterion)
        {
            ICriteria criteria = NHibernateSession.CreateCriteria(persitentType);

            foreach (ICriterion criterium in criterion)
            {
                criteria.Add(criterium);
            }

            return criteria.List<T>() as List<T>;
        }

        public List<T> GetByCriteria(DetachedCriteria detachedCriteria)
        {
            ICriteria criteria = detachedCriteria.GetExecutableCriteria(NHibernateSession);

            return criteria.List<T>() as List<T>;
        }

        /// <summary>
        /// Loads every instance of the requested type using the supplied <see cref="ICriterion" />.
        /// If no <see cref="ICriterion" /> is supplied, this behaves like <see cref="GetAll" />.
        /// </summary>
        public T GetUniqueByCriteria(params ICriterion[] criterion)
        {
            ICriteria criteria = NHibernateSession.CreateCriteria(persitentType);

            foreach (ICriterion criterium in criterion)
            {
                criteria.Add(criterium);
            }

            return criteria.UniqueResult<T>();
        }

        public T GetUniqueByCriteria(DetachedCriteria detachedCriteria)
        {
            ICriteria criteria = detachedCriteria.GetExecutableCriteria(NHibernateSession);

            return criteria.UniqueResult<T>();
        }

        public List<T> GetByExample(T exampleInstance, params string[] propertiesToExclude)
        {
            ICriteria criteria = NHibernateSession.CreateCriteria(persitentType);
            Example example = Example.Create(exampleInstance);

            foreach (string propertyToExclude in propertiesToExclude)
            {
                example.ExcludeProperty(propertyToExclude);
            }

            criteria.Add(example);

            return criteria.List<T>() as List<T>;
        }

        /// <summary>
        /// Looks for a single instance using the example provided.
        /// </summary>
        /// <exception cref="NonUniqueResultException" />
        public T GetUniqueByExample(T exampleInstance, params string[] propertiesToExclude)
        {
            List<T> foundList = GetByExample(exampleInstance, propertiesToExclude);

            if (foundList.Count > 1)
            {
                throw new NonUniqueResultException(foundList.Count);
            }

            if (foundList.Count > 0)
            {
                return foundList[0];
            }
            else
            {
                return default(T);
            }
        }

        /// <summary>
        /// For entities that have assigned ID's, you must explicitly call Save to add a new one.
        /// See http://www.hibernate.org/hib_docs/reference/en/html/mapping.html#mapping-declaration-id-assigned.
        /// </summary>
        public T Save(T entity)
        {
            NHibernateSession.Save(entity);
            return entity;
        }

        /// <summary>
        /// For entities with automatatically generated IDs, such as identity, SaveOrUpdate may 
        /// be called when saving a new entity.  SaveOrUpdate can also be called to update any 
        /// entity, even if its ID is assigned.
        /// </summary>
        public T SaveOrUpdate(T entity)
        {
            NHibernateSession.SaveOrUpdate(entity);
            return entity;
        }

        public void Delete(T entity)
        {
            NHibernateSession.Delete(entity);
        }

        public void Refresh(T entity) {
            NHibernateSession.Refresh(entity);
        }

        /// <summary>
        /// Commits changes regardless of whether there's an open transaction or not
        /// </summary>
        public void CommitChanges()
        {
            if (NHibernateSessionManager.Instance.HasOpenTransaction())
            {
                NHibernateSessionManager.Instance.CommitTransaction();
            }
            else
            {
                // If there's no transaction, just flush the changes
                NHibernateSessionManager.Instance.GetSession().Flush();
            }
        }

        /// <summary>
        /// Exposes the ISession used within the DAO.
        /// </summary>
        private ISession NHibernateSession
        {
            get
            {
                return NHibernateSessionManager.Instance.GetSession();
            }
        }

        private Type persitentType = typeof(T);
    }
}
