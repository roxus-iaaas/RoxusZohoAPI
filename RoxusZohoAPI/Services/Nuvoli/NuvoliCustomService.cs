using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RoxusZohoAPI.Helpers;
using RoxusZohoAPI.Helpers.Constants;
using RoxusZohoAPI.Models.Common;
using RoxusZohoAPI.Models.DateTimeCalculation;
using RoxusZohoAPI.Models.Nuvoli.Halo;
using RoxusZohoAPI.Models.PureFinance.Pipedrive;
using RoxusZohoAPI.Models.Zoho.ZohoDesk;
using RoxusZohoAPI.Services.Nuvoli.Halo;
using Syncfusion.Compression.Zip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Services.Nuvoli
{
    public class NuvoliCustomService : INuvoliCustomService
    {

        private readonly IHaloService _haloService;

        public NuvoliCustomService(IHaloService haloService)
        {
            _haloService = haloService;
        }

        #region Halo

        public async Task<ApiResultDto<GetTicketByIdResponse>> GetTicketById(string ticketId)
        {

            var apiResult = new ApiResultDto<GetTicketByIdResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = NuvoliConstants.GTBI_400
            };

            try
            {

                var getTicketByIdResponse = await _haloService.GetTicketById(ticketId);

                if (getTicketByIdResponse.Code != ResultCode.OK)
                {
                    apiResult.Message = NuvoliConstants.GTBI_200;
                    return apiResult;
                }

                var ticketDetails = getTicketByIdResponse.Data;

                apiResult.Code = ResultCode.OK;
                apiResult.Message = NuvoliConstants.GTBI_200;
                apiResult.Data = ticketDetails;

                return apiResult;


            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }

        }

        public async Task<ApiResultDto<GetTicketActionsResponse>> GetTicketActions(string ticketId)
        {

            var apiResult = new ApiResultDto<GetTicketActionsResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = NuvoliConstants.GTABI_400
            };

            try
            {

                var getTicketActionsResponse = await _haloService
                    .GetTicketActions(ticketId);

                if (getTicketActionsResponse.Code != ResultCode.OK)
                {
                    apiResult.Message = NuvoliConstants.GTABI_200;
                    return apiResult;
                }

                var ticketDetails = getTicketActionsResponse.Data;

                apiResult.Code = ResultCode.OK;
                apiResult.Message = NuvoliConstants.GTABI_200;
                apiResult.Data = ticketDetails;

                return apiResult;

            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }

        }

        public ApiResultDto<ExtractTicketDetailsResponse> ExtractTicketDetails
            (string ticketDetails)
        {

            var apiResult = new ApiResultDto<ExtractTicketDetailsResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = NuvoliConstants.ETD_400
            };

            try
            {

                string username = string.Empty;
                string userEmailAddress = string.Empty;
                string mobileUsersLineManager = string.Empty;
                string preferredDeliveryAddress = string.Empty;
                string deliveryContactNumber = string.Empty;
                string nhsTicketUrl = string.Empty;
                string postcode = string.Empty;

                var extractTicketResponse = new ExtractTicketDetailsResponse();

                string tempTicketDetails = string.Empty;
                string tempTicketSplit = string.Empty;

                // Step 1: Extract Username
                tempTicketDetails = ticketDetails;
                if (tempTicketDetails.Contains("Username"))
                {
                    tempTicketSplit = tempTicketDetails.Split("Username")[1];
                    if (tempTicketSplit.Contains("User email address"))
                    {
                        if (tempTicketSplit.Contains("5. User email address"))
                        {
                            username = tempTicketSplit.Split("5. User email address")[0];
                        }
                        else
                        {
                            username = tempTicketSplit.Split("User email address")[0];
                        }
                    }
                }
                username = username.Replace("\n", "")
                        .Replace("\r", "").Replace("?", "").Replace(":", "")
                        .Trim();
                if (!string.IsNullOrEmpty(username))
                {
                    username = StringHelpers.ConvertEmailToName(userEmailAddress);
                    extractTicketResponse.username = username;
                }  

                // Step 2: Extract User Email Address
                tempTicketDetails = ticketDetails;
                if (tempTicketDetails.Contains("User email address"))
                {
                    tempTicketSplit = tempTicketDetails.Split("User email address")[1];
                    if (tempTicketSplit.Contains("Mobile Users Line Manager"))
                    {

                        if (userEmailAddress.Contains("6. Mobile Users Line Manager"))
                        {
                            userEmailAddress = tempTicketSplit.Split("6. Mobile Users Line Manager")[0];
                        }
                        else
                        {
                            userEmailAddress = tempTicketSplit.Split("Mobile Users Line Manager")[0];
                        }
                    }
                    else if (tempTicketSplit.Contains("Tablet Users Line Manager"))
                    {
                        if (userEmailAddress.Contains("6. Tablet Users Line Manager"))
                        {
                            userEmailAddress = tempTicketSplit.Split("6. Mobile Users Line Manager")[0];
                        }
                        else
                        {
                            userEmailAddress = tempTicketSplit.Split("Tablet Users Line Manager")[0];
                        }
                    }
                }
                string cleanEmailAddress = userEmailAddress
                    .Replace("\n", "")
                    .Replace("\r", "")
                    .Replace("\u003F", "")
                    .Replace("?", "")
                    .Replace(":", "")
                    .Trim();

                cleanEmailAddress = StringHelpers.ExtractEmail(cleanEmailAddress);
                extractTicketResponse.userEmailAddress = cleanEmailAddress;
                if (!string.IsNullOrEmpty(cleanEmailAddress))
                {
                    username = StringHelpers.ConvertEmailToName(cleanEmailAddress);
                    extractTicketResponse.username = username;
                }    

                // Step 3: Extract Mobile Users Line Manager
                tempTicketDetails = ticketDetails;
                if (tempTicketDetails.Contains("Users Line Manager"))
                {
                    tempTicketSplit = tempTicketDetails.Split("Users Line Manager")[1];
                    if (tempTicketSplit.Contains("Mobile Asset tag"))
                    {
                        if (tempTicketSplit.Contains("7. Mobile Asset tag"))
                        {
                            mobileUsersLineManager = tempTicketSplit.Split("7. Mobile Asset tag")[0];
                        }
                        else
                        {
                            mobileUsersLineManager = tempTicketSplit.Split("Mobile Asset tag")[0];
                        }
                    }
                    else if (tempTicketSplit.Contains("Tablet Asset tag"))
                    {
                        if (tempTicketSplit.Contains("7. Tablet Asset tag"))
                        {
                            mobileUsersLineManager = tempTicketSplit.Split("7. Tablet Asset tag")[0];
                        }
                        else
                        {
                            mobileUsersLineManager = tempTicketSplit.Split("Tablet Asset tag")[0];
                        }
                    }
                }
                mobileUsersLineManager = mobileUsersLineManager
                        .Replace("\n", "").Replace("\r", "").Replace(":", "").Replace("?", "")
                        .Trim();
                extractTicketResponse.mobileUsersLineManager = mobileUsersLineManager;

                // STEP 4: Extract Preferred Delivery Address
                tempTicketDetails = ticketDetails;
                if (tempTicketDetails.Contains("Preferred delivery address"))
                {
                    tempTicketSplit = tempTicketDetails.Split("Preferred delivery address")[1];
                    if (tempTicketSplit.Contains("10. Delivery contact number"))
                    {
                        preferredDeliveryAddress = tempTicketSplit.Split("10. Delivery contact number")[0];
                    }
                    else
                    {
                        preferredDeliveryAddress = tempTicketSplit.Split("Delivery contact number")[0];
                    }
                    
                }
                preferredDeliveryAddress = preferredDeliveryAddress
                        .Replace("\n", "").Replace("\r", "").Replace(":", "")
                        .Trim();
                extractTicketResponse.preferredDeliveryAddress = preferredDeliveryAddress;

                // STEP 5: Extract Delivery Contact Number
                tempTicketDetails = ticketDetails;
                if (tempTicketDetails.Contains("Delivery contact number"))
                {
                    tempTicketSplit = tempTicketDetails.Split("Delivery contact number")[1];
                    deliveryContactNumber = tempTicketSplit.Split("Click for details ")[0];
                    deliveryContactNumber = deliveryContactNumber.Split("Parent ticket")[0];
                    deliveryContactNumber = deliveryContactNumber
                        .Replace("\n", "").Replace("\r", "").Replace("?", "").Replace(":", "")
                        .Trim();
                    deliveryContactNumber = StringHelpers.ExtractUKPhone(deliveryContactNumber);
                }
                extractTicketResponse.deliveryContactNumber = deliveryContactNumber;

                // STEP 6: Extract NHS Ticket URL
                tempTicketDetails = ticketDetails;
                if (tempTicketDetails.Contains("Click for details"))
                {
                    tempTicketSplit = tempTicketDetails.Split("Click for details")[1];
                    nhsTicketUrl = tempTicketSplit.Split("NHS Property Services Ltd")[0];
                    nhsTicketUrl = StringHelpers.ExtractNHSServiceDeskURL(nhsTicketUrl);
                }
                extractTicketResponse.nhsTicketUrl = nhsTicketUrl;

                // STEP 7: Extract Postcode
                if (!string.IsNullOrEmpty(preferredDeliveryAddress))
                {
                    postcode = StringHelpers.ExtractUKPostcode(preferredDeliveryAddress);
                }
                extractTicketResponse.postcode = postcode;

                apiResult.Code = ResultCode.OK;
                apiResult.Message = NuvoliConstants.ETD_200;
                apiResult.Data = extractTicketResponse;

                return apiResult;

            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message}";
                return apiResult; 
            }

        }

        public async Task<ApiResultDto<UpdateTicketResponse>> UpdateTicket(UpdateTicketRequest updateTicketRequest)
        {

            var apiResult = new ApiResultDto<UpdateTicketResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = NuvoliConstants.UHT_400
            };

            try
            {

                var updateTicketResponse = await _haloService.UpdateTicket(updateTicketRequest);

                if (updateTicketResponse.Code != ResultCode.OK)
                {
                    apiResult.Message = NuvoliConstants.THT_400;
                    return apiResult;
                }

                var updateTicketDetails = updateTicketResponse.Data;

                apiResult.Code = ResultCode.OK;
                apiResult.Message = NuvoliConstants.UHT_200;
                apiResult.Data = updateTicketDetails;

                return apiResult;


            }
            catch (Exception ex)
            {

                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            
            }

        }

        public async Task<ApiResultDto<GetCannedTextByIdResponse>> GetCannedTextById(string cannedTextId)
        {

            var apiResult = new ApiResultDto<GetCannedTextByIdResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = NuvoliConstants.UHT_400
            };

            try
            {

                var updateTicketResponse = await _haloService.GetCannedTextById(cannedTextId);

                if (updateTicketResponse.Code != ResultCode.OK)
                {
                    apiResult.Message = NuvoliConstants.THT_400;
                    return apiResult;
                }

                var updateTicketDetails = updateTicketResponse.Data;

                apiResult.Code = ResultCode.OK;
                apiResult.Message = NuvoliConstants.UHT_200;
                apiResult.Data = updateTicketDetails;

                return apiResult;


            }
            catch (Exception ex)
            {

                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;

            }

        }

        public async Task<ApiResultDto<string>> ExecuteAction(string requestBody)
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = NuvoliConstants.ETA_400
            };

            try
            {

                var getTicketByIdResponse = await _haloService.ExecuteTicketActions(requestBody);

                if (getTicketByIdResponse.Code != ResultCode.OK)
                {
                    return apiResult;
                }

                var ticketDetails = getTicketByIdResponse.Data;

                apiResult.Code = ResultCode.OK;
                apiResult.Message = NuvoliConstants.ETA_200;
                apiResult.Data = ticketDetails;

                return apiResult;

            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
        }

        #endregion

        #region Custom Functions

        public async Task<ApiResultDto<string>> HandleNuvoliStarterPhase1(string ticketId)
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = NuvoliConstants.NNSP1_400
            };

            try
            {

                // STEP 1: Extract ticket details
                // Step 1.1: Get Ticket by Id
                var getTicketByIdResponse = await _haloService.GetTicketById(ticketId);
                if (getTicketByIdResponse.Code != ResultCode.OK)
                {
                    apiResult.Message = NuvoliConstants.NNSP1_E01;
                    return apiResult;
                }

                // TODO: Add Ticket Information to Transaction Table

                var ticketData = getTicketByIdResponse.Data;
                string ticketDetails = ticketData.details;

                // Step 1.2: Extract data from ticket details
                string username = string.Empty;
                string userEmailAddress = string.Empty;
                string mobileUsersLineManager = string.Empty;
                string preferredDeliveryAddress = string.Empty;
                string deliveryContactNumber = string.Empty;
                string nhsTicketUrl = string.Empty;

                string tempTicketDetails = string.Empty;
                string tempTicketSplit = string.Empty;

                // Step 1.2.1: Extract Username
                tempTicketDetails = ticketDetails;
                tempTicketSplit = tempTicketDetails.Split("Username:")[1];
                username = tempTicketSplit.Split("User email address:")[0];
                username = username.Replace(" ", "").Replace("\n", "").Replace("\r", "");

                // Step 1.2.2: Extract User Email Address
                tempTicketDetails = ticketDetails;
                tempTicketSplit = tempTicketDetails.Split("User email address:")[1];
                userEmailAddress = tempTicketSplit.Split("Mobile Users Line Manager:")[0];
                userEmailAddress = userEmailAddress.Replace(" ", "").Replace("\n", "").Replace("\r", "");

                // Step 1.2.3: Extract Mobile Users Line Manager
                tempTicketDetails = ticketDetails;
                tempTicketSplit = tempTicketDetails.Split("Mobile Users Line Manager:")[1];
                mobileUsersLineManager = tempTicketSplit.Split("Mobile Asset tag:")[0];
                mobileUsersLineManager = mobileUsersLineManager.Replace(" ", "").Replace("\n", "").Replace("\r", "");

                // STEP 1.2.4: Extract Preferred Delivery Address
                tempTicketDetails = ticketDetails;
                tempTicketSplit = tempTicketDetails.Split("Preferred delivery address:")[1];
                preferredDeliveryAddress = tempTicketSplit.Split("Delivery contact number:")[0];
                preferredDeliveryAddress = mobileUsersLineManager.Replace(" ", "").Replace("\n", "").Replace("\r", "");

                // STEP 1.2.5: Extract Delivery Contact Number
                tempTicketDetails = ticketDetails;
                tempTicketSplit = tempTicketDetails.Split("Delivery contact number:")[1];
                deliveryContactNumber = tempTicketSplit.Split("Click for details :")[0];
                deliveryContactNumber = preferredDeliveryAddress.Replace(" ", "").Replace("\n", "").Replace("\r", "");

                // TODO: Call API to trigger the process in case Delivery Address or Delivery Contact Number is EMPTY

                // STEP 1.2.6: Extract NHS Ticket URL
                tempTicketDetails = ticketDetails;
                tempTicketSplit = tempTicketDetails.Split("Click for details :")[1];
                nhsTicketUrl = tempTicketSplit.Split("NHS Property Services Ltd")[0];
                nhsTicketUrl = nhsTicketUrl.Replace(" ", "").Replace("\n", "").Replace("\r", "");

                // STEP 2: Triage Ticket
                
                
                // STEP 3: Update Ticket Details
                var updateTicketRequest = new UpdateTicketRequest();
                var customFields = new List<UpdateCustomField>();
                // Step 3.1: Delivery Details
                // Step 3.1.1: Delivery Address
                var customField = new UpdateCustomField()
                {
                    id = NuvoliConstants.Halo_DeliveryDetails_ContactEmailId,
                    value = userEmailAddress
                };
                customFields.Add(customField);

                // Step 3.1.2: Postcode
                string postcode = StringHelpers.ExtractUKPostcode(preferredDeliveryAddress);
                // TODO: Call API to trigger the process in case Post Code is EMPTY

                customField = new UpdateCustomField()
                {
                    id = NuvoliConstants.Halo_DeliveryDetails_PostcodeId,
                    value = postcode
                };
                customFields.Add(customField);

                // Step 3.1.3: Contact Name
                string contactName = $"{mobileUsersLineManager}/{username}";
                customField = new UpdateCustomField()
                {
                    id = NuvoliConstants.Halo_DeliveryDetails_ContactNameId,
                    value = username
                };
                customFields.Add(customField);

                // Step 3.1.4: Contact Number
                customField = new UpdateCustomField()
                {
                    id = NuvoliConstants.Halo_DeliveryDetails_ContactNumberId,
                    value = deliveryContactNumber
                };
                customFields.Add(customField);

                // Step 3.2: Connection Details
                // Step 3.2.1: Username
                customField = new UpdateCustomField()
                {
                    id = NuvoliConstants.Halo_ConnectionDetails_UsernameId,
                    value = username
                };
                customFields.Add(customField);

                // Step 3.2.2: User Email Address
                customField = new UpdateCustomField()
                {
                    id = NuvoliConstants.Halo_ConnectionDetails_UserEmailAddressId,
                    value = userEmailAddress
                };
                customFields.Add(customField);

                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }

        }

        public async Task<ApiResultDto<ExtractPhase2Response>>
            HandleEmailUpdatePhase2(string ticketId)
        {

            var apiResult = new ApiResultDto<ExtractPhase2Response>()
            {
                Code = ResultCode.BadRequest,
                Message = NuvoliConstants.HEOP2_400
            };

            try
            {

                var extractPhase2 = new ExtractPhase2Response();

                // Step 1: Get Ticket Details
                var getTicketByIdResponse = await _haloService
                    .GetTicketById(ticketId);

                if (getTicketByIdResponse.Code != ResultCode.OK)
                {
                    apiResult.Message = NuvoliConstants.HEOP2_E01;
                    return apiResult;
                }

                var ticketData = getTicketByIdResponse.Data;
                var customFields = ticketData.customfields;

                string currentConsignment = string.Empty;
                string currentMobileNumber = string.Empty;
                string username = string.Empty;
                string nhsTicketUrl = string.Empty;
                string postcode = string.Empty;
                string deviceType = "Mobile";

                string ticketSummary = ticketData.summary;
                if (ticketSummary.Contains("Tablet Device",
                    StringComparison.InvariantCultureIgnoreCase))
                {
                    deviceType = "Tablet";
                }
                extractPhase2.DeviceType = deviceType;

                extractPhase2.ReferenceNumber = string.Empty;
                extractPhase2.NewMobileNumber = string.Empty;
                extractPhase2.Username = string.Empty;
                extractPhase2.Postcode = string.Empty;
                extractPhase2.TicketId = ticketId;

                apiResult.Data = extractPhase2;

                string ticketDetails = ticketData.details;
                nhsTicketUrl = StringHelpers.ExtractNHSServiceDeskURL(ticketDetails);
                extractPhase2.NHSUrl = nhsTicketUrl;

                foreach (var customField in customFields)
                {
                    string customFieldId = customField.id.ToString();
                    string customFieldValue = customField.value;

                    if (customFieldId ==
                        NuvoliConstants.Halo_DeliveryDetails_ConsignmentId)
                    {
                        currentConsignment = customFieldValue;
                        if (!string.IsNullOrEmpty(currentConsignment))
                        {
                            apiResult.Code = ResultCode.OK;
                            apiResult.Message = NuvoliConstants.HEOP2_200;
                            return apiResult;
                        }
                    }
                    else if (customFieldId ==
                        NuvoliConstants.Halo_ConnectionDetails_MobileNumberId)
                    {
                        currentMobileNumber = customFieldValue;
                    }
                    else if (customFieldId ==
                        NuvoliConstants.Halo_ConnectionDetails_UsernameId)
                    {
                        username = customFieldValue;
                    }
                    else if (customFieldId ==
                        NuvoliConstants.Halo_DeliveryDetails_PostcodeId)
                    {
                        postcode = customFieldValue;
                    }

                }
                extractPhase2.ReferenceNumber = currentConsignment;
                extractPhase2.NewMobileNumber = currentMobileNumber;
                extractPhase2.Username = username;
                extractPhase2.Postcode = postcode;
                extractPhase2.TicketId = ticketId;

                // Step 2: Get all Ticket Actions
                string referenceNumber = string.Empty;
                string mobileNumber = string.Empty;

                var getTicketActionsResponse = await _haloService
                    .GetTicketActions(ticketId);

                if (getTicketActionsResponse.Code != ResultCode.OK)
                {
                    apiResult.Message = NuvoliConstants.GTABI_200;
                    return apiResult;
                }

                var ticketActions = getTicketActionsResponse.Data.actions;

                foreach (var action in ticketActions)
                {
                    string outcome = action.outcome;
                    if (outcome != "Email Update" && outcome != "Supplier Update")
                    {
                        // Outcome MUST BE Email Update
                        continue;
                    }
                    string who = action.who;
                    if (!who.Equals("nhsproperty@o2.com",
                        StringComparison.InvariantCultureIgnoreCase) && 
                        !who.Equals("Supplier: NHSPS O2 Service Team", StringComparison.InvariantCultureIgnoreCase))
                    {
                        // Who MUST BE nhsproperty@o2.com
                        continue;
                    }

                    string note = action.note;
                    if (!note.Contains("Reference Number ="))
                    {
                        // Note MUST CONTAIN Reference Number =
                        continue;
                    }

                    var result = StringHelpers.ExtractReferenceAndNumber(note);
                    if (result != null)
                    {
                        mobileNumber = result.Item1;
                        referenceNumber = result.Item2;
                    }

                }

                apiResult.Code = ResultCode.OK;
                apiResult.Message = NuvoliConstants.HEOP2_200;
                // apiResult.Data = ticketDetails;

                // Step 3: Check if should update ticket
                bool shouldUpdateTicket = false;
                var updateTicketRequest = new UpdateTicketRequest();
                var ticketCustomFields = new List<UpdateCustomField>();
                if (!string.IsNullOrEmpty(mobileNumber)
                    && currentMobileNumber != mobileNumber)
                {
                    extractPhase2.NewMobileNumber = mobileNumber;
                    shouldUpdateTicket = true;
                    var ticketCustomField = new UpdateCustomField();
                    ticketCustomField.id = NuvoliConstants.Halo_ConnectionDetails_MobileNumberId;
                    ticketCustomField.value = mobileNumber;
                    ticketCustomFields.Add(ticketCustomField);
                }
                if (!string.IsNullOrEmpty(referenceNumber)
                    && string.IsNullOrEmpty(currentConsignment))
                {
                    extractPhase2.ReferenceNumber = referenceNumber;
                    shouldUpdateTicket = true;
                    var ticketCustomField = new UpdateCustomField();
                    ticketCustomField.id = NuvoliConstants.Halo_DeliveryDetails_ConsignmentId;
                    ticketCustomField.value = referenceNumber;
                    ticketCustomFields.Add(ticketCustomField);
                }

                updateTicketRequest.id = ticketId;
                updateTicketRequest.customfields = ticketCustomFields;
                if (shouldUpdateTicket)
                {
                    var updateTicketResponse = await _haloService
                        .UpdateTicket(updateTicketRequest);
                    if (updateTicketResponse.Code != ResultCode.OK)
                    {
                        apiResult.Message = NuvoliConstants.HEOP2_E02;
                        return apiResult;
                    }
                }

                apiResult.Code = ResultCode.OK;
                apiResult.Message = NuvoliConstants.HEOP2_200;
                apiResult.Data = extractPhase2;

                return apiResult;

            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
        }

        public async Task<ApiResultDto<string>> HandleAssetPhase2(string ticketId)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = NuvoliConstants.HAP2_400
            };

            try
            {

                // STEP 1: Get Ticket Details by Id
                var getTicketByIdResponse = await _haloService.GetTicketById(ticketId);
                if (getTicketByIdResponse.Code != ResultCode.OK)
                {
                    apiResult.Message = NuvoliConstants.HAP2_E01;
                    return apiResult;
                }

                // STEP 2: Extract Ticket Details
                var ticketDetails = getTicketByIdResponse.Data;
                string summary = ticketDetails.summary;
                var customFields = ticketDetails.customfields;

                string mobileNumber = string.Empty;
                string username = string.Empty;
                string email = string.Empty;

                foreach (var customField in customFields)
                {
                    string customFieldId = customField.id.ToString();
                    string customFieldValue = customField.value;

                    if (customFieldId ==
                        NuvoliConstants.Halo_ConnectionDetails_MobileNumberId)
                    {
                        mobileNumber = customFieldValue;
                    }
                    else if (customFieldId ==
                        NuvoliConstants.Halo_ConnectionDetails_UsernameId)
                    {
                        username = customFieldValue;
                    }
                    else if (customFieldId ==
                        NuvoliConstants.Halo_ConnectionDetails_UserEmailAddressId)
                    {
                         email = customFieldValue;
                    }

                }

                if (!string.IsNullOrEmpty(email))
                {
                    username = StringHelpers.ConvertEmailToName(email);
                }

                // STEP 3: List Users by Username
                var listUserRequest = new ListUsersRequest()
                {
                    SiteId = NuvoliConstants.NHS_SiteId,
                    Username = username
                };
                var listUsersResponse = await _haloService.ListUsers(listUserRequest);
                if (listUsersResponse.Code != ResultCode.OK)
                {
                    apiResult.Message = NuvoliConstants.HAP2_E02;
                    return apiResult;
                }

                var users = listUsersResponse.Data.users;
                string userId = string.Empty;

                foreach (var user in users)
                {

                    string currentId = user.id.ToString();
                    string currentEmail = user.emailaddress;
                    if (!string.IsNullOrEmpty(currentEmail) && currentEmail == email) { 
                        userId = currentId;
                        break;
                    }

                }

                // STEP 4: Create User in case User Id is EMPTY
                if (string.IsNullOrEmpty(userId))
                {

                    var createUserData = new CreateUserData()
                    {
                        name = username,
                        emailaddress = email,
                        site_id = int.Parse(NuvoliConstants.NHS_SiteId)
                    };

                    var createUserResponse = await _haloService.CreateUser(createUserData);

                    if (createUserResponse.Code != ResultCode.OK)
                    {
                        apiResult.Message = NuvoliConstants.HAP2_E03;
                        return apiResult;
                    }

                    userId = createUserResponse.Data.id.ToString();

                }

                // STEP 5: List Assets by Inventory Number
                string assetId = string.Empty;
                var listAssetsRequest = new ListAssetsRequest()
                {
                    ClientId = NuvoliConstants.NHS_ClientId,
                    InventoryNumber = mobileNumber
                };

                var listAssetsResponse = await _haloService.ListAssets(listAssetsRequest);
                if (listAssetsResponse.Code != ResultCode.OK)
                {
                    apiResult.Message = NuvoliConstants.HAP2_E04;
                    return apiResult;
                }

                var assets = listAssetsResponse.Data.assets;
                if (assets.Count > 0)
                {
                    var firstAsset = assets[0];
                    assetId = firstAsset.id.ToString();
                }

                // STEP 6: Handle Upsert Asset
                var upsertAssetData = new UpsertAssetData();
                if (!string.IsNullOrEmpty(assetId))
                {
                    upsertAssetData.id = int.Parse(assetId);
                }
                var assetUser = new AssetUser()
                {
                    id = int.Parse(userId)
                };
                var assetUsers = new List<AssetUser>
                {
                    assetUser
                };
                upsertAssetData.users = assetUsers;
                upsertAssetData.assettype_id = int.Parse(NuvoliConstants.NHS_Asset_TypeId);
                upsertAssetData.site_id = int.Parse(NuvoliConstants.NHS_SiteId);
                upsertAssetData.inventory_number = mobileNumber;
                upsertAssetData.status_id = int.Parse(NuvoliConstants.NHS_Asset_StatusId);
                
                var assetFields = new List<UpsertAssetField>();
                var assetField = new UpsertAssetField()
                {
                    id = NuvoliConstants.NHS_Asset_Mobile_FieldId,
                    value = mobileNumber
                };
                assetFields.Add(assetField);

                assetField = new UpsertAssetField()
                {
                    id = NuvoliConstants.NHS_Asset_Username_FieldId,
                    value = username
                };
                assetFields.Add(assetField);

                assetField = new UpsertAssetField()
                {
                    id = NuvoliConstants.NHS_Asset_Provider_FieldId,
                    value = "O2 - Direct"
                };
                assetFields.Add (assetField);

                upsertAssetData.fields = assetFields;

                var upsertAssetResponse = await _haloService.UpsertAsset(upsertAssetData);
                if (upsertAssetResponse.Code != ResultCode.OK)
                {
                    apiResult.Message = NuvoliConstants.HAP2_E05;
                    return apiResult;
                }

                // STEP 7: Handle Create Child Ticket
                var createChildTicketData = new CreateChildTicketData();
                createChildTicketData.tickettype_id = 2;
                // Step 7.1: Handle Summary
                string requestId = StringHelpers.ExtractRequestId(summary);
                createChildTicketData.summary = $"NHSP - {username} - Add user to AD - {requestId}";
                createChildTicketData.category_1 = "Non BAU";
                createChildTicketData.status_id = 1;
                createChildTicketData.client_id = int.Parse(NuvoliConstants.NHS_ClientId);
                createChildTicketData.site_id = int.Parse(NuvoliConstants.NHS_SiteId);
                createChildTicketData.parent_id = int.Parse(ticketId);
                createChildTicketData.details_html = $"We need to add {mobileNumber} " +
                    $"to {username} on AD, New starter ticket is {ticketId}";
                createChildTicketData.team_id = 22;
                var createChildTicketResponse = await _haloService
                    .CreateChildTicket(createChildTicketData);
                if (createChildTicketResponse.Code != ResultCode.OK)
                {
                    apiResult.Message = NuvoliConstants.HAP2_E06;
                    return apiResult;
                }

                // STEP 8: Handle Create Internal Note
                // Step 8.1: Get Canned Text by Id
                var getCannedTextByIdResponse = await _haloService
                    .GetCannedTextById(NuvoliConstants.InternalNote_NewStarter_CannedTextId);
                if (getCannedTextByIdResponse.Code != ResultCode.OK)
                {
                    apiResult.Message = NuvoliConstants.HAP2_E07;
                    return apiResult;
                }
                var cannedTextDetails = getCannedTextByIdResponse.Data;
                string cannedTextHtml = cannedTextDetails.html;

                string textHtml = cannedTextHtml.Replace("{username}", username)
                    .Replace("1. Check all details are provided -&nbsp;",
                        "1. Check all details are provided -&nbsp;Complete")
                    .Replace("2. Send request to O2 -&nbsp;",
                        "2. Send request to O2 -&nbsp;Complete")
                    .Replace("3. Add consignment number onto delivery details and new mobile number onto connection details -&nbsp;",
                        "3. Add consignment number onto delivery details and new mobile number onto connection details -&nbsp;Complete")
                    .Replace("Update ManagedEngine ticket for dispatch -&nbsp;",
                        "Update ManagedEngine ticket for dispatch -&nbsp;Complete")
                    .Replace("Halo asset added -&nbsp;", 
                        "Halo asset added -&nbsp;Complete")
                    .Replace("6. Add mobile number to AD using Remote Desktop -&nbsp;",
                        "6. Add mobile number to AD using Remote Desktop -&nbsp;Complete");

                var createInternalNoteData = new CreateInteralNoteData()
                {
                    ticket_id = ticketId,
                    new_status = "5",
                    outcome_id = "7",
                    note_html = textHtml
                };
                var createInternalNoteRequest = new List<CreateInteralNoteData>
                {
                    createInternalNoteData
                };
                string createInternalNoteStr = JsonConvert.SerializeObject(createInternalNoteRequest);
                var createInternalNoteResponse = await _haloService
                    .ExecuteTicketActions(createInternalNoteStr);
                if (createInternalNoteResponse.Code != ResultCode.OK)
                {
                    apiResult.Message = NuvoliConstants.HAP2_E08;
                    return apiResult;
                }

                apiResult.Code = ResultCode.OK;
                apiResult.Message = NuvoliConstants.HAP2_200;
                return apiResult;
            
            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }

        }

        public async Task<ApiResultDto<string>> HandleSendEmail(HandleSendEmailRequest sendEmailRequest)
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = NuvoliConstants.HSE_400
            };

            try
            {

                string cannedTextId = sendEmailRequest.CannedTextId;
                string ticketId = sendEmailRequest.TicketId;
                string emailTo = sendEmailRequest.EmailTo;

                // STEP 1: Get Canned Text by Id
                if (string.IsNullOrEmpty(cannedTextId))
                {
                    apiResult.Message = NuvoliConstants.HSE_E01;
                    return apiResult;
                }    

                var getCannedTextByIdResponse = await _haloService
                    .GetCannedTextById(cannedTextId);

                string cannedTextHtml = getCannedTextByIdResponse.Data.html;

                // STEP 2: Get Time Code 1
                var getTimeRequest = new GetCurrentTimeRequest()
                {
                    TimeZone = "GMT Standard Time",
                    AddDay = 2,
                    Format = "yyyy-MM-dd"
                };
                var getTimeResponse = DateTimeHelpers
                    .GetCurrentTime(getTimeRequest);
                string followUpDate = getTimeResponse;
                followUpDate = followUpDate + "T09:00:00.000";

                // STEP 3: Before Email Supplier
                var beforeEmailRequest = new dynamic[]
                {
                    new {
                        ticket_id = ticketId,
                        outcome_id = "6",
                        emailto = emailTo,
                        from_mailbox_id = 1,
                        from_address_override = "support@nuvoli.net",
                        new_agent = 142,
                        appointment_complete_status = 0,
                        sendemail = true,
                        hiddenfromuser = true,
                        important = false,
                        follow = false,
                        send_survey = false,
                        actioncalendarstatus = 0,
                        new_status = 5,
                        new_slastatus = 1,
                        chargerate = 0,
                        action_showpreview = true,
                        new_supplier_contact_id = '0',
                        note_html = cannedTextHtml,
                        utcoffset = -420,
                        dont_do_rules = true,
                        _validate_travel = true
                    }
                };
                string beforeEmailRequestStr = JsonConvert.SerializeObject(beforeEmailRequest);

                var beforeEmailResponse = await _haloService.ExecuteTicketActions(beforeEmailRequestStr);

                if (beforeEmailResponse.Code != ResultCode.OK)
                {
                    apiResult.Message = NuvoliConstants.HSE_E03;
                    return apiResult;
                }
                var beforeEmail = JObject.Parse(beforeEmailResponse.Data);
                var firstBeforeEmail = beforeEmail;

                string emailNote = (string) firstBeforeEmail["note"];
                string emailNoteHtml = (string)firstBeforeEmail["note_html"];
                string emailSubject = (string) firstBeforeEmail["emailsubject"];
                string emailBody = (string) firstBeforeEmail["emailbody"];
                string emailBodyHtml = (string) firstBeforeEmail["emailbody_html"];

                // STEP 4: Send Email

                var emailRequest = new dynamic[]
                {
                    new {
                        ticket_id = ticketId,
                        sendemail = true,
                        outcome_id = 6,
                        emailto = emailTo,
                        from_mailbox_id = 1,
                        from_address_override = "support@nuvoli.net",
                        new_agent = 142,
                        appointment_complete_status = 0,
                        hiddenfromuser = true,
                        important = false,
                        follow = false,
                        send_survey = false,
                        actioncalendarstatus = 0,
                        new_status = 5,
                        emailsubject = emailSubject,
                        new_slastatus = -1,
                        chargerate = 0,
                        action_showpreview = false,
                        timerinuse = true,
                        _ignore_ai = true,
                        new_supplier_contact_id = 0,
                        note_html = emailNoteHtml,
                        new_followupdate = followUpDate,
                        utcoffset = -420,
                        outcome = "Email Supplier",
                        who = "Roxus",
                        who_type = 1,
                        who_agentid = 142,
                        note = emailNote,
                        replied_to_ticket_id = 0,
                        replied_to_action_id = 0,
                        created_from_ticket_id = 0,
                        created_from_action_id = 0,
                        action_contract_id = -1,
                        action_travel_contract_id = -1,
                        project_id = 539295,
                        actionby_agent_id = 142,
                        actioncontractid = 0,
                        actisbillable = true,
                        actisreadyforprocessing = false,
                        travelisreadyforprocessing = false,
                        emailbody = emailBody,
                        emailbody_html = emailBodyHtml,
                        actioninvoicenumber_isproject = false,
                        actiontravelamount = 0,
                        actionmileageamount = 0,
                        actionbillingplanid = 0,
                        actionsmsstatus = 0,
                        asset_id = 0,
                        asset_site=  0,
                        lwwarrantyreported = false,
                        labourwarranty = false,
                        actreviewed = true,
                        reply_direct = false,
                        actioninformownerofaction = false,
                        agentnotificationneeded = 0,
                        travel_surchargeid = 0,
                        achargetotalprocessed = false,
                        emailtemplate_id = 62,
                        _canupdate = true,
                        dont_do_rules = true,
                        _validate_travel = true,
                        action_systemid = 18,
                        timetakendays = 0,
                        timetakenadjusted = 0,
                        nonbilltime = 0,
                        old_status = 5,
                        emailfrom = "support@nuvoli.net",
                        emailtonew = emailTo,
                        emailccnew = "",
                        emaildirection = 0,
                        emailsubjectnew = emailSubject,
                        colour = "#fe9200",
                        attachment_count = 0,
                        unread = 1,
                        actionby_application_id = "nethelpdesk-agent-web-application",
                        actionby_user_id = 0,
                        isemailpreview = true
                    }
                };
                
                string emailRequestBody = JsonConvert.SerializeObject(emailRequest);
                var sendEmailResponse = await _haloService.ExecuteTicketActions(emailRequestBody);
                if (sendEmailResponse.Code != ResultCode.OK)
                {
                    apiResult.Message = NuvoliConstants.HSE_E04;
                    return apiResult;
                }    

                apiResult.Message = NuvoliConstants.HSE_200;
                apiResult.Code = ResultCode.OK;

                return apiResult;
            
            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }

        }

        #endregion

    }
}
