import React, { useState, useEffect } from "react";
import Lecturer from "./Lecturer";
import "./Lecturers.scss";

function Lecturers(props) {
  const [isLoading, setIsLoading] = useState(true);
  const [failedToLoad, setFailedToLoad] = useState(false);
  const [lecturers, setLecturers] = useState([]);

  useEffect(() => {
    setIsLoading(true);
    let retryCount = 0;
    const maxRetries = 3;

    async function fetchModules() {
      try {
        const response = await Promise.race([
          fetch(`/api/lecturer/`),
          new Promise((_, reject) =>
            setTimeout(() => reject(new Error("Timeout")), 5000)
          ),
        ]);
        const data = await response.json();
        setLecturers(data);
        setIsLoading(false);
      } catch (error) {
        console.error(error);
        if (retryCount < maxRetries) {
          retryCount++;
          console.log(`Retrying fetch... Attempt ${retryCount}`);
          fetchModules();
        } else {
          console.error(`Failed to fetch modules after ${maxRetries} attempts`);
          setFailedToLoad(true);
        }
      }
    }
    fetchModules();
  }, []);

  return isLoading ? ( // check if loading is true
    failedToLoad ? (
      <div>Failed to load modules. Please try again later.</div>
    ) : (
      <div>Loading...</div>
    )
  ) : (
    <div className="lecturers">
      {lecturers.map((lecturer, index) => (
        <Lecturer key={index} name={lecturer.name} id={lecturer.id} />
      ))}
    </div>
  );
}

export default Lecturers;
