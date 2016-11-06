'use strict';

import React from 'react';
import ReactDOM from 'react-dom';
import MuiThemeProvider from 'material-ui/styles/MuiThemeProvider';
import injectTapEventPlugin from 'react-tap-event-plugin';
import {Provider} from 'react-redux';
injectTapEventPlugin();

import configureStore from './store/configureStore';
import {loadDocuments} from './actions/documentActions'; 
import NavigationBar from './components/NavigationBar';
import Routes from './components/Routes';

const store = configureStore();
store.dispatch(loadDocuments());

const App = () => (
  <MuiThemeProvider>
    <div>
        <NavigationBar />
        <div style={{
            display: 'flex',
            justifyContent: 'center',
            width: '100%'
        }}>
            <Routes />
        </div>
    </div>
  </MuiThemeProvider>
);

ReactDOM.render(
  <Provider store={store}>
    <App />
  </Provider>,
  document.getElementById('app')
);