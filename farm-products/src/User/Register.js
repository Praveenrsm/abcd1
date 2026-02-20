import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';

const Register = () => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [userName, setUserName] = useState('');
    const [phoneNumber, setPhoneNumber] = useState('');
    const [role, setRole] = useState('');
    const [error, setError] = useState('');

    const navigate = useNavigate();

    const handleRegister = async (e) => {
        e.preventDefault();
        try {
            await axios.post('https://localhost:7292/api/Account/register', {
                email,
                password,
                userName,
                phoneNumber,
                role
            });

            alert("Registration Successful! Please Login.");
            navigate('/');

        } catch (error) {
            setError("Registration failed");
        }
    };

    return (
        <div className="register-page">
            <div className="register-card">
                <h2>Create Account</h2>
                <form onSubmit={handleRegister}>
                    <input type="email" placeholder="Email"
                        value={email} onChange={(e) => setEmail(e.target.value)} />

                    <input type="password" placeholder="Password"
                        value={password} onChange={(e) => setPassword(e.target.value)} />

                    <input type="text" placeholder="User Name"
                        value={userName} onChange={(e) => setUserName(e.target.value)} />

                    <input type="text" placeholder="Phone Number"
                        value={phoneNumber} onChange={(e) => setPhoneNumber(e.target.value)} />

                    <input type="text" placeholder="Role"
                        value={role} onChange={(e) => setRole(e.target.value)} />

                    <button type="submit">Register</button>
                </form>

                {error && <p style={{ color: 'red' }}>{error}</p>}
            </div>
        </div>
    );
};

export default Register;
