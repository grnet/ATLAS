var endpoint = '';
var auth = null;

ServiceProxy = function () {
    function call(method, url, jsonData, onSuccess, onError) {
        var data = null;
        if (jsonData == null) {
            data = {};
        }
        else {
            data = JSON.stringify(jsonData);
        }
        $.ajax({
            url: url,
            data: data,
            type: method,
            beforeSend: function (xhrObj) {
                xhrObj.setRequestHeader("access_token", auth);
            },
            cache: false,
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            success: function (data) {
                if (typeof (onSuccess) === 'function') {
                    onSuccess(data);
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                if (typeof (onError) === 'function') {
                    onError({
                        HttpCode: xhr.status,
                        StatusText: textStatus,
                        HttpStatus: errorThrown,
                        ResponseText: xhr.responseText
                    });
                }
            }
        });
    }

    return {
        Login: function (jsonData, onSuccess, onError) {
            var url = endpoint + 'Login';
            call('POST', url, jsonData, onSuccess, onError);
        },

        GetAvailablePositionGroups: function (jsonData, onSuccess, onError) {
            var url = endpoint + 'GetAvailablePositionGroups';
            call('POST', url, jsonData, onSuccess, onError);
        },

        GetPositionGroupDetails: function (ID, onSuccess, onError) {
            var url = endpoint + 'GetPositionGroupDetails?ID=' + ID;
            call('GET', url, null, onSuccess, onError);
        },

        GetProviderDetails: function (ID, onSuccess, onError) {
            var url = endpoint + 'GetProviderDetails?ID=' + ID;
            call('GET', url, null, onSuccess, onError);
        },

        GetProvidersByAFM: function (AFM, onSuccess, onError) {
            var url = endpoint + 'GetProvidersByAFM?AFM=' + AFM;
            call('GET', url, null, onSuccess, onError);
        },

        PreAssignPositions: function (jsonData, onSuccess, onError) {
            var url = endpoint + 'PreAssignPositions';
            call('POST', url, jsonData, onSuccess, onError);
        },

        RollbackPreAssignment: function (jsonData, onSuccess, onError) {
            var url = endpoint + 'RollbackPreAssignment';
            call('POST', url, jsonData, onSuccess, onError);
        },

        RollbackPreAssignmentInfo: function (jsonData, onSuccess, onError) {
            var url = endpoint + 'RollbackPreAssignment/info';
            call('POST', url, jsonData, onSuccess, onError);
        },

        GetPreAssignedPositions: function (jsonData, onSuccess, onError) {
            var url = endpoint + 'GetPreAssignedPositions';
            call('POST', url, jsonData, onSuccess, onError);
        },

        AssignStudent: function (jsonData, onSuccess, onError) {
            var url = endpoint + 'AssignStudent';
            call('POST', url, jsonData, onSuccess, onError);
        },

        ChangeAssignedStudent: function (jsonData, onSuccess, onError) {
            var url = endpoint + 'ChangeAssignedStudent';
            call('POST', url, jsonData, onSuccess, onError);
        },

        ChangeAssignedStudent: function (jsonData, onSuccess, onError) {
            var url = endpoint + 'ChangeAssignedStudent';
            call('POST', url, jsonData, onSuccess, onError);
        },

        ChangeImplementationData: function (jsonData, onSuccess, onError) {
            var url = endpoint + 'ChangeImplementationData';
            call('POST', url, jsonData, onSuccess, onError);
        },

        DeleteAssignment: function (jsonData, onSuccess, onError) {
            var url = endpoint + 'DeleteAssignment';
            call('POST', url, jsonData, onSuccess, onError);
        },

        DeleteAssignmentInfo: function (jsonData, onSuccess, onError) {
            var url = endpoint + 'DeleteAssignment/info';
            call('POST', url, jsonData, onSuccess, onError);
        },

        GetAssignedPositions: function (jsonData, onSuccess, onError) {
            var url = endpoint + 'GetAssignedPositions';
            call('POST', url, jsonData, onSuccess, onError);
        },

        CompletePosition: function (jsonData, onSuccess, onError) {
            var url = endpoint + 'CompletePosition';
            call('POST', url, jsonData, onSuccess, onError);
        },

        GetCompletedPositions: function (jsonData, onSuccess, onError) {
            var url = endpoint + 'GetCompletedPositions';
            call('POST', url, jsonData, onSuccess, onError);
        },

        CancelPosition: function (jsonData, onSuccess, onError) {
            var url = endpoint + 'CancelPosition';
            call('POST', url, jsonData, onSuccess, onError);
        },

        GetRegisteredStudents: function (jsonData, onSuccess, onError) {
            var url = endpoint + 'GetRegisteredStudents';
            call('POST', url, jsonData, onSuccess, onError);
        },

        GetStudentDetailsID: function (StudentID, onSuccess, onError) {
            var url = endpoint + 'GetStudentDetails?StudentID=' + StudentID;
            call('GET', url, null, onSuccess, onError);
        },

        GetStudentDetailsPrincipalName: function (PrincipalName, onSuccess, onError) {
            var url = endpoint + 'GetStudentDetails?PrincipalName=' + PrincipalName;
            call('GET', url, null, onSuccess, onError);
        },

        GetStudentDetailsAcademicIDNumber: function (AcademicIDNumber, onSuccess, onError) {
            var url = endpoint + 'GetStudentDetails?AcademicIDNumber=' + AcademicIDNumber;
            call('GET', url, null, onSuccess, onError);
        },

        GetStudentDetailsStudentNumber: function (AcademicID, StudentNumber, onSuccess, onError) {
            var url = endpoint + 'GetStudentDetails?StudentNumber=' + StudentNumber + "&AcademicID=" + AcademicID;
            call('GET', url, null, onSuccess, onError);
        },

        FindStudentWithAcademicIDNumber: function (jsonData, onSuccess, onError) {
            var url = endpoint + 'FindStudentWithAcademicIDNumber';
            call('POST', url, jsonData, onSuccess, onError);
        },

        FindAcademicIDNumber: function (jsonData, onSuccess, onError) {
            var url = endpoint + 'FindAcademicIDNumber';
            call('POST', url, jsonData, onSuccess, onError);
        },

        RegisterNewStudent: function (jsonData, onSuccess, onError) {
            var url = endpoint + 'RegisterNewStudent';
            call('POST', url, jsonData, onSuccess, onError);
        },

        UpdateStudent: function (jsonData, onSuccess, onError) {
            var url = endpoint + 'UpdateStudent';
            call('POST', url, jsonData, onSuccess, onError);
        },

        GetAcademics: function (ID, onSuccess, onError) {
            var url = endpoint + 'GetAcademics';
            call('GET', url, null, onSuccess, onError);
        },

        GetInstitutions: function (ID, onSuccess, onError) {
            var url = endpoint + 'GetInstitutions';
            call('GET', url, null, onSuccess, onError);
        },

        GetCountries: function (ID, onSuccess, onError) {
            var url = endpoint + 'GetCountries';
            call('GET', url, null, onSuccess, onError);
        },

        GetPrefectures: function (ID, onSuccess, onError) {
            var url = endpoint + 'GetPrefectures';
            call('GET', url, null, onSuccess, onError);
        },

        GetCities: function (ID, onSuccess, onError) {
            var url = endpoint + 'GetCities';
            call('GET', url, null, onSuccess, onError);
        },

        GetPhysicalObjects: function (ID, onSuccess, onError) {
            var url = endpoint + 'GetPhysicalObjects';
            call('GET', url, null, onSuccess, onError);
        },

        RegisterFinishedPosition: function (jsonData, onSuccess, onError) {
            var url = endpoint + 'RegisterFinishedPosition';
            call('POST', url, jsonData, onSuccess, onError);
        },

        GetFinishedPositions: function (jsonData, onSuccess, onError) {
            var url = endpoint + 'GetFinishedPositions';
            call('POST', url, jsonData, onSuccess, onError);
        },

        DeleteFinishedPosition: function (jsonData, onSuccess, onError) {
            var url = endpoint + 'DeleteFinishedPosition';
            call('POST', url, jsonData, onSuccess, onError);
        },

        ChangeFundingType: function (jsonData, onSuccess, onError) {
            var url = endpoint + 'ChangeFundingType';
            call('POST', url, jsonData, onSuccess, onError);
        },

        GetFundingType: function (positionID, onSuccess, onError) {
            var url = endpoint + 'GetFundingType?positionID=' + positionID;
            call('GET', url, null, onSuccess, onError);
        }

        /*------------------------------------------------------------------------------------------*/
    };
}();