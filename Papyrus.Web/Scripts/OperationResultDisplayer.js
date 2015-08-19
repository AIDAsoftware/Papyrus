var papyrus = papyrus || {};

(function(ns) {

    function OperationResultDisplayer() {

        var displayMessage = function(message) {
            var displayColor = (message.type == "success") ? "rgb(166, 215, 133)" : "rgb(204, 0, 13)";
            $("#message-notifier").css("background-color", displayColor);
            $("#message-notifier").children("h3").text(message.title);
            $("#message-notifier").css("display", "inline-block");
        };

        return {
            displayMessage: displayMessage
        }

    };

    ns.OperationResultDisplayer = OperationResultDisplayer;

})(papyrus);