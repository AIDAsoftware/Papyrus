export default function documents(state = [], action) {
    if (action.type == 'LOAD_DOCUMENTS') {
        return action.documents;
    }
    return state;
}