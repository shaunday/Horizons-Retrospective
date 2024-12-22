using AutoMapper;
using AutoMapper.EquivalencyExpression;
using JTA.Journal.Entities;
using JTA.Web.Services.Models.Journal;

namespace JTA.Web.API.Mapping
{
    public class JournalObjectsMappingProfile : Profile
    {
        public JournalObjectsMappingProfile()
        {
            CreateMap<DataElement, DataElementModel>().EqualityComparison((dto, m) => dto.Id == m.Id);
            CreateMap<TradeElement, TradeElementModel>().EqualityComparison((dto, m) => dto.Id == m.Id);
            CreateMap<TradeComposite, TradeCompositeModel>().EqualityComparison((dto, m) => dto.Id == m.Id);
        }
    }
}
