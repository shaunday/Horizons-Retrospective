using HsR.Web.Services.Models.Journal;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using HsR.Journal.Entities;
using HsR.Journal.Services;
using HsR.Journal.Entities.TradeJournal;
using AssetsFlowWeb.Services.Models.Journal;

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
                    opt => opt.MapFrom(src =>
                        src.TradeElements.Any(ele => !ele.IsAllRequiredFields())))
                .ForMember(dest => dest.IdeaDate,
                    opt => opt.MapFrom(src =>
                        src.TradeElements
                            .Where(te => te.TradeActionType == TradeActionType.Origin)
                            .OrderBy(te => te.TimeStamp)
                            .Select(te => te.TimeStamp)
                            .FirstOrDefault()))
                .ForMember(dest => dest.OpenedAt,
                    opt => opt.MapFrom(src =>
                        src.TradeElements
                            .Where(te => te.TradeActionType == TradeActionType.Add)
                            .OrderBy(te => te.TimeStamp)
                            .Select(te => te.TimeStamp)
                            .FirstOrDefault()))
                .ForMember(dest => dest.ClosedAt,
                    opt => opt.MapFrom(src =>
                        src.TradeElements
                            .Where(te => te.TradeActionType == TradeActionType.Reduce)
                            .OrderByDescending(te => te.TimeStamp)
                            .Select(te => te.TimeStamp)
                            .FirstOrDefault()));


            CreateMap<TradeComposite, TradeCompositeModel>()
                .IncludeBase<TradeComposite, TradeCompositeInfo>();

            CreateMap<UpdatedStatesCollation, UpdatedStatesModel>();
        }
    }
}
