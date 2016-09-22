import React from 'react';
import { Tabs, Tab } from 'material-ui/Tabs';

class NavigationBar extends React.Component {
    render() {
        return (
            <div>
                <Tabs>
                    <Tab label="Item One"></Tab>
                    <Tab label="Item Two"></Tab>
                    <Tab label="onActive"
                    data-route="/home"></Tab>
                </Tabs>
            </div>
        )
    }
}

export default NavigationBar;
