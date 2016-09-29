var Resources = (function () {
    var _resources = {
        Init: function (resx) {
            for (var i = 0 ; i < resx.length; i++) {
                _resources[resx[i].Key] = resx[i].Value;
            }
        }
    }
    return _resources;
})();