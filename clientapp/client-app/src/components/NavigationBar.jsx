import { Link } from 'react-router-dom';

const NavigationBar = () => {
    return (
        <>
            <Link to="/books" className="btn_collection_middle">
                See list of books
            </Link>
        </>
    );
}

export default NavigationBar;