using System;
using System.Linq;
using System.Linq.Expressions;

namespace APICatalogo.Repository
{
    public interface IRepository<T> //interface generica <T>
    {

        //nao existem metodos que SALVAR (persistem) pq é tudo em memoria
        //nao é papel do Repository persistir... para isso é utilizado o padrão Unit Of Work
        IQueryable<T> Get();
        T GetById(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
