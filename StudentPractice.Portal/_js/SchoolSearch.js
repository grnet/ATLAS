/// <reference name="MicrosoftAjax.js" />

Type.registerNamespace("StudentPractice");

StudentPractice.SchoolSearch = function () {

    var _schoolCodeHiddenFieldID = '';
    var _institutionNameLabelID = '';
    var _schoolNameLabelID = '';
    var _departmentNameLabelID = '';
    var _addressLabelID = '';
    var _zipCodeLabelID = '';
    var _prefectureID = '';
    var _cityID = '';

    return {
        onSchoolSelected: function (selectedValues) {
            var hfSchoolCode = document.getElementById(_schoolCodeHiddenFieldID);
            var txtInstitution = document.getElementById(_institutionNameLabelID);
            var txtSchool = document.getElementById(_schoolNameLabelID);
            var txtDepartment = document.getElementById(_departmentNameLabelID);
            var lblAddress = document.getElementById(_addressLabelID);
            var lblZipCode = document.getElementById(_zipCodeLabelID);
            var lblPrefecture = document.getElementById(_prefectureID);
            var lblCity = document.getElementById(_cityID);

            hfSchoolCode.value = selectedValues[0];
            txtInstitution.value = selectedValues[1];
            txtSchool.value = (selectedValues[2] == null ? '' : selectedValues[2]);
            txtDepartment.value = selectedValues[3];

            if (lblAddress != null)
                lblAddress.innerHTML = selectedValues[4];

            if (lblZipCode != null)
                lblZipCode.innerHTML = selectedValues[5];

            if (lblPrefecture != null)
                lblPrefecture.innerHTML = selectedValues[6];

            if (lblCity != null)
                lblCity.innerHTML = selectedValues[7];

            document.getElementById('lnkRemoveSchoolSelection').style.display = '';
            document.getElementById('lnkSelectSchool').style.display = 'none';
            popUp.hide();

            if (typeof (schoolSelectedCallback) !== 'undefined') {
                schoolSelectedCallback();
            }
        },

        removeSchoolSelection: function () {
            var hfSchoolCode = document.getElementById(_schoolCodeHiddenFieldID);
            var txtInstitution = document.getElementById(_institutionNameLabelID);
            var txtSchool = document.getElementById(_schoolNameLabelID);
            var txtDepartment = document.getElementById(_departmentNameLabelID);
            var lblAddress = document.getElementById(_addressLabelID);
            var lblZipCode = document.getElementById(_zipCodeLabelID);
            var lblPrefecture = document.getElementById(_prefectureID);
            var lblCity = document.getElementById(_cityID);

            hfSchoolCode.value = '';
            txtInstitution.value = '';
            txtSchool.value = '';
            txtDepartment.value = '';

            if (lblAddress != null)
                lblAddress.innerHTML = '';
            if (lblZipCode != null)
                lblZipCode.innerHTML = '';
            if (lblPrefecture != null)
                lblPrefecture.innerHTML = '';
            if (lblCity != null)
                lblCity.innerHTML = '';

            document.getElementById('lnkRemoveSchoolSelection').style.display = 'none';
            document.getElementById('lnkSelectSchool').style.display = '';
        },

        init: function (hfSchoolCodeClientID, txtInstitutionNameClientID, txtSchoolNameClientID, txtDepartmentNameClientID, lblAddressClientID, lblZipCodeClientID, lblPrefectureID, lblCityID) {
            _schoolCodeHiddenFieldID = hfSchoolCodeClientID;
            _institutionNameLabelID = txtInstitutionNameClientID;
            _schoolNameLabelID = txtSchoolNameClientID;
            _departmentNameLabelID = txtDepartmentNameClientID;
            _addressLabelID = lblAddressClientID;
            _zipCodeLabelID = lblZipCodeClientID;
            _cityID = lblCityID;
            _prefectureID = lblPrefectureID;
            if (document.getElementById(_schoolCodeHiddenFieldID) != null) {
                if (document.getElementById(_schoolCodeHiddenFieldID).value != '') {
                    if (document.getElementById('lnkRemoveSchoolSelection') != null) {
                        document.getElementById('lnkRemoveSchoolSelection').style.display = '';
                    }
                    if (document.getElementById('lnkSelectSchool') != null) {
                        document.getElementById('lnkSelectSchool').style.display = 'none';
                    }
                }
                else {
                    document.getElementById('lnkRemoveSchoolSelection').style.display = 'none';
                    document.getElementById('lnkSelectSchool').style.display = '';
                }
            }
        }
    };
}();

var hd = StudentPractice.SchoolSearch;