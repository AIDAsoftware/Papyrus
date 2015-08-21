var papyrus = papyrus || {};

(function(ns) {

    function OperationResultDisplayer() {

        var displayMessage = function (message, idWithSpecialChar) {
            var messagePrinter = papyrus.messagePrinter(idWithSpecialChar);
            messagePrinter.print(message);
        };

        return {
            displayMessage: displayMessage
        };

    };

    ns.OperationResultDisplayer = OperationResultDisplayer;

})(papyrus);