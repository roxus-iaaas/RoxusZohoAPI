using Microsoft.AspNetCore.Mvc;
using RoxusZohoAPI.Helpers.Constants;
using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Services.CompleteASAP;
using RoxusZohoAPI.Services.PureFinance;
using System.Threading.Tasks;
using System;
using RoxusZohoAPI.Models.CompleteASAP.Hoowla;

namespace RoxusZohoAPI.Controllers
{

    [Route("api/asap")]
    [ApiController]
    public class CompleteASAPController : ControllerBase
    {

        private readonly IHoowlaService _hoowlaService;

        public CompleteASAPController(IHoowlaService hoowlaService)
        {
            _hoowlaService = hoowlaService;
        }

        [HttpGet("person/{personId}")]
        public async Task<IActionResult> GetPersonById(string personId)
        {
            var apiResultDto = new ApiResultDto<GetPersonByIdResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CompleteASAPConstants.GPI_400,
                Data = null
            };

            try
            {

                apiResultDto = await _hoowlaService.GetPersonById(personId);

                if (apiResultDto.Code == ResultCode.OK)
                {
                    return Ok(apiResultDto);
                }

                return BadRequest(apiResultDto);

            }
            catch (Exception)
            {

                return BadRequest(apiResultDto);
            
            }
        }

        [HttpPost("people/person")]
        public async Task<IActionResult> PeopleCreateAPersonCard
            ([FromBody] PeopleCreateAPersonCardRequest request)
        {
            var apiResultDto = new ApiResultDto<PeopleCreateAPersonCardResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CompleteASAPConstants.PCAPC_400,
                Data = null
            };

