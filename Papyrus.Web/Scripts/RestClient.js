var papyrus = papyrus || {}

(function (ns) {

    function RestClient() {

        function get(url) {
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

    ns.RestClient = RestClient();
})(papyrus);