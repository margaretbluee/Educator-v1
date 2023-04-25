import React, { useEffect, useState } from "react";
import "./modules.scss";
import Module from "./module";
import Paginator from "./paginator";
import { useNavigate, useLocation } from "react-router-dom";

// const events = [
//   {
//     school: "ΜΗΧ. ΠΛΗΡΟΦΟΡΙΚΗΣ & ΗΛΕΚΤΡΟΝΙΚΩΝ ΣΥΣΤΗΜΑΤΩΝ",
//     subject: "Γλώσσες & Τεχν. Ιστού",
//     subject_type: "ΕΡΓΑΣΤΗΡΙΟ & ΘΕΩΡΙΑ",
//     difficulty: "MEDIUM",
//     rating: "5(10 ratings)",
//     enrolled: "500",
//   }
//   // Add more events as needed
// ];

function Modules(props) {
  const navigate = useNavigate();
  const location = useLocation();
  const [activeIndex, setActiveIndex] = useState(
    parseInt(new URLSearchParams(location.search).get("page")) || 1
  );
  const [limit, setLimit] = useState(10);
  const [offset, setOffset] = useState(0);
  const [modules, setModules] = useState([]);
  const [pages, setPages] = useState(null);
  const [isLoading, setIsLoading] = useState(true);
  const [failedToLoad, setFailedToLoad] = useState(false);

  useEffect(() => {
    if (pages === null) return;
    if (activeIndex > pages) {
      setActiveIndex(pages);
    }
  }, [pages, activeIndex]);

  useEffect(() => {
    setIsLoading(true);
    let retryCount = 0;
    const maxRetries = 3;

    async function fetchModules() {
      try {
        const response = await Promise.race([
          fetch(`/api/module/stack/${limit}/${offset}`),
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
    setLimit(10);
    setOffset((activeIndex - 1) * limit);
    navigate(`?page=${activeIndex}`, { replace: true });
  }, [activeIndex, limit, navigate]);

  return (
    <>
      {isLoading ? ( // check if loading is true
        failedToLoad ? (
          <div>Failed to load modules. Please try again later.</div>
        ) : (
          <div>Loading...</div>
        )
      ) : (
        <>
          <div className="modules">
            {modules.map((module, index) => (
              <Module
                key={module.id}
                index={index}
                school={module.name}
                subject={module.name}
                subject_type={module.moduleType}
                difficulty={module.difficulty}
                rating={module.rating}
                enrolled={module.price}
              />
            ))}
          </div>
          {pages > 0 && (
            <Paginator
              pageCount={pages}
              setActiveIndex={setActiveIndex}
              activeIndex={activeIndex}
            />
          )}
        </>
      )}
    </>
  );
}
export default Modules;
