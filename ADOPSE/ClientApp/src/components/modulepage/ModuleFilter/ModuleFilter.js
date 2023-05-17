import React, { useEffect, useRef, useState } from "react";
import "./ModuleFilter.scss";
import Slider from "@mui/material/Slider";
import { ReactiveBase, RatingsFilter } from "@appbaseio/reactivesearch";

function ModuleFilter(props) {
  const minValueRef = useRef(null);
  const maxValueRef = useRef(null);

  const [showOverlay, setShowOverlay] = useState(true);

  const priceRangeLimit = props.priceRangeLimit;
  const priceRange = props.priceRange;
  const setPriceRange = props.setPriceRange;

  const [priceRangeTemp, setPriceRangeTemp] = useState(priceRange);

  useEffect(() => {
    minValueRef.current.value = priceRange[0];
    maxValueRef.current.value = priceRange[1];
  }, [priceRange]);

  const handlePriceChange = (event, newValue) => {
    setPriceRangeTemp(newValue);
  };

  const handlePriceCommited = (event, newValue) => {
    setPriceRange(newValue);
    minValueRef.current.value = newValue[0];
    maxValueRef.current.value = newValue[1];
  };

  const handleMinPriceChange = (event) => {
    if (event.target.value) {
      const value = parseFloat(event.target.value);
      if (value <= priceRange[1]) {
        setPriceRange([value, priceRange[1]]);
        setPriceRangeTemp([value, priceRange[1]]);
      }
    }
  };

  const handleMaxPriceChange = (event) => {
    if (event.target.value) {
      const value = parseFloat(event.target.value);
      if (value >= priceRange[0]) {
        setPriceRange([priceRange[0], value]);
        setPriceRangeTemp([priceRange[0], value]);
      }
    }
  };

  const handleStarChange = (selectedRatings) => {
    props.setStars(selectedRatings);
  };

  // useEffect(() => {
  //   console.log("price range: ", priceRange)
  // }, [priceRange]);

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
      <ReactiveBase
        app="good-books-ds"
        url="https://a03a1cb71321:75b6603d-9456-4a5a-af6b-a487b309eb61@appbase-demo-ansible-abxiydt-arc.searchbase.io"
      >
        <div className="filter-button">
          <div className="filter-box">
            <svg
              className={`filter-burger ${
                showOverlay ? "toggled" : ""
              } bi bi-filter`}
              onClick={toggleOverlay}
              xmlns="http://www.w3.org/2000/svg"
              fill="currentColor"
              viewBox="0 0 16 16"
            >
              <path d="M6 10.5a.5.5 0 0 1 .5-.5h3a.5.5 0 0 1 0 1h-3a.5.5 0 0 1-.5-.5zm-2-3a.5.5 0 0 1 .5-.5h7a.5.5 0 0 1 0 1h-7a.5.5 0 0 1-.5-.5zm-2-3a.5.5 0 0 1 .5-.5h11a.5.5 0 0 1 0 1h-11a.5.5 0 0 1-.5-.5z" />
            </svg>
          </div>
        </div>
        <div className={`overlay ${showOverlay ? "visible" : ""}`}>
          <div className="filter-container">
            <div className="filter-group">
              <label>Price</label>
              <div className="input-group">
                <div className="slider-div">
                  <div>
                    <input
                      ref={minValueRef}
                      type="text"
                      placeholder="Min Price"
                      onChange={handleMinPriceChange}
                    />
                    <input
                      ref={maxValueRef}
                      type="text"
                      placeholder="Max Price"
                      onChange={handleMaxPriceChange}
                    />
                  </div>
                  <Slider
                    className="slider"
                    value={priceRangeTemp}
                    min={priceRangeLimit[0]}
                    step={1}
                    max={priceRangeLimit[1]}
                    onChange={handlePriceChange}
                    onChangeCommitted={handlePriceCommited}
                    valueLabelDisplay="auto"
                    aria-labelledby="non-linear-slider"
                  />
                </div>
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
              <RatingsFilter
                componentId="CarCategorySensor"
                dataField="ratings"
                title="Ratings Filter"
                data={[
                  { start: 5, end: 5, label: "5" },
                  { start: 4, end: 5, label: ">4" },
                  { start: 3, end: 5, label: ">3" },
                  { start: 2, end: 5, label: ">2" },
                  { start: 1, end: 5, label: "All" },
                ]}
                defaultValue={{
                  start: 1,
                  end: 5,
                }}
                URLParams={false}
                onValueChange={handleStarChange}
              />
            </div>
          </div>
        </div>
      </ReactiveBase>
    </div>
  );
}

export default ModuleFilter;
