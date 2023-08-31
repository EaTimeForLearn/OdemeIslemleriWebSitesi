using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class MusteriDto
    {
        public string OperatorAdi { get; set; }
        public DateTime KiraBitisTarihi { get; set; }
        public string Bayiler { get; set; }
        public List<FiyatBilgisiDto> Fiyatlar { get; set; }
        public int ID { get; set; }
    }
}
