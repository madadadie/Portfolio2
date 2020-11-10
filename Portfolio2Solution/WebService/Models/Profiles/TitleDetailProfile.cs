﻿using DataServiceLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace WebService.Models.Profiles
{
    public class TitleDetailProfile : Profile
    {
        public TitleDetailProfile()
        {
            CreateMap<TitleRating, TitleDetailsDto>();
        }
        
    }
}
