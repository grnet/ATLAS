var btnEmail;
var btnMobilePhone;
var btnUnlock;
var emailError;
var mobilePhoneError;
var unlockError;

$(function () {
    btnEmail = $('#btnEmail');
    btnMobilePhone = $('#btnMobilePhone');
    btnUnlock = $('#btnUnlock');
    emailError = $('#emailError');
    mobilePhoneError = $('#mobilePhoneError');
    unlockError = $('#mobilePhoneError');

    if (btnEmail.length != 0) {
        btnEmail.click(function () {
            var buttons = {};
            buttons[Resources.UserContactDetails_EmailChange] = true;
            buttons[Resources.Global_Cancel] = false;

            var onblur = "$(this).removeClass('focused');";
            var onfocus = "$(this).addClass('focused');";
            var txt = Resources.UserContactDetails_EmailNew +': <input type="text" class="tb" onfocus="' + onfocus + '" onblur="' + onblur + '" id="email" name="email" value="" />';

            $.prompt(txt, { 
                callback: beginChangeEmail, 
                show: 'dropIn', 
                buttons: buttons
            });
            return false;
        });
    }

    if (btnMobilePhone.length != 0) {
        btnMobilePhone.click(function () {
            var buttons = {};
            buttons[Resources.UserContactDetails_MobileChange] = true;
            buttons[Resources.Global_Cancel] = false;

            var onblur = "$(this).removeClass('focused');";
            var onfocus = "$(this).addClass('focused');";
            var txt = Resources.UserContactDetails_MobileNew + ': <input type="text" class="tb" onfocus="' + onfocus + '" onblur="' + onblur + '" id="mobilePhone" name="mobilePhone" value="" />';

            $.prompt(txt, { 
                callback: beginChangeMobilePhone, 
                show: 'dropIn', 
                buttons: buttons
            });
            return false;
        });
    }

    if (btnUnlock.length != 0) {
        btnUnlock.click(function () {
            btnUnlock.addClass('bg-loading');
            begin(unlock);
            return false;
        });
    }
});

function beginChangeEmail(v, m, f) {
    if (v != undefined && v == true) {
        var newEmail = m.find('#email').val();
        if (!newEmail.match(/^([a-zA-Z0-9_\-])+(\.([a-zA-Z0-9_\-])+)*@((\[(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5]))\]))|((([a-zA-Z0-9])+(([\-])+([a-zA-Z0-9])+)*\.)+([a-zA-Z])+(([\-])+([a-zA-Z0-9])+)*))$/)) {
            showError(emailError, Resources.UserContactDetails_EmailInvalid.replace('{newEmail}', newEmail));
            return;
        }
        var userContext = {};
        userContext.errorArea = emailError;
        userContext.btn = btnEmail;
        userContext.keepBtnVisible = true;
        userContext.email = newEmail;
        userContext.errorMessage = Resources.UserContactDetails_ErrorMessage;
        userContext.successMessage = Resources.UserContactDetails_SuccessMessage;
        userContext.failMessage = Resources.UserContactDetails_EmailInUse.replace('{email}', newEmail);

        if (typeof (_USERNAME) === 'undefined') {
            StudentPractice.Portal.PortalServices.Services.ChangeEmail(null, newEmail, pageMethodCompleted, onFailed, userContext);
        }
        else {
            StudentPractice.Portal.PortalServices.Services.ChangeEmail(_USERNAME, newEmail, pageMethodCompleted, onFailed, userContext);
        }
    }
}

function beginChangeMobilePhone(v, m, f) {
    if (v != undefined && v == true) {
        var newMobilePhone = m.find('#mobilePhone').val();
        if (!newMobilePhone.match(/^69[0-9]{8}$/)) {
            showError(mobilePhoneError, Resources.UserContactDetails_EmailInvalid.replace('{newMobilePhone}', newMobilePhone));
            return;
        }
        var userContext = {};
        userContext.errorArea = mobilePhoneError;
        userContext.btn = btnMobilePhone;
        userContext.keepBtnVisible = true;
        userContext.mobilePhone = newMobilePhone;
        userContext.errorMessage = Resources.UserContactDetails_ErrorMessage;
        userContext.successMessage = Resources.UserContactDetails_MobileSuccess;
        userContext.failMessage = Resources.UserContactDetails_MobileLimit;

        if (typeof (_USERNAME) === 'undefined') {
            StudentPractice.Portal.PortalServices.Services.ChangeMobilePhone(null, newMobilePhone, pageMethodCompleted, onFailed, userContext);
        }
        else {
            StudentPractice.Portal.PortalServices.Services.ChangeMobilePhone(_USERNAME, newMobilePhone, pageMethodCompleted, onFailed, userContext);
        }
    }
}

function begin(fn) {
    setTimeout(fn, 1000);
}

function unlock() {
    var userContext = {};
    userContext.btn = btnUnlock;
    userContext.errorArea = unlockError;
    userContext.unlock = Resources.Global_No;
    userContext.errorMessage = Resources.UserContactDetails_ErrorMessage;
    
    if (typeof (_USERNAME) === 'undefined') {
        StudentPractice.Portal.PortalServices.Services.UnlockUser(null, pageMethodCompleted, onFailed, userContext);
    }
    else {
        StudentPractice.Portal.PortalServices.Services.UnlockUser(_USERNAME, pageMethodCompleted, onFailed, userContext);
    }
}


//Helpers
function onFailed(args, userContext) {
    userContext.btn.removeClass('bg-loading');
    showError(userContext.errorArea, userContext.errorMessage);
}

function pageMethodCompleted(args, userContext) {
    if (args != null) {
        if (args) {
            if (userContext.successMessage) {
                showSuccess(userContext.errorArea, userContext.successMessage);
            }
            if (userContext.email) {
                if ($('#txtEmail').length != 0) {
                    $('#txtEmail').val(userContext.email);
                }
                if ($('#ltrEmail').length != 0) {
                    $('#ltrEmail').html(userContext.email);
                }
            }
            else if (userContext.mobilePhone) {
                if ($('#txtMobilePhone').length != 0) {
                    $('#txtMobilePhone').val(userContext.mobilePhone);
                }
                if ($('#ltrMobilePhone').length != 0) {
                    $('#ltrMobilePhone').html(userContext.mobilePhone);
                }
            }
            else if (userContext.unlock) {
                if ($('#ltrIsLockedOut').length != 0) {
                    $('#ltrIsLockedOut').html(userContext.unlock);
                }
            }
            if (!userContext.keepBtnVisible)
                userContext.btn.fadeOut();
        }
        else {
            userContext.btn.removeClass('bg-loading');
            showError(userContext.errorArea, userContext.failMessage);
        }
    }
    else {
        userContext.btn.removeClass('bg-loading');
        showError(userContext.errorArea, userContext.errorMessage);
    }
}

function showSuccess(element, message) {
    element.html('<span style="color:green;"> ' + Imis.Lib.HtmlDecode(message) + '</span>');
    setTimeout(function () {
        element.children().fadeOut('normal', function () {
            element.children().remove();
        });
    }, 8000);
}

function showError(element, message) {
    element.html('<span style="color:red"> ' + Imis.Lib.HtmlDecode(message) + '</span>');
    setTimeout(function () {
        element.children().fadeOut('normal', function () {
            element.children().remove();
        });
    }, 8000);
}

