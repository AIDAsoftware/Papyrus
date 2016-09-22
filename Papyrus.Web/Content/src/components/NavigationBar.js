import React from 'react';
import { Tabs, Tab } from 'material-ui/Tabs';
import autobind from 'autobind-decorator';
import { hashHistory } from 'react-router';

export default class NavigationBar extends React.Component {

    constructor() {
        super();        
        this.state = {
            value : 'home'
        };
    }

    @autobind
    handleChange(value) {
        this.setState({
            value: value
        });
        const routes = {
            'home' : '/',
            'new-document' : 'documents/new'
        };
        hashHistory.push(routes[value]);
    }

    render() {
        return (
            <div>
                <Tabs value={this.state.value} onChange={this.handleChange}>
                    <Tab label="Home" value="home" />
                    <Tab label="New Document" value="new-document" />
                </Tabs>
            </div>
        )
    }
}


