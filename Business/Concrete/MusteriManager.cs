using Business.Abstract;
using Business.Contants;
using Core.Aspects.AutoFac.Caching;
using Core.Aspects.AutoFac.Logging;
using Core.Aspects.AutoFac.Performance;
using Core.CrossCuttingConcerns.Logging.Log4Net.Loggers;
using Core.Utilities.IoC;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class MusteriManager : IMusteriService
    {
        private readonly IMusteriDal _musteriDal;
        IHttpContextAccessor _httpContextAccessor;
        public MusteriManager(IMusteriDal musteriDal)
        {
            this._musteriDal = musteriDal;
            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
        }

        

        [CacheAspect(duration: 1, Priority = 1)]
        [PerformanceAspect(interval: 5, typeof(FileLogger), Priority = 2)]
        [LogAspect(typeof(FileLogger), Priority = 3)]
        public IDataResult<List<Musteri>> GetList()
        {
            var userID = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "";
            if (!string.IsNullOrEmpty(userID))
            {
               var retval= _musteriDal.GetDataFromSP("PAYMENT_GetUsers", new string[] { "@ID" }, new object[] {Convert.ToInt32(userID) }).ToList();
                
                return new SuccessDataResult<List<Musteri>>(retval);
            }

            return new ErrorDataResult<List<Musteri>>(Messages.UserIdNotFound);

        }



    }
}
