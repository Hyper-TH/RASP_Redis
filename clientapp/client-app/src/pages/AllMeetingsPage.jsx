import { Link } from "react-router-dom";
import PropTypes from "prop-types";
import ReturnButton from '../components/ReturnButton.jsx'; 

const AllMeetingsPage = ({ backTo }) => {
    return (
        <>
            <h1>Title</h1>
            <Link to={backTo}>
                <ReturnButton />
            </Link>
        </>
    )
};


AllMeetingsPage.propTypes = {
    backTo: PropTypes.string.isRequired,   
};

export default AllMeetingsPage;