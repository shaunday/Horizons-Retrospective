using AutoMapper;
using AutoMapper.EquivalencyExpression;
using DayJT.Journal.Data;
using DayJT.Web.API.Models;

namespace DayJT.Web.API.Mapping
{
    public class JournalObjectsMappingProfile : Profile
    {
        public JournalObjectsMappingProfile()
        {
            CreateMap<Cell, CellModel>().EqualityComparison((dto, m) => dto.Id == m.Id);
            CreateMap<TradeElement, TradeElementModel>().EqualityComparison((dto, m) => dto.Id == m.Id);
            CreateMap<TradeComposite, TradeCompositeModel>().EqualityComparison((dto, m) => dto.Id == m.Id);
        }
    }
}
