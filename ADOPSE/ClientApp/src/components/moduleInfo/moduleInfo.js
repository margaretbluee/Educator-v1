import React, { useEffect, useState } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faUsers,
  faStar,
  faMoneyBillWave,
  faHardHat,
} from "@fortawesome/free-solid-svg-icons";
import { useLocation } from "react-router-dom";
import "./moduleInfo.scss";

function ModuleInfo() {
  // Array of events data
  const events = [
    {
      key: 1,
      date: "April 10, 2023",
      subject: "Mathematics",
      time: "10:00 AM - 12:00 PM",
    },
    {
      key: 2,
      date: "April 15, 2023",
      subject: "Chemistry",
      time: "2:00 PM - 4:00 PM",
    },
    {
      key: 3,
      date: "April 20, 2023",
      subject: "Physics",
      time: "3:30 PM - 5:30 PM",
    },
    {
      key: 4,
      date: "April 25, 2023",
      subject: "Biology",
      time: "9:00 AM - 11:00 AM",
    },
    {
      key: 5,
      date: "April 25, 2023",
      subject: "Biology",
      time: "9:00 AM - 11:00 AM",
    },
    {
      key: 6,
      date: "April 25, 2023",
      subject: "Biology",
      time: "9:00 AM - 11:00 AM",
    },
    {
      key: 7,
      date: "April 25, 2023",
      subject: "Biology",
      time: "9:00 AM - 11:00 AM",
    },
    {
      key: 8,
      date: "April 25, 2023",
      subject: "Biology",
      time: "9:00 AM - 11:00 AM",
    },
    {
      key: 9,
      date: "April 25, 2023",
      subject: "Biology",
      time: "9:00 AM - 11:00 AM",
    },
    {
      key: 10,
      date: "April 25, 2023",
      subject: "Biology",
      time: "9:00 AM - 11:00 AM",
    },
    // Add more events as needed
  ];

  const location = useLocation();
  const [moduleId] = useState(
    parseInt(new URLSearchParams(location.search).get("id")) || 1
  );
  const [module, setModule] = useState({}); // module info
  const [isLoading, setIsLoading] = useState(true);
  const [failedToLoad, setFailedToLoad] = useState(false);

  useEffect(() => {
    setIsLoading(true);
    let retryCount = 0;
    const maxRetries = 3;

    async function fetchModules() {
      try {
        const response = await Promise.race([
          fetch(`/api/module/${moduleId}`),
          new Promise((_, reject) =>
            setTimeout(() => reject(new Error("Timeout")), 5000)
          ),
        ]);
        const data = await response.json();
        setModule(data);
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
  }, [moduleId]);

  return isLoading ? (
    failedToLoad ? (
      <div>Failed to load modules. Please try again later.</div>
    ) : (
      <div>Loading...</div>
    )
  ) : module.status === 404 ? (
    <div>Module not found.</div>
  ) : (
    <div className="module-info">
      <h1 className="title">{module.name}</h1>
      <div className="subtitle">{module.description}</div>
      <div className="teacher">
        Teacher: <a href="/">{module.lecturerName}</a>
      </div>
      <div className="course-details">
        <div className="course-first">
          <div className="course-info">
            <div className="icon">
              <FontAwesomeIcon icon={faHardHat} style={{ color: "white" }} />
            </div>
            <div className="text">Difficulty: {module.difficultyName}</div>
          </div>
          <div className="course-info">
            <div className="icon">
              <FontAwesomeIcon icon={faUsers} style={{ color: "white" }} />
            </div>
            <div className="text">Participants: {module.price}</div>
          </div>
          <div className="course-info">
            <div className="icon">
              <FontAwesomeIcon icon={faStar} style={{ color: "white" }} />
            </div>
            <div className="text">Rating: {module.rating}</div>
          </div>
          <div className="course-info">
            <div className="icon">
              <FontAwesomeIcon
                icon={faMoneyBillWave}
                style={{ color: "white" }}
              />
            </div>
            <div className="text">Price: {module.price}</div>
          </div>
        </div>
        <div className="course-second">
          <button className="buy-now-button">Enroll</button>
        </div>
      </div>
      <div className="upcoming-events">
        <h2>Upcoming Events</h2>
        <br className="orange-br" />
        <div className="event-slider">
          <div className="slider-card-container">
            {events.map((event, index) => (
              <div key={index} className="slider-card">
                <div className="event-date">{event.date}</div>
                <div className="event-subject">{event.subject}</div>
                <div className="event-time">{event.time}</div>
              </div>
            ))}
          </div>
        </div>
      </div>
    </div>
  );
}

export default ModuleInfo;
