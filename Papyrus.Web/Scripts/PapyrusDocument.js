function PapyrusDocument() {}

PapyrusDocument.prototype = {
    withId: function(id) {
        this.Id = id;
        return this;
    },

    withTitle: function(title) {
        this.Title = title;
        return this;
    },

    withDescription: function (description) {
        this.Description = description;
        return this;
    },

    withContent: function (content) {
        this.Content = content;
        return this;
    },

    forLanguage: function (language) {
        this.Language = language;
        return this;
    }
};