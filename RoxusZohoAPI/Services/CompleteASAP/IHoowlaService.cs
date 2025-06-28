using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.CompleteASAP.Hoowla;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Services.CompleteASAP
{

    public interface IHoowlaService
    {

        Task<ApiResultDto<CasesViewACaseResponse>> CasesViewACase
            (string caseId);

        Task<ApiResultDto<CasesUpdateManyCustomFieldsResponse>> 
            CasesUpdateManyCustomFields(string caseId, CasesUpdateManyCustomFieldsRequest updateRequest);

        Task<ApiResultDto<CasesListCustomFieldsResponse>> CasesListCustomFields
            (string caseId);

        Task<ApiResultDto<CasesListCasesForUserResponse>> CasesListCasesForUser
            (CasesListCasesForUserRequest listCasesRequest);

        Task<ApiResultDto<CasesListCaseTypesResponse>> CasesListCaseTypes();

        Task<ApiResultDto<CasesListNotesResponse>> CasesListNotes
            (string caseId);

        Task<ApiResultDto<CasesListTasksResponse>> CasesListTasks
            (string caseId);

        Task<ApiResultDto<CasesCreateANewCaseResponse>> CasesCreateANewCase
            (CasesCreateANewCaseRequest casesCreateANewCaseRequest);

        Task<ApiResultDto<CasesAddPersonToACaseResponse>> CasesAddPersonToACase
            (CasesAddPersonToACaseRequest casesAddPersonToACaseRequest);

        Task<ApiResultDto<CasesCreateANoteResponse>> CasesCreateANote
            (CasesCreateANoteRequest casesCreateANoteRequest);

        Task<ApiResultDto<CasesUpdateACaseResponse>> CasesUpdateACase
            (string caseId, CasesUpdateACaseRequest casesUpdateACaseRequest);

        Task<ApiResultDto<CasesUpdateTheCaseWorkerResponse>> CasesUpdateTheCaseWorker
            (string caseId, CasesUpdateTheCaseWorkerRequest casesUpdateTheCaseWorkerRequest);

        Task<ApiResultDto<CasesUpdateATaskResponse>> CasesUpdateATask
            (CasesUpdateATaskRequest casesUpdateATaskRequest);

        Task<ApiResultDto<List<CasesListDocumentEntitiesResponse>>> CasesListDocumentEntities
            (string caseId);

        Task<ApiResultDto<CasesCompleteATaskResponse>> CasesCompleteATask (string taskId);

        Task<ApiResultDto<CasesUpdateTheFeeEarnerResponse>> CasesUpdateTheFeeEarner(string caseId, CasesUpdateTheFeeEarnerRequest casesUpdateTheFeeEarnerRequest);

        Task<ApiResultDto<List<CasesGetBillableInfoByCaseResponse>>> CasesGetBillableInfoByCase (string caseId);

        Task<ApiResultDto<List<QuoteGetCompanyCustomQuoteSituationsResponse>>> QuoteGetCompanyCustomQuoteSituations();

        Task<ApiResultDto<QuoteCalcCreateAQuoteForAPanelResponse>> QuoteCalcCreateAQuoteForAPanel(QuoteCalcCreateAQuoteForAPanelRequest quoteCalcCreateAQuoteForAPanelRequest);

        Task<ApiResultDto<List<UsersListUsersEmployeesResponse>>> UsersListUsersEmployees();

        Task<ApiResultDto<GetPersonByIdResponse>> GetPersonById(string personId);

        Task<ApiResultDto<GetPersonByEmailResponse>> GetPersonByEmail
            (GetPersonByEmailRequest getPersonByEmailRequest);

        Task<ApiResultDto<PeopleCreateAPersonCardResponse>> PeopleCreateAPersonCard
            (PeopleCreateAPersonCardRequest createPersonCardRequest);

        Task<ApiResultDto<PeopleAddRelationshipToPersonResponse>> PeopleAddRelationshipToPerson
            (PeopleAddRelationshipToPersonRequest addRelationshipRequest);

    }

}
