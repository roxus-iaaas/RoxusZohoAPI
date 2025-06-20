using Microsoft.EntityFrameworkCore;
using RoxusZohoAPI.Contexts;
using RoxusZohoAPI.Entities.RoxusDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Repositories
{

    public class TWRepository : ITWRepository
    {

        private readonly RoxusContext _roxusContext;

        public TWRepository(RoxusContext roxusContext)
        {
            _roxusContext = roxusContext;
        }

        public async Task<IEnumerable<TWPowerBIRecord>> GetFailedRecords()
        {
            
            int currentHour = DateTime.UtcNow.Hour;
            string currentDate = DateTime.UtcNow.ToString("yyyy/MM/dd");
            if (currentHour >= 0 && currentHour <= 10)
            {
                return await _roxusContext.TWPowerBIRecords
                    .FromSqlRaw($"SELECT * FROM TW_PowerBIRecords WHERE IsSuccess = 0 AND StartTime >= '{currentDate} 00:00:00' AND StartTime < '{currentDate} 10:00:00';").ToListAsync();
            }
            else
            {
                return await _roxusContext.TWPowerBIRecords
                    .FromSqlRaw($"SELECT * FROM TW_PowerBIRecords WHERE IsSuccess = 0 AND StartTime >= '{currentDate} 10:00:00' AND StartTime < '{currentDate} 20:00:00';").ToListAsync();
            }

        }

        public async Task<IEnumerable<TWPowerBIRecord>> GetSuccessfulRecords()
        {

            int currentHour = DateTime.UtcNow.Hour;
            string currentDate = DateTime.UtcNow.ToString("yyyy/MM/dd");
            
            if (currentHour >= 18 && currentHour <= 23)
            {
                return await _roxusContext.TWPowerBIRecords
                    .FromSqlRaw($"SELECT * FROM TW_PowerBIRecords WHERE IsSuccess = 1 AND StartTime >= '{currentDate} 18:00:00' AND StartTime < '{currentDate} 23:59:59';").ToListAsync();
            }
            else
            {
                return await _roxusContext.TWPowerBIRecords
                    .FromSqlRaw($"SELECT * FROM TW_PowerBIRecords WHERE IsSuccess = 1 AND StartTime >= '{currentDate} 09:00:00' AND StartTime < '{currentDate} 14:59:59';").ToListAsync();
            }

        }

    }
}
