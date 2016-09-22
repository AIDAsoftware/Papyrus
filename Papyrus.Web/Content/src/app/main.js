'use strict';

import React from 'react';
import ReactDOM from 'react-dom';
import MuiThemeProvider from 'material-ui/styles/MuiThemeProvider';
import injectTapEventPlugin from 'react-tap-event-plugin';
injectTapEventPlugin();

import NavigationBar from './components/NavigationBar';
import Routes from './components/Routes';

const App = () => (
  <MuiThemeProvider>
    <div>
        <NavigationBar />
        <Routes />
    </div>    
  </MuiThemeProvider>
);
 
ReactDOM.render(
  <App />,
  document.getElementById('app')
);