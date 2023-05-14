import React, { useState } from "react";
import "./ModuleFilter.scss";
import { Button } from "reactstrap";

function ModuleFilter() {
  const [showOverlay, setShowOverlay] = useState(true);
  const [toggled, setToggled] = useState(true);
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
    setToggled(!toggled);
  };

  return (
    <div className="module-filter">
      <div className="filter-button">
        <div className="filter-box">
          <svg
            className={`filter-burger ${toggled ? "toggled" : ""} bi bi-filter`}
            onClick={toggleOverlay}
            xmlns="http://www.w3.org/2000/svg"
            fill="currentColor"
            viewBox="0 0 16 16"
          >
            <path d="M6 10.5a.5.5 0 0 1 .5-.5h3a.5.5 0 0 1 0 1h-3a.5.5 0 0 1-.5-.5zm-2-3a.5.5 0 0 1 .5-.5h7a.5.5 0 0 1 0 1h-7a.5.5 0 0 1-.5-.5zm-2-3a.5.5 0 0 1 .5-.5h11a.5.5 0 0 1 0 1h-11a.5.5 0 0 1-.5-.5z" />
          </svg>
        </div>
      </div>
      {/* <button >Filter</button> */}
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
