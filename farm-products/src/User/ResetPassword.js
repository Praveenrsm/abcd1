import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate, useLocation } from 'react-router-dom';
import './ResetPassword.css';

const ResetPassword = () => {
  const [newPassword, setNewPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [message, setMessage] = useState('');
  const navigate = useNavigate();
  const location = useLocation();

  const queryParams = new URLSearchParams(location.search);
  const token = queryParams.get('token');

  const handleResetPassword = async (e) => {
    e.preventDefault();

    if (newPassword !== confirmPassword) {
      setMessage("Passwords don't match!");
      return;
    }

    try {
      await axios.post(
        `https://localhost:7292/api/ForgotPassword/reset-password?token=${encodeURIComponent(token)}`,
        JSON.stringify(newPassword),
        {
          headers: { 'Content-Type': 'application/json' }
        }
      );

      setMessage('Password reset successful! Redirecting...');

      setTimeout(() => {
        navigate('/'); // ✅ home page
      }, 1500);

    } catch (error) {
      setMessage(
        error.response?.data ||
        'Failed to reset password. The link might be expired or invalid.'
      );
    }
  };

  return (
    <div className="reset-page">
      <div className="reset-card">
        <h2>Reset Password</h2>
        <p className="subtitle">Enter your new password below</p>

        <form onSubmit={handleResetPassword}>
          <input
            type="password"
            value={newPassword}
            onChange={(e) => setNewPassword(e.target.value)}
            placeholder="New password"
            required
          />

          <input
            type="password"
            value={confirmPassword}
            onChange={(e) => setConfirmPassword(e.target.value)}
            placeholder="Confirm new password"
            required
          />

          <button type="submit">Reset Password</button>
        </form>

        {message && <p className="info-text">{message}</p>}
      </div>
    </div>
  );
};

export default ResetPassword;
