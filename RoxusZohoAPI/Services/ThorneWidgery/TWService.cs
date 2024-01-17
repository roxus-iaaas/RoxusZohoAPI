using Newtonsoft.Json;
using RoxusZohoAPI.Entities.RoxusDB;
using RoxusZohoAPI.Helpers;
using RoxusZohoAPI.Helpers.Constants;
using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Services.ThorneWidgery
{
    public class TWService : ITWService
    {
        private readonly ITWRepository _twRepository;
        private readonly IRoxusLoggingRepository _roxusLoggingRepository;

        public TWService(ITWRepository twRepository, IRoxusLoggingRepository roxusLoggingRepository)
        {
            _twRepository = twRepository;
            _roxusLoggingRepository = roxusLoggingRepository;
        }

        public async Task<ApiResultDto<IEnumerable<TWPowerBIRecord>>> GetSuccessfulRecords()
        {
            var apiResult = new ApiResultDto<IEnumerable<TWPowerBIRecord>>()
            {
                Code = ResultCode.BadRequest,
                Message = "FAILED"
            };

            try
            {
                var twRecords = await _twRepository.GetSuccessfulRecords();
                apiResult.Code = ResultCode.OK;
                apiResult.Message = "SUCCESS";
                apiResult.Data = twRecords;
                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message}\n{ex.StackTrace}";
                return apiResult;
            }
        }

        public async Task<ApiResultDto<string>> MonitorTWProcess(int numberOfReports)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = "FAILED"
            };
            try
            {
                string currentDate = DateTime.UtcNow.ToString("dd-MM-yyyy");
                int currentHour = DateTime.UtcNow.Hour;
                string dayTime = string.Empty;
                if (currentHour >= 18 && currentHour < 23)
                {
                    dayTime = "Morning";
                }
                else
                {
                    dayTime = "Afternoon";
                }

                var emailContent = new EmailContent()
                {
                    Email = EmailConstants.TwEmail,
                    Body = string.Empty,
                    Clients = "help@roxus.io",
                    // Clients = "hoangtran7292@gmail.com",
                    Subject = $"[TW] Refresh Report Error Alert on {currentDate} - {dayTime}",
                    SmtpPort = EmailConstants.SmtpPort,
                    SmtpServer = EmailConstants.Outlook_Email_SmtpServer
                };
                var appConfiguration = await _roxusLoggingRepository.GetAppConfigurationById(EmailConstants.TwId);
                emailContent.Password = appConfiguration.Password;

                var twSuccessRecords = await _twRepository.GetSuccessfulRecords();
                int numberOfSuccess = twSuccessRecords.Count();

                if (numberOfSuccess <= numberOfReports)
                {
                    var twFailedRecords = await _twRepository.GetFailedRecords();
                    string emailBody = $"Dear Roxus Team,<br><br>Please check the issue with TW Refresh on {currentDate}, details below:<br><b>Number of successful reports</b>: {numberOfSuccess}/{numberOfReports}<br><b>Failed Records (JSON Format):</b><br>{JsonConvert.SerializeObject(twFailedRecords)}<br><br>Thanks & Regards,<br>Roxus Automation";
                    emailContent.Body = emailBody;
                    apiResult.Message = "TW Reports refreshed failed.";
                    await EmailHelpers.SendEmail(emailContent);
                    return apiResult;
                }
                apiResult.Code = ResultCode.OK;
                apiResult.Message = "TW Reports refreshed successfully.";
                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message}\n{ex.StackTrace}";
                return apiResult;
            }
        }
    }
}
