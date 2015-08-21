// The Jasmine Test Framework
/// <reference path="~/../Papyrus.Web/Scripts/jquery-1.11.3.min.js"/>
/// <reference path="~/../Papyrus.Web/Scripts/MessagePrinter.js"/>
/// <reference path="~/../Papyrus.Web/Scripts/OperationResultDisplayer.js"/>
/// <reference path="~/Web/lib/jasmine-2.3.4/jasmine.js" />

describe("OperationResultDisplayer", function () {

    var displayer;
    var messageNotifierId;

    beforeEach(function () {
        messageNotifierId = "#message-notifier"; //TODO: think a name
        displayer = new papyrus.OperationResultDisplayer();
    });

    it("should show green confirmation message when a document is created", function() {
        var message = {
            title: "Document created",
            type: "success"
        }
        
        displayer.displayMessage(message, messageNotifierId);
        var green = "rgb(166, 215, 133)";
        expectMessageIsShownWith(message.title, green);
    });

    it("should show error message in red when a document is not created", function () {
        var message = {
            title: "Cant create the document",
            type: "fail"
        }

        var red = "rgb(204, 0, 13)";
        displayer.displayMessage(message, messageNotifierId);
        expectMessageIsShownWith(message.title, red);
    });

    afterEach(function() {                    
        $("#message-notifier").remove();
    });

    function expectMessageIsShownWith(title, color) {
        var messageNotifier = $(messageNotifierId);
        expect(messageNotifier.children("h3").text()).toEqual(title);
        expect(messageNotifier.css("background-color")).toEqual(color);
        expect(messageNotifier.css("display")).toEqual("inline-block");
    }
});