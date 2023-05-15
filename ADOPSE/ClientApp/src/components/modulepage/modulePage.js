import React, { useState } from "react";
import Modules from "./modules/";
import ModuleFilter from "./ModuleFilter";
import "./modulepage.scss";

function ModulesPage() {
  const [priceRangeLimit] = useState([0, 100]);

  const [priceRange, setPriceRange] = useState(priceRangeLimit);
  const [stars, setStars] = useState([0, 5]);
  const [type, setType] = useState(1);
  const [difficulty, setDifficulty] = useState(1);

  return (
    <div className="module-page">
      <ModuleFilter
        priceRangeLimit={priceRangeLimit}
        priceRange={priceRange}
        setPriceRange={setPriceRange}
        type={type}
        setType={setType}
        difficulty={difficulty}
        setDifficulty={setDifficulty}
        setStars={setStars}
      />
      <Modules
        priceRange={priceRange}
        type={type}
        difficulty={difficulty}
        stars={stars}
      />
    </div>
  );
}

export default ModulesPage;
