using System;
using System.Threading.Tasks;

namespace AssetsFlow.Server.AssetsFlow.Journal.HsR.Journal.Repository.Services.DataElementRepo
{
    public interface IDataElementRepository
    {
        Task<(DataElement updatedCell, UpdatedStatesCollation updatedStates)> UpdateCellContentAsync(int componentId, string newContent, string changeNote);
    }
} 