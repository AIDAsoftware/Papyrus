// The Jasmine Test Framework
/// <reference path="~/../Papyrus.Web/Scripts/jquery-1.11.3.min.js"/>
/// <reference path="~/../Papyrus.Web/Scripts/MessageNotifier.js"/>
/// <reference path="~/Web/lib/jasmine-2.3.4/jasmine.js" />

describe("Message Notifier", function () {

    var messageNotifierId;

    beforeEach(function () {
        messageNotifierId = "#message-notifier";
    });

    it("should show success message when a document is created", function() {
        var message = {
            title: "Document created",
            type: "success"
        }
        
        papyrus.notifier(messageNotifierId).notify(message);

        var notifierSuccessCssClass = "notifier-success-message";
        expectMessageIsShownWith(message.title, notifierSuccessCssClass);
    });

    it("should show error message when a document is not created", function () {
        var message = {
            title: "Cant create the document",
            type: "error"
        }

        papyrus.notifier(messageNotifierId).notify(message);

        var cssNotifierErrorClass = "notifier-error-message";
        expectMessageIsShownWith(message.title, cssNotifierErrorClass);
    });

    afterEach(function() {                    
        $("#message-notifier").remove();
    });

    function expectMessageIsShownWith(title, cssClass) {
        var messageNotifier = $(messageNotifierId);
        expect(messageNotifier.children("h3").text()).toEqual(title);
        expect(messageNotifier.hasClass(cssClass)).toBeTruthy();
    }
});