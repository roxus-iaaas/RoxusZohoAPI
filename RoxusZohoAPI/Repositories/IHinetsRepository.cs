using RoxusZohoAPI.Entities.RoxusDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Repositories
{
    public interface IHinetsRepository
    {
        Task<List<HinetsRecord>> GetHinetsRecordsByDealId(string dealId);
        Task<List<HinetsRecord>> GetHinetsRecordsByDealIdAndType(string dealId, string type);
        Task CreateHinetsRecord(HinetsRecord hinetsRecord);
        Task UpdateHinetsRecord(HinetsRecord hinetsRecord);
    }
}
