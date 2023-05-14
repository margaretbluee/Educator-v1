import React, { useState } from "react";
import "./Register.scss";
import { useNavigate } from "react-router-dom";
import { message } from "antd";

function Register(props) {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [email, setEmail] = useState("");
  const navigate = useNavigate();
  const key = "updatable";

  const [messageApi, contextHolder] = message.useMessage();
  const success = () => {
    messageApi.open({
      key,
      type: "loading",
      content: "creation...",
      style: {
        marginTop: "60px",
      },
    });
  };

  const handleRegister = async (event) => {
    event.preventDefault();

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
        success();
        messageApi.open({
          key,
          type: "success",
          content: "Loaded!",
          duration: 2,
        });
        // navigate("/login");
      } else {
        throw new Error("Register failed");
      }
    } catch (error) {
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
