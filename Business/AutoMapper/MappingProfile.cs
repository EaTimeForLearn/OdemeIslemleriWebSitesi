using AutoMapper;
using Entities.Concrete;
using Entities.Dtos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.AutoMapper
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Musteri, MusteriDto>()
            .ForMember(dest => dest.Fiyatlar, opt => opt.MapFrom(src => JsonConvert.DeserializeObject<List<FiyatBilgisiDto>>(src.Fiyatlar)));
        }
    }
}
