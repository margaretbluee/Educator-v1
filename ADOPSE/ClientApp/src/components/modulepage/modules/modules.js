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

  useEffect(() => {
    if (pages === null) return;
    if (activeIndex > pages) {
      setActiveIndex(pages);
    }
  }, [pages, activeIndex]);

  useEffect(() => {
    async function fetchModules() {
      try {
        const response = await fetch(`/api/module/stack/${limit}/${offset}`);
        const data = await response.json();
        setModules(data.modules);
        setPages(Math.ceil(data.count / limit));
      } catch (error) {
        console.error(error);
      }
    }
    setLimit(10);
    setOffset((activeIndex - 1) * limit);
    navigate(`?page=${activeIndex}`, { replace: true });
    fetchModules();
  }, [activeIndex, limit, offset, navigate]);

  return (
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
  );
}
export default Modules;
