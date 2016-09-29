/// <reference path="ASPxScriptIntelliSense.js" />
$(function () {
    if (typeof (ASPxClientPopupControl) !== 'undefined') {
        $.extend(ASPxClientPopupControl.prototype, {
            ShowInView: function (maxWidth, maxHeight, widthToSubtract, heightToSubtract) {
                var popup = ASPxClientPopupControl.Cast(this);

                var width = maxWidth || 900;
                var height = maxHeight || 850;

                var subtractWidth = widthToSubtract || 100;
                var subtractHeight = heightToSubtract || 100;

                if (width > (window.innerWidth - subtractWidth))
                    width = window.innerWidth - subtractWidth;

                if (height > (window.innerHeight - subtractHeight))
                    height = window.innerHeight - subtractHeight;

                popup.SetSize(width, height);
                popup.Show();
                popup.UpdatePosition();
            }
        });
    }
});