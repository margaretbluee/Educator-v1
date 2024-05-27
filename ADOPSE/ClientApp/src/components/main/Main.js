import React, { useEffect, useState, useRef } from "react";
import "./MainPage.scss";
import { Calendar, momentLocalizer } from "react-big-calendar";
import moment from "moment";
import "react-big-calendar/lib/css/react-big-calendar.css";
import { hasJWT } from "../authentication/authentication";
import girl from "./girl.png";
import { useNavigate } from "react-router-dom";
import './calendarStyle.scss';

const localizer = momentLocalizer(moment);

function convertStringToDate(str) {
  const dateParts = str.split("T")[0].split("-");
  const timeParts = str.split("T")[1].split(":");

  const year = parseInt(dateParts[0]);
  const month = parseInt(dateParts[1]) - 1; // Months are zero-based (0-11)
  const day = parseInt(dateParts[2]);
  const hour = parseInt(timeParts[0]);
  const minute = parseInt(timeParts[1]);

  return new Date(year, month, day, hour, minute);
}

const MainPage = () => {
  const [events, setEvents] = useState([]);
  const navigate = useNavigate();
  const calendarContainerRef = useRef(null);
  const [calendarHeight, setCalendarHeight] = useState(0);

  useEffect(() => {
    var myHeaders = new Headers();
    myHeaders.append(
      "Authorization",
      `Bearer ${localStorage.getItem("token")}`
    );

    var requestOptions = {
      method: "GET",
      headers: myHeaders,
      redirect: "follow",
    };

    fetch("/api/calendar/", requestOptions)
      .then((response) => response.json())
      .then((result) => {
        let listaEvents = [];
        console.log(result);
        result.forEach((event) => {
          listaEvents.push({
            title: event.name,
            desc: event.desc, 
            start: convertStringToDate(event.starts),
            end: convertStringToDate(event.ends),
          });
        });

        console.log(listaEvents);

        setEvents(listaEvents);
      })
      .catch((error) => console.log("error", error));
  }, []);

  const handleJoinFree = () => {
    navigate("/register");
  };

  useEffect(() => {
    const handleResize = () => {
      if (calendarContainerRef.current) {
        const width = calendarContainerRef.current.offsetWidth;
        const height = Math.floor(width / 2);
        setCalendarHeight(height);
      }
    };

    window.addEventListener("resize", handleResize);
    handleResize();

    return () => {
      window.removeEventListener("resize", handleResize);
    };
  }, []);

   

    async function FetchAllEventsFromGoogleAndSync() {
        var myHeaders = new Headers();
        myHeaders.append("Content-Type", "application/json");
        myHeaders.append(
            "Authorization", `Bearer ${localStorage.getItem("token")}`
        );

        var requestOptions = {
            method: "GET",
            headers: myHeaders,
            redirect: "follow",
        };    
        
        let eventList = await fetch("/api/calendar/fetchAndSync", requestOptions)
            .then(response => {                
                if (!response.ok) {
                    throw new Error("Could not fetch resource")
                }
                return response.json()                
            })
            .then(data => {                
                console.log(data)
                return data;                          
            })
            .catch((error) => {
                console.log("error", error)
            });

        console.log(eventList);        

        alert("Synchronized as well as posible! Thanks for your services.");

    }

  return (
    <div className="main-page">
      {hasJWT() ? (
        <div ref={calendarContainerRef}>
          <h1 className="heading">Welcome to Educator!</h1>
          <Calendar
            localizer={localizer}
            events={events}
            startAccessor="start"
            endAccessor="end"
            style={{ height: calendarHeight }}
          />
          <div className="down-section">
          <>
              <button
              className="fetchAndSync-button"
              onClick={() => FetchAllEventsFromGoogleAndSync()}> Fetch And Sync</button>
          </>
          </div>   
        </div>
        
      ) : (
        <main className="app-main">
          <div className="left-section">
            <p className="left-text">
              "Streamline your studies with our university course scheduling
              program - never miss a class again!"
            </p>
            <button className="orange-button" onClick={handleJoinFree}>
              Join Free
            </button>
          </div>
          <div className="right-section">
            <img src={girl} alt="placeholder" />
          </div>
        </main>
      )}
    </div>
  );
};

export default MainPage;
