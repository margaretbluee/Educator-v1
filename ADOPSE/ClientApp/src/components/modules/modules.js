import React, { useEffect, useState } from "react";
import "./modules.scss";
import Module from "./module";

// const events = [
//   {
//     school: "ΜΗΧ. ΠΛΗΡΟΦΟΡΙΚΗΣ & ΗΛΕΚΤΡΟΝΙΚΩΝ ΣΥΣΤΗΜΑΤΩΝ",
//     subject: "Γλώσσες & Τεχν. Ιστού",
//     subject_type: "ΕΡΓΑΣΤΗΡΙΟ & ΘΕΩΡΙΑ",
//     difficulty: "MEDIUM",
//     rating: "5(10 ratings)",
//     enrolled: "500",
//   },
//   {
//     school: "ΜΗΧ. ΠΛΗΡΟΦΟΡΙΚΗΣ & ΗΛΕΚΤΡΟΝΙΚΩΝ ΣΥΣΤΗΜΑΤΩΝ",
//     subject: "Ανάπτυξη Πληρ/κών Συστημάτων",
//     subject_type: "ΘΕΩΡΙΑ",
//     difficulty: "HARD",
//     rating: "5(10 ratings)",
//     enrolled: "500",
//   },
//   {
//     school: "ΜΗΧ. ΠΛΗΡΟΦΟΡΙΚΗΣ & ΗΛΕΚΤΡΟΝΙΚΩΝ ΣΥΣΤΗΜΑΤΩΝ",
//     subject: "Ανάκτηση Πληροφοριών",
//     subject_type: "ΘΕΩΡΙΑ",
//     difficulty: "HARD",
//     rating: "5(10 ratings)",
//     enrolled: "500",
//   },
//   {
//     school: "ΜΗΧ. ΠΛΗΡΟΦΟΡΙΚΗΣ & ΗΛΕΚΤΡΟΝΙΚΩΝ ΣΥΣΤΗΜΑΤΩΝ",
//     subject: "Μηχανές Αναζήτησης",
//     subject_type: "ΘΕΩΡΙΑ",
//     difficulty: "HARD",
//     rating: "5(10 ratings)",
//     enrolled: "500",
//   },
//   {
//     school: "ΜΗΧ. ΠΛΗΡΟΦΟΡΙΚΗΣ & ΗΛΕΚΤΡΟΝΙΚΩΝ ΣΥΣΤΗΜΑΤΩΝ",
//     subject: "Ευφυή Συστήματα",
//     subject_type: "ΘΕΩΡΙΑ",
//     difficulty: "HARD",
//     rating: "5(10 ratings)",
//     enrolled: "311",
//   },
//   // Add more events as needed
// ];

function Modules(props) {
  const [activeIndex, setActiveIndex] = useState(1);
  const [limit, setLimit] = useState(10);
  const [offset, setOffset] = useState(0);
  const [modules, setModules] = useState([]);
  const [pages, setPages] = useState(null);

  useEffect(() => {
    setActiveIndex(1);
    setLimit(10);
    fetch(`/api/module/stack/${limit}/${offset}`)
      .then((response) => response.json())
      .then((data) => {
        setModules(data.modules);
        setPages(Math.ceil(data.count / limit));
      })
      .catch((error) => console.error(error));
  }, [offset, limit]);

  useEffect(() => {
    setOffset((activeIndex - 1) * limit);
    pages !== null && console.log(pages);
  }, [activeIndex, limit, pages]);

  return (
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
  );
}

export default Modules;
