import React from 'react';

import AppBar from 'react-toolbox/lib/app_bar';
import { Navigation } from 'react-toolbox/lib/navigation';
import {Tab, Tabs} from 'react-toolbox';
import { Link, hashHistory } from 'react-router';

class NavigationBar extends React.Component {

    constructor() {
        super();
        this.state = {
            inverseIndex : 0
        };
        this.handleInverseTabChange = (index) => {
            const routes = {
                0 : '/',
                1 : '/documents/create',
                2 : '/documents'
            }
            this.setState({inverseIndex: index});
            hashHistory.push(routes[index]);
        };
    }

    render() {
        return (
            <div>
                <Tabs index={this.state.inverseIndex} onChange={this.handleInverseTabChange} inverse>
                    <Tab label='Home' />
                    <Tab label='Create Document' />
                    <Tab label='Documents' />
                </Tabs>
            </div>
        );
    }
}

export default NavigationBar;