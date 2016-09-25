import ajax from 'ajax';

export default {

    createDocument: (title, language, description, content) => {
        ajax.post('http://localhost/Papyrus.Api/products/1/versions/1/documents',
        {
            title,
            language,
            description,
            content
        }, (response) => {
            console.log(response);
        }, (err) => {
            console.log(err);
        });
    }
};