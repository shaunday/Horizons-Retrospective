using AutoMapper;
using AutoMapper.EquivalencyExpression;
using TraJedi.Journal.Data;
using TraJedi.Web.API.Models;

namespace TraJedi.Web.API.Mapping
{
    public class JournalObjectsMappingProfile : Profile
    {
        public JournalObjectsMappingProfile()
        {
            CreateMap<CellContent, CellContentModel>().EqualityComparison((dto, m) => dto.Id == m.Id);
            CreateMap<Cell, CellModel>().EqualityComparison((dto, m) => dto.Id == m.Id);
            CreateMap<TradeComponent, TradeComponentModel>().EqualityComparison((dto, m) => dto.Id == m.Id);
            CreateMap<TradePositionComposite, TradePositionCompositeModel>().EqualityComparison((dto, m) => dto.Id == m.Id);
        }
    }
}
