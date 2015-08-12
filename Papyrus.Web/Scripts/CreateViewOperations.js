function createButtonClick() {
    var document = {
        Title: $("#inputTitle").val(),
        Description: $("#inputDescription").val(),
        Content: $("#inputContent").val(),
        Language: $("#inputLanguage").val()
    };
    new papyrus.DocumentService().createDocument(document);
}