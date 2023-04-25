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

  useEffect(() => {
    if (pages === null) return;
    if (activeIndex > pages) {
      setActiveIndex(pages);
    }
  }, [pages, activeIndex]);

  useEffect(() => {
    setIsLoading(true);
    async function fetchModules() {
      try {
        const response = await fetch(`/api/module/stack/${limit}/${offset}`);
        const data = await response.json();
        setModules(data.modules);
        setPages(Math.ceil(data.count / limit));
      } catch (error) {
        console.error(error);
      } finally {
        setIsLoading(false); // set loading to false when fetch is complete
      }
    }
    fetchModules();
    // const timeoutId = setTimeout(() => {
    //   fetchModules();
    // }, 2000);

    // // Clear the timeout if the component unmounts
    // return () => {
    //   clearTimeout(timeoutId);
    // };
  }, [limit, offset]);

  useEffect(() => {
    setLimit(10);
    setOffset((activeIndex - 1) * limit);
    navigate(`?page=${activeIndex}`, { replace: true });
  }, [activeIndex, limit, navigate]);

  return (
    <>
      {isLoading ? ( // check if loading is true
        <div>Loading...</div>
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
        </>
      )}
      {pages > 0 && (
        <Paginator
          pageCount={pages}
          setActiveIndex={setActiveIndex}
          activeIndex={activeIndex}
        />
      )}
    </>
  );
}
export default Modules;
