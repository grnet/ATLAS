var popUp = function () {
    var _onHideCallback = null;

    function UpdatePosition(width, height) {
        var w = width;
        if (w > (window.innerWidth - 200))
            w = window.innerWidth - 200;

        var h = height;
        if (h > (window.innerHeight - 100))
            h = window.innerHeight - 100;

        devExPopup.SetSize(w, h);
        devExPopup.UpdatePosition();
    }

    return {
        show: function (url, title, callback, width, height) {
            devExPopup.SetContentUrl(url);

            if (title)
                devExPopup.SetHeaderText(title);

            if (callback)
                _onHideCallback = callback;

            devExPopup.Show();
            if (width != null && height != null) {
                UpdatePosition(width, height);
            }
        },

        showDynamic: function (url, title, width, height, callback) {
            devExPopup.SetContentUrl(url);

            if (title)
                devExPopup.SetHeaderText(title);

            if (callback)
                _onHideCallback = callback;

            screenHeight = screen.height;

            if (screenHeight < height + 150) {
                height = height - 250;
            }

            devExPopup.Show();
            UpdatePosition(width, height);
        },

        hide: function () {
            devExPopup.SetContentUrl('about:blank');
            devExPopup.Hide();

            if (typeof _onHideCallback == 'function') {
                _onHideCallback();
            }
        },

        hideWithMessage: function (msg) {
            devExPopup.SetContentUrl('about:blank');
            devExPopup.Hide();

            if (msg) {
                alert(msg);
            }

            if (typeof _onHideCallback == 'function') {
                _onHideCallback();
            }
        },

        hideWithoutRefresh: function () {
            devExPopup.SetContentUrl('about:blank');
            devExPopup.Hide();
        },

        init: function () {
            return false;
        },

        print: function () {
            var popupContentWindow = devExPopup.GetContentIFrameWindow();
            popupContentWindow.focus();
            popupContentWindow.print();
        }
    };
}();