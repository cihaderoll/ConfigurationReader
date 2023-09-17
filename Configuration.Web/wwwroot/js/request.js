var request = {
    get: function (url, parameters, callback) {
        return $.get(url, parameters, function (res) {
            if (callback)
                callback(res);
        })
    },

    post: function (url, parameters, callback) {
        return $.post(url, parameters, function (res) {
            if (callback)
                callback(res);
        })
    }
}