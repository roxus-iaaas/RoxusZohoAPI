using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using RoxusZohoAPI.Helpers;
using RoxusZohoAPI.Helpers.Constants;
using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.DateTimeCalculation;
using RoxusZohoAPI.Models.Zoho.Custom;
using RoxusZohoAPI.Models.Zoho.ZohoBooks;
using RoxusZohoAPI.Models.Zoho.ZohoCRM.Hinets;
using RoxusZohoAPI.Models.Zoho.ZohoProjects;
using RoxusZohoAPI.Services.Hinets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Controllers
{
    [Route("api/datetime")]
    [ApiController]
    public class DateTimeController : ControllerBase
    {
        
        public DateTimeController(IHinetsBooksService hinetsBookService, IHinetsProjectsService hinetsProjectsService, IHinetsCustomService hinetsCustomService, IHinetsCrmService hinetsCrmService)
        {
        }

        #region ZohoCRM
        [HttpPost("calculate-month-gaps")]
        public IActionResult CalculateMonthGaps([FromBody] MonthGapsModel monthModel)
        {
            try
            {
                int fromMonth = int.Parse(monthModel.FromMonth);
                int fromYear = int.Parse(monthModel.FromYear);
                int toMonth = int.Parse(monthModel.ToMonth);
                int toYear = int.Parse(monthModel.ToYear);

                var fromDateTime = new DateTime(fromYear, fromMonth, 1);
                var toDateTime = new DateTime(toYear, toMonth, 1);

                return Ok(fromDateTime > toDateTime ? DateTimeHelpers.GetMonthsBetween(fromDateTime, toDateTime)
                    : DateTimeHelpers.GetMonthsBetween(toDateTime, fromDateTime));
            }
            catch (Exception)
            {
                return BadRequest("Calculate Month Gaps FAILED");
            }
        }


        #endregion

        [HttpPost("get-time")]
        public IActionResult GetCurrentTime([FromBody] GetCurrentTimeRequest request)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400,
            };

            try
            {

                string dateTimeNow = DateTimeHelpers.GetCurrentTime(request);

                apiResult.Code = ResultCode.OK;
                apiResult.Message = CommonConstants.MSG_200;
                apiResult.Data = dateTimeNow;

                return Ok(apiResult);

            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return BadRequest(apiResult);
            }
        }

    }
}
