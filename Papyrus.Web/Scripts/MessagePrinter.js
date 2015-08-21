var papyrus = papyrus || {};

(function (ns) {
    function messagePrinter(idWithSpecialChar) {
        var idWithoutSpecialChar = idWithSpecialChar.substr(1);
        $("body").prepend('<div id="' + idWithoutSpecialChar + '" style="display: none"><h3></h3></div>');

        function colorForType(messageType) {
            return (messageType == "success") ? "rgb(166, 215, 133)" : "rgb(204, 0, 13)";
        }

        var print = function (message) {
            var notifierWidget = $(idWithSpecialChar);
            notifierWidget.css("background-color", colorForType(message.type));
            notifierWidget.children("h3").text(message.title);
            notifierWidget.css("display", "inline-block");
        }
        return {
            print: print
        }
    }
    ns.messagePrinter = messagePrinter;
    
})(papyrus)