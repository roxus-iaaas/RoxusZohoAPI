using RoxusZohoAPI.Entities.RoxusDB;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Services.Zoho
{
    public interface IZohoAuthService
    {
        Task<AppConfiguration> GetAccessToken(string apiKey);
    }
}
