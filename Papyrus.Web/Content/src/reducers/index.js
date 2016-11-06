import { combineReducers } from 'redux';
import documents from './documentsReducer';

const rootReducer = combineReducers({
  documents,
});

export default rootReducer;