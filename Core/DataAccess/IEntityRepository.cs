using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess
{
    //IEntity yi implement eden ve newlenebilir her class ı buraya generic olarak gönderebilirsin.
    //new() ibaresi IEntity nin kendisi gönderilmesin diye ekleniyor. Interface newlenemez.
    public interface IEntityRepository<T> where T:class, IEntity, new()
    {
        T Get(Expression<Func<T, bool>> filter);
        IList<T> GetList(Expression<Func<T, bool>> filter=null);
        void Add(T entity);
        void Update(T entity);
        void UpdateFields(T entity, string updatedProperties);
        void Delete(T entity);
        IList<T> GetDataFromSQL(string SQL, string[] ParamNames, object[] ParamValues);
        IList<T> GetDataFromSP(string SPName, string[] ParamNames, object[] ParamValues);
        int ExecuteSQL(string SQL, string[] ParamNames, object[] ParamValues);
        int ExecuteSP(string SPName, string[] ParamNames, object[] ParamValues);
        int ExecuteSPReturnValue(string SPName, string[] ParamNames, object[] ParamValues);
    }
}
