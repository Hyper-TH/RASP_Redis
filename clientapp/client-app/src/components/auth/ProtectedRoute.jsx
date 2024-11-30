import { Navigate } from "react-router-dom";
import { useAuth } from "../../context/AuthContext";
import PropTypes from "prop-types";

const ProtectedRoute = ({ element: Component }) => {
    const { isAuthenticated } = useAuth();

    return isAuthenticated ? Component : <Navigate to="/" replace />;
};

ProtectedRoute.propTypes = {
    element: PropTypes.element.isRequired,
};

export default ProtectedRoute;