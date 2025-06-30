using HsR.Journal.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HsR.Journal.Repository.Services.CompositeRepo
{
    public interface ITradeCompositeRepository
    {
        Task<UpdatedStatesCollation> CloseTradeAsync(Guid userId, string tradeId, string closingPrice);
    }
}
