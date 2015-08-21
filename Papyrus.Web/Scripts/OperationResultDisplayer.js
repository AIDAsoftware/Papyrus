var papyrus = papyrus || {};

(function(ns) {

    function OperationResultDisplayer() {

        function displayMessage(message, idWithSpecialChar) {
            var messagePrinter = papyrus.messagePrinter(idWithSpecialChar);
            messagePrinter.print(message);
        };

        return {
            displayMessage: displayMessage
        };

    };

    ns.OperationResultDisplayer = OperationResultDisplayer;

})(papyrus);