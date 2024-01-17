using RoxusZohoAPI.Entities.RoxusDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Services.MicrosoftGraph
{
    public interface IMicrosoftGraphAuthService
    {

        Task<AppConfiguration> GetAccessToken(string apiKey);

    }
}
