﻿using AutoMapper;
using AutoMapper.EquivalencyExpression;
using HsR.Journal.Entities;
using HsR.Web.Services.Models.Journal;

namespace HsR.Web.API.Mapping
{
    public class EntitiesToDtosMappingProfile : Profile
    {
        public EntitiesToDtosMappingProfile()
        {
            CreateMap<ContentRecord, ContentRecordModel>();
            CreateMap<DataElement, DataElementModel>().EqualityComparison((dto, m) => dto.Id == m.Id);
            CreateMap<TradeElement, TradeElementModel>().EqualityComparison((dto, m) => dto.Id == m.Id);
            CreateMap<TradeComposite, TradeCompositeModel>().EqualityComparison((dto, m) => dto.Id == m.Id);
        }
    }
}