import { useState } from 'react'
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext';
import SignUpForm from '../forms/SignUpForm.jsx';

const SignUp = () => {
    const { register } = useAuth();
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");
    const [error, setError] = useState("");

    const navigate = useNavigate();

    const signUp = async (e) => {
        e.preventDefault();
        setError("");

        if (password !== confirmPassword) {
            setError("Passwords do not match.");
            return;
        }

        try {
            const userCredential = await register(email, password);

            if (userCredential) {
                console.log(`Successful login!`);
                navigate('/home');
            }
        } catch (error) {
            setError(error.message);
            console.error(error);
        }
    };

    const returnLogin = () => {
        navigate('/');
    };

    return (
        <div className="sub_container">
            <SignUpForm
                email={email}
                setEmail={setEmail}
                password={password}
                setPassword={setPassword}
                confirmPassword={confirmPassword}
                setConfirmPassword={setConfirmPassword}
                signUp={signUp}
                error={error}
                returnLogin={returnLogin}
            />
        </div>
    );
};

export default SignUp;