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
    public class ItemProfile : Profile
    {
        public ItemProfile()
        {
            CreateMap<Item, ForUpdateItemDTO>().ReverseMap();
            CreateMap<InventoryItem, UpdateItemDTO>().ReverseMap();
        }
    }
}
