using Microsoft.EntityFrameworkCore;
using RoxusZohoAPI.Contexts;
using RoxusZohoAPI.Entities.RoxusDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Repositories
{
    public class HinetsRepository : IHinetsRepository
    {
        private readonly RoxusContext _roxusContext;

        public HinetsRepository(RoxusContext roxusContext)
        {
            _roxusContext = roxusContext;
        }

        public async Task<List<HinetsRecord>> GetHinetsRecordsByDealId(string dealId)
        {
            return await _roxusContext.HinetsRecords
                .Where(hr => hr.OpportunityId == dealId)
                .ToListAsync();
        }

        public async Task<List<HinetsRecord>> GetHinetsRecordsByDealIdAndType(string dealId, string type)
        {
            return await _roxusContext.HinetsRecords
                .Where(hr => hr.OpportunityId == dealId && hr.ModuleType == type)
                .ToListAsync();
        }

        public async Task CreateHinetsRecord(HinetsRecord hinetsRecord)
        {
            _roxusContext.Add(hinetsRecord);
            await _roxusContext.SaveChangesAsync();
        }

        public async Task UpdateHinetsRecord(HinetsRecord hinetsRecord)
        {
            await _roxusContext.SaveChangesAsync();
        }
    }
}
