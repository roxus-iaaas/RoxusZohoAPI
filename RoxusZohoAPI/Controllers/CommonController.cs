using Microsoft.AspNetCore.Mvc;
using RoxusZohoAPI.Helpers;
using RoxusZohoAPI.Helpers.Constants;
using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.DateTimeCalculation;
using RoxusZohoAPI.Services.Hinets;
using System;

namespace RoxusZohoAPI.Controllers
{

    [Route("api/common")]
    [ApiController]
    public class CommonController : ControllerBase
    {

        public CommonController()
        {

        }

        [HttpPost("extract-uk-postcode")]
        public IActionResult ExtractUkPostcode(
            [FromBody] ExtractUKPostcodeRequest request)
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400,
                Data = null
            };

            try
            {

                string address = request.Address;
                string postcode = StringHelpers.ExtractUKPostcode(address);

                apiResult.Code = ResultCode.OK;
                apiResult.Message = CommonConstants.MSG_200;
                apiResult.Data = postcode;

                return Ok(apiResult);

            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return BadRequest(apiResult);
            }
        }

        [HttpPost("convert-email-to-name")]
        public IActionResult ConvertEmailToName(
            [FromBody] ConvertEmailToNameRequest request)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400,
                Data = null
            };
            try
            {
                string email = request.Email;
                string name = StringHelpers.ConvertEmailToName(email);
                apiResult.Code = ResultCode.OK;
                apiResult.Message = CommonConstants.MSG_200;
                apiResult.Data = name;
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
