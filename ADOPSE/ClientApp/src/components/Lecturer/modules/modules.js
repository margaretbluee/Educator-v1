import React, { useEffect, useState } from "react";
import "./modules.scss";
import Module from "./module";
import { useNavigate, useLocation } from "react-router-dom";
import Paginator from "./paginator";

function Modules(props) {
  const navigate = useNavigate();
  const location = useLocation();
  const [activeIndex, setActiveIndex] = useState(1);
  const [limit, setLimit] = useState(100);
  const [offset, setOffset] = useState(0);
  const [modules, setModules] = useState([]);
  const [pages, setPages] = useState(null);
  const [isLoading, setIsLoading] = useState(true);
  const [failedToLoad, setFailedToLoad] = useState(false);

  const handleScroll = (event) => {
    const { scrollLeft, clientWidth, scrollWidth } = event.target;
    const maxScrollLeft = scrollWidth - clientWidth;
    const scrollPercentage = scrollLeft / maxScrollLeft;
    const newIndex = Math.ceil(scrollPercentage * pages);
    setActiveIndex(newIndex);
  };

  // useEffect(() => {
  //   if (pages === null) return;
  //   if (activeIndex > pages) {
  //     setActiveIndex(pages);
  //   }
  // }, [pages, activeIndex]);

  const handlePrevClick = () => {
    setActiveIndex(activeIndex - 1);
  };

  const handleNextClick = () => {
    setActiveIndex(activeIndex + 1);
  };

  useEffect(() => {
    setIsLoading(true);
    let retryCount = 0;
    const maxRetries = 3;

    async function fetchModules() {
      try {
        const response = await Promise.race([
          fetch(`/api/module/stack/${limit}/${offset}/`),
          new Promise((_, reject) =>
            setTimeout(() => reject(new Error("Timeout")), 5000)
          ),
        ]);
        const data = await response.json();
        setModules(data.modules);
        setPages(Math.ceil(data.count / limit));
        setIsLoading(false);
      } catch (error) {
        console.error(error);
        if (retryCount < maxRetries) {
          retryCount++;
          console.log(`Retrying fetch... Attempt ${retryCount}`);
          fetchModules();
        } else {
          console.error(`Failed to fetch modules after ${maxRetries} attempts`);
          setFailedToLoad(true);
        }
      }
    }
    fetchModules();
  }, [limit, offset]);

  useEffect(() => {
    // if (activeIndex <= 0) {
    //   setActiveIndex(0);
    //   return;
    // }
    // setLimit(getModulesLimit());
    setOffset((activeIndex - 1) * limit);
    // navigate(`?page=${activeIndex}`, { replace: true });
  }, [activeIndex, limit]);

  return (
    <div className="lecturer-modules">
      {isLoading ? ( // check if loading is true
        failedToLoad ? (
          <div>Failed to load modules. Please try again later.</div>
        ) : (
          <div>Loading...</div>
        )
      ) : (
        <>
          {modules.length > 0 ? (
            <div className="modules-main">
              <button
                className="nav-button"
                onClick={handlePrevClick}
                disabled={activeIndex === 1}
              >
                Prev
              </button>
              <div className="modules-scroll">
                <div className="modules-cards" onScroll={handleScroll}>
                  {modules.map((module, index) => (
                    <Module
                      key={module.id}
                      id={module.id}
                      index={index}
                      school={module.name}
                      subject={module.name}
                      subject_type={module.moduleTypeName}
                      difficulty={module.difficultyName}
                      rating={module.rating}
                      enrolled={module.price}
                      price={module.price}
                    />
                  ))}
                </div>
              </div>
              <button
                className="nav-button"
                onClick={handleNextClick}
                disabled={activeIndex >= pages}
              >
                Next
              </button>

            </div>
          ) : (
            <div>No modules found for the selected Filters.</div>
          )}
          {pages > 0 && (
            <Paginator
              pageCount={pages}
              setActiveIndex={setActiveIndex}
              activeIndex={activeIndex}
            />
          )}
        </>
      )}
    </div>
  );
}
export default Modules;
