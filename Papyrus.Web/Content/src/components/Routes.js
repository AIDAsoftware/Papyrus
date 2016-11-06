import React from 'react';
import { Router, Route, hashHistory } from 'react-router';

import HomePage from './HomePage';
import NewDocumentPage from './documents/NewDocumentPage';
import DocumentsPage from './documents/DocumentsPage';

export default class Routes extends React.Component {

    render() {
        return (
            <Router history={hashHistory}>
                <Route path='/' component={HomePage}/>
                <Route path='/documents/new' component={NewDocumentPage} />
                <Route path='/documents' component={DocumentsPage} />
            </Router>    
        );
    }

}