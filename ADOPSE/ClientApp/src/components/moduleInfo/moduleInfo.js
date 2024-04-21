import React, { useEffect, useState } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faUsers,
  faStar,
  faMoneyBillWave,
  faHardHat,
} from "@fortawesome/free-solid-svg-icons";
import { useLocation, useNavigate } from "react-router-dom";
import "./moduleInfo.scss";
import { message } from "antd";
import { hasJWT } from "../authentication/authentication";

function ModuleInfo() {
  const location = useLocation();
  const navigate = useNavigate();

  const [moduleId] = useState(
    parseInt(new URLSearchParams(location.search).get("id")) || 1
  );
  const [module, setModule] = useState({}); // module info
  const [isLoading, setIsLoading] = useState(true);
  const [failedToLoad, setFailedToLoad] = useState(false);
  const [isLoadingCalendarCreate, setIsLoadingCalendarCreate] = useState(false);

  const [eventsIsLoading, setEventsIsLoading] = useState(true);
  const [eventsfailedToLoad, setEventsFailedToLoad] = useState(false);
  const [events, setEvents] = useState([]);
  const [isEnrolled, setIsEnrolled] = useState(true);

  const handleGoLecturer = () => {
    navigate(`/lecturer?id=${module.leaderId}`);
  };

  const [messageApi, contextHolder] = message.useMessage();

  const success = () => {
    messageApi.open({
      type: "success",
      content: "success enrollment",
      style: {
        marginTop: "60px",
      },
    });
  };

  const error = () => {
    messageApi.open({
      type: "error",
      content: "error on enrollment",
      style: {
        marginTop: "60px",
      },
    });
  };

  const calendarCreationLoading = () => {
    messageApi.open({
      type: "loading",
      content: "Loading",
      style: {
        color: "darkorange",
        marginTop: "60px",
      },
    });
  };

  // const calendarCreationFailed = () => {
  //   messageApi.open({
  //     type: "error",
  //     content: "Calendar creation failed",
  //     style: {
  //       color: "red",
  //       marginTop: "60px",
  //     },
  //   });
  // };

    // const calendarCreationSuccessed = () => {
  //   messageApi.open({
  //     type: "success",
  //     content: "Calendar successfully created",
  //     style: {
  //       color: "lightgreen",
  //       marginTop: "60px",
  //     },
  //   });
  // };

  useEffect(() => {
    let retryCount = 0;
    const maxRetries = 3;

    async function fetchIsEnrolled() {
      try {
        const headers = {
          Authorization: `Bearer ${localStorage.getItem("token")}`,
          "Content-Type": "application/json",
        };
        const response = await Promise.race([
          fetch(`/api/enrolled/isEnrolled/${moduleId}`, { headers }),
          new Promise((_, reject) =>
            setTimeout(() => reject(new Error("Timeout")), 5000)
          ),
        ]);
        const data = await response.json();
        setIsEnrolled(data.isEnrolled);
      } catch (error) {
        console.error(error);
        if (retryCount < maxRetries) {
          retryCount++;
          console.log(`Retrying fetch... Attempt ${retryCount}`);
          fetchIsEnrolled();
        } else {
          console.error(`Failed to fetch modules after ${maxRetries} attempts`);
          // setFailedToLoad(true);
        }
      }
    }
    fetchIsEnrolled();
  }, [isEnrolled, moduleId]);

  const handleEnrollClick = (event) => {
    event.stopPropagation(); // Stop event propagation to parent
    var myHeaders = new Headers();
    myHeaders.append(
      "Authorization",
      `Bearer ${localStorage.getItem("token")}`
    );

    var requestOptions = {
      method: "POST",
      headers: myHeaders,
      redirect: "follow",
    };

    fetch("/api/enrolled/" + moduleId, requestOptions)
      .then((response) => {
        if (response.ok) {
          setIsEnrolled(true);
          return response.text();
        } else {
          return null;
        }
      })
      .then((result) => {
        console.log(result);
        if (result) {
          success();
        } else {
          error();
        }
      })
      .catch((error) => {
        console.log("error", error);
        error();
      });
  };

  const handleCreateCalendarClick = () => {
    var myHeaders = new Headers();
    myHeaders.append(
        "Authorization",
        `Bearer ${localStorage.getItem("token")}`
    );

    var requestOptions = {
        method: "PUT",
        headers: myHeaders,
        redirect: "follow",
    };
  
    setIsLoadingCalendarCreate(true)
    fetch(`/api/module/${moduleId}/googleCalendarId`, requestOptions)
    .then((response) => {
            if (response.ok) {
              console.log(response)
                return response.json();
            }
        })        
        .then((result) => {
          setIsLoadingCalendarCreate(false)
            if (result) {
                // success();
                // calendarCreationSuccesed();
                alert("Calendar successfuly created");
            } else {
                alert("Calendar creation failed");
              // calendarCreationFailed();
              // error();
            }                
        })
        .catch((error) => {
          setIsLoadingCalendarCreate(false)
            console.log("error", error);
            error();
        })
        .finally(
          setIsLoadingCalendarCreate(false)
        );
    };

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

  useEffect(() => {
    setEventsIsLoading(true);
    let retryCount = 0;
    const maxRetries = 3;
    async function fetchEvents() {
      try {
        const response = await Promise.race([
          fetch(`/api/events/${moduleId}`),
          new Promise((_, reject) =>
            setTimeout(() => reject(new Error("Timeout")), 5000)
          ),
        ]);
        const data = await response.json();
        setEvents(data);
        setEventsIsLoading(false);
      } catch (error) {
        console.error(error);
        if (retryCount < maxRetries) {
          retryCount++;
          console.log(`Retrying fetch... Attempt ${retryCount}`);
          fetchEvents();
        } else {
          console.error(`Failed to fetch modules after ${maxRetries} attempts`);
          setEventsFailedToLoad(true);
        }
      }
    }
    fetchEvents();
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
      {contextHolder}
      <h1 className="title">{module.name}</h1>
      <div className="subtitle">{module.description}</div>
      <div className="teacher">
        Teacher:{" "}
        <div onClick={handleGoLecturer} className="name">
          <p>{module.lecturerName}</p>
        </div>
      </div>
      <div className="course-details">
        <div className={`course-first ${hasJWT() ? "" : "full-border"}`}>
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
        {hasJWT() && (
          <div className="course-second">            
            <button
              className="buy-now-button"
              onClick={handleEnrollClick}
              disabled={isEnrolled}
            >
              {!isEnrolled ? "Enroll" : "Enrolled"}
            </button>
            <button            
              className = "create-callendar-button"
              onClick= {handleCreateCalendarClick}
            >
              Create Calendar
            </button>
          </div>                    
        )}
      </div>
      <div className="course-third">        
      {isLoadingCalendarCreate ? 
        (
          <h5 style={{display: "inline-block"}}>Loading...</h5> 
        ) : (
          null
        )
      }
        <button
          onClick={handleCreateCalendarClick}
          className="createCalendar-button"          
          >            
            Create Calendar
          </button>
      </div>
      <div className="upcoming-events">
        <h2>Upcoming Events</h2>
        <br className="orange-br" />

        {eventsIsLoading ? (
          eventsfailedToLoad ? (
            <div>Failed to load Load. Please try again later.</div>
          ) : (
            <div>Loading...</div>
          )
        ) : module.status === 404 ? (
          <div>Events Not Fount.</div>
        ) : (
          <div className="event-slider">
            <div className="slider-card-container">
              {events.map((event, index) => (
                <div key={index} className="slider-card">
                  <div className="event-date">{formatDate(event.starts)}</div>
                  <div className="event-subject">{event.name}</div>
                  <div className="event-time">
                    {formatTime(event.starts)} - {formatTime(event.ends)}
                  </div>
                </div>
              ))}
            </div>
          </div>
        )}
      </div>
    </div>
  );
}

function formatDate(timestamp) {
  const date = new Date(timestamp);
  const formattedDate = date.toLocaleDateString("en-US", {
    year: "numeric",
    month: "long",
    day: "numeric",
  });
  return formattedDate;
}

function formatTime(timestamp) {
  const date = new Date(timestamp);

  const formattedTime = date.toLocaleTimeString("en-US", {
    hour: "numeric",
    minute: "numeric",
    hour12: true,
  });

  return formattedTime;
}

export default ModuleInfo;
