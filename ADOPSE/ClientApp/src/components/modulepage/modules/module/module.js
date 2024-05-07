import React, { useEffect, useState } from "react";
import "./module.scss";
import graph from "./icons/graph.png";
import users from "./icons/users.png";
import ratingStar from "./icons/rating-star.png";
import { useNavigate } from "react-router-dom";
import { message } from "antd";
import { hasJWT } from "../../../authentication/authentication";

function Module(props) {
  const navigate = useNavigate();

  const [messageApi, contextHolder] = message.useMessage();
  const [isEnrolled, setIsEnrolled] = useState(true);

  useEffect(() => {
    if (props.isEnrolled === undefined) return;
    setIsEnrolled(props.isEnrolled);
  }, [props.isEnrolled]);

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

  const handleGoToModule = () => {
    navigate(`/module?id=${props.id}&query=${props.query}`);
  };

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

    fetch("/api/enrolled/" + props.id, requestOptions)
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
 

  return (
    <div className="module" onClick={handleGoToModule}>
      {contextHolder}
      <div className="price-box">
        <p className="number">
          {props.price !== 0 ? props.price + " $" : "Free"}
        </p>
      </div>
      <div className="box">
        <div className="title">
          <p className="school">{props.school}</p>
          {/* <p className="school">SCHOOL NAME HERE</p> */}
          <div className="subject">
            <p className="subject-text">{props.subject}</p>
          </div>
        </div>
        <div className="desc-tooltip-container"><span className="desc-tooltip">{props.description}</span></div>
        {/* <p className="event-subtype">{props.subject_type}</p> */}
        <p className="subtype">{props.subject_type}</p>
        <div className="difficulty">
          <img src={graph} alt="" className="icon"></img>
          {/* <p className="event-difficulty">{props.difficulty}</p> */}
          <p className="difficulty-value">{props.difficulty}</p>
        </div>
        <div className="rating">
          <img src={ratingStar} alt="" className="icon" />
          <p className="number">{props.rating}</p>
        </div>
        <div className="enrolled">
          <img src={users} alt="" className="icon" />
          <p className="number">{props.enrolled}</p>
        </div>
        {/* <button className="enrolledButton" onClick={enrollStudents}> */}
        {!props.isLoadingEnrolled && hasJWT() && (
          <button
            onClick={handleEnrollClick}
            className="enrolled-button"
            disabled={isEnrolled}
          >
            {!isEnrolled ? "Enroll" : "Enrolled"}
          </button>
        )}
      </div>
    </div>
  );
}

export default Module;
