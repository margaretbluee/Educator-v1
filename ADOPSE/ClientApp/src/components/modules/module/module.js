import React from "react";
import "./module.scss";
import graph from "./icons/graph.png";
import users from "./icons/users.png";
import ratingStar from "./icons/rating-star.png";

function Module(props) {
  return (
    <div className="module" key={props.key}>
      <div className="box">
        <p className="event-school">{props.school}</p>
        <p className="event-subject">{props.subject}</p>
        <p className="event-subtype">{props.subject_type}</p>
        <img src={graph} alt="" className="vector-icon"></img>
        <p className="event-difficulty">{props.difficulty}</p>
        <img src={ratingStar} alt="" className="rating-icon" />
        <p className="event-rating">{props.rating}</p>
        <img src={users} alt="" className="enrolled-icon" />
        <p className="event-enrolled">{props.enrolled}</p>
        {/* <button className="enrolledButton" onClick={enrollStudents}> */}
        <button className="enrolled-button">Enroll</button>
      </div>
    </div>
  );
}

export default Module;
