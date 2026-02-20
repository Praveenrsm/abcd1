import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
const API_URL = process.env.REACT_APP_API_URL;
const Login = () => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [loginFailed, setLoginFailed] = useState(false);
    const navigate = useNavigate();

    const handleLogin = async (e) => {
        e.preventDefault();
        try {
            const response = await axios.post(`${API_URL}/api/Account/login`, {
                email,
                password
            });
// Log the entire response to see its structure
console.log('Login response:', response.data);

const { role, token } = response.data; // Ensure the token is retrieved from the response

// Log the token to check its value
console.log('Token received:', token);

// Save the token in localStorage or sessionStorage
localStorage.setItem('token', token);
            if (role === 'admin') {
                navigate('/Products'); 
            } else if (role === 'supplier') {
                navigate('/supplier'); 
            } else {
                setLoginFailed(true); 
            }
        } catch (error) {
            setLoginFailed(true); 
        }
    };

    return (
  <div className="login-page">
    <div className="login-card">
      <h2 className="login-title">Welcome Back</h2>
      <p className="login-subtitle">Please login to continue</p>

      <form onSubmit={handleLogin}>
        <div className="input-group">
          <input
            type="email"
            placeholder="Email address"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
          />
        </div>

        <div className="input-group">
          <input
            type="password"
            placeholder="Password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />
        </div>

        <button type="submit" className="login-btn">
          Login
        </button>
      </form>
      <p className="login-footer">
        Don't have an account? <a href="/register">Register</a>
      </p>

      {loginFailed && (
        <p className="error-text">Invalid email or password</p>
      )}
    </div>
  </div>
);

};

export default Login;
