using Core.Entities;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Musteri:IEntity
    {
        public string OperatorAdi { get; set; }
        public DateTime KiraBitisTarihi { get; set; }
        public string Bayiler { get; set; }
        public string Fiyatlar { get; set; }
        public int ID { get; set; }

    }
}
