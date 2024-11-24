import { Link } from "react-router-dom";
import AddBookForm from '../components/AddBookForm.jsx';
import ReturnButton from '../components/ReturnButton.jsx'; 
import PropTypes from "prop-types";

const AddBookPage = ({ backTo }) => {
    return (
        <>
            <Link to={backTo}>
                <ReturnButton />
            </Link>
            <AddBookForm />
        </>
    )
}

AddBookPage.propTypes = {
    backTo: PropTypes.string.isRequired,  // backTo must be a string and is required
};

export default AddBookPage;