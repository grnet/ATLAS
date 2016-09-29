Type.registerNamespace("StudentPractice.Portal.UserControls.GenericControls");

StudentPractice.Portal.UserControls.GenericControls.AcademicPositionRulesPopup = function (element) {
    StudentPractice.Portal.UserControls.GenericControls.AcademicPositionRulesPopup.initializeBase(this, [element]);
    this._popup = null;
    this._acceptTermsArea = null;
    this._cancelArea = null;
    this._btnSubmit = null;
    this._btnCancel = null;
    this._rulesArea = null;
    this._requireTermsAcceptance = null;
    this._cache = {};
};

StudentPractice.Portal.UserControls.GenericControls.AcademicPositionRulesPopup.prototype = {
    initialize: function () {
        StudentPractice.Portal.UserControls.GenericControls.AcademicPositionRulesPopup.callBaseMethod(this, 'initialize');

        $(this._btnSubmit).click(Function.createDelegate(this, this._submitClick));
        $(this._btnCancel).click(Function.createDelegate(this, this._cancelClick));

        this.render();
    },

    render: function () {
        if (this._requireTermsAcceptance) {
            $(this._acceptTermsArea).show();
            $(this._cancelArea).hide();
        }
        else {
            $(this._acceptTermsArea).hide();
            $(this._cancelArea).show();
        }
    },

    dispose: function () {
        StudentPractice.Portal.UserControls.GenericControls.AcademicPositionRulesPopup.callBaseMethod(this, 'dispose');
    },

    onTermsAccepted: function (handler) {
        if (typeof (handler) === 'function') {
            this.get_events().addHandler('termsAccepted', handler);
        }
    },

    show: function (ids, width, height) {
        if (typeof (width) === 'undefined') {
            width = 800;
        }
        if (typeof (height) === 'undefined') {
            height = 600;
        }

        $(this._rulesArea).css('max-width', (width - 40) + 'px');
        if (typeof (ids) === 'undefined') {
            StudentPractice.Portal.PortalServices.Services.GetAcademicPositionRules([], Function.createDelegate(this, function (result) {
                //this._addInCache(result);
                this._renderResult(result);
                this._popup.ShowInView(width, height, 40, 40);
            }));
        }
        else {
            var newIDs = this._getIDsNotInCache(ids);
            if (newIDs.length != 0) {
                StudentPractice.Portal.PortalServices.Services.GetAcademicPositionRules(newIDs, Function.createDelegate(this, function (result) {
                    this._addInCache(result);
                    this._renderResult(this._selectFromCache(ids));
                    this._popup.ShowInView(width, height, 40, 40);
                }));
            }
            else {
                this._renderResult(this._selectFromCache(ids));
                this._popup.ShowInView(width, height, 40, 40);
            }
        }
    },

    hide: function () {
        this._popup.Hide();
    },

    _renderResult: function (result) {
        if (result.length != 0) {
            var strBuilder = [];
            for (var i = 0; i < result.length; i++) {
                strBuilder.push('<h3>');
                strBuilder.push(result[i].Institution + ' - ' + result[i].Department);
                strBuilder.push('</h3>');
                strBuilder.push("<pre class='rulesPreviewArea'>");
                strBuilder.push(result[i].PositionRules);
                strBuilder.push("</pre>");
            }
            $(this._rulesArea).html(strBuilder.join(''));
        }
    },

    _submitClick: function () {
        this._raiseEvent('termsAccepted', null);
        return false;
    },

    _cancelClick: function () {
        this._popup.Hide();
        return false;
    },

    _selectFromCache: function(ids) {
        var results = [];
        for (var i = 0; i < ids.length; i++) {
            var item = this._cache[ids[i].toString()];
            if (item) results.push(item);
        }
        return results;
    },

    _existsInCache: function (id) {
        if (this._cache[id.toString()])
            return true;
        else
            return false;
    },

    _getIDsNotInCache: function (ids) {
        var newIDs = [];
        for (var i = 0; i < ids.length; i++) {
            if (!this._existsInCache(ids[i])) {
                newIDs.push(ids[i]);
            }
        }
        return newIDs;
    },

    _addInCache: function (items) {
        for (var i = 0; i < items.length; i++) {
            this._cache[items[i].ID.toString()] = items[i];
        }
    },

    _raiseEvent: function (eventName, eventArgs) {
        var handler = this.get_events().getHandler(eventName);
        if (handler) {
            eventArgs = eventArgs || Sys.EventArgs.Empty;
            handler(this, eventArgs);
        }
    },

    get_popup: function () { return this._popup; },
    set_popup: function (val) { this._popup = window[val]; },

    get_acceptTermsArea: function () { return this._acceptTermsArea; },
    set_acceptTermsArea: function (val) { this._acceptTermsArea = val; },

    get_cancelArea: function () { return this._cancelArea; },
    set_cancelArea: function (val) { this._cancelArea = val; },

    get_btnSubmit: function () { return this._btnSubmit; },
    set_btnSubmit: function (val) { this._btnSubmit = val; },

    get_btnCancel: function () { return this._btnCancel; },
    set_btnCancel: function (val) { this._btnCancel = val; },

    get_rulesArea: function () { return this._rulesArea; },
    set_rulesArea: function (val) { this._rulesArea = val; },

    get_requireTermsAcceptance: function () { return this._requireTermsAcceptance; },
    set_requireTermsAcceptance: function (val) { this._requireTermsAcceptance = val; }

};

StudentPractice.Portal.UserControls.GenericControls.AcademicPositionRulesPopup.registerClass('StudentPractice.Portal.UserControls.GenericControls.AcademicPositionRulesPopup', Sys.UI.Control);