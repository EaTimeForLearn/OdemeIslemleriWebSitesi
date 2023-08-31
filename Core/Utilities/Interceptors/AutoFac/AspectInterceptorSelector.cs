using Castle.DynamicProxy;
using Core.Aspects.AutoFac.ExceptionHandling;
using Core.CrossCuttingConcerns.Logging.Log4Net.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Interceptors.AutoFac
{
    public class AspectInterceptorSelector : IInterceptorSelector
    {
        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            var classAttributes = type.GetCustomAttributes<MethodInterceptionBaseAttribute>(inherit: true).ToList();
            var methodAttributes = type.GetMethod(method.Name).GetCustomAttributes<MethodInterceptionBaseAttribute>(inherit:true);
            classAttributes.AddRange(methodAttributes);
            classAttributes.Add(new ExceptionLogAspect(typeof(FileLogger)) { Priority=-1 });//ExceptionAspect ilk sırada çalışması için Priority -1 olarak atanıyor.
            //classAttributes.Add(new ExceptionLogAspect(typeof(SmtpLogger)) { Priority = -1 });//ExceptionAspect ilk sırada çalışması için Priority -1 olarak atanıyor.
            return classAttributes.OrderBy(x=>x.Priority).ToArray();    

        }
    }
}
