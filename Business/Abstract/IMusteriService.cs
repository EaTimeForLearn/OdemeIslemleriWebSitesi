using Core.Utilities.Results;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface  IMusteriService
    {
        IDataResult<List<Musteri>> GetList();
        //IDataResult <List<Musteri>> GetAll();
    }
}
