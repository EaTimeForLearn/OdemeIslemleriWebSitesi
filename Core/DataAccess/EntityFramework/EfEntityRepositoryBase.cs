using Core.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess.EntityFramework
{
    public class EfEntityRepositoryBase<TEntity, TContext> : IEntityRepository<TEntity>
        where TEntity : class, IEntity, new()
        where TContext : DbContext, new()
    {
        public void Add(TEntity entity)
        {
            using (var context = new TContext())
            {
                var addedEntity = context.Entry<TEntity>(entity);
                addedEntity.State = EntityState.Added;
                context.SaveChanges();
            }
        }

        public void Delete(TEntity entity)
        {
            using (var context = new TContext())
            {
                var deletedEntity = context.Entry<TEntity>(entity);
                deletedEntity.State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public IList<TEntity> GetList(Expression<Func<TEntity, bool>> filter = null)
        {
            using (var context = new TContext())
            {
                return filter == null
                    ? context.Set<TEntity>().ToList()
                    : context.Set<TEntity>().Where(filter).ToList();
            }
        }

        public TEntity Get(Expression<Func<TEntity, bool>> filter)
        {
            using (var context = new TContext())
            {
                return context.Set<TEntity>().SingleOrDefault(filter);
            }
        }

        public void Update(TEntity entity)
        {
            using (var context = new TContext())
            {
                var updatedEntity = context.Entry<TEntity>(entity);
                updatedEntity.State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void UpdateFields(TEntity entity, string updatedProperties)
        {
            using (var context = new TContext())
            {
                var updatedEntity = context.Entry<TEntity>(entity);
                var updatedPropertiesArr = updatedProperties.Split(',', StringSplitOptions.RemoveEmptyEntries);
                foreach (var prop in updatedPropertiesArr)
                {
                    updatedEntity.Property(prop.Trim()).IsModified = true;
                }
                context.SaveChanges();
            }
        }

        public IList<TEntity> GetDataFromSQL(string SQL, string[] ParamNames, object[] ParamValues)
        {
            SqlParameter param;
            List<SqlParameter> parameters = null;
            if (ParamNames != null)
            {
                parameters = new List<SqlParameter>();
                for (int i = 0; i <= ParamNames.Length - 1; i++)
                {
                    if (ParamValues[i] == null)
                    {
                        param = new SqlParameter(ParamNames[i], DBNull.Value);
                    }
                    else
                    {
                        param = new SqlParameter(ParamNames[i], ParamValues[i]);
                    }
                    parameters.Add(param);
                }
            }
            using (var context = new TContext())
            {
                if (parameters == null)
                    return context.Set<TEntity>().FromSqlRaw(SQL).ToList();
                else
                    return context.Set<TEntity>().FromSqlRaw(SQL, parameters.ToArray()).ToList();
            }
        }

        public IList<TEntity> GetDataFromSP(string SPName, string[] ParamNames, object[] ParamValues)
        {
            SqlParameter param;
            List<SqlParameter> parameters = null;
            if (ParamNames != null)
            {
                parameters = new List<SqlParameter>();
                for (int i = 0; i <= ParamNames.Length - 1; i++)
                {
                    if (ParamValues[i] == null)
                    {
                        param = new SqlParameter(ParamNames[i], DBNull.Value);
                    }
                    else
                    {
                        param = new SqlParameter(ParamNames[i], ParamValues[i]);
                    }
                    parameters.Add(param);
                }
            }
            using (var context = new TContext())
            {
                if (parameters == null)
                    return context.Set<TEntity>().FromSqlRaw(String.Format("EXEC {0}", SPName)).ToList();
                else
                    return context.Set<TEntity>().FromSqlRaw(String.Format("EXEC {0} {1}", SPName, String.Join(",", ParamNames)), parameters.ToArray()).ToList();
            }
        }
        public int ExecuteSQL(string SQL, string[] ParamNames, object[] ParamValues)
        {
            SqlParameter param;
            List<SqlParameter> parameters = null;
            if (ParamNames != null)
            {
                parameters = new List<SqlParameter>();
                for (int i = 0; i <= ParamNames.Length - 1; i++)
                {
                    if (ParamValues[i] == null)
                    {
                        param = new SqlParameter(ParamNames[i], DBNull.Value);
                    }
                    else
                    {
                        param = new SqlParameter(ParamNames[i], ParamValues[i]);
                    }
                    parameters.Add(param);
                }
            }
            using (var context = new TContext())
            {
                if (parameters == null)
                    return context.Database.ExecuteSqlRaw(SQL);
                else
                    return context.Database.ExecuteSqlRaw(SQL, parameters.ToArray());
            }
        }

        public int ExecuteSP(string SPName, string[] ParamNames, object[] ParamValues)
        {
            SqlParameter param;
            List<SqlParameter> parameters = null;
            if (ParamNames != null)
            {
                parameters = new List<SqlParameter>();
                for (int i = 0; i <= ParamNames.Length - 1; i++)
                {
                    if (ParamValues[i] == null)
                    {
                        param = new SqlParameter(ParamNames[i], DBNull.Value);
                    }
                    else
                    {
                        param = new SqlParameter(ParamNames[i], ParamValues[i]);
                    }
                    parameters.Add(param);
                }
            }
            using (var context = new TContext())
            {
                if (parameters == null)
                    return context.Database.ExecuteSqlRaw(String.Format("EXEC {0}", SPName));
                else
                    return context.Database.ExecuteSqlRaw(String.Format("EXEC {0} {1}", SPName, String.Join(",", ParamNames)), parameters.ToArray());
            }
        }

        public int ExecuteSPReturnValue(string SPName, string[] ParamNames, object[] ParamValues)
        {
            SqlParameter param;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if (ParamNames != null)
            {
                for (int i = 0; i <= ParamNames.Length - 1; i++)
                {
                    if (ParamValues[i] == null)
                    {
                        param = new SqlParameter(ParamNames[i], DBNull.Value);
                    }
                    else
                    {
                        param = new SqlParameter(ParamNames[i], ParamValues[i]);
                    }
                    parameters.Add(param);
                }
            }
            SqlParameter returnParameter = new SqlParameter("retval", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output,
                ParameterName = "retval",
                SqlDbType = System.Data.SqlDbType.Int
            };
            parameters.Add(returnParameter);
            using (var context = new TContext())
            {
                if (ParamNames == null)
                    context.Database.ExecuteSqlRaw(String.Format("EXEC @retval = {0}", SPName), parameters);
                else
                    context.Database.ExecuteSqlRaw(String.Format("EXEC @retval = {0} {1}", SPName, String.Join(",", ParamNames)), parameters);
                return (int)returnParameter.Value;
            }
        }

      
    }
}
