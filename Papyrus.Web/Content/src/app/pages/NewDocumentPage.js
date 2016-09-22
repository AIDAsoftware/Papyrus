import React from 'react';
import TextField from 'material-ui/TextField';

export default class NewDocumentPage extends React.Component {

    centerStyle() {
        return {
            margin: "auto",
            display: "block",
            padding: "0 30%"
        }
    }

    textFieldStyle() {
        return {
            width: "100%"
        }
    }

    render () {
        return (
            <div style={this.centerStyle()}>
                <TextField hintText="Title" style={this.textFieldStyle()}/>
                <TextField 
                    hintText="Content"
                    rows={4}
                    rowsMax={15}
                    style={this.textFieldStyle()}/>
            </div>
        );
        
    }
}