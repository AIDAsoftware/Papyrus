var papyrus = papyrus || {};

(function (ns) {

    function restClient() {
        var ajaxContentType = "application/json; charset=utf-8";
        var apiUrl = "http://localhost:8888/papyrusapi/";

        var get = function (resourceUrl) {
            return $.ajax({
                type: "GET",
                contentType: ajaxContentType,
                url: apiUrl + resourceUrl
            });
        }

        var post = function(resourceUrl, document) {
            return $.ajax({
                type: "POST",
                contentType: ajaxContentType,
                url: apiUrl + resourceUrl,
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

    ns.restClient = restClient;
})(papyrus);