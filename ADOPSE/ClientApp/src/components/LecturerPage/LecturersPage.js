import React, { useState } from "react";
import Lecturers from "./Lecturers";
// import Modules from "./modules/";
// import ModuleFilter from "./ModuleFilter";
// import "./modulepage.scss";
import "./LecturersPage.scss";

function LecturersPage() {
  return (
    <div className="lecturer-page">
      <Lecturers />
    </div>
  );
}

export default LecturersPage;
