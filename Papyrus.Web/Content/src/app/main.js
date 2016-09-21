'use strict';

import React from 'react';
import ReactDOM from 'react-dom';
import Routes from './components/Routes';
import NavigationBar from './components/NavigationBar';

const App = () => {
    return (
        <div>
            <NavigationBar />
            <Routes />
        </div>
    );
}    

ReactDOM.render(<App />, document.getElementById('app'));