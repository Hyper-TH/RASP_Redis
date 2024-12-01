import Error from '../props/Error';
import PropTypes from 'prop-types';

const SignInForm = ({ email, setEmail, password, setPassword, handleSubmit, error, signUpDirect }) => {
    return (
        <>
            <div className='sign_in_container'>
                <div className='sign_in'>
                    <h1 className='sign_in_title'>
                        Sign in to your account
                    </h1>

                    <form onSubmit={handleSubmit} className='sign_in_form'>
                        <div>
                            <label>Your email</label>
                            <input
                                type="text"
                                placeholder='example@domain.com'
                                value={email}
                                onChange={(e) => setEmail(e.target.value)}
                                required />
                        </div>

                        <div>
                            <label>Password</label>
                            <input
                                type="password"
                                placeholder='Enter your password'
                                value={password}
                                onChange={(e) => setPassword(e.target.value)}
                                required />
                        </div>

                        <button className='btn_login'>Login</button>
                    </form>

                    <p>
                        Register here
                    </p>

                    <div className='btn_collection_sign_in' role="group">
                        <button className='sign_up_btn' onClick={signUpDirect}>
                            Sign Up
                        </button>
                    </div>

                    {error && <Error error={error} />}
                </div>
            </div>
        </>
    );
};

SignInForm.propTypes = {
    email: PropTypes.string.isRequired,
    setEmail: PropTypes.func.isRequired,
    password: PropTypes.string.isRequired,
    setPassword: PropTypes.func.isRequired,
    handleSubmit: PropTypes.func.isRequired,
    error: PropTypes.string,
    signUpDirect: PropTypes.func.isRequired,
};


export default SignInForm;