// The Jasmine Test Framework
/// <reference path="~/../Papyrus.Web/Scripts/jquery-1.11.3.min.js"/>
/// <reference path="~/../Papyrus.Web/Scripts/MessagePrinter.js"/>
/// <reference path="~/../Papyrus.Web/Scripts/OperationResultDisplayer.js"/>
/// <reference path="~/Web/lib/jasmine-2.3.4/jasmine.js" />

describe("OperationResultDisplayer", function () {

    var displayer;
    var $message_notifier;

    beforeEach(function () {
        $("body").append('<div id="message-notifier" style="display: none"><h3></h3></div>');
        $message_notifier = $("#message-notifier"); //TODO: think a name
        displayer = new papyrus.OperationResultDisplayer();
    });

    it("show green confirmation message when a document is created", function() {
        var message = {
            title: "Document created",
            type: "success"
        }
        
        displayer.displayMessage(message, $message_notifier);
        var green = "rgb(166, 215, 133)";
        expectMessageIsShownWith(message.title, green);
    });

    it("show error message in red when a document is not created", function () {
        var message = {
            title: "Cant create the document",
            type: "fail"
        }

        var red = "rgb(204, 0, 13)";
        displayer.displayMessage(message, $message_notifier);
        expectMessageIsShownWith(message.title, red);
    });

    afterEach(function() {                    
        $("#message-notifier").remove();
    });

    function expectMessageIsShownWith(title, color) {
        expect($message_notifier.children("h3").text()).toEqual(title);
        expect($message_notifier.css("background-color")).toEqual(color);
        expect($message_notifier.css("display")).toEqual("inline-block");
    }
});