import React, {PropTypes} from 'react';
import {connect} from 'react-redux';
import _ from 'lodash';

class DocumentsPage extends React.Component {
    constructor(props, context) {
        super(props, context);
    }

    render() {
        const documents = _.map(this.props.documents, document => <li key={document.id}>{document.title}</li>);
        return (
            
            <div>
                <h1>Documents</h1>
                <ul>
                    {documents}
                </ul>
            </div>
        );
    }
}

DocumentsPage.propTypes = {
    documents: PropTypes.array.isRequired
};

function mapStateToProps(state) {
    return {
        documents: state.documents
    };
}

export default connect(mapStateToProps)(DocumentsPage);