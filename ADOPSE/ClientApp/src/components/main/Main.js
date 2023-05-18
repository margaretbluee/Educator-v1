import React, {useEffect, useState} from "react";
import "./MainPage.scss";
import { Calendar, momentLocalizer } from "react-big-calendar";
import moment from "moment";
import "react-big-calendar/lib/css/react-big-calendar.css";

const localizer = momentLocalizer(moment);


/*{
  title: "Meeting 1",
      start: new Date(2023, 5, 19, 10, 0),
    end: new Date(2023, 5, 19, 11, 0),
},
{
  title: "Meeting 2",
      start: new Date(2023, 4, 6, 14, 0),
    end: new Date(2023, 4, 6, 15, 0),
},*/

function convertStringToDate(str) {
  const dateParts = str.split('T')[0].split('-');
  const timeParts = str.split('T')[1].split(':');

  const year = parseInt(dateParts[0]);
  const month = parseInt(dateParts[1]) - 1; // Months are zero-based (0-11)
  const day = parseInt(dateParts[2]);
  const hour = parseInt(timeParts[0]);
  const minute = parseInt(timeParts[1]);

  return new Date(year, month, day, hour, minute);
}

const MainPage = () => {
  
  const [events , setEvents] = useState([]);
  
  useEffect(() => {
    var myHeaders = new Headers();
    myHeaders.append("Authorization", `Bearer ${localStorage.getItem("token")}`);

    var requestOptions = {
      method: 'GET',
      headers: myHeaders,
      redirect: 'follow'
    };

    fetch("https://localhost:44442/api/calendar/", requestOptions)
        .then(response => response.json())
        .then(result => {
          let listaEvents = []
          console.log(result);
          result.forEach((event) => {
            listaEvents.push({
              title : event.name,
              start: convertStringToDate(event.starts),
              end: convertStringToDate(event.ends),
            });
          });
          
          console.log(listaEvents);
          
          setEvents(listaEvents);
        })
        .catch(error => console.log('error', error));
  },[]);
  
  return (
    <div className="main-page">
      <h1 className="heading">Welcome to Educator!</h1>
      <Calendar
        localizer={localizer}
        events={events}
        startAccessor="start"
        endAccessor="end"
        style={{ height: "100vh" }}
      />
      {/* <main className="App-main">
          <div className="left-section">
            <p className="left-text">"Streamline your studies with our university course scheduling program - never miss a class again!"</p>
            <button className="orange-button">Join Free</button>
          </div>
          <div className="right-section">
            <img src="" alt="placeholder" />
          </div>
      </main> */}
    </div>
  );
};

export default MainPage;
