import { createContext, useContext } from "react";
import PropTypes from "prop-types";
import { useAuth as useAuthHook } from "../hooks/useAuth";

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
    const { token, uID, username, location, login, register, logout } = useAuthHook();

    const isAuthenticated = !!token;

    return (
        <AuthContext.Provider value={{ isAuthenticated, token, uID, username, location, login, register, logout }}>
            {children}
        </AuthContext.Provider>
    );
};

AuthProvider.propTypes = {
children: PropTypes.node.isRequired,
};

export const useAuth = () => useContext(AuthContext);