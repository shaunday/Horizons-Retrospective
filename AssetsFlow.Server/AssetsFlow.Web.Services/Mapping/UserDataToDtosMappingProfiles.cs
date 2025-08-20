using AssetsFlowWeb.Services.Models;
using AutoMapper;
using HsR.Journal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetsFlowWeb.Services.Mapping
{
    public class UserDataToDtosMappingProfiles : Profile
    {
        public UserDataToDtosMappingProfiles() 
        {
            CreateMap<UserData, UserDataDTO>();
        }
    }
}
