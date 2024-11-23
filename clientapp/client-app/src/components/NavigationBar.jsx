import { Link } from 'react-router-dom';

const NavigationBar = () => {
    return (
        <>
            <Link to="/books" className="btn_collection_top">
                See list of books
            </Link>

            <Link to="/addBook" className="btn_collection_bottom">
                Add a book
            </Link>
        </>
    );
}

export default NavigationBar;