import React from 'react';
import { Router, Route, hashHistory } from 'react-router';

import HomePage from './../pages/HomePage';
import NewDocumentPage from './../pages/NewDocumentPage';

export default class Routes extends React.Component {

    render() {
        return (
            <Router history={hashHistory}>
                <Route path='/' component={HomePage}/>
                <Route path='/documents/new' component={NewDocumentPage} />
            </Router>    
        );
    }

}