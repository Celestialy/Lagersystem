using AutoMapper;
using SKPLager.Shared.Models;
using SKPLager.Shared.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKPLager.Shared.Profiles
{
    public class LoanProfile : Profile
    {
        public LoanProfile()
        {
            CreateMap<LoanItem, CreateLoanDTO>().ReverseMap();
            CreateMap<LoanItem, ReturnLoanDTO>().ReverseMap();
        }
    }
}
