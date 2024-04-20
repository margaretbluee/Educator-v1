import React, { useState } from "react";
import Modules from "./modules";
import ModuleFilter from "./ModuleFilter";
import "./MyModules.scss";

function MyModules() {
  const [priceRangeLimit] = useState([0, 100]);

  const [priceRange, setPriceRange] = useState(priceRangeLimit);
  const [stars, setStars] = useState([1, 5]);
  const [type, setType] = useState(0);
  const [searchType, setSearchType] = useState(0);
  const [difficulty, setDifficulty] = useState(0);

  return (
    <div className="my-module-page">
      <ModuleFilter
        priceRangeLimit={priceRangeLimit}
        priceRange={priceRange}
        setPriceRange={setPriceRange}
        type={type}
        setType={setType}
        searchType = {searchType}
        setSearchType = {setSearchType}
        difficulty={difficulty}
        setDifficulty={setDifficulty}
        setStars={setStars}
      />
      <Modules
        priceRange={priceRange}
        type={type}
        difficulty={difficulty}
        stars={stars}
        searchType={searchType}
      />
    </div>
  );
}

export default MyModules;
