﻿using HsR.Web.Services.Models.Journal;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using HsR.Journal.Entities;
using HsR.Journal.Services;
using HsR.Journal.Entities.TradeJournal;

namespace HsR.Web.API.Mapping
{
    public class EntitiesToDtosMappingProfile : Profile
    {
        public EntitiesToDtosMappingProfile()
        {
            CreateMap<ContentRecord, ContentRecordModel>();

            CreateMap<DataElement, DataElementModel>().EqualityComparison((dto, m) => dto.Id == m.Id);
            CreateMap<TradeElement, TradeElementModel>()
                .EqualityComparison((dto, m) => dto.Id == m.Id)
                .ForMember(dest => dest.IsAnyContentMissing,
                opt => opt.MapFrom(src => !src.IsAllRequiredFields()));

            CreateMap<TradeComposite, TradeCompositeInfo>()
                .EqualityComparison((dto, m) => dto.Id == m.Id)
                .ForMember(dest => dest.IsAnyContentMissing,
                    opt => opt.MapFrom(src => src.TradeElements.Any(ele => !ele.IsAllRequiredFields())));

            CreateMap<TradeComposite, TradeCompositeModel>()
                .IncludeBase<TradeComposite, TradeCompositeInfo>();

            CreateMap<UpdatedStatesCollation, UpdatedStatesModel>();
        }
    }
}
