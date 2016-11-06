import {loadDocuments as loadAllDocuments} from '../api/documentsApi';

export function loadDocuments() {
    const documents = loadAllDocuments();
    return {type: 'LOAD_DOCUMENTS', documents};
}