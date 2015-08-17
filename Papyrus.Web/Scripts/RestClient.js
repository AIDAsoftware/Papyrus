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

        var post = function(url, document) {
            return $.ajax({
                type: "POST",
                contentType: ajaxContentType,
                url: apiUrl + url,
                data: JSON.stringify(document),
                success: function () {
                    console.log("document created");
                },
                error: function (request, status, error) {
                    console.log(request.responseText);
                }
            });
        }

        return {
            get : get,
            post : post
        }

    }

    ns.RestClient = RestClient;
})(papyrus);