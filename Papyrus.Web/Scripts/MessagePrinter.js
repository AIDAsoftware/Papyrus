var papyrus = papyrus || {};

(function (ns) {
    function MessagePrinter($element) {
        var print = function(messageTitle, color) {
            $element.css("background-color", color);
            $element.children("h3").text(messageTitle);
            $element.css("display", "inline-block");
        }

        return {
            print: print
        }
    }
    ns.MessagePrinter = MessagePrinter;
    
})(papyrus)