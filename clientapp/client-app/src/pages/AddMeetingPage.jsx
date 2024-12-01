import { Link } from "react-router-dom";
import AddMeetingForm from '../components/forms/AddMeetingForm.jsx';
import ReturnButton from '../components/ReturnButton.jsx';
import PropTypes from "prop-types";

const AddMeetingPage = ({ backTo }) => {
    return (
        <>
            <Link to={backTo}>
                <ReturnButton />
            </Link >

            <AddMeetingForm />
        </>
    )
};

AddMeetingPage.PropTypes = {
    backTo: PropTypes.string.isRequired,
};

export default AddMeetingPage;