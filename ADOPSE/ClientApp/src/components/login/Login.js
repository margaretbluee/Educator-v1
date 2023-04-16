import React from "react";
import "./Login.scss";

function Login(props) {
  return (
    <div className="login-screen">
      <h2 className="login-title">Educator</h2>
      <div className="login-box">
        <form>
          <label htmlFor="username">Username:</label>
          <input type="text" id="username" name="username" />
          <br />
          <label htmlFor="password">Password:</label>
          <input type="password" id="password" name="password" />
          <br />
          <div className="login-options">
            <label>
              <input type="checkbox" name="rememberMe" /> Remember me
            </label>
            <a href="/login">Forgot your password?</a>
          </div>
          <button type="submit">Login</button>
        </form>
        <p className="register-link">
          Not a member? <a href="/register">Register</a>
        </p>
      </div>
    </div>
  );
}

export default Login;
