var papyrus = papyrus || {};

(function (ns) {
    function MessagePrinter() {
        var print = function(messageTitle, color) {
            $("#message-notifier").css("background-color", color);
            $("#message-notifier").children("h3").text(messageTitle);
            $("#message-notifier").css("display", "inline-block");
        }

        return {
            print: print
        }
    }
    ns.MessagePrinter = MessagePrinter;
    
})(papyrus)