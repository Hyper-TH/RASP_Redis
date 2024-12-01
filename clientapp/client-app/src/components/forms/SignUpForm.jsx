import PropTypes from 'prop-types';
import Error from '../props/Error';

const SignUpForm = ({
    email,
    setEmail,
    password,
    setPassword,
    confirmPassword,
    setConfirmPassword,
    signUp,
    error,
    returnLogin
}) => {
    return (
        <div className='sign_in_container'>
            <div className='sign_in'>
                <h1 className='main_title'>Create your account</h1>

                <form onSubmit={signUp} className='sign_in_form'>
                    <div>
                        <label>Email Address: </label>
                        <input
                            type="text"
                            placeholder="Enter your email"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                            required
                        />
                    </div>

                    <div>
                        <label>Password: </label>
                        <input
                            type="password"
                            placeholder="Enter your password"
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                            required
                        />
                    </div>

                    <div>
                        <label>Confirm Password: </label>
                        <input
                            type="password"
                            placeholder="Enter your password"
                            value={confirmPassword}
                            onChange={(e) => setConfirmPassword(e.target.value)}
                            required
                        />
                    </div>

                    <button className='btn_login'>Sign Up</button>
                </form>

                <button className='btn_login' onClick={returnLogin}>Back to login</button>

                {error && <Error error={error} />}
            </div>
        </div>
    );
};

SignUpForm.propTypes = {
    email: PropTypes.string.isRequired,
    setEmail: PropTypes.func.isRequired,
    password: PropTypes.string.isRequired,
    setPassword: PropTypes.func.isRequired,
    confirmPassword: PropTypes.string.isRequired,
    setConfirmPassword: PropTypes.func.isRequired,
    signUp: PropTypes.func.isRequired,
    error: PropTypes.string,
    returnLogin: PropTypes.func.isRequired,
};

export default SignUpForm;
