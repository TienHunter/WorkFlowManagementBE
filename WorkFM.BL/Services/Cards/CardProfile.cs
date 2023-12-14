using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common.Data.Cards;
using WorkFM.Common.Data.Tags;

namespace WorkFM.BL.Services.Cards
{
    public class CardProfile:Profile
    {
        public CardProfile() {
            CreateMap<CardCreateDto, Card>();
            CreateMap<Card,CardDto>().ForMember(dest => dest.Tags, opt => opt.MapFrom(src => JsonConvert.DeserializeObject<List<Tag>>(src.Tags)));
            CreateMap<CardDto, Card>().ForMember(dest => dest.Tags, opt => opt.MapFrom(src => JsonConvert.SerializeObject(src.Tags)));
        }
    }
}
