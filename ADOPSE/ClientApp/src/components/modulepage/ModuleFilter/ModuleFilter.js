import React, { useEffect, useRef, useState } from "react";
import "./ModuleFilter.scss";
import Slider from "@mui/material/Slider";
import { ReactiveBase, RatingsFilter } from "@appbaseio/reactivesearch";

function ModuleFilter(props) {
  const minValueRef = useRef(null);
  const maxValueRef = useRef(null);

  const [showOverlay, setShowOverlay] = useState(true);

  const priceRangeLimit = props;

  const { priceRange, setPriceRange } = props;
  const { type, setType } = props;
  const { difficulty, setDifficulty } = props;
  const { setStars } = props;

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
  
 

  const handleTypeChange = (event) => {
    const selectedType = parseInt(event.target.value, 10);
    setType(selectedType);
  };
  

  const handleDifficultyChange = (event) => {
    const selectedDifficulty = parseInt(event.target.value, 10);
    setDifficulty(selectedDifficulty);
  };

  const handleStarChange = (selectedRatings) => {
    if (!selectedRatings) return;
    setStars(selectedRatings);
  };

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
              <div className="radio-buttons">
                <label>
                  <input
                    type="radio"
                    value="0"
                    name="type"
                    checked={type === 0}
                    onChange={handleTypeChange}
                  />
                  <span>ALL</span>
                </label>

                <label>
                  <input
                    type="radio"
                    value="1"
                    name="type"
                    checked={type === 1}
                    onChange={handleTypeChange}
                  />
                  <span>Εργαστήριο</span>
                </label>
                <label>
                  <input
                    type="radio"
                    value="2"
                    name="type"
                    checked={type === 2}
                    onChange={handleTypeChange}
                  />
                  <span>Θεωρία</span>
                </label>
                <label>
                  <input
                    type="radio"
                    value="3"
                    name="type"
                    checked={type === 3}
                    onChange={handleTypeChange}
                  />
                  <span>Mix</span>
                </label>
              </div>
            </div>
            <div className="filter-group">
              <label>Difficulty</label>
              <div className="radio-buttons">
                <label>
                  <input
                    type="radio"
                    value="0"
                    name="Difficulty"
                    checked={difficulty === 0}
                    onChange={handleDifficultyChange}
                  />
                  <span>ALL</span>
                </label>

                <label>
                  <input
                    type="radio"
                    value="1"
                    name="Difficulty"
                    checked={difficulty === 1}
                    onChange={handleDifficultyChange}
                  />
                  <span>Easy</span>
                </label>
                <label>
                  <input
                    type="radio"
                    value="2"
                    name="Difficulty"
                    checked={difficulty === 2}
                    onChange={handleDifficultyChange}
                  />
                  <span>Medium</span>
                </label>
                <label>
                  <input
                    type="radio"
                    value="3"
                    name="Difficulty"
                    checked={difficulty === 3}
                    onChange={handleDifficultyChange}
                  />
                  <span>Hard</span>
                </label>
              </div>
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
                // defaultValue={{
                //   start: 1,
                //   end: 5,
                // }}
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
