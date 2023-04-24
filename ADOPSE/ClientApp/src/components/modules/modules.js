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
  const [activeIndex, setActiveIndex] = useState(20);

  const [modules, setModules] = useState([]);
  // const eventsToShow = events.slice(0, 0 + 10);

  useEffect(() => {
    setActiveIndex(20);
    fetch(`/api/module/stack/${activeIndex}`)
      .then((response) => response.json())
      .then((data) => setModules(data))
      .catch((error) => console.error(error));
  }, [activeIndex]);

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
