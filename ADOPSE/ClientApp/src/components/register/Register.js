import React, { useState } from "react";
import "./Register.scss";
import { useNavigate } from "react-router-dom";
import { message } from "antd";

function Register(props) {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [email, setEmail] = useState("");
  //const [role, setRole] = useState("");
  const navigate = useNavigate();
  const key = "updatable";

  const [messageApi, contextHolder] = message.useMessage();
  const success = () => {
    messageApi.open({
      key,
      type: "loading",
      content: "Creating user ...",
      style: {
        marginTop: "60px",
      },
    });
  };

  const close = () => {
    messageApi.open({
      key,
      type: "success",
      content: "Created!",
      duration: 1,
      style: {
        marginTop: "60px",
      },
      onClose: () => {
        navigate("/login");
      },
    });
  };

  /*const errorM = () => {
    messageApi.open({
      key,
      type: "error",
      content: "Error on user creation",
      duration: 1,
      style: {
        marginTop: "60px",
      },
    });
  };*/
  const errorM = (errorMessage) => {
    messageApi.open({
      key,
      type: "error",
      content: errorMessage,
      duration: 1,
      style: {
        marginTop: "60px",
        color: "red", 
      },
    });
  };
  const isValidEmail = (email) => {
    const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailPattern.test(email);
  };

  const handleRegister = async (event) => {
    event.preventDefault();
    
    if (!username || !password || !email) {
      errorM("All fields are required."); 
      return;
    }

    if (!isValidEmail(email)) { 
      errorM("Please enter a valid email address.");
      return;
    }

    const agreeToTerms = document.querySelector('input[name="agreeToTerms"]').checked;
    if (!agreeToTerms) {
      errorM("You must agree to the Terms of Service.");
      return;
    }

    if (password.length < 8) {
      errorM("Password must be at least 8 characters long.");
      return;
    }

    if (!/\d/.test(password)) {
      errorM("Password must contain at least one number.");
      return;
    }

    if (!/[a-z]/.test(password)) {
      errorM("Password must contain at least one lowercase letter.");
      return;
    }

    if (!/[A-Z]/.test(password)) {
      errorM("Password must contain at least one uppercase letter.");
      return;
    }

    if (!/[^a-zA-Z0-9]/.test(password)) {
      errorM("Password must contain at least one special character.");
      return;
    }

    success();

    console.log(
      `{"username": "${username}","password": "${password}","email": "${email}"}`
    );

    try {
      const response = await fetch("/api/authentication/register", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Accept: "*/*",
          "Accept-Encoding": "gzip, deflate, br",
          Connection: "keep-alive",
        },
        body: `{"username": "${username}","password": "${password}","email": "${email}"}`,
        
      });

      if (response.ok) {
        const data = await response.json();
        console.log(data);

        close();
      } else {
        errorM("");
        throw new Error("Register failed");
      }
    } catch (error) {
      errorM(" not ok");
      console.error(error);
    }

 
  };

  return (
    <div className="register-screen">
      {contextHolder}
      <h2 className="register-title">Educator</h2>
      <div className="register-box">
        <form>
          <label htmlFor="username">Username:</label>
            <input
              type="text"
              id="username"
              name="username"
              onChange={(e) => setUsername(e.target.value)}
            />
          
          <br />
          <label htmlFor="email">Email:</label>
          <input
            type="email"
            id="email"
            name="email"
            onChange={(e) => setEmail(e.target.value)}
          />
          <br />
          <label htmlFor="password">Password:</label>
          <input
            type="password"
            id="password"
            name="password"
            onChange={(e) => setPassword(e.target.value)}
          />
          <br />
          
          
        
          <div className="register-options">
            <label>
              <input type="checkbox" name="agreeToTerms" /> Agree to Terms of
              Service
            </label>
          </div>
          <button type="submit" onClick={handleRegister}>
            Register
          </button>
        </form>
        <p className="login-link">
          Already a member? <a href="/login">Login</a>
        </p>
      </div>
    </div>
  );
}

export default Register;
