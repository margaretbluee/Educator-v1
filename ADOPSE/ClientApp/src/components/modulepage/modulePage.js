import React from "react";
import Modules from "./modules/";
import ModuleFilter from "./ModuleFilter";
import "./modulepage.scss";

function ModulesPage() {
  return (
    <div className="module-page">
      <ModuleFilter />
      <Modules />
    </div>
  );
}

export default ModulesPage;
