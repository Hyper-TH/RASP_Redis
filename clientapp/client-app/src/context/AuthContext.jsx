import { createContext, useContext } from "react";
import PropTypes from "prop-types";
import { useAuth as useAuthHook } from "../hooks/useAuth";

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
    const { token, login, register, logout } = useAuthHook();

    const isAuthenticated = !!token;

    return (
        <AuthContext.Provider value ={ { isAuthenticated, login, register, logout } }>
            {children}
        </ AuthContext.Provider>
    );
};

AuthProvider.propTypes = {
children: PropTypes.node.isRequired,
};

export const useAuth = () => useContext(AuthContext);