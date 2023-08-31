using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class FiyatBilgisiDto:IDto
    {
        public string Tanim { get; set; }
        public int EklenecekAy { get; set; }

       
        public decimal Fiyat { get; set; }
    }
}
