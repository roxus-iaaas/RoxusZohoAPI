using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.CompanyHouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Services.CompaniesHouse
{
    public interface ICompaniesHouseService
    {

        public Task<ApiResultDto<SearchCompaniesResponse>> GetCompanies(string query);

    }
}
