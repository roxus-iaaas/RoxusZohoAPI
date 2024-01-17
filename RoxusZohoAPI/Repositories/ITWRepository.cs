using RoxusZohoAPI.Entities.RoxusDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Repositories
{
    public interface ITWRepository
    {
        Task<IEnumerable<TWPowerBIRecord>> GetSuccessfulRecords();
        Task<IEnumerable<TWPowerBIRecord>> GetFailedRecords();
    }
}
