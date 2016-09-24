import React from 'react';
import TextField from 'material-ui/TextField';
import SelectField from 'material-ui/SelectField';
import MenuItem from 'material-ui/MenuItem';

export default class NewDocumentPage extends React.Component {

    constructor() {
        super();
        this.state = {
            value: 1
        };
    }

    style() {
        return {
            container: {
                display: 'flex',
                alignItems: 'center',
                flexDirection: 'column'
            }
        };
    }

    handleChange = (event, index, value)  => {
        this.setState({value});
    }

    render () {
        const languages = [
            <MenuItem key={1} value={1} primaryText="Spanish" />,
            <MenuItem key={2} value={2} primaryText="English" />
        ];
        return (
            <div style={this.style().container}>
                <TextField
                    hintText="Title"
                    fullWidth/>
                <SelectField
                    value={this.state.value}
                    onChange={this.handleChange}
                    fullWidth>
                    {languages}
                </SelectField>
                <TextField
                    hintText="Description"
                    fullWidth/>
                <TextField
                    hintText="Content"
                    rows={4}
                    multiLine={true}
                    rowsMax={10}
                    fullWidth/>
            </div>
        );

    }
}