import React, { useState, useEffect } from 'react';
import { message } from "antd";
import "./Roles.scss";
import Paginatorr from "./rolespaginator";

function UsersList() {
  const [users, setUsers] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(null);
  const [searchTerm, setSearchTerm] = useState("");
  const [usersPerPage, setUsersPerPage] = useState(10);
  
  
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
      
      message.success(messageText, 5);

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
      
      message.success('Role updated successfully!',5);

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
  
  // Αναζήτηση των χρηστών με βάση το searchTerm
const filteredUsers = users.filter(user =>
  user.username.toLowerCase().includes(searchTerm.toLowerCase()) ||
  user.email.toLowerCase().includes(searchTerm.toLowerCase())
);


  
const [currentPage, setCurrentPage] = useState(1);




const handleNumberChange = (e) => {
  const newNumber = parseInt(e.target.value);
  setUsersPerPage(newNumber);
  setCurrentPage(1); // Επαναφέρουμε την τρέχουσα σελίδα στην πρώτη όταν αλλάζει ο αριθμός των χρηστών ανά σελίδα
};


// Υπολογισμός του index του τελευταίου χρήστη σε κάθε σελίδα
const indexOfLastUser = currentPage * usersPerPage;
const indexOfFirstUser = indexOfLastUser - usersPerPage;
const currentUsers = filteredUsers.slice(indexOfFirstUser, indexOfLastUser);
const totalPages = Math.ceil(filteredUsers.length / usersPerPage);

  if (isLoading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>{error}</div>;
  }

  return (
    <div>
      <h2 className='titleRoles'>Role Management</h2>
      <div className='searchdiv'>
        <input
          className='searchRoles'
          type="text"
          placeholder="Search by username or email"
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
        />
        <svg className='icon' viewBox="0 0 24 24">
              <path
                fill="#666666"
                d="M9.5,3A6.5,6.5 0 0,1 16,9.5C16,11.11 15.41,12.59 14.44,13.73L14.71,14H15.5L20.5,19L19,20.5L14,15.5V14.71L13.73,14.44C12.59,15.41 11.11,16 9.5,16A6.5,6.5 0 0,1 3,9.5A6.5,6.5 0 0,1 9.5,3M9.5,5C7,5 5,7 5,9.5C5,12 7,14 9.5,14C12,14 14,12 14,9.5C14,7 12,5 9.5,5Z"
              />
        </svg> 
      </div>
      <div className='numofusers'>
        <h3 className='usersperpage'>Users per page:</h3>
        <select 
          value={usersPerPage}
          onChange={(e) => handleNumberChange(e , usersPerPage)}
        >
          <option value='10'>10</option>
          <option value='100'>100</option>
          <option value='300'>300</option>
          <option value='500'>500</option>
        </select>
      </div>
      <table className='table'>
        <thead>
          <tr>
            <th></th>
            <th>Id</th>
            <th>Username</th>
            <th>Email</th>
            <th>Role</th>
            <th>Suspend</th>
          </tr>
        </thead>
      <tbody>
        {currentUsers.map(user => (//usersToDisplay
          <tr key={user.id}>
            <td></td>
            <td>{user.id}</td>
            <td>{user.username}</td>
            <td>{user.email}</td>
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
    <Paginatorr
        activeIndex={currentPage}
        setActiveIndex={setCurrentPage}
        pageCount={totalPages}
      />
  </div>
  );
}

export default UsersList;
