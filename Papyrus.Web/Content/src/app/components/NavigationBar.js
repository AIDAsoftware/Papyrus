import React from 'react';

import AppBar from 'react-toolbox/lib/app_bar';
import Navigation from 'react-toolbox/lib/navigation'

export default function NavigationBar() {

    function render() {
        return (
            <AppBar fixed flat>
                <a href="/home">React Toolbox Docs</a>  
                <Navigation />
            </AppBar>
        );
    } 

    return {
        render
    }
}