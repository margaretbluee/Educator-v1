import React from 'react';
import './main-page.css';
import { Calendar, momentLocalizer } from 'react-big-calendar';
import moment from 'moment';
import 'react-big-calendar/lib/css/react-big-calendar.css';

const localizer = momentLocalizer(moment);

const events = [
  {
    title: 'Meeting 1',
    start: new Date(2023, 3, 5, 10, 0),
    end: new Date(2023, 3, 5, 11, 0),
  },
  {
    title: 'Meeting 2',
    start: new Date(2023, 3, 6, 14, 0),
    end: new Date(2023, 3, 6, 15, 0),
  },
];

const MainPage = () => {
  return (
    <div className="main-page">
      <h1>Welcome to Educator!</h1>
      <Calendar
        localizer={localizer}
        events={events}
        startAccessor="start"
        endAccessor="end"
        style={{ height: '100vh' }}
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
