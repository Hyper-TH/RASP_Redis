import { Link } from 'react-router-dom';

const NavigationBar = () => {
    return (
        <>
            <Link to="/books" className="btn_collection_top">
                See list of books
            </Link>

            <Link to="/addBook" className="btn_collection_middle">
                Add a book
            </Link>

            <Link to="/calendar" className="btn_collection_bottom">
                Check Calendar
            </Link>
        </>
    );
}

export default NavigationBar;