import React from "react";
import "./Register.scss";

function Register(props) {
  return (
    <div className="register-screen">
      <h2 className="register-title">Educator</h2>
      <div className="register-box">
        <form>
          <label htmlFor="username">Username:</label>
          <input type="text" id="username" name="username" />
          <br />
          <label htmlFor="email">Email:</label>
          <input type="email" id="email" name="email" />
          <br />
          <label htmlFor="password">Password:</label>
          <input type="password" id="password" name="password" />
          <br />
          <div className="register-options">
            <label>
              <input type="checkbox" name="agreeToTerms" /> Agree to Terms of
              Service
            </label>
          </div>
          <button type="submit">Register</button>
        </form>
        <p className="login-link">
          Already a member? <a href="/login">Login</a>
        </p>
      </div>
    </div>
  );
}

export default Register;
