using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Criterion;

namespace WIT.Common.NHibernate
{
    public interface IEntityDAO<T, IdT>
    {
        T GetById(IdT id, bool shouldLock);
        T GetUniqueByQuery(string query, Dictionary<string, object> parameters);
        List<T> GetByQuery(string query, Dictionary<string, object> parameters);
        List<T> GetByQueryPerPage(string query, Dictionary<string, object> parameters, int pageNum, int maxRecordsPerPage);
        int GetScalarByQuery(string query, Dictionary<string, object> parameters);
        List<T> GetByCriteria(params ICriterion[] criterion);
        List<T> GetByCriteria(DetachedCriteria detachedCriteria);
        T GetUniqueByCriteria(params ICriterion[] criterion);
        T GetUniqueByCriteria(DetachedCriteria detachedCriteria);
        List<T> GetAll();
        List<T> GetByExample(T exampleInstance, params string[] propertiesToExclude);
        T GetUniqueByExample(T exampleInstance, params string[] propertiesToExclude);
        T Save(T entity);
        T SaveOrUpdate(T entity);
        void Delete(T entity);
        void CommitChanges();
    }
}
