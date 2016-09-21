import React from 'react';

import { Router, Route, IndexRoute, hashHistory} from 'react-router';
import HomePage from './../pages/HomePage';
import CreateDocumentPage from './../pages/CreateDocumentPage';
import DocumentsPage from './../pages/DocumentsPage';

class Routes extends React.Component {

    render () {        
        return (
            <Router history={hashHistory}>
                <Route path="/" component={HomePage}/>
                <Route path="/documents/create" component={CreateDocumentPage}/>
                <Route path="documents" component={DocumentsPage} />
            </Router>
        );
    }
}

export default Routes;