import React, { useState } from "react";
import "./ModuleFilter.scss";
import { Button } from "reactstrap";

function ModuleFilter() {
  const [showOverlay, setShowOverlay] = useState(true);
  // const [minPrice, setMinPrice] = useState('');
  // const [maxPrice, setMaxPrice] = useState('');
  // const [workshop, setWorkshop] = useState(false);
  // const [theory, setTheory] = useState(false);
  // const [beginner, setBeginner] = useState(false);
  // const [intermediate, setIntermediate] = useState(false);
  // const [advanced, setAdvanced] = useState(false);
  // const [oneStar, setOneStar] = useState(false);
  // const [twoStars, setTwoStars] = useState(false);
  // const [threeStars, setThreeStars] = useState(false);
  // const [fourStars, setFourStars] = useState(false);
  // const [fiveStars, setFiveStars] = useState(false);

  const toggleOverlay = () => {
    setShowOverlay(!showOverlay);
  };

  return (
    <div className="module-filter">
      <button onClick={toggleOverlay}>Filter</button>
      {showOverlay && (
        <div className="overlay">
          <div className="filter-container">
            <div className="filter-group">
              <label>Price</label>
              <div className="input-group">
                <input type="text" placeholder="Min Price" />
                <input type="text" placeholder="Max Price" />
                <Button></Button>
              </div>
            </div>
            <div className="filter-group">
              <label>Type</label>
              <ul>
                <li>
                  <label>
                    <input type="checkbox" />
                    Εργαστήριο
                  </label>
                </li>
                <li>
                  <label>
                    <input type="checkbox" />
                    Θεωρία
                  </label>
                </li>
              </ul>
            </div>
            <div className="filter-group">
              <label>Hard Level</label>
              <ul>
                <li>
                  <label>
                    <input type="checkbox" />
                    Εύκολο
                  </label>
                </li>
                <li>
                  <label>
                    <input type="checkbox" />
                    Μέτριο
                  </label>
                </li>
                <li>
                  <label>
                    <input type="checkbox" />
                    Δύσκολο
                  </label>
                </li>
              </ul>
            </div>
            <div className="filter-group">
              <label>Rate</label>
              <ul>
                <li>
                  <label>
                    <input type="checkbox" />1 Star
                  </label>
                </li>
                <li>
                  <label>
                    <input type="checkbox" />2 Stars
                  </label>
                </li>
                <li>
                  <label>
                    <input type="checkbox" />3 Stars
                  </label>
                </li>
                <li>
                  <label>
                    <input type="checkbox" />4 Stars
                  </label>
                </li>
                <li>
                  <label>
                    <input type="checkbox" />5 Stars
                  </label>
                </li>
              </ul>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}

export default ModuleFilter;
