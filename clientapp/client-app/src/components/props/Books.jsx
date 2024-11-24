import PropTypes from 'prop-types';

const Books = ({ id, isbn, name, price, category, author, deleteBook }) => {
    return (
        <>
            <div className="book-card" id={id}>
                <h2>{name}</h2>
                <p><strong>DocID:</strong> {id}</p>
                <p><strong>ISBN:</strong> {isbn}</p>
                <p><strong>Author:</strong> {author}</p>
                <p><strong>Category:</strong> {category}</p>
                <p><strong>Price:</strong> {price}</p>

                <button onClick={() => deleteBook(isbn)}>Delete</button>
            </div>
        </>
    )
}

Books.propTypes = {
    id: PropTypes.string.isRequired,
    isbn: PropTypes.string.isRequired,
    name: PropTypes.string.isRequired,
    price: PropTypes.number.isRequired,
    category: PropTypes.string.isRequired,
    author: PropTypes.string.isRequired,
    deleteBook: PropTypes.func.isRequired
};

export default Books;