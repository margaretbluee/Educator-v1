import React, { useState, useEffect } from 'react';
import { message } from "antd";
import "./Roles.scss";

function UsersList() {
  const [users, setUsers] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(null);
  
  useEffect(() => {
    async function fetchUsers() {
      try {
        const response = await fetch('/api/authentication/getUsers');
        const data = await response.json(); 
        console.log(data);
        setUsers(data);
        setIsLoading(false);
      } catch (error) {
        console.error('Error fetching users:', error);
        setError('Error fetching users. Please try again later.');
        setIsLoading(false);
      }
    }

    fetchUsers();
  }, []);
  

  //Αλλαγή κατάστασης Suspend/UnSuspend
  const SuspendButton = async (e, userId) => {
    const activity = e.target.value === "1" ? true : false;
    console.log('SuspendButton - Activity:', activity);
    console.log('SuspendButton - UserId:', userId);

    let messageText = activity ? 'User suspended successfully!' : 'User unsuspended successfully!';
    try {
    
      // Αποστολή για ενημέρωση του suspend στη βάση
       await fetch(`/api/authentication/suspendUser`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          Accept: "*/*",
          "Accept-Encoding": "gzip, deflate, br",
          Connection: "keep-alive",
        },
        body: JSON.stringify({ userId: userId, suspend: activity }),
      }); 

      
      setUsers(users.map(user => {
        if (user.id === userId) {
          return { ...user, suspend: activity };
        }
        return user;
      }));
      
      message.success(messageText, 3);

    } catch (error) {
      console.error('Error suspending user:', error);
     
      setUsers(users.map(user => {
        if (user.id === userId) {
          return { ...user, suspend: user.suspend };
        }
        return user;
      }));
    }
};


  //Αλλαγή ρόλου στην βάση
  const handleRoleChange = async (e, userId) => {
    const newRole = e.target.value;
    console.log(userId);
    console.log(newRole);
  
    try {
    
      // Αποστολή για ενημέρωση του ρόλου στη βάση
      await fetch(`/api/authentication/updateUserRole`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          Accept: "*/*",
          "Accept-Encoding": "gzip, deflate, br",
          Connection: "keep-alive",
        },
        body: JSON.stringify({ UserId: userId, newRole: newRole }),
      });

      console.log(JSON.stringify({ UserId: userId, newRole: newRole }));

      // Ενημέρωση του τοπικού state με το νέο ρόλο
      setUsers(users.map(user => {
        if (user.id === userId) {
          return { ...user, role: newRole };
        }
        return user;
      }));
      
      message.success('Role updated successfully!',3);

    } catch (error) {
      console.error('Error updating role:', error);
      // Επαναφορά του προηγούμενου ρόλου 
      setUsers(users.map(user => {
        if (user.id === userId) {
          return { ...user, role: user.role };
        }
        return user;
      }));
    }

  };
  
  
  
 


  if (isLoading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>{error}</div>;
  }

  return (
    <div>
      <h2 className='titleRoles'>Role Management</h2>
     
      <table className='table'>
        <thead>
          <tr>
            <th></th>
            <th>Id</th>
            <th>Username</th>
            <th>Email</th>
            <th>Password</th>
            <th>Role</th>
            <th>Suspend</th>
          </tr>
        </thead>
      <tbody>
        {users.map(user => (
          <tr key={user.id}>
            <td></td>
            <td>{user.id}</td>
            <td>{user.username}</td>
            <td>{user.email}</td>
            <td>{user.password}</td>
            <td>
              <select
                value={user.role}
                onChange={(e) => handleRoleChange(e, user.id)}
              >
                <option value="Admin">Admin</option>
                <option value="Student">Student</option>
                <option value="Lecturer">Lecturer</option>
              </select>
            </td>
            <td>
              <select
                value={user.suspend ? "1" : "0"}
                onChange={(e) => SuspendButton(e, user.id)}
              >
                <option value="1">Suspend</option>
                <option value="0">Unsuspend</option>
                
              </select>
            </td>
          </tr>
        ))}
      </tbody>
    </table>
  </div>
  );
}

export default UsersList;
