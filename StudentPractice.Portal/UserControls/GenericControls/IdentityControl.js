/// <reference name="MicrosoftAjax.js"/>
/// <reference path="/_js/jquery.js"/>
Type.registerNamespace("StudentPractice.Portal.UserControls.GenericControls");

StudentPractice.Portal.UserControls.GenericControls.IdentityControl = function (element) {
    StudentPractice.Portal.UserControls.GenericControls.IdentityControl.initializeBase(this, [element]);
    this._cssClass = 'SelfPublisher';
    this._idType = null;
    this._rblist = null;
    this._trIdIssuer = null;
    this._trIdIssueDate = null;
    this._txtIdIssuer = null;
    this._txtIdIssueDate = null;
    this._txtIdNumber = null;
    this._lblIdNumber = null;
    this._valGroup = null;
    this._cvIssueDate = null;
    this._rfvIssueDate = null;
    this._rfvIssuer = null;
    this._rfvIdNumber = null;
    this._rfvType = null;
}

StudentPractice.Portal.UserControls.GenericControls.IdentityControl.prototype = {
    initialize: function () {
        StudentPractice.Portal.UserControls.GenericControls.IdentityControl.callBaseMethod(this, 'initialize');

        this._trIdIssuer = $(this.get_element()).find('.idIssuer');
        this._trIdIssueDate = $(this.get_element()).find('.idIssueDate');
        $addHandler(this._rblist, 'click', Function.createDelegate(this,
        function () {
            this.showIdDetails();
            this.raiseRecordsChanged();
        }
        ));

        this.showIdDetails(true);
        this._valGroup = this.get_element().id + '_vgIdDetails';
        this._txtIdIssueDate.bind('change', Function.createDelegate(this, this.raiseRecordsChanged));
        this._txtIdNumber.bind('change', Function.createDelegate(this, this.raiseRecordsChanged));
        this._txtIdIssuer.bind('change', Function.createDelegate(this, this.raiseRecordsChanged));
        // Add custom initialization here
    },
    dispose: function () {
        //Add custom dispose actions here
        StudentPractice.Portal.UserControls.GenericControls.IdentityControl.callBaseMethod(this, 'dispose');
    },
    addRecordsChanged: function (handler) {
        this.get_events().addHandler('clientRecordsChanged', handler);
    },
    removeRecordsChanged: function (handler) {
        this.get_events().removeHandler('clientRecordsChanged', handler);
    },
    raiseRecordsChanged: function () {
        var handler = this.get_events().getHandler('clientRecordsChanged');
        if (handler) {
            var e = this;
            handler(e);
        }
    },
    getValue: function () {
        var value = {};
        value.IdNumber = this._txtIdNumber.val();
        value.IdType = this._idType;
        value.IdIssueDate = this._txtIdIssueDate.val();
        value.IdIssuer = this._txtIdIssuer.val();
        return value;
    },
    setValue: function (value) {
        this._txtIdNumber.val(value.IdNumber);
        this._idType = value.IdType;
        this._txtIdIssueDate.val(value.IdIssueDate);
        this._txtIdIssuer.val(value.IdIssuer);
        $(this.get_element()).find('.' + this._cssClass + ' input').each(function () {
            if ($(this).val() == value.IdType)
                $(this).attr('checked', true);
            else
                $(this).attr('checked', false);
        });
        this.showIdDetails();
    },
    validateIssuer: function (s, e) {
        e.IsValid = true;
        if (this._idType == 1) {
            e.IsValid = this._txtIdIssuer.val() != '';
        }
        var vce = $find(this._txtIdIssuer.attr('id') + 'callout');
        if (vce) {
            vce._ensureCallout();
            if (e.IsValid)
                vce.hide();
            else
                vce.show();
        }
    },
    validateNumber: function (s, e) {
        e.IsValid = this._txtIdNumber.val() != '';
        var vce = $find(this._txtIdNumber.attr('id') + 'callout');
        if (vce) {
            vce._ensureCallout();
            if (e.IsValid)
                vce.hide();
            else
                vce.show();
        }
    },
    validateIssueDate: function (s, e) {
        e.IsValid = true;
        if (this._idType == 1) {
            e.IsValid = this._txtIdIssueDate.val() != '';
        }
        var vce = $find(this._txtIdIssueDate.attr('id') + 'callout');
        if (vce) {
            vce._ensureCallout();
            if (e.IsValid)
                vce.hide();
            else
                vce.show();
        }
    },
    clear: function () {
        this._txtIdIssuer.val('');
        this._txtIdIssueDate.val('');
        this._txtIdNumber.val('');
    },
    hide: function () {
        $(this.get_element()).hide();
    },
    show: function () {
        $(this.get_element()).show();
    },
    showIdDetails: function (isInit) {
        var _id = null;
        $(this.get_element()).find('.' + this._cssClass + ' input').each(function () {
            if ($(this).attr('checked')) {
                _id = $(this).val();
            }
        });
        this._idType = _id;
        if (this._idType == null) {
            this._trIdIssuer.hide();
            this._trIdIssueDate.hide();
        }
        else if (this._idType == 1) {
            this._trIdIssuer.show();
            this._trIdIssueDate.show();
            this._lblIdNumber.html(Resources.ProviderInput_ProviderIdNumber);
        }
        else if (this._idType == 2) {
            this._trIdIssuer.hide();
            this._trIdIssueDate.hide();
            this._lblIdNumber.html(Resources.ProviderInput_ProviderPassNumber);
            this._txtIdIssuer.val('');
            this._txtIdIssueDate.val('');            
        }
    },

    //Properties
    get_rblist: function () { return this._rblist; },
    set_rblist: function (val) { this._rblist = val; },
    get_txtIdIssueDate: function () { return this._txtIdIssueDate; },
    set_txtIdIssueDate: function (val) { this._txtIdIssueDate = $('#' + val); },
    get_txtIdIssuer: function () { return this._txtIdIssuer; },
    set_txtIdIssuer: function (val) { this._txtIdIssuer = $('#' + val); },
    get_txtIdNumber: function () { return this._txtIdNumber; },
    set_txtIdNumber: function (val) { this._txtIdNumber = $('#' + val); },
    get_idType: function () { return this._idType; },
    set_idType: function (val) { this._idType = val; },
    get_lblIdNumber: function () { return this._lblIdNumber; },
    set_lblIdNumber: function (val) { this._lblIdNumber = $('#' + val); }
}

StudentPractice.Portal.UserControls.GenericControls.IdentityControl.registerClass('StudentPractice.Portal.UserControls.GenericControls.IdentityControl', Sys.UI.Control);
