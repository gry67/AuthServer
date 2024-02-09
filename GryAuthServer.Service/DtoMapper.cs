using AutoMapper;
using GryAuthServer.Core.DTOs;
using GryAuthServer.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GryAuthServer.Service
{
    public class DtoMapper:Profile
    {
        public DtoMapper()
        {
            CreateMap<ProductDto, Product>().ReverseMap(); //reverseMap ile tersine dönüştürme olabilir
            CreateMap<UserAppDto, UserApp>().ReverseMap();
        }
    }
}
