import React, { useEffect, useState } from "react";

import { useLocation } from "react-router-dom";

import Modules from "./modules";
// import "./moduleInfo.scss";

function Lecturer() {

  const location = useLocation();
  const [LecturerId] = useState(
    parseInt(new URLSearchParams(location.search).get("id")) || 1
  );
  const [lecturer, setLecturer] = useState({}); // module info
  const [isLoading, setIsLoading] = useState(true);
  const [failedToLoad, setFailedToLoad] = useState(false);

  useEffect(() => {
    setIsLoading(true);
    let retryCount = 0;
    const maxRetries = 3;

    async function fetchModules() {
      try {
        const response = await Promise.race([
          fetch(`/api/lecturer/${LecturerId}`),
          new Promise((_, reject) =>
            setTimeout(() => reject(new Error("Timeout")), 5000)
          ),
        ]);
        const data = await response.json();
        setLecturer(data);
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
  }, [LecturerId]);

  return isLoading ? (
    failedToLoad ? (
      <div>Failed to load modules. Please try again later.</div>
    ) : (
      <div>Loading...</div>
    )
  ) : lecturer.status === 404 ? (
    <div>Lecturer not found.</div>
  ) : (
    <div className="medium-container">
      <div className="prof-block">
        <h1 className="prof-details">INSTRUCTOR</h1>
        <h2 className="prof-details">
          <span style={{ color: "darkorange" }}>{lecturer.name}</span>
        </h2>
        <h3 className="prof-details">
          <span style={{ fontSize: "19px" }}>
            Head of Web Development in IHU
          </span>
        </h3>
      </div>
      <div className="prof-image">
        <img
          src="public\midPictures\4388-150x150.jpeg"
          alt="michalis salampasis"
        ></img>
      </div>
      <div className="small-icons">
        <img
          src="public\midPictures\pngfind.com-mail-png-479941.png"
          style={{ width: "17px", height: "17px", marginBottom: "20px" }}
        ></img>
        <img
          src="public\midPictures\pngfind.com-telephone-png-1203170.png"
          style={{ width: "17px", height: "17px", marginBottom: "20px" }}
        ></img>
        <img
          src="public\midPictures\pngfind.com-users-png-4650781.png"
          style={{ width: "17px", height: "17px", marginBottom: "20px" }}
        ></img>
      </div>
      <div className="information-details">
        <p style={{ fontWeight: "bold" }}>salampasis@ihu.gr</p>
        <p style={{ fontWeight: "bold" }}>+30 3210013061</p>
        <p style={{ fontWeight: "bold" }}>2.311 Enrolled</p>
      </div>
      <div className="paragraph-style">
        <p
          style={{
            fontWeight: "bold",
            fontSize: "17px",
            textAlign: "justify",
          }}
        >
          {lecturer.bio}
        </p>
      </div>
      <div className="event-header">
        <h1 className="courses-style">
          Courses By{" "}
          <span style={{ color: "darkorange" }}>Μιχάλης Σαλαμπάσης</span>
        </h1>
      </div>

      <Modules />
    </div>
  );
}

export default Lecturer;
