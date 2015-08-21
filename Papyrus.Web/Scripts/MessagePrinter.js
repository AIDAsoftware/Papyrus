var papyrus = papyrus || {};

(function (ns) {
    function messagePrinter(element) {
        var id = element.substr(1);
        $("body").prepend('<div id="' + id + '" style="display: none"><h3></h3></div>');
        var print = function (messageTitle, color) {
            var notifierWidget = $(element);            
            notifierWidget.css("background-color", color);
            notifierWidget.children("h3").text(messageTitle);
            notifierWidget.css("display", "inline-block");
        }

        return {
            print: print
        }
    }
    ns.messagePrinter = messagePrinter;
    
})(papyrus)