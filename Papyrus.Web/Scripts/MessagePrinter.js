var papyrus = papyrus || {};

(function (ns) {
    function messagePrinter(idWithSpecialChar) {
        var idWithoutSpecialChar = idWithSpecialChar.substr(1);
        $("body").prepend('<div id="' + idWithoutSpecialChar + '" style="display: none"><h3></h3></div>');
        var print = function (message) {
            var notifierWidget = $(idWithSpecialChar);
            var color = (message.type == "success") ? "rgb(166, 215, 133)" : "rgb(204, 0, 13)";
            notifierWidget.css("background-color", color);
            notifierWidget.children("h3").text(message.title);
            notifierWidget.css("display", "inline-block");
        }

        return {
            print: print
        }
    }
    ns.messagePrinter = messagePrinter;
    
})(papyrus)