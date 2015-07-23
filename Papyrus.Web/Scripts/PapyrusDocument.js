function PapyrusDocument() {}

PapyrusDocument.prototype = {
    withId: function(id) {
        this.id = id;
        return this;
    },

    withTitle: function(title) {
        this.title = title;
        return this;
    },

    withDescription: function (description) {
        this.description = description;
        return this;
    },

    withContent: function (content) {
        this.content = content;
        return this;
    },

    forLanguage: function (language) {
        this.language = language;
        return this;
    }
};