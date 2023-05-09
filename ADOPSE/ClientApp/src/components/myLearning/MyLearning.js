import React, { useEffect, useState } from "react";
import { hasJWT } from "../authentication/authentication";
function MyLearning() {
  const [modules, setModules] = useState([]);
  const fetchModules = async () => {
    try {
      const headers = {
        Authorization: `Bearer ${localStorage.getItem("token")}`,
      };
      const response = await fetch("/api/enrolled", { headers });

      if (response.ok) {
        const data = await response.json();
        setModules(data);
        console.log("Login response " + modules);
        console.log(data);
      } else {
        throw new Error("Error fetching");
      }
    } catch (error) {
      console.error(error);
    }
  };

  useEffect(() => {
    fetchModules();
  }, []);

  return <div>{hasJWT() ? <div>You are auth</div> : <div>No auth</div>}</div>;
}

export default MyLearning;
