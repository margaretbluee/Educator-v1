import React, { useEffect, useState } from "react";

import { useLocation } from "react-router-dom";

import Modules from "./modules";
import "./lecturer.scss";
import profile from "./icons/profile.png";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faEnvelope, faGlobe } from "@fortawesome/free-solid-svg-icons";
import DOMPurify from "dompurify";

function convertURLsToLinks(text) {
  const urlRegex = /((?:https?:\/\/)?(?:[\w-]+\.)+[\w-]+\.[\w-]+)/g;
  return text.replace(urlRegex, (url) => {
    const prefixedURL = url.startsWith("http") ? url : `http://${url}`;
    return `<a href="${prefixedURL}" target="_blank" rel="noopener noreferrer">${url}</a>`;
  });
}

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
    <div className="lecturer">
      <div className="lecturer-whole">
        <div className="medium-container">
          <div>
            <h1 className="instructor">INSTRUCTOR</h1>
            <h2 className="subtitle">
              <span style={{ color: "darkorange" }}>{lecturer.name}</span>
            </h2>
            {/* <h3 className="prof-details">
            <span style={{ fontSize: "19px" }}>
              Head of Web Development in IHU
            </span>
          </h3> */}
          </div>
          <div className="lecturer-details">
            <div className="prof-image">
              <img src={profile} alt="profile"></img>
            </div>
            <div className="lecturer-info">
              <div className="item">
                <div className="icon">
                  <FontAwesomeIcon icon={faEnvelope} />
                </div>
                <div className="text">
                  <a href={`mailto:${lecturer.email}`}>{lecturer.email}</a>
                </div>
              </div>
              <div className="item">
                <div className="icon">
                  <FontAwesomeIcon icon={faGlobe} />
                </div>
                <div className="text">
                  <a
                    href={
                      lecturer.website.startsWith("http")
                        ? lecturer.website
                        : `http://${lecturer.website}`
                    }
                  >
                    {lecturer.website}
                  </a>
                </div>
              </div>
              {/* <div className="item">
              <div className="icon">
                <FontAwesomeIcon icon={faGlobe} />
              </div>
              <div className="text"></div>
            </div> */}
            </div>
          </div>
        </div>
        <div className="lecturer-bio">
          <p
            dangerouslySetInnerHTML={{
              __html: DOMPurify.sanitize(convertURLsToLinks(lecturer.bio)),
            }}
          ></p>
        </div>
      </div>
      <div>
        <h1 className="courses-style">
          Courses By <span className="name">{lecturer.name}</span>
        </h1>
      </div>
      <Modules />
    </div>
  );
}

export default Lecturer;
