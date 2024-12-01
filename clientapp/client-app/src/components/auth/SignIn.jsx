import { useState } from 'react'
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext';
import SignInForm from '../forms/SignInForm.jsx';

const Signin = () => {
    const { login } = useAuth();
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState("");

    const navigate = useNavigate();

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');

        try {
            const token = await login(email, password); 

            if (token) {
                navigate('/home'); 
            } else {
                setError('Authentication failed.');
            }
        } catch (error) {
            setError('Incorrect email/password');
            console.error('Error during login:', error);
        }
    };


    const signUpDirect = () => {
        navigate('/signup');
    };

    return (
        <>  
            <div className='sub_container'>
                <SignInForm
                    email={email}
                    setEmail={setEmail}
                    password={password}
                    setPassword={setPassword}
                    handleSubmit={handleSubmit}
                    error={error}
                    signUpDirect={signUpDirect}
                />
            </div>
        </>
    );
};

export default Signin;