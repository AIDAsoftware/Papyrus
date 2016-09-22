import React from 'react';
import { Tabs, Tab } from 'material-ui/Tabs';

export default class NavigationBar extends React.Component {

    constructor() {
        super();        
        this.state = {
            value : 'documents'
        };
    }

    handleChange(value) {
        this.setState({
            value: value
        });
    }

    render() {
        return (
            <div>
                <Tabs value={this.state.value} onChange={this.handleChange.bind(this)}>
                    <Tab label="Create Document" value="create-document">fahidah</Tab>
                    <Tab label="Documents" value="documents">gafgafagw</Tab>
                </Tabs>
            </div>
        )
    }
}


