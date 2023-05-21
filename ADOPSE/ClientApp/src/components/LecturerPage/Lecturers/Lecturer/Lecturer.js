import React from "react";
import profile from "./icons/profile.png";
import "./Lecturer.scss";
import { useNavigate } from "react-router-dom";

function Lecturer(props) {
  const navigate = useNavigate();

  const handleGoToLecturer = () => {
    navigate(`/lecturer?id=${props.id}`);
  };

  return (
    <div className="lecturer-card" onClick={handleGoToLecturer}>
      <div className="prof-image">
        <img src={profile} alt="profile"></img>
      </div>
      <span className="name">{props.name}</span>
    </div>
  );
}

export default Lecturer;
