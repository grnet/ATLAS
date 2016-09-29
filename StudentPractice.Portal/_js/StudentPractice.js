/*
* Callbacks and Validators
*/
function clearErrors() {
    $('#divErrors').hide();
}

/*
* Used at:
*   EditOffice.aspx
*   EditProvider.aspx
*   AddOfficeUser.aspx
*   EditOfficeUser.aspx
*   AddProviderUser.aspx
*   EditProviderUser.aspx
*   PositionsDetails.aspx
*   ContactInfoDetails.aspx
*/
function ValidatePage(pageCode) {
    //Function which calls the whole validation for the page.
    if (Page_ClientValidate(pageCode)) {
        switch (pageCode) {
            case "vgEditOffice":
                return confirm(Resources.StudentPractice_vgEditOffice);
                break;
            case "vgEditProvider":
                return confirm(Resources.StudentPractice_vgEditProvider);
                break;
            case "vgOfficeUser":
            case "vgProviderUser":
            case "vdApplication":
            default:
                return true;
                break;
        }
    }
    else {
        hideValidatorCallout();
        return false;
    }
}

function hideValidatorCallout() {
    Sys.Extended.UI.ValidatorCalloutBehavior._currentCallout.hide();
}

function onSchoolSelected() {
    gv.GetRowValues(gv.GetFocusedRowIndex(), 'ID;Institution;School;Department;Address;ZipCode;PrefectureName;CityName', window.parent.hd.onSchoolSelected);
    return false;
}

/*
* Used at:
*   HelpDeskUsers.aspx
*   OfficeUsers.aspx
*   ProviderUsers.aspx
*/
function onActionDoneUsers(result, pageCode) {
    var splitted = result.split(':');
    if (splitted[0] == "ERROR") {
        alert('Προέκυψε ένα σφάλμα κατά την εκτέλεση της ενέργειας! Παρακαλώ προσπαθήστε ξανά.');
    }
    else if (splitted[0] == "CANNOTDELETE") {
        switch (pageCode) {
            case "HelpDeskUsers":
                alert(Resources.StudentPractice_HelpDeskUsers);
                break;
            case "OfficeUsers":
                alert(Resources.StudentPractice_OfficeUsers);
                break;
            case "ProviderUsers":
                alert(Resources.StudentPractice_ProviderUsers);
                break;
        }
    }
    else if (splitted[0] == "CANDELETE") {
        var params = ['delete', splitted[1]].join(':');
        gv.PerformCallback(params);
    }
    else {
        window.location = splitted[0];
    }
}

/*
* Used at:
*   BlockedPositions.aspx
*   SearchPositionGroups.aspx
*   WithDrawPositions.aspx
*   OfficeDetails.aspx
*   InternshipPositions.aspx
*   PositionAcademics.aspx
*   PositionPhysicalObject.aspx
*/
function onActionDone(result) {
    if (result == "ERROR")
        alert(Resources.StudentPractice_Error);
    else if (result == "DATABIND")
        gv.PerformCallback('databind:0');
    else
        window.location = result;
}

function doAction(actionName, id, pageCode) {
    var params = [actionName, id].join(':');

    switch (pageCode) {
        case "OfficeDetails":
        case "PositionAcademics":
            if (actionName == 'delete') {
                gv.GetValuesOnCustomCallback(params, onActionDone);
            }
            break;

        case "InternshipPositions":
            if ((actionName == 'clone' && confirm(Resources.InternshipPositions_Clone))
               || actionName == 'edit') {
                gv.GetValuesOnCustomCallback(params, onActionDone);
            }
            else if ((actionName == 'delete' && confirm(Resources.InternshipPositions_Delete))
                    || (actionName == 'cancel' && confirm(Resources.InternshipPositions_Cancel))
                    || actionName == 'publish'
                    || actionName == 'unpublish') {
                gv.PerformCallback(params);
            }
            break;

        case "PositionPhysicalObject":
            if (actionName == 'delete') {
                gv.PerformCallback(params);
            }
            break;

        case "SelectedPositions":
            if ((actionName == 'rollbackpreassignment' && confirm(Resources.SelectedPositions_RollbackPreassignment)) ||
                (actionName == 'rollbackpreassignment2' && confirm(Resources.SelectedPositions_RollbackPreassignment2)) ||
                (actionName == 'rollbackcompletion' && confirm(Resources.SelectedPositions_RollbackCompletion)) ||
                (actionName == 'rollbackcancellation' && confirm(Resources.SelectedPositions_RollbackCancellation)) ||
                (actionName == 'deleteassignment' && confirm(Resources.SelectedPositions_DeleteAssignment)) ||
                (actionName == 'deleteassignment2' && confirm(Resources.SelectedPositions_DeleteAssignment2)) ||
                (actionName == 'deletefinishedposition' && confirm(Resources.SelectedPositions_DeleteFinishedPosition))) {
                gv.PerformCallback(params);
            }
            break;

        case "HelpDeskUsers":
        case "ProviderUsers":
        case "OfficeUsers":
            if (actionName == 'delete' && confirm(Resources.OfficeUsers_Delete)) {
                gv.GetValuesOnCustomCallback(params, function (result) { onActionDoneUsers(result, pageCode); });
            }
            else if ((actionName == 'lock' && confirm(Resources.OfficeUsers_Lock)) ||
                     (actionName == 'unlock' && confirm(Resources.OfficeUsers_Unlock))) {
                gv.PerformCallback(params);
            }
            break;
        default:
            if (actionName == 'undeletegroup' && confirm(Resources.StudentPractice_UndeleteGroup)) {
                gv.PerformCallback(params);
            }
            if (actionName == 'unpublish' ||
                actionName == 'publish' ||
               (actionName == 'revoke' && confirm(Resources.StudentPractice_Revoke)) ||
               (actionName == 'rollback' && confirm(Resources.StudentPractice_Rollback)) ||
               (actionName == 'rollbackpublish' && confirm(Resources.StudentPractice_RollbackPublish))) {
                gv.PerformCallback(params);
            }
            if (actionName == 'deleteblockedpositiongroup' && confirm(Resources.StudentPractice_DeleteBlocked)) {
                gv.PerformCallback(params);
            }
            break;
    }

    return false;
}

/*
* Used at:
*   StudentDetails.aspx
*/
function validate(group) {
    return Page_ClientValidate(group);
}