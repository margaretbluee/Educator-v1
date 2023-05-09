import React, {useState} from "react";
import "./Login.scss";
import axios from "axios";

function Login(props) {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');

  const handleLogin = async (event) => {
    event.preventDefault(); 
    
    console.log("Start Login")
    try {
      const response = await fetch('/api/authentication/login', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Accept' : '*/*',
          'Accept-Encoding' : 'gzip, deflate, br',
          'Connection' : 'keep-alive'
        },
        body: `{"username": "${username}","password": "${password}","email": "x@x.x"}`
      });

      if (response.ok) {
        const data = await response.json();
        console.log("Login response")
        console.log(data)
        localStorage.setItem('token', data.token);
        // Redirect to dashboard or home page
      } else {
        throw new Error('Login failed');
      }
    } catch (error) {
      console.error(error);
    }
  };
  
  
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
            onChange={(e) => setUsername(e.target.value)}
          />
          <br />
          <label htmlFor="password">Password:</label>
          <input
            type="password"
            id="password"
            name="password"
            className="login-input"
            onChange={(e) => setPassword(e.target.value)}
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
          <button type="submit" className="login-btn"  onClick={handleLogin}>
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
