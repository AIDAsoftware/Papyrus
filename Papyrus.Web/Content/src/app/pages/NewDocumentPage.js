import React from 'react';
import TextField from 'material-ui/TextField';
import SelectField from 'material-ui/SelectField';
import MenuItem from 'material-ui/MenuItem';
import RaisedButton from 'material-ui/RaisedButton';

const DocumentActions  = {
    createDocument: (title, language, description, content) => {
        console.log(title, language, description, content);
    }
};

export default class NewDocumentPage extends React.Component {

    constructor() {
        super();
        this.state = {
            title : '',
            language: 'spanish',
            description : '',
            content : ''
        };
    }

    languageChange = (event, index, language)  => {
        this.setState({language});
    }

    titleChange = (event, title) => {
        this.setState({title});
    }

    descriptionChange = (event, description) => {
        this.setState({description});
    }

    contentChange = (event, content) => {
        this.setState({content});
    }

    createDocument = () => {
        DocumentActions.createDocument(this.state.title, this.state.language, this.state.description, this.state.content);
    }

    render () {
        const languages = [
            <MenuItem key={1} value={'spanish'} primaryText="Spanish" />,
            <MenuItem key={2} value={'english'} primaryText="English" />
        ];
        return (
            <div >
                <TextField
                    onChange={this.titleChange}
                    value={this.state.title}
                    hintText="Title"
                    fullWidth/>
                <SelectField
                    value={this.state.language}
                    onChange={this.languageChange}
                    fullWidth>
                    {languages}
                </SelectField>
                <TextField
                    onChange={this.descriptionChange}
                    value={this.state.description}
                    hintText="Description"
                    fullWidth/>
                <TextField
                    onChange={this.contentChange}
                    value={this.state.content}
                    hintText="Content"
                    rows={4}
                    multiLine={true}
                    rowsMax={10}
                    fullWidth/>
                <RaisedButton onClick={this.createDocument} label="Create"/>
            </div>
        );

    }
}