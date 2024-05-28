import React, { useState, useEffect } from 'react';
import { message } from "antd";
import "./profile.scss";

function UsersList() {
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState(null);
    const [userData, setUserData] = useState({
        id:'',
        username: '',
        email: '',
        password: ''
    });




    const userRole = localStorage.getItem("role");

    useEffect(() => {
        async function fetchUserData() {
            try {
                const response = await fetch('/api/authentication/getCurrentUserDetails', {
                    method: 'GET',
                    headers: {
                        'Authorization': `Bearer ${localStorage.getItem('token')}`
                    }
                });
                const data = await response.json(); 
                console.log(data);
                setUserData(data);
                setIsLoading(false);
            } catch (error) {
                console.error('Error fetching user data:', error);
                setError('Error fetching user data. Please try again later.');
                setIsLoading(false);
            }
        }

        fetchUserData();
    }, []);

    const isValidEmail = (email) => {
        const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return emailPattern.test(email);
      };
    
    const handleSubmit = async (e) => {
        e.preventDefault();
        
        const { email, password } = userData;
        if (!isValidEmail(email)) { 
            message.error("Please enter a valid email address.");
            return;
        }
        if (password.length < 8) {
            message.error("Password must be at least 8 characters long.");
            return;
        }
      
        if (!/\d/.test(password)) {
            message.error("Password must contain at least one number.");
            return;
        }
      
        if (!/[a-z]/.test(password)) {
            message.error("Password must contain at least one lowercase letter.");
            return;
        }
      
        if (!/[A-Z]/.test(password)) {
            message.error("Password must contain at least one uppercase letter.");
            return;
        }
      
        if (!/[^a-zA-Z0-9]/.test(password)) {
            message.error("Password must contain at least one special character.");
            return;
        }
      
        try {
            const response = await fetch('/api/authentication/updateUser', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    Accept: "*/*",
                    "Accept-Encoding": "gzip, deflate, br",
                    Connection: "keep-alive",
                },
                body: JSON.stringify(userData)
            });
            const data = await response.json();
            if (response.ok) {
                message.success('User data updated successfully!');
            } else {
                throw new Error(data.message || 'Something went wrong');
            }
        } catch (error) {
            console.error('Error updating user data:', error);
            message.error('Error updating user data. Please try again.');
        }
    };

    const handleEditRoles = () => {
        window.location.href = './roles';
    };

    if (isLoading) {
        return <div>Loading...</div>;
    }

    if (error) {
        return <div>{error}</div>;
    }

    return (
        <div className="screen">
            {userRole === 'Admin' && <button className='editrolesbtn' onClick={handleEditRoles}>Edit Roles</button>}
            <h2 className='titlehello'>Hello {userRole}, {userData.username}</h2>
            <h3 className='titleprofile'>Here is your information</h3>
            <div className="userform">
                <form className='profileform'>
                    <label htmlFor="username">Username:</label>
                    <input className='inputU'
                        type="text"
                        id="username"
                        name="username"
                        value={userData.username}
                        onChange={(e) => setUserData({ ...userData, username: e.target.value })}
                    />
                    <br />
                    <label htmlFor="email">Email:</label>
                    <input className='inputE'
                        type="email"
                        id="email"
                        name="email"
                        value={userData.email}
                        onChange={(e) => setUserData({ ...userData, email: e.target.value })}
                    />
                    <br />
                    <label htmlFor="password">Password:</label>
                    <input className='inputP'
                        type="text"
                        id="password"
                        name="password"
                        value={userData.password}
                        onChange={(e) => setUserData({ ...userData, password: e.target.value })}
                    />
                    <br />
                   
                    <button className='changebtn' type="submit" onClick={handleSubmit}>
                        Change
                    </button>
                </form>
            </div>
        </div>
    );
}

export default UsersList;
