var papyrus = papyrus || {};

(function (ns) {

    function RestClient() {
        var ajaxContentType = "application/json; charset=utf-8";
        var apiUrl = "http://localhost:8888/papyrusapi/";

        var get = function (url) {
            return $.ajax({
                type: "GET",
                contentType: ajaxContentType,
                url: apiUrl + url
            });
        }

        return {
            get: get
        }

    }

    ns.RestClient = RestClient;
})(papyrus);