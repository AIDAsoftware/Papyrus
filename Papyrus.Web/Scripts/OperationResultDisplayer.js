var papyrus = papyrus || {};

(function(ns) {

    function OperationResultDisplayer() {

        var displayMessage = function (message, element) {
            var messagePrinter = papyrus.MessagePrinter(element);
            var displayColor = (message.type == "success") ? "rgb(166, 215, 133)" : "rgb(204, 0, 13)";
            messagePrinter.print(message.title, displayColor);
        };

        return {
            displayMessage: displayMessage
        };

    };

    ns.OperationResultDisplayer = OperationResultDisplayer;

})(papyrus);