            try
            {

                apiResultDto = await _hoowlaService.PeopleCreateAPersonCard(request);

                if (apiResultDto.Code == ResultCode.OK)
                {
                    return Ok(apiResultDto);
                }

                return BadRequest(apiResultDto);

            }
            catch (Exception)
            {

                return BadRequest(apiResultDto);

            }
        }

        [HttpPost("people/person/relationships")]
        public async Task<IActionResult> PeopleAddRelationshipToPerson
            ([FromBody] PeopleAddRelationshipToPersonRequest addRelationshipRequest)
        {
            var apiResultDto = new ApiResultDto<PeopleAddRelationshipToPersonResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CompleteASAPConstants.PARP_400,
                Data = null
            };
            try
            {
                apiResultDto = await _hoowlaService.PeopleAddRelationshipToPerson(addRelationshipRequest);
                if (apiResultDto.Code == ResultCode.OK)
                {
                    return Ok(apiResultDto);
                }
                return BadRequest(apiResultDto);
            }
            catch (Exception)
            {
                return BadRequest(apiResultDto);
            }
        }

        #region Cases
        [HttpGet("case/{caseId}")]
        public async Task<IActionResult> CasesViewACase(string caseId)
        {

            var apiResultDto = new ApiResultDto<CasesViewACaseResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CompleteASAPConstants.CVAC_400,
                Data = null
            };

            try
            {

                apiResultDto = await _hoowlaService.CasesViewACase(caseId);

                if (apiResultDto.Code == ResultCode.OK)
                {
                    return Ok(apiResultDto);
                }

                return BadRequest(apiResultDto);

            }
            catch (Exception)
            {

                return BadRequest(apiResultDto);

            }
        }

        [HttpPut("case/{caseId}/custom-fields/many")]
        public async Task<IActionResult> CasesUpdateManyCustomFields(string caseId, 
            [FromBody] CasesUpdateManyCustomFieldsRequest updateRequest)
        {

            var apiResultDto = new ApiResultDto<CasesUpdateManyCustomFieldsResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CompleteASAPConstants.CUMCF_400,
                Data = null
            };

            try
            {

                apiResultDto = await _hoowlaService
                    .CasesUpdateManyCustomFields(caseId, updateRequest);

                if (apiResultDto.Code == ResultCode.OK)
                {
                    return Ok(apiResultDto);
                }

                return BadRequest(apiResultDto);

            }
            catch (Exception)
            {

                return BadRequest(apiResultDto);

            }

        }

        [HttpPost("cases/info/user")]
        public async Task<IActionResult> CasesListCasesForUser
            ([FromBody] CasesListCasesForUserRequest listCasesForUserRequest)
        {

            var apiResultDto = new ApiResultDto<CasesListCasesForUserResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CompleteASAPConstants.CLC4U_400,
                Data = null
            };

            try
            {

                apiResultDto = await _hoowlaService.CasesListCasesForUser
                    (listCasesForUserRequest);
                if (apiResultDto.Code == ResultCode.OK)
                {
                    return Ok(apiResultDto);
                }

                return BadRequest(apiResultDto);

            }
            catch (Exception)
            {

                return BadRequest(apiResultDto);

            }

        }

        [HttpGet("case/case-types")]
        public async Task<IActionResult> CasesListCaseTypes()
        {

            var apiResultDto = new ApiResultDto<CasesListCaseTypesResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CompleteASAPConstants.CLCT_400,
                Data = null
            };

            try
            {

                apiResultDto = await _hoowlaService.CasesListCaseTypes();

                if (apiResultDto.Code == ResultCode.OK)
                {
                    return Ok(apiResultDto);
                }

                return BadRequest(apiResultDto);

            }
            catch (Exception)
            {

                return BadRequest(apiResultDto);

            }

        }

        [HttpGet("case/{caseId}/custom-fields")]
        public async Task<IActionResult> CasesListCustomFields(string caseId)
        {

            var apiResultDto = new ApiResultDto<CasesListCustomFieldsResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CompleteASAPConstants.CLN_400,
                Data = null
            };

            try
            {

                apiResultDto = await _hoowlaService.CasesListCustomFields(caseId);

                if (apiResultDto.Code == ResultCode.OK)
                {
                    return Ok(apiResultDto);
                }

                return BadRequest(apiResultDto);

            }
            catch (Exception)
            {

                return BadRequest(apiResultDto);

            }

        }

        [HttpGet("case/{caseId}/notes")]
        public async Task<IActionResult> CasesListNotes(string caseId)
        {

            var apiResultDto = new ApiResultDto<CasesListNotesResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CompleteASAPConstants.CLN_400,
                Data = null
            };

            try
            {

                apiResultDto = await _hoowlaService.CasesListNotes(caseId);

                if (apiResultDto.Code == ResultCode.OK)
                {
                    return Ok(apiResultDto);
                }

                return BadRequest(apiResultDto);

            }
            catch (Exception)
            {

                return BadRequest(apiResultDto);

            }

        }

        [HttpGet("case/{caseId}/tasks")]
        public async Task<IActionResult> CasesListTasks(string caseId)
        {

            var apiResultDto = new ApiResultDto<CasesListTasksResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CompleteASAPConstants.CLN_400,
                Data = null
            };

            try
            {

                apiResultDto = await _hoowlaService.CasesListTasks(caseId);

                if (apiResultDto.Code == ResultCode.OK)
                {
                    return Ok(apiResultDto);
                }

                return BadRequest(apiResultDto);

            }
            catch (Exception)
            {

                return BadRequest(apiResultDto);

            }

        }

        [HttpPost("cases/create")]
        public async Task<IActionResult> CasesCreateANewCase ([FromBody] CasesCreateANewCaseRequest request)
        {
            var apiResultDto = new ApiResultDto<CasesCreateANewCaseResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CompleteASAPConstants.PCAPC_400,
                Data = null
            };

            try
            {
                apiResultDto = await _hoowlaService.CasesCreateANewCase(request);

                if (apiResultDto.Code == ResultCode.OK)
                {
                    return Ok(apiResultDto);
                }

                return BadRequest(apiResultDto);
            }
            catch (Exception)
            {
                return BadRequest(apiResultDto);
            }
        }

        [HttpPost("cases/add-person")]
        public async Task<IActionResult> CasesAddPersonToACase([FromBody] CasesAddPersonToACaseRequest request)
        {
            var apiResultDto = new ApiResultDto<CasesAddPersonToACaseResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CompleteASAPConstants.PCAPC_400,
                Data = null
            };

            try
            {
                apiResultDto = await _hoowlaService.CasesAddPersonToACase(request);

                if (apiResultDto.Code == ResultCode.OK)
                {
                    return Ok(apiResultDto);
                }

                return BadRequest(apiResultDto);
            }
            catch (Exception)
            {
                return BadRequest(apiResultDto);
            }
        }
        #endregion

        [HttpPost("person/byemail")]
        public async Task<IActionResult> GetPersonByEmail
            ([FromBody] GetPersonByEmailRequest getPersonByEmailRequest)
        {
            var apiResultDto = new ApiResultDto<GetPersonByEmailResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = PureFinanceConstants.Airtable_GetToken_400,
                Data = null
            };

            try
            {

                apiResultDto = await _hoowlaService.GetPersonByEmail(getPersonByEmailRequest);

                if (apiResultDto.Code == ResultCode.OK)
                {
                    return Ok(apiResultDto);
                }

                return BadRequest(apiResultDto);

            }
            catch (Exception)
            {

                return BadRequest(apiResultDto);

            }
        }

    }

}
