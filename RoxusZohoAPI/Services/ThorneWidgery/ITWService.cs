using RoxusZohoAPI.Entities.RoxusDB;
using RoxusZohoAPI.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Services.ThorneWidgery
{
    public interface ITWService
    {
        Task<ApiResultDto<IEnumerable<TWPowerBIRecord>>> GetSuccessfulRecords();
        Task<ApiResultDto<string>> MonitorTWProcess(int numberOfReports);
    }
}
