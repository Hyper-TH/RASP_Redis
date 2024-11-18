import PropTypes from 'prop-types';

const Books = ({ id, name, price, category, author }) => {
    return (
        <div className="book-card" id={id}>
            <h2>{name}</h2>
            <p><strong>ID:</strong> {id}</p>
            <p><strong>Author:</strong> {author}</p>
            <p><strong>Category:</strong> {category}</p>
            <p><strong>Price:</strong> {price}</p>
        </div>
    )
}

Books.propTypes = {
    id: PropTypes.string.isRequired,
    name: PropTypes.string.isRequired,
    price: PropTypes.number.isRequired,
    category: PropTypes.string.isRequired,
    author: PropTypes.string.isRequired, 
};

export default Books;