using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Security.Principal;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;
using System.Net;
using System.Data;
using System.Web;
using System.Resources;
using System.Web.Security;
using Imis.Domain;
using StudentPractice.Utils;
using log4net;
using StudentPractice.Mails;
using StudentPractice.BusinessModel;
using StudentPractice.BusinessModel.Flow;
using System.IO;


namespace StudentPractice.Web.Api
{
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public partial class StudentPractiseApiOffice : StudentPractiseApiBase
    {
        #region [ Auth Services ]

        [WebInvoke(ResponseFormat = WebMessageFormat.Json)]
        public virtual ServiceResponse<AuthenticationResult> Login(AuthenticationRequest request)
        {
            bool auth = false;
            if (request == null || string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                throw new WebFaultException(HttpStatusCode.BadRequest);

            try
            {
                var office = _officeRepository.Value.FindByUsername(request.Username);
                if (office != null)
                    auth = Membership.ValidateUser(office.UserName, request.Password);
                if (auth && office != null)
                {
                    if (CanAccessServices(office))
                        return Success(BuildAuthenticationResult(office));
                    else
                        return Failure<AuthenticationResult>(ErrorMessages.AccessServices, enErrorCode.AccessServices);
                }
                else
                    return Failure<AuthenticationResult>(ErrorMessages.UserValidation, enErrorCode.UserValidation);

            }
            catch (Exception ex)
            {
                LogException(ex);
                return Failure<AuthenticationResult>(ErrorMessages.InternalServerError, enErrorCode.InternalServerError);
            }
        }

        #endregion

        #region [ PreAssign Services ]

        [WebInvoke(ResponseFormat = WebMessageFormat.Json)]
        public virtual ServiceResponse<ProviderGroupPairList> GetAvailablePositionGroups(Paging request)
        {
            Validate();

            try
            {
                int count;
                int take = request == null || !request.Take.HasValue ? ApiConfig.Current.MaximumItemsReturned : request.Take.Value;
                int skip = request == null || !request.Skip.HasValue ? 0 : request.Skip.Value;
                var positionGroups = _positionGroupRepository.Value.FindByInternshipOffice(OfficeCaller, skip, take, out count);
                if (positionGroups == null)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "GetAvailablePositionGroups", OfficeCaller.ID, string.Empty, true, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Success<ProviderGroupPairList>(null);
                }
                var result = new ProviderGroupPairList()
                {
                    Pairs = positionGroups.Select(x => x.ToProviderGroupPairResult()).ToList(),
                    NumberOfItems = count
                };
                StudentPractiseApiLog.Log(enServiceCaller.Office, "GetAvailablePositionGroups", OfficeCaller.ID, string.Empty, true, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                return Success<ProviderGroupPairList>(result);
            }
            catch (WebFaultException wfEx)
            {
                throw wfEx;
            }
            catch (Exception ex)
            {
                return UnhandledException<ProviderGroupPairList>(ex, "GetAvailablePositionGroups", OfficeCaller.ID);
            }
        }

        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        public virtual ServiceResponse<PositionGroupResult> GetPositionGroupDetails(int ID)
        {
            Validate();

            try
            {
                var serializer = new Serializer<string>();
                var result = _positionGroupRepository.Value.FindGroupByInternshipOffice(OfficeCaller, ID);
                if (result == null)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "GetPositionGroupDetails", OfficeCaller.ID, enErrorCode.ObjectNotFount.ToString(), false, serializer.Serialize(HttpContext.Current.Request.Url.ToString()), HttpContext.Current.Request.UserHostAddress);
                    return Failure<PositionGroupResult>(ErrorMessages.ObjectNotFount, enErrorCode.ObjectNotFount);
                }
                StudentPractiseApiLog.Log(enServiceCaller.Office, "GetPositionGroupDetails", OfficeCaller.ID, string.Empty, true, serializer.Serialize(HttpContext.Current.Request.Url.ToString()), HttpContext.Current.Request.UserHostAddress);
                return Success<PositionGroupResult>(result.ToPositionGroupResult());
            }
            catch (WebFaultException wfEx)
            {
                throw wfEx;
            }
            catch (Exception ex)
            {
                return UnhandledException<PositionGroupResult>(ex, "GetPositionGroupDetails", OfficeCaller.ID);
            }
        }

        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        public virtual ServiceResponse<ProviderResult> GetProviderDetails(int ID)
        {
            Validate();

            try
            {
                var serializer = new Serializer<string>();
                var result = _providerRepository.Value.FindByIDVerified(ID);
                if (result == null)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "GetProviderDetails", OfficeCaller.ID, enErrorCode.ObjectNotFount.ToString(), false, serializer.Serialize(HttpContext.Current.Request.Url.ToString()), HttpContext.Current.Request.UserHostAddress);
                    return Failure<ProviderResult>(ErrorMessages.ObjectNotFount, enErrorCode.ObjectNotFount);
                }
                StudentPractiseApiLog.Log(enServiceCaller.Office, "GetProviderDetails", OfficeCaller.ID, string.Empty, true, serializer.Serialize(HttpContext.Current.Request.Url.ToString()), HttpContext.Current.Request.UserHostAddress);
                return Success<ProviderResult>(result.ToProviderResult());
            }
            catch (WebFaultException wfEx)
            {
                throw wfEx;
            }
            catch (Exception ex)
            {
                return UnhandledException<ProviderResult>(ex, "GetProviderDetails", OfficeCaller.ID);
            }
        }

        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        public virtual ServiceResponse<ProviderResult[]> GetProvidersByAFM(string AFM)
        {
            Validate();

            try
            {
                var serializer = new Serializer<string>();
                var result = _providerRepository.Value.FindByAFM(AFM);
                if (result == null)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "GetProvidersByAFM", OfficeCaller.ID, string.Empty, true, serializer.Serialize(HttpContext.Current.Request.Url.ToString()), HttpContext.Current.Request.UserHostAddress);
                    return Success<ProviderResult[]>(null);
                }
                StudentPractiseApiLog.Log(enServiceCaller.Office, "GetProvidersByAFM", OfficeCaller.ID, string.Empty, true, serializer.Serialize(HttpContext.Current.Request.Url.ToString()), HttpContext.Current.Request.UserHostAddress);
                return Success<ProviderResult[]>(result.Select(x => x.ToProviderResult()).ToArray());
            }
            catch (WebFaultException wfEx)
            {
                throw wfEx;
            }
            catch (Exception ex)
            {
                return UnhandledException<ProviderResult[]>(ex, "GetProvidersByAFM", OfficeCaller.ID);
            }
        }

        [WebInvoke(ResponseFormat = WebMessageFormat.Json)]
        public virtual ServiceResponse<int[]> PreAssignPositions(PositionGroupRequest request)
        {
            Validate();

            try
            {
                #region [Validate Request Data]

                if (request == null)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "PreAssignPositions", OfficeCaller.ID, HttpStatusCode.BadRequest.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    throw new WebFaultException(HttpStatusCode.BadRequest);
                }
                if (!request.AcademicID.HasValue)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "PreAssignPositions", OfficeCaller.ID, enErrorCode.NoAcademinID.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Failure<int[]>(ErrorMessages.NoAcademinID, enErrorCode.NoAcademinID);
                }
                if (!request.GroupID.HasValue)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "PreAssignPositions", OfficeCaller.ID, enErrorCode.NoGroupID.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Failure<int[]>(ErrorMessages.NoGroupID, enErrorCode.NoGroupID);
                }

                if (!request.NumberOfPositions.HasValue)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "PreAssignPositions", OfficeCaller.ID, enErrorCode.NoPositionNumber.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Failure<int[]>(ErrorMessages.NoPositionNumber, enErrorCode.NoPositionNumber);
                }

                var academic = OfficeCaller.Academics.First(x => x.ID == request.AcademicID.Value);
                if (academic == null)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "PreAssignPositions", OfficeCaller.ID, enErrorCode.OfficeIDError.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Failure<int[]>(ErrorMessages.OfficeIDError, enErrorCode.OfficeIDError);
                }
                if (!academic.IsActive)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "PreAssignPositions", OfficeCaller.ID, enErrorCode.AcademicNotActive.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Failure<int[]>(ErrorMessages.AcademicNotActive, enErrorCode.AcademicNotActive);
                }
                var availablePositionsForPreAssignment = academic.MaxAllowedPreAssignedPositions - academic.PreAssignedPositions;
                if (request.NumberOfPositions > availablePositionsForPreAssignment)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "PreAssignPositions", OfficeCaller.ID, enErrorCode.AvailablePreAssignLimit.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Failure<int[]>(string.Format(ErrorMessages.AvailablePreAssignLimit, availablePositionsForPreAssignment), enErrorCode.AvailablePreAssignLimit);
                }

                var positinGroup = _positionGroupRepository.Value.FindGroupByInternshipOffice(OfficeCaller, request.GroupID.Value);
                if (positinGroup == null)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "PreAssignPositions", OfficeCaller.ID, enErrorCode.ObjectNotFount.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Failure<int[]>(ErrorMessages.ObjectNotFount, enErrorCode.ObjectNotFount);
                }

                var positions = positinGroup.Positions.Where(x => x.PositionStatus == enPositionStatus.Available).ToList();
                if (request.NumberOfPositions > positions.Count())
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "PreAssignPositions", OfficeCaller.ID, enErrorCode.NumberOfPositionsError.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Failure<int[]>(ErrorMessages.NumberOfPositionsError, enErrorCode.NumberOfPositionsError);
                }

                if (positinGroup.IsVisibleToAllAcademics.HasValue && !positinGroup.IsVisibleToAllAcademics.Value && positinGroup.Academics.Where(x => x.ID == request.AcademicID.Value) == null)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "PreAssignPositions", OfficeCaller.ID, enErrorCode.PreAssignWrongAcademic.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Failure<int[]>(ErrorMessages.PreAssignWrongAcademic, enErrorCode.PreAssignWrongAcademic);
                }

                if (OfficeCaller.Academics.Where(x => x.ID == request.AcademicID.Value) == null)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "PreAssignPositions", OfficeCaller.ID, enErrorCode.PreAssignNoAcademic.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Failure<int[]>(ErrorMessages.PreAssignNoAcademic, enErrorCode.PreAssignNoAcademic);
                }

                #endregion

                List<int> positionIDs = new List<int>();

                for (int i = 0; i < request.NumberOfPositions; i++)
                {
                    #region [ PreAssign Position ]
                    InternshipPositionTriggersParams triggersParams = new InternshipPositionTriggersParams();
                    triggersParams.OfficeID = OfficeCaller.ID;

                    if (OfficeCaller.MasterAccountID.HasValue)
                        triggersParams.MasterAccountID = OfficeCaller.MasterAccountID.Value;
                    else
                        triggersParams.MasterAccountID = OfficeCaller.ID;

                    triggersParams.Academic = academic;
                    triggersParams.Username = Thread.CurrentPrincipal.Identity.Name;
                    triggersParams.ExecutionDate = DateTime.Now;
                    triggersParams.UnitOfWork = UnitOfWork;

                    var stateMachine = new InternshipPositionStateMachine(positions[i]);
                    if (stateMachine.CanFire(enInternshipPositionTriggers.PreAssign))
                    {
                        stateMachine.PreAssign(triggersParams);
                        positionIDs.Add(positions[i].ID);
                    }
                    else
                    {
                        StudentPractiseApiLog.Log(enServiceCaller.Office, "PreAssignPositions", OfficeCaller.ID, enErrorCode.PreAssignPositionError.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                        return Failure<int[]>(ErrorMessages.PreAssignPositionError, enErrorCode.PreAssignPositionError);
                    }
                    #endregion
                }

                positinGroup.AvailablePositions -= request.NumberOfPositions.Value;
                positinGroup.PreAssignedPositions += request.NumberOfPositions.Value;
                UnitOfWork.Commit();

                StudentPractiseApiLog.Log(enServiceCaller.Office, "PreAssignPositions", OfficeCaller.ID, string.Empty, true, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                return Success<int[]>(positionIDs.ToArray());
            }
            catch (WebFaultException wfEx)
            {
                throw wfEx;
            }
            catch (Exception ex)
            {
                return UnhandledException<int[]>(ex, "PreAssignPositions", OfficeCaller.ID);
            }
        }

        [WebInvoke(ResponseFormat = WebMessageFormat.Json)]
        public virtual ServiceResponse<string> RollbackPreAssignment(Delete request)
        {
            Validate();

            try
            {
                var position = _positionRepository.Value.LoadByOffice(request.PositionID, OfficeCaller, (int)enPositionStatus.PreAssigned,
                    x => x.AssignedToStudent, x => x.InternshipPositionGroup, x => x.InternshipPositionGroup.PhysicalObjects, x => x.PreAssignedForAcademic,
                    x => x.InternshipPositionGroup.Academics, x => x.AssignedToStudent.Academic);
                if (position == null)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "RollbackPreAssignment", OfficeCaller.ID, enErrorCode.ObjectNotFount.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Failure<string>(ErrorMessages.ObjectNotFount, enErrorCode.ObjectNotFount);
                }
                int daysLeft = position.DaysLeftForAssignment.Value;

                #region [ PreAssign Position ]

                InternshipPositionTriggersParams triggersParams = new InternshipPositionTriggersParams();
                triggersParams.OfficeID = OfficeCaller.ID;

                if (OfficeCaller.MasterAccountID.HasValue)
                    triggersParams.MasterAccountID = OfficeCaller.MasterAccountID.Value;
                else
                    triggersParams.MasterAccountID = OfficeCaller.ID;

                triggersParams.Username = Thread.CurrentPrincipal.Identity.Name;
                triggersParams.ExecutionDate = DateTime.Now;
                triggersParams.Academic = position.PreAssignedForAcademic;
                triggersParams.UnitOfWork = UnitOfWork;

                var stateMachine = new InternshipPositionStateMachine(position);
                if (stateMachine.CanFire(enInternshipPositionTriggers.RollbackPreAssignment))
                    stateMachine.RollbackPreAssignment(triggersParams);
                else
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "RollbackPreAssignment", OfficeCaller.ID, enErrorCode.RollbackPreAssignmentError.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Failure<string>(string.Format(ErrorMessages.RollbackPreAssignmentError, request.PositionID), enErrorCode.RollbackPreAssignmentError);
                }
                UnitOfWork.Commit();

                #endregion

                StudentPractiseApiLog.Log(enServiceCaller.Office, "RollbackPreAssignment", OfficeCaller.ID, string.Empty, true, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                if (daysLeft < StudentPracticeConstants.Default_MaxDaysForAssignment)
                    return Success<string>(ErrorMessages.RollbackPreAssignmentMessage);
                else
                    return Success<string>(ErrorMessages.RollbackPreAssignmentMessageNoPentaly);
            }
            catch (WebFaultException wfEx)
            {
                throw wfEx;
            }
            catch (Exception ex)
            {
                return UnhandledException<string>(ex, "RollbackPreAssignment", OfficeCaller.ID);
            }
        }

        [WebInvoke(UriTemplate = "RollbackPreAssignment/info", ResponseFormat = WebMessageFormat.Json)]
        public virtual ServiceResponse<string> RollbackPreAssignmentInfo(Delete request)
        {
            Validate();

            try
            {
                var position = _positionRepository.Value.LoadByOffice(request.PositionID, OfficeCaller, (int)enPositionStatus.PreAssigned,
                    x => x.AssignedToStudent, x => x.InternshipPositionGroup, x => x.InternshipPositionGroup.PhysicalObjects, x => x.InternshipPositionGroup.Academics, x => x.AssignedToStudent.Academic);
                if (position == null)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "RollbackPreAssignment/info", OfficeCaller.ID, enErrorCode.ObjectNotFount.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Failure<string>(ErrorMessages.ObjectNotFount, enErrorCode.ObjectNotFount);
                }

                if (position.DaysLeftForAssignment.Value < StudentPracticeConstants.Default_MaxDaysForAssignment)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "RollbackPreAssignment/info", OfficeCaller.ID, string.Empty, true, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Success<string>(ErrorMessages.RollbackPreAssignmentPenalty);
                }

                #region [ PreAssign Position ]

                InternshipPositionTriggersParams triggersParams = new InternshipPositionTriggersParams();
                triggersParams.OfficeID = OfficeCaller.ID;

                if (OfficeCaller.MasterAccountID.HasValue)
                    triggersParams.MasterAccountID = OfficeCaller.MasterAccountID.Value;
                else
                    triggersParams.MasterAccountID = OfficeCaller.ID;

                triggersParams.Username = Thread.CurrentPrincipal.Identity.Name;
                triggersParams.ExecutionDate = DateTime.Now;
                triggersParams.Academic = position.PreAssignedForAcademic;
                triggersParams.UnitOfWork = UnitOfWork;

                var stateMachine = new InternshipPositionStateMachine(position);
                if (!stateMachine.CanFire(enInternshipPositionTriggers.RollbackPreAssignment))
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "RollbackPreAssignment/info", OfficeCaller.ID, string.Empty, true, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Success<string>(string.Format(ErrorMessages.RollbackPreAssignmentError, request.PositionID));
                }


                #endregion

                StudentPractiseApiLog.Log(enServiceCaller.Office, "RollbackPreAssignment/info", OfficeCaller.ID, string.Empty, true, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                return Success<string>("Μπορείτε να κάνετε αποδέσμευση θέση χωρίς ποινή");
            }
            catch (WebFaultException wfEx)
            {
                throw wfEx;
            }
            catch (Exception ex)
            {
                return UnhandledException<string>(ex, "RollbackPreAssignment/info", OfficeCaller.ID);
            }
        }

        [WebInvoke(ResponseFormat = WebMessageFormat.Json)]
        public virtual ServiceResponse<PositionResult[]> GetPreAssignedPositions(Paging request)
        {
            Validate();

            try
            {
                int take = request == null || !request.Take.HasValue ? ApiConfig.Current.MaximumItemsReturned : request.Take.Value;
                int skip = request == null || !request.Skip.HasValue ? 0 : request.Skip.Value;
                var positions = _positionRepository.Value.FindByOffice(OfficeCaller, (int)enPositionStatus.PreAssigned, skip, take,
                    x => x.AssignedToStudent, x => x.InternshipPositionGroup, x => x.InternshipPositionGroup.PhysicalObjects, x => x.InternshipPositionGroup.Academics, x => x.AssignedToStudent.Academic);
                if (positions == null)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "GetPreAssignedPositions", OfficeCaller.ID, string.Empty, true, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Success<PositionResult[]>(null);
                }
                StudentPractiseApiLog.Log(enServiceCaller.Office, "GetPreAssignedPositions", OfficeCaller.ID, string.Empty, true, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                return Success<PositionResult[]>(positions.Select(x => x.ToPositionResult()).ToArray());
            }
            catch (WebFaultException wfEx)
            {
                throw wfEx;
            }
            catch (Exception ex)
            {
                return UnhandledException<PositionResult[]>(ex, "GetPreAssignedPositions", OfficeCaller.ID);
            }
        }

        #endregion

        #region [ Assign Services ]

        [WebInvoke(ResponseFormat = WebMessageFormat.Json)]
        public virtual ServiceResponse<PositionResult> AssignStudent(Assignment request)
        {
            Validate();

            if (request == null)
            {
                StudentPractiseApiLog.Log(enServiceCaller.Office, "AssignStudent", OfficeCaller.ID, HttpStatusCode.BadRequest.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                throw new WebFaultException(HttpStatusCode.BadRequest);
            }

            try
            {
                var position = _positionRepository.Value.LoadByOffice(request.PositionID, OfficeCaller, (int)enPositionStatus.PreAssigned,
                    x => x.AssignedToStudent, x => x.InternshipPositionGroup, x => x.InternshipPositionGroup.PhysicalObjects, x => x.InternshipPositionGroup.Academics, x => x.InternshipPositionGroup.Provider, x => x.AssignedToStudent.Academic);
                if (position == null)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "AssignStudent", OfficeCaller.ID, enErrorCode.ObjectNotFount.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Failure<PositionResult>(ErrorMessages.ObjectNotFount, enErrorCode.ObjectNotFount);
                }

                #region [Validate Data]

                if (request.ImplementationStartDate.HasValue && request.ImplementationEndDate.HasValue)
                {
                    position.ImplementationStartDate = request.ImplementationStartDate.Value;
                    position.ImplementationEndDate = request.ImplementationEndDate.Value;
                }
                else if (!string.IsNullOrEmpty(request.ImplementationStartDateString) && !string.IsNullOrEmpty(request.ImplementationEndDateString))
                {
                    string implementationStartDateStringFormat = string.IsNullOrWhiteSpace(request.ImplementationStartDateStringFormat) ? "dd/MM/yyyy" : request.ImplementationStartDateStringFormat;
                    string implementationEndDateStringFormat = string.IsNullOrWhiteSpace(request.ImplementationEndDateStringFormat) ? "dd/MM/yyyy" : request.ImplementationEndDateStringFormat;

                    position.ImplementationStartDate = DateTime.ParseExact(request.ImplementationStartDateString, implementationStartDateStringFormat, System.Globalization.CultureInfo.InvariantCulture);
                    position.ImplementationEndDate = DateTime.ParseExact(request.ImplementationEndDateString, implementationEndDateStringFormat, System.Globalization.CultureInfo.InvariantCulture);
                }
                else
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "AssignStudent", OfficeCaller.ID, HttpStatusCode.BadRequest.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    throw new WebFaultException(HttpStatusCode.BadRequest);
                }

                if (position.ImplementationEndDate < position.ImplementationStartDate)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "AssignStudent", OfficeCaller.ID, enErrorCode.DateTimeEndLessThanStart.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Failure<PositionResult>(ErrorMessages.DateTimeEndLessThanStart, enErrorCode.DateTimeEndLessThanStart);
                }

                position.FundingType = enFundingType.ESPA;

                #endregion

                var student = _studentRepository.Value.LoadByOfficeWithID(OfficeCaller, request.StudentID);
                if (student == null)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "AssignStudent", OfficeCaller.ID, enErrorCode.ObjectNotFount.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Failure<PositionResult>(ErrorMessages.ObjectNotFount, enErrorCode.ObjectNotFount);
                }
                if (!student.IsActive)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "AssignStudent", OfficeCaller.ID, enErrorCode.ObjectNotFount.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Failure<PositionResult>(ErrorMessages.ObjectNotFount, enErrorCode.StudentNotActive);
                }
                #region [ AssignStudent ]

                if (position.AssignedToStudent != null)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "AssignStudent", OfficeCaller.ID, enErrorCode.PositionIsAssigned.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Failure<PositionResult>(ErrorMessages.PositionIsAssigned, enErrorCode.PositionIsAssigned);
                }
                if (student.IsAssignedToPosition.HasValue && student.IsAssignedToPosition.Value)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "AssignStudent", OfficeCaller.ID, enErrorCode.StudentIsAssigned.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Failure<PositionResult>(ErrorMessages.StudentIsAssigned, enErrorCode.StudentIsAssigned);
                }
                if (position.PreAssignedForAcademicID != student.AcademicID)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "AssignStudent", OfficeCaller.ID, enErrorCode.AssignWrongAcademic.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Failure<PositionResult>(ErrorMessages.ObjectNotFount, enErrorCode.AssignWrongAcademic);
                }
                position.AssignedToStudent = student;
                position.AssignedAt = DateTime.Now;

                InternshipPositionTriggersParams triggersParams = new InternshipPositionTriggersParams();
                triggersParams.OfficeID = OfficeCaller.ID;

                if (OfficeCaller.MasterAccountID.HasValue)
                    triggersParams.MasterAccountID = OfficeCaller.MasterAccountID.Value;
                else
                    triggersParams.MasterAccountID = OfficeCaller.ID;

                triggersParams.Username = Thread.CurrentPrincipal.Identity.Name;
                triggersParams.ExecutionDate = DateTime.Now;
                triggersParams.ImplementationStartDate = position.ImplementationStartDate.Value;
                triggersParams.ImplementationEndDate = position.ImplementationEndDate.Value;
                triggersParams.FundingType = position.FundingType;
                triggersParams.UnitOfWork = UnitOfWork;

                var stateMachine = new InternshipPositionStateMachine(position);

                if (position.ImplementationStartDate <= DateTime.Now.Date)
                    stateMachine.AssignAndBeginImplementation(triggersParams);
                else
                    stateMachine.Assign(triggersParams);

                student.IsAssignedToPosition = true;
                //student.PositionCount++;

                var provider = position.InternshipPositionGroup.Provider;
                var academic = StudentPracticeCacheManager<Academic>.Current.Get(student.AcademicID.Value);

                if (!string.IsNullOrEmpty(student.ContactEmail))
                {
                    var emailToStudent = MailSender.SendAssignedPositionStudentNotification(student.ID, student.ContactEmail, string.Format("{0} {1}", student.GreekFirstName, student.GreekLastName), position.InternshipPositionGroup.ID, position.InternshipPositionGroup.Title, provider.Name);
                    UnitOfWork.MarkAsNew(emailToStudent);
                }

                var emailToProvider = MailSender.SendAssignedPositionProviderNotification(provider.ID, provider.ContactEmail, provider.UserName, position.InternshipPositionGroup.ID, position.InternshipPositionGroup.Title, string.Format("{0} {1}", student.GreekFirstName, student.GreekLastName), academic.Institution, academic.School ?? "-", academic.Department, student.StudentNumber, provider.Language.GetValueOrDefault());
                UnitOfWork.MarkAsNew(emailToProvider);

                UnitOfWork.Commit();

                #endregion

                StudentPractiseApiLog.Log(enServiceCaller.Office, "AssignStudent", OfficeCaller.ID, string.Empty, true, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                return Success<PositionResult>(position.ToPositionResult());
            }
            catch (WebFaultException wfEx)
            {
                throw wfEx;
            }
            catch (Exception ex)
            {
                return UnhandledException<PositionResult>(ex, "AssignStudent", OfficeCaller.ID);
            }
        }

        [WebInvoke(ResponseFormat = WebMessageFormat.Json)]
        public virtual ServiceResponse<PositionResult> ChangeAssignedStudent(AssignStudent request)
        {
            Validate();

            try
            {
                var position = _positionRepository.Value.LoadAssignedByOffice(request.PositionID, OfficeCaller,
                    x => x.AssignedToStudent, x => x.InternshipPositionGroup, x => x.InternshipPositionGroup.Provider, x => x.InternshipPositionGroup.PhysicalObjects, x => x.InternshipPositionGroup.Academics, x => x.AssignedToStudent.Academic);
                if (position == null)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "ChangeAssignedStudent", OfficeCaller.ID, enErrorCode.ObjectNotFount.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Failure<PositionResult>(ErrorMessages.ObjectNotFount, enErrorCode.ObjectNotFount);
                }

                var student = _studentRepository.Value.LoadByOfficeWithID(OfficeCaller, request.StudentID);
                if (student == null)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "ChangeAssignedStudent", OfficeCaller.ID, enErrorCode.ObjectNotFount.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Failure<PositionResult>(ErrorMessages.ObjectNotFount, enErrorCode.ObjectNotFount);
                }
                if (!student.IsActive)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "ChangeAssignedStudent", OfficeCaller.ID, enErrorCode.ObjectNotFount.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Failure<PositionResult>(ErrorMessages.ObjectNotFount, enErrorCode.StudentNotActive);
                }
                #region [ AssignNewStudent ]

                if (student.IsAssignedToPosition.HasValue && student.IsAssignedToPosition.Value)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "ChangeAssignedStudent", OfficeCaller.ID, enErrorCode.StudentIsAssigned.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Failure<PositionResult>(ErrorMessages.StudentIsAssigned, enErrorCode.StudentIsAssigned);
                }

                position.AssignedToStudent.IsAssignedToPosition = false;
                //position.AssignedToStudent.PositionCount--;
                position.AssignedAt = DateTime.Now;
                student.IsAssignedToPosition = true;
                //student.PositionCount++;
                position.AssignedToStudentID = student.ID;

                InternshipPositionLog logEntry = new InternshipPositionLog();
                logEntry.InternshipPositionID = position.ID;
                logEntry.OldStatus = position.PositionStatus;
                logEntry.NewStatus = position.PositionStatus;
                logEntry.AssignedByOfficeID = OfficeCaller.ID;

                if (OfficeCaller.MasterAccountID.HasValue)
                    logEntry.AssignedByMasterAccountID = OfficeCaller.MasterAccountID.Value;
                else
                    logEntry.AssignedByMasterAccountID = OfficeCaller.ID;

                logEntry.AssignedToStudentID = student.ID;
                logEntry.CreatedAt = DateTime.Now;
                logEntry.CreatedAtDateOnly = DateTime.Now.Date;
                logEntry.CreatedBy = Thread.CurrentPrincipal.Identity.Name;
                UnitOfWork.MarkAsNew(logEntry);

                var provider = position.InternshipPositionGroup.Provider;
                var academic = StudentPracticeCacheManager<Academic>.Current.Get(student.AcademicID.Value);

                var emailToStudent = MailSender.SendAssignedPositionStudentNotification(student.ID, student.ContactEmail, string.Format("{0} {1}", student.GreekFirstName, student.GreekLastName), position.InternshipPositionGroup.ID, position.InternshipPositionGroup.Title, provider.Name);
                UnitOfWork.MarkAsNew(emailToStudent);

                var emailToProvider = MailSender.SendAssignedPositionProviderNotification(provider.ID, provider.ContactEmail, provider.UserName, position.InternshipPositionGroup.ID, position.InternshipPositionGroup.Title, string.Format("{0} {1}", student.GreekFirstName, student.GreekLastName), academic.Institution, academic.School ?? "-", academic.Department, student.StudentNumber, provider.Language.GetValueOrDefault());
                UnitOfWork.MarkAsNew(emailToProvider);

                UnitOfWork.Commit();

                #endregion

                StudentPractiseApiLog.Log(enServiceCaller.Office, "ChangeAssignedStudent", OfficeCaller.ID, string.Empty, true, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                return Success<PositionResult>(position.ToPositionResult());
            }
            catch (WebFaultException wfEx)
            {
                throw wfEx;
            }
            catch (Exception ex)
            {
                return UnhandledException<PositionResult>(ex, "ChangeAssignedStudent", OfficeCaller.ID);
            }
        }

        [WebInvoke(ResponseFormat = WebMessageFormat.Json)]
        public virtual ServiceResponse<PositionResult> ChangeImplementationData(Reassignment request)
        {
            Validate();

            #region [ Validate Data ]

            DateTime newImplementationStartDate;
            DateTime newImplementationEndDate;
            if (request == null)
            {
                StudentPractiseApiLog.Log(enServiceCaller.Office, "ChangeImplementationData", OfficeCaller.ID, HttpStatusCode.BadRequest.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                throw new WebFaultException(HttpStatusCode.BadRequest);
            }
            if (request.ImplementationStartDate.HasValue && request.ImplementationEndDate.HasValue)
            {
                newImplementationStartDate = request.ImplementationStartDate.Value; ;
                newImplementationEndDate = request.ImplementationEndDate.Value; ;
            }
            else if (!string.IsNullOrEmpty(request.ImplementationStartDateString) && !string.IsNullOrEmpty(request.ImplementationEndDateString))
            {
                string implementationStartDateStringFormat = string.IsNullOrWhiteSpace(request.ImplementationStartDateStringFormat) ? "dd/MM/yyyy" : request.ImplementationStartDateStringFormat;
                string implementationEndDateStringFormat = string.IsNullOrWhiteSpace(request.ImplementationEndDateStringFormat) ? "dd/MM/yyyy" : request.ImplementationEndDateStringFormat;

                newImplementationStartDate = DateTime.ParseExact(request.ImplementationStartDateString, implementationStartDateStringFormat, System.Globalization.CultureInfo.InvariantCulture);
                newImplementationEndDate = DateTime.ParseExact(request.ImplementationEndDateString, implementationEndDateStringFormat, System.Globalization.CultureInfo.InvariantCulture);
            }
            else
            {
                StudentPractiseApiLog.Log(enServiceCaller.Office, "ChangeImplementationData", OfficeCaller.ID, HttpStatusCode.BadRequest.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                throw new WebFaultException(HttpStatusCode.BadRequest);
            }

            if (newImplementationEndDate < newImplementationStartDate)
            {
                StudentPractiseApiLog.Log(enServiceCaller.Office, "ChangeImplementationData", OfficeCaller.ID, enErrorCode.DateTimeEndLessThanStart.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                return Failure<PositionResult>(ErrorMessages.DateTimeEndLessThanStart, enErrorCode.DateTimeEndLessThanStart);
            }

            #endregion

            try
            {
                var position = _positionRepository.Value.LoadAssignedByOffice(request.PositionID, OfficeCaller,
                    x => x.AssignedToStudent, x => x.InternshipPositionGroup, x => x.InternshipPositionGroup.PhysicalObjects, x => x.InternshipPositionGroup.Academics, x => x.AssignedToStudent.Academic);
                if (position == null)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "ChangeImplementationData", OfficeCaller.ID, enErrorCode.ObjectNotFount.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Failure<PositionResult>(ErrorMessages.ObjectNotFount, enErrorCode.ObjectNotFount);
                }

                #region [ ChangeImplementationData ]

                DateTime? oldImplementationStartDate = position.ImplementationStartDate;
                DateTime? oldImplementationEndDate = position.ImplementationEndDate;

                if (!oldImplementationStartDate.HasValue || !oldImplementationEndDate.HasValue ||
                    oldImplementationStartDate != newImplementationStartDate || oldImplementationEndDate != newImplementationEndDate)
                {
                    enPositionStatus newStatus;

                    if (position.PositionStatus == enPositionStatus.Assigned && newImplementationStartDate <= DateTime.Now.Date)
                        newStatus = enPositionStatus.UnderImplementation;

                    else if (position.PositionStatus == enPositionStatus.UnderImplementation && newImplementationStartDate > DateTime.Now.Date)
                        newStatus = enPositionStatus.Assigned;

                    else
                        newStatus = position.PositionStatus;

                    InternshipPositionLog logEntry = new InternshipPositionLog();
                    logEntry.InternshipPositionID = position.ID;
                    logEntry.OldStatus = position.PositionStatus;
                    logEntry.NewStatus = newStatus;
                    logEntry.AssignedByOfficeID = OfficeCaller.ID;

                    if (OfficeCaller.MasterAccountID.HasValue)
                    {
                        logEntry.AssignedByMasterAccountID = OfficeCaller.MasterAccountID.Value;
                    }
                    else
                    {
                        logEntry.AssignedByMasterAccountID = OfficeCaller.ID;
                    }

                    if (oldImplementationStartDate != newImplementationStartDate)
                    {
                        logEntry.ImplementationStartDate = newImplementationStartDate;
                        position.ImplementationStartDate = newImplementationStartDate;
                    }

                    if (oldImplementationEndDate != newImplementationEndDate)
                    {
                        logEntry.ImplementationEndDate = newImplementationEndDate;
                        position.ImplementationEndDate = newImplementationEndDate;
                    }

                    logEntry.CreatedAt = DateTime.Now;
                    logEntry.CreatedAtDateOnly = DateTime.Now.Date;
                    logEntry.CreatedBy = Thread.CurrentPrincipal.Identity.Name;
                    UnitOfWork.MarkAsNew(logEntry);

                    position.PositionStatus = newStatus;
                }
                UnitOfWork.Commit();
                #endregion

                StudentPractiseApiLog.Log(enServiceCaller.Office, "ChangeImplementationData", OfficeCaller.ID, string.Empty, true, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                return Success<PositionResult>(position.ToPositionResult());
            }
            catch (WebFaultException wfEx)
            {
                throw wfEx;
            }
            catch (Exception ex)
            {
                return UnhandledException<PositionResult>(ex, "ChangeImplementationData", OfficeCaller.ID);
            }
        }

        [WebInvoke(ResponseFormat = WebMessageFormat.Json)]
        public virtual ServiceResponse<string> DeleteAssignment(Delete request)
        {
            Validate();

            try
            {
                var position = _positionRepository.Value.LoadAssignedByOffice(request.PositionID, OfficeCaller,
                    x => x.AssignedToStudent,
                    x => x.InternshipPositionGroup,
                    x => x.InternshipPositionGroup.Academics,
                    x => x.PreAssignedByMasterAccount,
                    x => x.AssignedToStudent.Academic);
                if (position == null)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "DeleteAssignment", OfficeCaller.ID, enErrorCode.ObjectNotFount.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Failure<string>(ErrorMessages.ObjectNotFount, enErrorCode.ObjectNotFount);
                }

                #region [ Delete Assignment ]

                InternshipPositionTriggersParams triggersParams = new InternshipPositionTriggersParams();
                triggersParams.OfficeID = OfficeCaller.ID;

                if (OfficeCaller.MasterAccountID.HasValue)
                    triggersParams.MasterAccountID = OfficeCaller.MasterAccountID.Value;
                else
                    triggersParams.MasterAccountID = OfficeCaller.ID;

                triggersParams.Username = Thread.CurrentPrincipal.Identity.Name;
                triggersParams.ExecutionDate = DateTime.Now;
                triggersParams.UnitOfWork = UnitOfWork;

                var stateMachine = new InternshipPositionStateMachine(position);
                if (!stateMachine.CanFire(enInternshipPositionTriggers.DeleteAssignment))
                    return Failure<string>(ErrorMessages.DeleteAssignmentError, enErrorCode.DeleteAssignmentError);

                if (position.InternshipPositionGroup.PositionGroupStatus == enPositionGroupStatus.Revoked)
                {
                    if (position.DaysLeftForAssignment == 0)
                    {
                        triggersParams.CancellationReason = enCancellationReason.CanceledGroupCascade;
                        stateMachine.Cancel(triggersParams);
                    }
                    else
                    {
                        triggersParams.BlockingReason = enBlockingReason.None;
                        stateMachine.DeleteAssignment(triggersParams);
                    }
                }
                else if (position.InternshipPositionGroup.PositionGroupStatus == enPositionGroupStatus.UnPublished)
                {
                    if (position.DaysLeftForAssignment == 0)
                        stateMachine.UnPublish(triggersParams);
                    else
                    {
                        triggersParams.BlockingReason = enBlockingReason.None;
                        stateMachine.DeleteAssignment(triggersParams);
                    }
                }
                else
                {
                    if (position.DaysLeftForAssignment == 0)
                        triggersParams.BlockingReason = enBlockingReason.RolledbackAssignmentOutOfTime;

                    stateMachine.DeleteAssignment(triggersParams);
                }
                UnitOfWork.Commit();
                #endregion

                StudentPractiseApiLog.Log(enServiceCaller.Office, "DeleteAssignment", OfficeCaller.ID, string.Empty, true, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                if (position.DaysLeftForAssignment == 0)
                    return Success<string>(ErrorMessages.DeleteAssignmentMessage);
                else
                    return Success<string>(ErrorMessages.DeleteAssignmentMessageNoPentaly);
            }
            catch (WebFaultException wfEx)
            {
                throw wfEx;
            }
            catch (Exception ex)
            {
                return UnhandledException<string>(ex, "DeleteAssignment", OfficeCaller.ID);
            }
        }

        [WebInvoke(UriTemplate = "DeleteAssignment/info", ResponseFormat = WebMessageFormat.Json)]
        public virtual ServiceResponse<string> DeleteAssignmentInfo(Delete request)
        {
            Validate();

            try
            {
                var position = _positionRepository.Value.LoadAssignedByOffice(request.PositionID, OfficeCaller, x => x.InternshipPositionGroup, x => x.PreAssignedForAcademic);
                if (position == null)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "DeleteAssignment/info", OfficeCaller.ID, enErrorCode.ObjectNotFount.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Failure<string>(ErrorMessages.ObjectNotFount, enErrorCode.ObjectNotFount);
                }

                if (position.DaysLeftForAssignment == 0)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "DeleteAssignment/info", OfficeCaller.ID, string.Empty, true, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Success(ErrorMessages.DeleteAssignmentPenalty);
                }

                InternshipPositionTriggersParams triggersParams = new InternshipPositionTriggersParams();
                triggersParams.OfficeID = OfficeCaller.ID;

                if (OfficeCaller.MasterAccountID.HasValue)
                    triggersParams.MasterAccountID = OfficeCaller.MasterAccountID.Value;
                else
                    triggersParams.MasterAccountID = OfficeCaller.ID;

                triggersParams.Username = Thread.CurrentPrincipal.Identity.Name;
                triggersParams.ExecutionDate = DateTime.Now;
                triggersParams.UnitOfWork = UnitOfWork;

                var stateMachine = new InternshipPositionStateMachine(position);
                if (!stateMachine.CanFire(enInternshipPositionTriggers.DeleteAssignment))
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "DeleteAssignment/info", OfficeCaller.ID, string.Empty, true, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Success<string>(ErrorMessages.DeleteAssignmentError);
                }


                StudentPractiseApiLog.Log(enServiceCaller.Office, "DeleteAssignment/info", OfficeCaller.ID, string.Empty, true, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                return Success<string>("Μπορείτε να διαγράψετε την αντιστοίχηση χωρίς ποινή.");
            }
            catch (WebFaultException wfEx)
            {
                throw wfEx;
            }
            catch (Exception ex)
            {
                return UnhandledException<string>(ex, "DeleteAssignment/info", OfficeCaller.ID);
            }
        }

        [WebInvoke(ResponseFormat = WebMessageFormat.Json)]
        public virtual ServiceResponse<PositionResult[]> GetAssignedPositions(Paging request)
        {
            Validate();

            try
            {
                int take = request == null || !request.Take.HasValue ? ApiConfig.Current.MaximumItemsReturned : request.Take.Value;
                int skip = request == null || !request.Skip.HasValue ? 0 : request.Skip.Value;
                var positions = _positionRepository.Value.FindAssignedByOffice(OfficeCaller, skip, take,
                    x => x.AssignedToStudent, x => x.InternshipPositionGroup, x => x.InternshipPositionGroup.PhysicalObjects, x => x.InternshipPositionGroup.Academics, x => x.AssignedToStudent.Academic);
                if (positions == null)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "GetAssignedPositions", OfficeCaller.ID, string.Empty, true, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Success<PositionResult[]>(null);
                }
                StudentPractiseApiLog.Log(enServiceCaller.Office, "GetAssignedPositions", OfficeCaller.ID, string.Empty, true, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                return Success<PositionResult[]>(positions.Select(x => x.ToPositionResult()).ToArray());
            }
            catch (WebFaultException wfEx)
            {
                throw wfEx;
            }
            catch (Exception ex)
            {
                return UnhandledException<PositionResult[]>(ex, "GetAssignedPositions", OfficeCaller.ID);
            }
        }

        [WebInvoke(ResponseFormat = WebMessageFormat.Json)]
        public virtual ServiceResponse<PositionResult> CompletePosition(Completion request)
        {
            Validate();

            try
            {
                var position = _positionRepository.Value.LoadByOffice(request.PositionID, OfficeCaller, (int)enPositionStatus.UnderImplementation,
                    x => x.AssignedToStudent, x => x.InternshipPositionGroup, x => x.InternshipPositionGroup.PhysicalObjects, x => x.InternshipPositionGroup.Academics, x => x.AssignedToStudent.Academic);

                if (position == null)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "CompletePosition", OfficeCaller.ID, enErrorCode.ObjectNotFount.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Failure<PositionResult>(ErrorMessages.ObjectNotFount, enErrorCode.ObjectNotFount);
                }

                #region [ Validate Data ]

                if (request.ImplementationStartDate.HasValue)
                    position.ImplementationStartDate = request.ImplementationStartDate;

                else if (!string.IsNullOrEmpty(request.ImplementationStartDateString))
                {
                    string format = string.IsNullOrWhiteSpace(request.ImplementationStartDateStringFormat) ? "dd/MM/yyyy" : request.ImplementationStartDateStringFormat;
                    position.ImplementationStartDate = DateTime.ParseExact(request.ImplementationStartDateString, format, System.Globalization.CultureInfo.InvariantCulture);
                }

                if (request.ImplementationEndDate.HasValue)
                    position.ImplementationEndDate = request.ImplementationEndDate;

                else if (!string.IsNullOrEmpty(request.ImplementationEndDateString))
                {
                    string format = string.IsNullOrWhiteSpace(request.ImplementationEndDateStringFormat) ? "dd/MM/yyyy" : request.ImplementationEndDateStringFormat;
                    position.ImplementationEndDate = DateTime.ParseExact(request.ImplementationEndDateString, format, System.Globalization.CultureInfo.InvariantCulture);
                }

                #endregion

                position.CompletionComments = request.CompletionComments;

                #region [ Completion ]

                InternshipPositionTriggersParams triggersParams = new InternshipPositionTriggersParams();
                triggersParams.OfficeID = OfficeCaller.ID;
                if (OfficeCaller.MasterAccountID.HasValue)
                    triggersParams.MasterAccountID = OfficeCaller.MasterAccountID.Value;
                else
                    triggersParams.MasterAccountID = OfficeCaller.ID;

                triggersParams.Username = Thread.CurrentPrincipal.Identity.Name;
                triggersParams.ExecutionDate = DateTime.Now;
                triggersParams.UnitOfWork = UnitOfWork;
                triggersParams.Comment = request.CompletionComments;

                var stateMachine = new InternshipPositionStateMachine(position);
                if (!stateMachine.CanFire(enInternshipPositionTriggers.CompleteImplementation))
                    return Failure<PositionResult>(string.Format(ErrorMessages.CompleteImplementationError, request.PositionID), enErrorCode.CompleteImplementationError);

                stateMachine.CompleteImplementation(triggersParams);
                UnitOfWork.Commit();

                #endregion

                StudentPractiseApiLog.Log(enServiceCaller.Office, "CompletePosition", OfficeCaller.ID, string.Empty, true, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                return Success<PositionResult>(position.ToPositionResult());
            }
            catch (WebFaultException wfEx)
            {
                throw wfEx;
            }
            catch (Exception ex)
            {
                return UnhandledException<PositionResult>(ex, "CompletePosition", OfficeCaller.ID);
            }
        }

        [WebInvoke(ResponseFormat = WebMessageFormat.Json)]
        public virtual ServiceResponse<PositionResult[]> GetCompletedPositions(Paging request)
        {
            Validate();

            try
            {
                int take = request == null || !request.Take.HasValue ? ApiConfig.Current.MaximumItemsReturned : request.Take.Value;
                int skip = request == null || !request.Skip.HasValue ? 0 : request.Skip.Value;
                var positions = _positionRepository.Value.FindByOffice(OfficeCaller, (int)enPositionStatus.Completed, skip, take,
                    x => x.AssignedToStudent, x => x.InternshipPositionGroup, x => x.InternshipPositionGroup.PhysicalObjects, x => x.InternshipPositionGroup.Academics, x => x.AssignedToStudent.Academic);

                if (positions == null)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "GetCompletedPositions", OfficeCaller.ID, string.Empty, true, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Success<PositionResult[]>(null);
                }
                StudentPractiseApiLog.Log(enServiceCaller.Office, "GetCompletedPositions", OfficeCaller.ID, string.Empty, true, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                return Success<PositionResult[]>(positions.Select(x => x.ToPositionResult()).ToArray());
            }
            catch (WebFaultException wfEx)
            {
                throw wfEx;
            }
            catch (Exception ex)
            {
                return UnhandledException<PositionResult[]>(ex, "GetCompletedPositions", OfficeCaller.ID);
            }
        }

        [WebInvoke(ResponseFormat = WebMessageFormat.Json)]
        public virtual ServiceResponse<PositionResult> CancelPosition(Cancellation request)
        {
            Validate();

            if (string.IsNullOrWhiteSpace(request.CancellationReason))
            {
                StudentPractiseApiLog.Log(enServiceCaller.Office, "CancelPosition", OfficeCaller.ID, enErrorCode.CancelNoCancelReason.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                return Failure<PositionResult>(ErrorMessages.CancelNoCancelReason, enErrorCode.CancelNoCancelReason);
            }

            try
            {
                var position = _positionRepository.Value.LoadByOffice(request.PositionID, OfficeCaller, (int)enPositionStatus.UnderImplementation,
                    x => x.AssignedToStudent, x => x.InternshipPositionGroup, x => x.InternshipPositionGroup.PhysicalObjects, x => x.InternshipPositionGroup.Academics, x => x.AssignedToStudent.Academic);
                if (position == null)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "CancelPosition", OfficeCaller.ID, enErrorCode.ObjectNotFount.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Failure<PositionResult>(ErrorMessages.ObjectNotFount, enErrorCode.ObjectNotFount);
                }

                #region [ Completion ]

                InternshipPositionTriggersParams triggersParams = new InternshipPositionTriggersParams();
                triggersParams.OfficeID = OfficeCaller.ID;
                if (OfficeCaller.MasterAccountID.HasValue)
                    triggersParams.MasterAccountID = OfficeCaller.MasterAccountID.Value;
                else
                    triggersParams.MasterAccountID = OfficeCaller.ID;

                triggersParams.Username = Thread.CurrentPrincipal.Identity.Name;
                triggersParams.ExecutionDate = DateTime.Now;
                triggersParams.UnitOfWork = UnitOfWork;
                triggersParams.Comment = request.CancellationReason;
                triggersParams.CancellationReason = enCancellationReason.FromOffice;

                var stateMachine = new InternshipPositionStateMachine(position);
                if (!stateMachine.CanFire(enInternshipPositionTriggers.Cancel))
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "CancelPosition", OfficeCaller.ID, enErrorCode.CancelImplementationError.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Failure<PositionResult>(string.Format(ErrorMessages.CancelImplementationError, request.PositionID), enErrorCode.CancelImplementationError);
                }

                position.CancellationReasonInt = (int)enCancellationReason.FromOffice;
                position.CompletionComments = request.CancellationReason;
                stateMachine.Cancel(triggersParams);
                UnitOfWork.Commit();

                #endregion

                StudentPractiseApiLog.Log(enServiceCaller.Office, "CancelPosition", OfficeCaller.ID, string.Empty, true, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                return Success<PositionResult>(position.ToPositionResult());
            }
            catch (WebFaultException wfEx)
            {
                throw wfEx;
            }
            catch (Exception ex)
            {
                return UnhandledException<PositionResult>(ex, "CancelPosition", OfficeCaller.ID);
            }
        }

        #endregion

        #region [ Funding Type Services ]

        [WebInvoke(ResponseFormat = WebMessageFormat.Json)]
        public virtual ServiceResponse<ChangeFundingTypeResult> ChangeFundingType(ChangeFundingTypeRequest request)
        {
            Validate();

            if (request == null
                || request.PositionID <= 0
                || string.IsNullOrEmpty(request.FundingType))
            {
                StudentPractiseApiLog.Log(enServiceCaller.Office, "ChangeFundingType", OfficeCaller.ID, HttpStatusCode.BadRequest.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                throw new WebFaultException(HttpStatusCode.BadRequest);
            }

            try
            {
                enFundingType oldFundingType;
                enFundingType newFundingType;

                if (!Enum.TryParse<enFundingType>(request.FundingType, out newFundingType))
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "AssignStudent", OfficeCaller.ID, HttpStatusCode.BadRequest.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Failure<ChangeFundingTypeResult>(ErrorMessages.NoFundingType, enErrorCode.NoFundingType);
                }

                var position = _positionRepository.Value.LoadMoreThanPreassignedByOffice(request.PositionID, OfficeCaller);

                if (position == null)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "ChangeFundingType", OfficeCaller.ID, enErrorCode.ObjectNotFount.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Failure<ChangeFundingTypeResult>(ErrorMessages.ObjectNotFount, enErrorCode.ObjectNotFount);
                }

                oldFundingType = position.FundingType;

                position.FundingType = newFundingType;

                InternshipPositionLog logEntry = new InternshipPositionLog();
                logEntry.InternshipPositionID = position.ID;
                logEntry.FundingType = newFundingType;
                logEntry.CreatedAt = DateTime.Now;
                logEntry.CreatedAtDateOnly = DateTime.Now.Date;
                logEntry.CreatedBy = Thread.CurrentPrincipal.Identity.Name;

                UnitOfWork.MarkAsNew(logEntry);
                UnitOfWork.Commit();

                StudentPractiseApiLog.Log(enServiceCaller.Office, "ChangeFundingType", OfficeCaller.ID, string.Empty, true, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                return Success<ChangeFundingTypeResult>(new ChangeFundingTypeResult()
                {
                    PositionID = position.ID,
                    OldFundingType = oldFundingType.ToString(),
                    NewFundingType = newFundingType.ToString()
                });
            }
            catch (WebFaultException wfEx)
            {
                throw wfEx;
            }
            catch (Exception ex)
            {
                return UnhandledException<ChangeFundingTypeResult>(ex, "ChangeFundingType", OfficeCaller.ID);
            }
        }

        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        public virtual ServiceResponse<GetFundingTypeResult> GetFundingType(int positionID)
        {
            Validate();

            try
            {
                var position = _positionRepository.Value.LoadMoreThanPreassignedByOffice(positionID, OfficeCaller);

                if (position == null)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "GetFundingType", OfficeCaller.ID, enErrorCode.ObjectNotFount.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Failure<GetFundingTypeResult>(ErrorMessages.ObjectNotFount, enErrorCode.ObjectNotFount);
                }

                StudentPractiseApiLog.Log(enServiceCaller.Office, "GetFundingType", OfficeCaller.ID, string.Empty, true, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                return Success<GetFundingTypeResult>(new GetFundingTypeResult()
                {
                    PositionID = position.ID,
                    FundingType = position.FundingType.ToString()
                });
            }
            catch (WebFaultException wfEx)
            {
                throw wfEx;
            }
            catch (Exception ex)
            {
                return UnhandledException<GetFundingTypeResult>(ex, "GetFundingType", OfficeCaller.ID);
            }


            

            //if (request == null
            //    || request.PositionID <= 0)
            //{
            //    StudentPractiseApiLog.Log(enServiceCaller.Office, "GetFundingType", OfficeCaller.ID, HttpStatusCode.BadRequest.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
            //    throw new WebFaultException(HttpStatusCode.BadRequest);
            //}

            //try
            //{
            //    var position = _positionRepository.Value.LoadMoreThanPreassignedByOffice(request.PositionID, OfficeCaller);

            //    if (position == null)
            //    {
            //        StudentPractiseApiLog.Log(enServiceCaller.Office, "GetFundingType", OfficeCaller.ID, enErrorCode.ObjectNotFount.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
            //        return Failure<GetFundingTypeResult>(ErrorMessages.ObjectNotFount, enErrorCode.ObjectNotFount);
            //    }

            //    StudentPractiseApiLog.Log(enServiceCaller.Office, "GetFundingType", OfficeCaller.ID, string.Empty, true, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
            //    return Success<GetFundingTypeResult>(new GetFundingTypeResult()
            //    {
            //        PositionID = position.ID,
            //        FundingType = position.FundingType.ToString()
            //    });
            //}
            //catch (WebFaultException wfEx)
            //{
            //    throw wfEx;
            //}
            //catch (Exception ex)
            //{
            //    return UnhandledException<GetFundingTypeResult>(ex, "GetFundingType", OfficeCaller.ID);
            //}
        }

        #endregion

        #region [ Student Services ]

        [WebInvoke(ResponseFormat = WebMessageFormat.Json)]
        public virtual ServiceResponse<StudentResult[]> GetRegisteredStudents(Paging request)
        {
            Validate();

            try
            {
                int take = request == null || !request.Take.HasValue ? ApiConfig.Current.MaximumItemsReturned : request.Take.Value;
                int skip = request == null || !request.Skip.HasValue ? 0 : request.Skip.Value;
                var students = _studentRepository.Value.FindByOffice(OfficeCaller, skip, take);

                if (students == null)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "GetRegisteredStudents", OfficeCaller.ID, string.Empty, true, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Success<StudentResult[]>(null);
                }

                StudentPractiseApiLog.Log(enServiceCaller.Office, "GetRegisteredStudents", OfficeCaller.ID, string.Empty, true, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                return Success<StudentResult[]>(students.Select(x => x.ToStudentResult()).ToArray());
            }
            catch (WebFaultException wfEx)
            {
                throw wfEx;
            }
            catch (Exception ex)
            {
                return UnhandledException<StudentResult[]>(ex, "GetRegisteredStudents", OfficeCaller.ID);
            }
        }

        [WebInvoke(ResponseFormat = WebMessageFormat.Json)]
        public virtual ServiceResponse<StudentResultFromLDAP> FindStudentWithAcademicIDNumber(StudentRequest request)
        {
            Validate();

            try
            {
                BusinessModel.Student student;
                var academicIDNumber = request.AcademicIDNumber.Replace("-", "");

                student = _studentRepository.Value.FindByAcademicIDNumber(academicIDNumber);
                if (student != null)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "FindStudentWithAcademicIDNumber", OfficeCaller.ID, string.Empty, true, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Success<StudentResultFromLDAP>(student.ToStudentResult("Οι πληροφορίες αντλήθηκαν από τον Άτλαντα, ο Φοιτητής είναι εγγεγραμένος"));
                }


                var response = StudentCardNumberService.GetStudentInfoByAcademicCardNumber(OfficeCaller.ID, academicIDNumber);
                if (response != null)
                {
                    if (!response.Success)
                    {
                        StudentPractiseApiLog.Log(enServiceCaller.Office, "FindStudentWithAcademicIDNumber", OfficeCaller.ID, enErrorCode.AcademicIDNotFound.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                        return Failure<StudentResultFromLDAP>(ErrorMessages.AcademicIDNotFound, enErrorCode.AcademicIDNotFound);
                    }

                    var academic = StudentPracticeCacheManager<Academic>.Current.Get(response.AcademicID);
                    var previousAcademic = response.PreviousAcademicID.HasValue ? StudentPracticeCacheManager<Academic>.Current.Get(response.PreviousAcademicID.Value) : null;
                    if (academic == null || academic.Institution == null)
                    {
                        StudentPractiseApiLog.Log(enServiceCaller.Office, "FindStudentWithAcademicIDNumber", OfficeCaller.ID, enErrorCode.AcademicDeleted.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                        return Failure<StudentResultFromLDAP>(ErrorMessages.AcademicDeleted, enErrorCode.AcademicDeleted);
                    }

                    if (academic.InstitutionID != OfficeCaller.InstitutionID &&
                       (previousAcademic == null || previousAcademic.InstitutionID != OfficeCaller.InstitutionID))
                    {
                        StudentPractiseApiLog.Log(enServiceCaller.Office, "FindStudentWithAcademicIDNumber", OfficeCaller.ID, enErrorCode.AcademicIDNotInOffice.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                        return Failure<StudentResultFromLDAP>(ErrorMessages.AcademicIDNotInOffice, enErrorCode.AcademicIDNotInOffice);
                    }

                    if (!OfficeCaller.CanViewAllAcademics.GetValueOrDefault() &&
                       ((!OfficeCaller.Academics.Any(x => x.ID == academic.ID) && previousAcademic == null) ||
                       (!OfficeCaller.Academics.Any(x => (x.ID == academic.ID) || (previousAcademic != null && x.ID == previousAcademic.ID)))))
                    {
                        StudentPractiseApiLog.Log(enServiceCaller.Office, "FindStudentWithAcademicIDNumber", OfficeCaller.ID, enErrorCode.AcademicIDNotInOffice.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                        return Failure<StudentResultFromLDAP>(ErrorMessages.AcademicIDNotInOffice, enErrorCode.AcademicIDNotInOffice);
                    }

                    StudentPractiseApiLog.Log(enServiceCaller.Office, "FindStudentWithAcademicIDNumber", OfficeCaller.ID, string.Empty, true, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Success<StudentResultFromLDAP>(response.ToStudentResult("Οι πληροφορίες αντλήθηκαν από την Δράση Ακαδημαϊκης Κάρτας, ο Φοιτητής δεν είναι εγγεγραμένος στον Άτλαντα"));

                }
                else
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "FindStudentWithAcademicIDNumber", OfficeCaller.ID, enErrorCode.AcademicIDNotFound.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Failure<StudentResultFromLDAP>(ErrorMessages.AcademicIDNotFound, enErrorCode.AcademicIDNotFound);
                }
            }
            catch (WebFaultException wfEx)
            {
                throw wfEx;
            }
            catch (Exception ex)
            {
                return UnhandledException<StudentResultFromLDAP>(ex, "FindStudentWithAcademicIDNumber", OfficeCaller.ID);
            }
        }

        [WebInvoke(ResponseFormat = WebMessageFormat.Json)]
        public virtual ServiceResponse<StudentResultFromLDAP> FindAcademicIDNumber(StudentAcademicIDNumberRequest request)
        {
            Validate();

            try
            {
                #region [ Validate Data ]

                if (string.IsNullOrEmpty(request.StudentNumber) || !request.AcademicID.HasValue)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "FindStudentAcademicIDNumber", OfficeCaller.ID, HttpStatusCode.BadRequest.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    throw new WebFaultException(HttpStatusCode.BadRequest);
                }

                bool isRegistered = false;
                var student = _studentRepository.Value.FindByStudentNumber(OfficeCaller, request.StudentNumber, request.AcademicID.Value);
                if (student != null && !string.IsNullOrEmpty(student.AcademicIDNumber))
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "FindStudentAcademicIDNumber", OfficeCaller.ID, string.Empty, true, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Success<StudentResultFromLDAP>(student.ToStudentResult("Οι πληροφορίες αντλήθηκαν από τον Άτλαντα, ο Φοιτητής είναι εγγεγραμμένος και ο αριθμός της ακαδημαϊκής του ταυτότητας έχει δηλωθεί."));
                }
                else if (student != null && string.IsNullOrEmpty(student.AcademicIDNumber))
                    isRegistered = true;

                var serviceRequest = new AcademicIDCardRequest()
                {
                    AcademicID = request.AcademicID.Value,
                    ServiceCallerID = OfficeCaller.ID,
                    StudentNumber = request.StudentNumber
                };
                var response = StudentCardNumberService.GetStudentInfo(serviceRequest);
                if (response == null || !response.Success)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "FindStudentAcademicIDNumber", OfficeCaller.ID, enErrorCode.AcademicIDNotFound.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Failure<StudentResultFromLDAP>(ErrorMessages.AcademicIDNotFound, enErrorCode.AcademicIDNotFound);
                }

                var academic = StudentPracticeCacheManager<Academic>.Current.Get(response.AcademicID);
                var previousAcademic = response.PreviousAcademicID.HasValue ? StudentPracticeCacheManager<Academic>.Current.Get(response.PreviousAcademicID.Value) : null;
                if (academic == null || academic.Institution == null)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "FindStudentAcademicIDNumber", OfficeCaller.ID, enErrorCode.AcademicDeleted.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Failure<StudentResultFromLDAP>(ErrorMessages.AcademicDeleted, enErrorCode.AcademicDeleted);
                }

                if (academic.InstitutionID != OfficeCaller.InstitutionID &&
                   (previousAcademic == null || previousAcademic.InstitutionID != OfficeCaller.InstitutionID))
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "FindStudentAcademicIDNumber", OfficeCaller.ID, enErrorCode.AcademicIDNotInOffice.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Failure<StudentResultFromLDAP>(ErrorMessages.AcademicIDNotInOffice, enErrorCode.AcademicIDNotInOffice);
                }

                if (!OfficeCaller.CanViewAllAcademics.GetValueOrDefault() &&
                   ((!OfficeCaller.Academics.Any(x => x.ID == academic.ID) && previousAcademic == null) ||
                   (!OfficeCaller.Academics.Any(x => (x.ID == academic.ID) || (previousAcademic != null && x.ID == previousAcademic.ID)))))
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "FindStudentAcademicIDNumber", OfficeCaller.ID, enErrorCode.AcademicIDNotInOffice.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Failure<StudentResultFromLDAP>(ErrorMessages.AcademicIDNotInOffice, enErrorCode.AcademicIDNotInOffice);
                }

                #endregion

                StudentPractiseApiLog.Log(enServiceCaller.Office, "FindStudentAcademicIDNumber", OfficeCaller.ID, string.Empty, true, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                if (!isRegistered)
                    return Success<StudentResultFromLDAP>(response.ToStudentResult("Οι πληροφορίες αντλήθηκαν από την Ακαδημαϊκή Ταυτότητα, ο Φοιτητής δεν είναι εγγεγραμμένος στον Άτλαντα"));
                else
                    return Success<StudentResultFromLDAP>(response.ToStudentResult("Οι πληροφορίες αντλήθηκαν από την Ακαδημαϊκης Ταυτότητα, ο Φοιτητής είναι εγγεγραμένος αλλά ο αριθμός της ακαδημαϊκής του ταυτότητας δεν έχει δηλωθεί."));
            }
            catch (WebFaultException wfEx)
            {
                throw wfEx;
            }
            catch (Exception ex)
            {
                return UnhandledException<StudentResultFromLDAP>(ex, "FindStudentAcademicIDNumber", OfficeCaller.ID);
            }
        }

        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        public virtual ServiceResponse<StudentResult> GetStudentDetails(int StudentID, string AcademicIDNumber, int AcademicID, string StudentNumber, string PrincipalName)
        {
            Validate();

            try
            {
                var serializer = new Serializer<string>();
                if (StudentID != 0)
                {
                    var student = _studentRepository.Value.LoadByOfficeWithID(OfficeCaller, StudentID);
                    if (student == null)
                    {
                        StudentPractiseApiLog.Log(enServiceCaller.Office, "GetStudentDetails", OfficeCaller.ID, enErrorCode.ObjectNotFount.ToString(), false, serializer.Serialize(HttpContext.Current.Request.Url.ToString()), HttpContext.Current.Request.UserHostAddress);
                        return Failure<StudentResult>(ErrorMessages.ObjectNotFount, enErrorCode.ObjectNotFount);
                    }
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "GetStudentDetails", OfficeCaller.ID, string.Empty, true, serializer.Serialize(HttpContext.Current.Request.Url.ToString()), HttpContext.Current.Request.UserHostAddress);
                    return Success<StudentResult>(student.ToStudentResult());
                }

                else if (!string.IsNullOrEmpty(AcademicIDNumber))
                {
                    var student = _studentRepository.Value.LoadByOfficeWithAcademicIDNumber(OfficeCaller, AcademicIDNumber.Replace("-", "").Replace(" ", ""));
                    if (student == null)
                    {
                        StudentPractiseApiLog.Log(enServiceCaller.Office, "GetStudentDetails", OfficeCaller.ID, enErrorCode.ObjectNotFount.ToString(), false, serializer.Serialize(HttpContext.Current.Request.Url.ToString()), HttpContext.Current.Request.UserHostAddress);
                        return Failure<StudentResult>(ErrorMessages.ObjectNotFount, enErrorCode.ObjectNotFount);
                    }
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "GetStudentDetails", OfficeCaller.ID, string.Empty, true, serializer.Serialize(HttpContext.Current.Request.Url.ToString()), HttpContext.Current.Request.UserHostAddress);
                    return Success<StudentResult>(student.ToStudentResult());
                }

                else if (!string.IsNullOrWhiteSpace(StudentNumber) && AcademicID != 0)
                {
                    if (!OfficeCaller.Academics.Any(x => x.ID == AcademicID))
                    {
                        StudentPractiseApiLog.Log(enServiceCaller.Office, "GetStudentDetails", OfficeCaller.ID, enErrorCode.AcademicIDNotInOffice.ToString(), false, serializer.Serialize(HttpContext.Current.Request.Url.ToString()), HttpContext.Current.Request.UserHostAddress);
                        return Failure<StudentResult>(ErrorMessages.AcademicIDNotInOffice, enErrorCode.AcademicIDNotInOffice);
                    }

                    var student = _studentRepository.Value.FindByStudentNumber(OfficeCaller, StudentNumber, AcademicID);
                    if (student == null)
                    {
                        StudentPractiseApiLog.Log(enServiceCaller.Office, "GetStudentDetails", OfficeCaller.ID, enErrorCode.StudentNumberNotFound.ToString(), false, serializer.Serialize(HttpContext.Current.Request.Url.ToString()), HttpContext.Current.Request.UserHostAddress);
                        return Failure<StudentResult>(ErrorMessages.StudentNumberNotFound, enErrorCode.StudentNumberNotFound);
                    }
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "GetStudentDetails", OfficeCaller.ID, string.Empty, true, serializer.Serialize(HttpContext.Current.Request.Url.ToString()), HttpContext.Current.Request.UserHostAddress);
                    return Success<StudentResult>(student.ToStudentResult());
                }

                else if (!string.IsNullOrEmpty(PrincipalName))
                {
                    var student = _studentRepository.Value.LoadByOfficeWithPrincipalName(OfficeCaller, PrincipalName);
                    if (student == null)
                    {
                        StudentPractiseApiLog.Log(enServiceCaller.Office, "GetStudentDetails", OfficeCaller.ID, enErrorCode.ObjectNotFount.ToString(), false, serializer.Serialize(HttpContext.Current.Request.Url.ToString()), HttpContext.Current.Request.UserHostAddress);
                        return Failure<StudentResult>(ErrorMessages.ObjectNotFount, enErrorCode.ObjectNotFount);
                    }
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "GetStudentDetails", OfficeCaller.ID, string.Empty, true, serializer.Serialize(HttpContext.Current.Request.Url.ToString()), HttpContext.Current.Request.UserHostAddress);
                    return Success<StudentResult>(student.ToStudentResult());
                }
                else
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "GetStudentDetails", OfficeCaller.ID, HttpStatusCode.BadRequest.ToString(), false, serializer.Serialize(HttpContext.Current.Request.Url.ToString()), HttpContext.Current.Request.UserHostAddress);
                    throw new WebFaultException(HttpStatusCode.BadRequest);
                }
            }
            catch (WebFaultException wfEx)
            {
                throw wfEx;
            }
            catch (Exception ex)
            {
                return UnhandledException<StudentResult>(ex, "GetStudentDetails", OfficeCaller.ID);
            }
        }

        [WebInvoke(ResponseFormat = WebMessageFormat.Json)]
        public virtual ServiceResponse<StudentResult> RegisterNewStudent(StudentRequest request)
        {
            Validate();

            try
            {
                #region [ Validate Data ]

                if (request == null || string.IsNullOrWhiteSpace(request.AcademicIDNumber))
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "RegisterNewStudent", OfficeCaller.ID, HttpStatusCode.BadRequest.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    throw new WebFaultException(HttpStatusCode.BadRequest);
                }

                var academicIDNumber = request.AcademicIDNumber.Replace("-", "").Replace(" ", "");
                var student = _studentRepository.Value.FindByAcademicIDNumber(academicIDNumber);
                if (student != null)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "RegisterNewStudent", OfficeCaller.ID, enErrorCode.AcademicIDExists.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Failure<StudentResult>(string.Format(ErrorMessages.AcademicIDExists, request.AcademicIDNumber), enErrorCode.AcademicIDExists);
                }


                var response = StudentCardNumberService.GetStudentInfoByAcademicCardNumber(OfficeCaller.ID, academicIDNumber);
                if (response == null || !response.Success)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "RegisterNewStudent", OfficeCaller.ID, enErrorCode.AcademicIDNotFound.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Failure<StudentResult>(ErrorMessages.AcademicIDNotFound, enErrorCode.AcademicIDNotFound);
                }

                student = _studentRepository.Value.FindByUsernameFromLDAP(response.UsernameFromLDAP);
                if (student != null)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "RegisterNewStudent", OfficeCaller.ID, enErrorCode.AcademicIDExistsProceedToUpdate.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Failure<StudentResult>(string.Format(ErrorMessages.AcademicIDExistsProceedToUpdate, request.AcademicIDNumber), enErrorCode.AcademicIDExistsProceedToUpdate);
                }


                var academic = StudentPracticeCacheManager<Academic>.Current.Get(response.AcademicID);
                var previousAcademic = response.PreviousAcademicID.HasValue ? StudentPracticeCacheManager<Academic>.Current.Get(response.PreviousAcademicID.Value) : null;

                if (academic == null || academic.Institution == null)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "RegisterNewStudent", OfficeCaller.ID, enErrorCode.AcademicDeleted.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Failure<StudentResult>(ErrorMessages.AcademicDeleted, enErrorCode.AcademicDeleted);
                }

                if (!academic.IsActive)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "RegisterNewStudent", OfficeCaller.ID, enErrorCode.AcademicNotActive.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Failure<StudentResult>(ErrorMessages.AcademicNotActive, enErrorCode.AcademicNotActive);
                }

                if (academic.InstitutionID != OfficeCaller.InstitutionID &&
                   (previousAcademic == null || previousAcademic.InstitutionID != OfficeCaller.InstitutionID))
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "RegisterNewStudent", OfficeCaller.ID, enErrorCode.AcademicIDNotInOffice.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Failure<StudentResult>(ErrorMessages.AcademicIDNotInOffice, enErrorCode.AcademicIDNotInOffice);
                }

                if (!OfficeCaller.CanViewAllAcademics.GetValueOrDefault() &&
                   ((!OfficeCaller.Academics.Any(x => x.ID == academic.ID) && previousAcademic == null) ||
                   (!OfficeCaller.Academics.Any(x => (x.ID == academic.ID) || (previousAcademic != null && x.ID == previousAcademic.ID)))))
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "RegisterNewStudent", OfficeCaller.ID, enErrorCode.AcademicIDNotInOffice.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Failure<StudentResult>(ErrorMessages.AcademicIDNotInOffice, enErrorCode.AcademicIDNotInOffice);
                }

                #endregion

                using (var ctx = UnitOfWorkFactory.Create() as DBEntities)
                {
                    if (ctx.Connection.State != ConnectionState.Open)
                    {
                        ctx.Connection.Open();
                    }

                    using (var ts = ctx.Connection.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                    {
                        try
                        {
                            StudentRepository studentRepository = new StudentRepository(UnitOfWork);
                            var newStudent = studentRepository.CreateStudent(response, true);

                            var provider = Roles.Provider as StudentPracticeRoleProvider;
                            provider.AddUsersToRoles(new[] { newStudent.UserName }, new[] { RoleNames.Student }, ctx);
                            ts.Commit();

                            StudentPractiseApiLog.Log(enServiceCaller.Office, "RegisterNewStudent", OfficeCaller.ID, string.Empty, true, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                            return Success<StudentResult>(newStudent.ToStudentResult());
                        }
                        catch (Exception ex)
                        {
                            ts.Rollback();
                            LogHelper.LogError(ex, this, string.Format("Error while creating student from AcademicID with AcademicIDNumber {0}", request.AcademicIDNumber));
                            StudentPractiseApiLog.Log(enServiceCaller.Office, "RegisterNewStudent", OfficeCaller.ID, enErrorCode.AcIDCreationError.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                            return Failure<StudentResult>(string.Format(ErrorMessages.AcIDCreationError, request.AcademicIDNumber), enErrorCode.AcIDCreationError);
                        }
                    }
                }
            }
            catch (WebFaultException wfEx)
            {
                throw wfEx;
            }
            catch (Exception ex)
            {
                return UnhandledException<StudentResult>(ex, "RegisterNewStudent", OfficeCaller.ID);
            }
        }

        [WebInvoke(ResponseFormat = WebMessageFormat.Json)]
        public virtual ServiceResponse<StudentResult> UpdateStudent(StudentUpdateRequest request)
        {
            Validate();

            try
            {
                #region [ Validate Data]

                if (request == null || !request.ID.HasValue || string.IsNullOrWhiteSpace(request.AcademicIDNumber))
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "UpdateStudent", OfficeCaller.ID, HttpStatusCode.BadRequest.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    throw new WebFaultException(HttpStatusCode.BadRequest);
                }

                var student = _studentRepository.Value.Load(request.ID.Value, x => x.Academic);
                if (student == null)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "UpdateStudent", OfficeCaller.ID, enErrorCode.ObjectNotFount.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Failure<StudentResult>(ErrorMessages.ObjectNotFount, enErrorCode.ObjectNotFount);
                }

                if (!OfficeCaller.Academics.Any(x => x.ID == student.AcademicID))
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "UpdateStudent", OfficeCaller.ID, enErrorCode.OfficeHasNotAcademic.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Failure<StudentResult>(ErrorMessages.OfficeHasNotAcademic, enErrorCode.OfficeHasNotAcademic);
                }

                if (student.IsAssignedToPosition.HasValue && student.IsAssignedToPosition.Value)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "UpdateStudent", OfficeCaller.ID, enErrorCode.StudentUpdateErrorIsAssigned.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Failure<StudentResult>(string.Format(ErrorMessages.StudentUpdateErrorIsAssigned, request.ID.Value), enErrorCode.StudentUpdateErrorIsAssigned);
                }

                var academicIDNumber = request.AcademicIDNumber.Replace("-", "").Replace(" ", "");
                var details = StudentCardNumberService.GetStudentInfoByAcademicCardNumber(OfficeCaller.ID, academicIDNumber);
                if (details == null || !details.Success)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "UpdateStudent", OfficeCaller.ID, enErrorCode.AcademicIDNotFound.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Failure<StudentResult>(ErrorMessages.AcademicIDNotFound, enErrorCode.AcademicIDNotFound);
                }

                var academic = StudentPracticeCacheManager<Academic>.Current.Get(details.AcademicID);
                var previousAcademic = details.PreviousAcademicID.HasValue ? StudentPracticeCacheManager<Academic>.Current.Get(details.PreviousAcademicID.Value) : null;

                if (academic == null || academic.Institution == null)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "UpdateStudent", OfficeCaller.ID, enErrorCode.AcademicDeleted.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Failure<StudentResult>(ErrorMessages.AcademicDeleted, enErrorCode.AcademicDeleted);
                }

                if (!academic.IsActive)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "UpdateStudent", OfficeCaller.ID, enErrorCode.AcademicNotActive.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Failure<StudentResult>(ErrorMessages.AcademicNotActive, enErrorCode.AcademicNotActive);
                }

                if (details.AcademicID != student.AcademicID)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "UpdateStudent", OfficeCaller.ID, enErrorCode.StudentUpdateDifferentAcademicID.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Failure<StudentResult>(ErrorMessages.StudentUpdateDifferentAcademicID, enErrorCode.StudentUpdateDifferentAcademicID);
                }

                if (academic.InstitutionID != OfficeCaller.InstitutionID &&
                   (previousAcademic == null || previousAcademic.InstitutionID != OfficeCaller.InstitutionID))
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "UpdateStudent", OfficeCaller.ID, enErrorCode.AcademicIDNotInOffice.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Failure<StudentResult>(ErrorMessages.AcademicIDNotInOffice, enErrorCode.AcademicIDNotInOffice);
                }

                if (!OfficeCaller.CanViewAllAcademics.GetValueOrDefault() &&
                   ((!OfficeCaller.Academics.Any(x => x.ID == academic.ID) && previousAcademic == null) ||
                   (!OfficeCaller.Academics.Any(x => (x.ID == academic.ID) || (previousAcademic != null && x.ID == previousAcademic.ID)))))
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "UpdateStudent", OfficeCaller.ID, enErrorCode.AcademicIDNotInOffice.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Failure<StudentResult>(ErrorMessages.AcademicIDNotInOffice, enErrorCode.AcademicIDNotInOffice);
                }

                #endregion

                BusinessHelper.UpdateAllStudentInfo(student, details);
                UnitOfWork.Commit();

                StudentPractiseApiLog.Log(enServiceCaller.Office, "UpdateStudent", OfficeCaller.ID, string.Empty, true, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                return Success<StudentResult>(student.ToStudentResult());
            }
            catch (WebFaultException wfEx)
            {
                throw wfEx;
            }
            catch (Exception ex)
            {
                return UnhandledException<StudentResult>(ex, "UpdateStudent", OfficeCaller.ID);
            }
        }

        #endregion

        #region [ Lookup Table Services ]

        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        public virtual ServiceResponse<AcademicResult[]> GetAcademics()
        {
            Validate();
            var result = StudentPracticeCacheManager<Academic>.Current.GetItems();
            if (result == null) return Success<AcademicResult[]>(null);
            return Success<AcademicResult[]>(result.Select(x => x.ToServiceLookup()).ToArray());
        }

        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        public virtual ServiceResponse<InstitutionResult[]> GetInstitutions()
        {
            Validate();
            var result = StudentPracticeCacheManager<Institution>.Current.GetItems();
            if (result == null) return Success<InstitutionResult[]>(null);
            return Success<InstitutionResult[]>(result.Select(x => x.ToServiceLookup()).ToArray());
        }

        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        public virtual ServiceResponse<CountryResult[]> GetCountries()
        {
            Validate();
            var result = StudentPracticeCacheManager<Country>.Current.GetItems();
            if (result == null) return Success<CountryResult[]>(null);
            return Success<CountryResult[]>(result.Select(x => x.ToServiceLookup()).ToArray());
        }

        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        public virtual ServiceResponse<PrefectureResult[]> GetPrefectures()
        {
            Validate();
            var result = StudentPracticeCacheManager<Prefecture>.Current.GetItems();
            if (result == null) return Success<PrefectureResult[]>(null);
            return Success<PrefectureResult[]>(result.Select(x => x.ToServiceLookup()).ToArray());
        }

        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        public virtual ServiceResponse<CityResult[]> GetCities()
        {
            Validate();
            var result = StudentPracticeCacheManager<City>.Current.GetItems();
            if (result == null) return Success<CityResult[]>(null);
            return Success<CityResult[]>(result.Select(x => x.ToServiceLookup()).ToArray());
        }

        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        public virtual ServiceResponse<PhysicalObjectResult[]> GetPhysicalObjects()
        {
            Validate();
            var result = StudentPracticeCacheManager<PhysicalObject>.Current.GetItems();
            if (result == null) return Success<PhysicalObjectResult[]>(null);
            return Success<PhysicalObjectResult[]>(result.Select(x => x.ToServiceLookup()).ToArray());
        }

        #endregion

        #region [ FinishedPosition by GPA ]

        [WebInvoke(ResponseFormat = WebMessageFormat.Json)]
        public virtual ServiceResponse<GPAPosition> RegisterFinishedPosition(GPAPositionRequest request)
        {
            Validate();

            try
            {
                #region Validate Data

                if (request == null)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "RegisterFinishedPosition", OfficeCaller.ID, HttpStatusCode.BadRequest.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    throw new WebFaultException(HttpStatusCode.BadRequest);
                }
                if (!request.StudentID.HasValue ||
                    !request.ProviderID.HasValue ||
                    string.IsNullOrWhiteSpace(request.Title) ||
                    string.IsNullOrWhiteSpace(request.Description) ||
                    !request.PositionTypeInt.HasValue ||
                    !request.Duration.HasValue ||
                    string.IsNullOrWhiteSpace(request.ContactPhone) ||
                    !request.CountryID.HasValue ||
                    !request.PrefectureID.HasValue ||
                    !request.CityID.HasValue ||
                    request.PhysicalObjectsID == null ||
                    !request.PhysicalObjectsID.Any() ||
                    (!request.ImplementationStartDate.HasValue && string.IsNullOrWhiteSpace(request.ImplementationStartDateString)) ||
                    (!request.ImplementationEndDate.HasValue && string.IsNullOrWhiteSpace(request.ImplementationEndDateString)) ||
                    string.IsNullOrWhiteSpace(request.CompletionComments))
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "RegisterFinishedPosition", OfficeCaller.ID, HttpStatusCode.BadRequest.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    throw new WebFaultException(HttpStatusCode.BadRequest);
                }

                var prefecture = StudentPracticeCacheManager<Prefecture>.Current.Get(request.PrefectureID.Value);
                var city = StudentPracticeCacheManager<City>.Current.Get(request.CityID.Value);

                if (request.CountryID.Value != StudentPracticeConstants.GreeceCountryID && request.CountryID.Value != StudentPracticeConstants.CyprusCountryID)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "RegisterFinishedPosition", OfficeCaller.ID, HttpStatusCode.BadRequest.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    throw new WebFaultException(HttpStatusCode.BadRequest);
                }

                if (prefecture.CountryID != request.CountryID.Value)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "RegisterFinishedPosition", OfficeCaller.ID, HttpStatusCode.BadRequest.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    throw new WebFaultException(HttpStatusCode.BadRequest);
                }

                if (city.CountryID != request.CountryID.Value || city.PrefectureID != request.PrefectureID.Value)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "RegisterFinishedPosition", OfficeCaller.ID, HttpStatusCode.BadRequest.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    throw new WebFaultException(HttpStatusCode.BadRequest);
                }

                var provider = _providerRepository.Value.Load(request.ProviderID.Value);
                if (provider == null)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "RegisterFinishedPosition", OfficeCaller.ID, enErrorCode.ProviderNotFound.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Failure<GPAPosition>(ErrorMessages.ProviderNotFound, enErrorCode.ProviderNotFound);
                }
                var student = _studentRepository.Value.LoadByOfficeWithID(OfficeCaller, request.StudentID.Value);
                if (student == null)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "RegisterFinishedPosition", OfficeCaller.ID, enErrorCode.StudentNotFound.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Failure<GPAPosition>(ErrorMessages.StudentNotFound, enErrorCode.StudentNotFound);
                }
                //else if (student.IsAssignedToPosition.HasValue && student.IsAssignedToPosition.Value)
                //{
                //    StudentPractiseApiLog.Log(enServiceCaller.Office, "RegisterFinishedPosition", OfficeCaller.ID, enErrorCode.StudentIsAssigned.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                //    return Failure<GPAPosition>(ErrorMessages.StudentIsAssigned, enErrorCode.StudentIsAssigned);
                //}
                #endregion

                #region DateTime startDate;
                DateTime startDate;
                if (request.ImplementationStartDate.HasValue)
                {
                    startDate = request.ImplementationStartDate.Value;
                }
                else if (!string.IsNullOrWhiteSpace(request.ImplementationStartDateString))
                {
                    string format = string.IsNullOrWhiteSpace(request.ImplementationStartDateStringFormat) ? "dd/MM/yyyy" : request.ImplementationStartDateStringFormat;
                    startDate = DateTime.ParseExact(request.ImplementationStartDateString, format, System.Globalization.CultureInfo.InvariantCulture);
                }
                else
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "RegisterFinishedPosition", OfficeCaller.ID, HttpStatusCode.BadRequest.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    throw new WebFaultException(HttpStatusCode.BadRequest);
                }
                #endregion

                #region DateTime endDate;
                DateTime endDate;
                if (request.ImplementationEndDate.HasValue) endDate = request.ImplementationEndDate.Value;
                else if (!string.IsNullOrWhiteSpace(request.ImplementationEndDateString))
                {
                    string format = string.IsNullOrWhiteSpace(request.ImplementationEndDateStringFormat) ? "dd/MM/yyyy" : request.ImplementationEndDateStringFormat;
                    endDate = DateTime.ParseExact(request.ImplementationEndDateString, format, System.Globalization.CultureInfo.InvariantCulture);
                }
                else
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "RegisterFinishedPosition", OfficeCaller.ID, HttpStatusCode.BadRequest.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    throw new WebFaultException(HttpStatusCode.BadRequest);
                }
                #endregion

                #region Group

                var group = new InternshipPositionGroup();
                group.ProviderID = request.ProviderID.Value;
                group.TotalPositions = 1;
                group.AvailablePositions = 0;
                group.PreAssignedPositions = 1;
                group.IsVisibleToAllAcademics = false;
                group.Title = request.Title;
                group.Description = request.Description;
                group.Duration = request.Duration.Value;
                group.CityID = request.CityID.Value;
                group.PrefectureID = request.PrefectureID.Value;
                group.CountryID = request.CountryID.Value;
                group.NoTimeLimit = false;
                group.StartDate = startDate;
                group.EndDate = endDate;
                group.PositionTypeInt = request.PositionTypeInt.Value;
                group.ContactPhone = request.ContactPhone;
                group.Supervisor = request.Supervisor;
                group.SupervisorEmail = request.SupervisorEmail;
                group.PositionGroupStatus = enPositionGroupStatus.Published;
                group.PositionCreationType = enPositionCreationType.FromOffice;
                group.FirstPublishedAt = DateTime.Now;
                group.LastPublishedAt = DateTime.Now;
                UnitOfWork.MarkAsNew(group);

                foreach (var id in request.PhysicalObjectsID)
                {
                    group.PhysicalObjects.Add(new PhysicalObjectRepository(UnitOfWork).Load(id));
                }

                group.Academics.Add(new AcademicRepository(UnitOfWork).Load(student.AcademicID.Value));

                #endregion

                #region Position

                int? academicID = null;
                if (student.AcademicID.HasValue && OfficeCaller.Academics.Any(x => x.ID == student.AcademicID))
                    academicID = student.AcademicID.Value;
                else if (student.PreviousAcademicID.HasValue && OfficeCaller.Academics.Any(x => x.ID == student.PreviousAcademicID))
                    academicID = student.PreviousAcademicID.Value;
                else
                    academicID = student.AcademicID;

                var position = new InternshipPosition();
                position.InternshipPositionGroup = group;
                position.PositionStatusInt = (int)enPositionStatus.Completed;
                position.PreAssignedByMasterAccountID = OfficeCaller.IsMasterAccount ? OfficeCaller.ID : OfficeCaller.MasterAccountID;
                position.PreAssignedByOfficeID = OfficeCaller.ID;
                position.PreAssignedForAcademicID = academicID;
                position.PreAssignedAt = DateTime.Now;
                position.DaysLeftForAssignment = 0;
                position.AssignedToStudentID = request.StudentID;
                position.AssignedAt = DateTime.Now;
                position.ImplementationStartDate = startDate;
                position.ImplementationEndDate = endDate;
                position.CompletedAt = endDate;
                position.CompletionComments = request.CompletionComments;
                position.UpdatedAt = DateTime.Now;
                position.UpdatedBy = OfficeCaller.UserName;

                #endregion

                #region Group Log
                InternshipPositionGroupLog log = new InternshipPositionGroupLog();
                log.InternshipPositionGroup = group;
                log.OldStatus = group.PositionGroupStatus;
                log.NewStatus = group.PositionGroupStatus;
                log.CreatedAt = DateTime.Now;
                log.CreatedAtDateOnly = DateTime.Now.Date;
                log.CreatedBy = Thread.CurrentPrincipal.Identity.Name;
                log.UpdatedAt = DateTime.Now;
                log.UpdatedAtDateOnly = DateTime.Now.Date;
                log.UpdatedBy = Thread.CurrentPrincipal.Identity.Name;
                #endregion

                #region Position Log
                InternshipPositionLog logEntry = new InternshipPositionLog();
                logEntry.InternshipPosition = position;
                logEntry.OldStatus = enPositionStatus.UnPublished;
                logEntry.NewStatus = enPositionStatus.Canceled;
                logEntry.CreatedAt = DateTime.Now;
                logEntry.CreatedAtDateOnly = DateTime.Now.Date;
                logEntry.CreatedBy = Thread.CurrentPrincipal.Identity.Name;
                UnitOfWork.MarkAsNew(logEntry);
                #endregion

                UnitOfWork.Commit();


                StudentPractiseApiLog.Log(enServiceCaller.Office, "RegisterFinishedPosition", OfficeCaller.ID, string.Empty, true, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                return Success<GPAPosition>(position.ToGPAPosition());
            }
            catch (WebFaultException wfEx)
            {
                throw wfEx;
            }
            catch (Exception ex)
            {
                return UnhandledException<GPAPosition>(ex, "RegisterFinishedPosition", OfficeCaller.ID);
            }
        }

        [WebInvoke(ResponseFormat = WebMessageFormat.Json)]
        public virtual ServiceResponse<GPAPosition[]> GetFinishedPositions(Paging request)
        {
            Validate();

            try
            {
                int take = request == null || !request.Take.HasValue ? ApiConfig.Current.MaximumItemsReturned : request.Take.Value;
                int skip = request == null || !request.Skip.HasValue ? 0 : request.Skip.Value;
                var positions = _positionRepository.Value.FindFinishedPositions(OfficeCaller, skip, take,
                            x => x.InternshipPositionGroup, x => x.InternshipPositionGroup.PhysicalObjects, x => x.InternshipPositionGroup.Academics);
                if (positions == null)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "GetFinishedPositions", OfficeCaller.ID, string.Empty, true, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Success<GPAPosition[]>(null);
                }
                StudentPractiseApiLog.Log(enServiceCaller.Office, "GetFinishedPositions", OfficeCaller.ID, string.Empty, true, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                return Success<GPAPosition[]>(positions.Select(x => x.ToGPAPosition()).ToArray());
            }
            catch (WebFaultException wfEx)
            {
                throw wfEx;
            }
            catch (Exception ex)
            {
                return UnhandledException<GPAPosition[]>(ex, "GetFinishedPositions", OfficeCaller.ID);
            }
        }

        [WebInvoke(ResponseFormat = WebMessageFormat.Json)]
        public virtual ServiceResponse DeleteFinishedPosition(Delete request)
        {
            Validate();

            if (request == null || request.PositionID == 0)
            {
                StudentPractiseApiLog.Log(enServiceCaller.Office, "DeleteFinishedPosition", OfficeCaller.ID, HttpStatusCode.BadRequest.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                throw new WebFaultException(HttpStatusCode.BadRequest);
            }
            try
            {
                var position = _positionRepository.Value.Load(request.PositionID, x => x.InternshipPositionGroup, x => x.AssignedToStudent);
                if (position == null || position.InternshipPositionGroup.PositionCreationType == enPositionCreationType.FromProvider)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "DeleteFinishedPosition", OfficeCaller.ID, enErrorCode.FinishedPositionNotExists.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Failure(string.Format(ErrorMessages.FinishedPositionNotExists, request.PositionID), enErrorCode.FinishedPositionNotExists);
                }
                else if (position.CreatedBy != OfficeCaller.UserName)
                {
                    StudentPractiseApiLog.Log(enServiceCaller.Office, "DeleteFinishedPosition", OfficeCaller.ID, enErrorCode.FinishedPositionNotInUser.ToString(), false, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                    return Failure(string.Format(ErrorMessages.FinishedPositionNotInUser, request.PositionID), enErrorCode.FinishedPositionNotInUser);
                }

                position.InternshipPositionGroup.PositionGroupStatus = enPositionGroupStatus.Deleted;
                position.PositionStatus = enPositionStatus.Canceled;

                InternshipPositionGroupLog gLog = new InternshipPositionGroupLog();
                gLog.CreatedAt = DateTime.Now;
                gLog.CreatedAtDateOnly = DateTime.Now.Date;
                gLog.CreatedBy = OfficeCaller.UserName;
                gLog.GroupID = position.GroupID;
                gLog.OldStatus = enPositionGroupStatus.Published;
                gLog.NewStatus = enPositionGroupStatus.Deleted;

                InternshipPositionLog pLog = new InternshipPositionLog();
                pLog.CreatedAt = DateTime.Now;
                pLog.CreatedAtDateOnly = DateTime.Now.Date;
                pLog.CreatedBy = OfficeCaller.UserName;
                pLog.InternshipPositionID = position.ID;
                pLog.OldStatus = enPositionStatus.Completed;
                pLog.NewStatus = enPositionStatus.Canceled;

                UnitOfWork.MarkAsNew(pLog);
                UnitOfWork.MarkAsNew(gLog);
                UnitOfWork.Commit();

                StudentPractiseApiLog.Log(enServiceCaller.Office, "DeleteFinishedPosition", OfficeCaller.ID, string.Empty, true, OperationContext.Current.RequestContext.RequestMessage.ToString(), HttpContext.Current.Request.UserHostAddress);
                return Success();
            }
            catch (WebFaultException wfEx)
            {
                throw wfEx;
            }
            catch (Exception ex)
            {
                return UnhandledException(ex, "DeleteFinishedPosition", OfficeCaller.ID);
            }
        }

        #endregion
    }
}