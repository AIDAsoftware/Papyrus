var papyrus = papyrus || {};

(function (ns) {
    function notifier(idWithSpecialChar) {
        var idWithoutSpecialChar = idWithSpecialChar.substr(1);
        $("body").prepend('<div id="' + idWithoutSpecialChar + '" style="display: none"><h3></h3></div>');

        function cssClassFor(messageType) {
            return (messageType == "success") ? "notifier-success-message" : "notifier-error-message";
        }

        function notify (message) {
            var $notifierWidget = $(idWithSpecialChar);
            $notifierWidget.children("h3").text(message.title);
            $notifierWidget.addClass(cssClassFor(message.type));
        }
        return {
            notify: notify
        }
    }
    ns.notifier = notifier;
    
})(papyrus)