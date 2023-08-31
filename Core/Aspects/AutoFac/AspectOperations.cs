using Castle.DynamicProxy;
using Core.CrossCuttingConcerns.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Aspects.AutoFac
{
    public static class AspectOperations
    {
        public static LogDetail GetLogDetail(IInvocation invocation)
        {
            var logParameters = new List<LogParameter>();
            for (int i = 0; i < invocation.Arguments.Length; i++)
            {
                logParameters.Add(new LogParameter
                {
                    Name = invocation.GetConcreteMethod().GetParameters()[i].Name,
                    Value = invocation.Arguments[i],
                    Type = invocation.Arguments[i].GetType().Name,
                });
            }
            var logDetail = new LogDetail()
            {
                LogParameters = logParameters,
                MethodName = invocation.Method.Name,
                LogDate = DateTime.Now
            };
            return logDetail;
        }
    }
}
