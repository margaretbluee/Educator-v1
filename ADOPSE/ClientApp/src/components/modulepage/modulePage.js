import React from "react";
import Modules from "./modules/";
import ModuleFilter from "./ModuleFilter";

function ModulesPage() {
  return (
    <div style={{ display: "flex", justifyContent: "space-between" }}>
      <div>
        <ModuleFilter />
      </div>
      <div style={{ display: "flex", gap: "10px" }}>
        <Modules />
      </div>
    </div>
  );
}

export default ModulesPage;
