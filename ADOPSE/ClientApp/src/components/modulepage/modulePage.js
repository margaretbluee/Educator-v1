import React, { useState } from "react";
import Modules from "./modules/";
import ModuleFilter from "./ModuleFilter";
import "./modulepage.scss";

function ModulesPage() {
  const [priceRangeLimit] = useState([0, 100]);
  const [priceRange, setPriceRange] = useState(priceRangeLimit);
  const [stars, setStars] = useState([0, 5]);

  return (
    <div className="module-page">
      <ModuleFilter
        setStars={setStars}
        priceRange={priceRange}
        priceRangeLimit={priceRangeLimit}
        setPriceRange={setPriceRange}
      />
      <Modules stars={stars} priceRange={priceRange} />
    </div>
  );
}

export default ModulesPage;
