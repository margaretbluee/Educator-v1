import React from "react";
import "./Login.scss";

function Login(props) {
  return (
    <div className="page-login">
      <h2 className="login-title">Educator</h2>
      <div className="login-box module-login">
        <form>
          <label htmlFor="username">Username:</label>
          <input
            type="text"
            id="username"
            name="username"
            className="login-input"
          />
          <br />
          <label htmlFor="password">Password:</label>
          <input
            type="password"
            id="password"
            name="password"
            className="login-input"
          />
          <br />
          <div className="login-options">
            <label>
              <input type="checkbox" name="rememberMe" /> Remember me
            </label>
            <a href="/login" className="forgot-password-link">
              Forgot your password?
            </a>
          </div>
          <button type="submit" className="login-btn">
            Login
          </button>
        </form>
        <p className="register-link">
          Not a member?{" "}
          <a href="/register" className="register-link-text">
            Register
          </a>
        </p>
      </div>
    </div>
  );
}

export default Login;
