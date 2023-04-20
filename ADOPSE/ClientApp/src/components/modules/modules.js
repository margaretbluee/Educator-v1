import React from "react";
import "./modules.scss";
import Module from "./module";

const events = [
  {
    school: "ΜΗΧ. ΠΛΗΡΟΦΟΡΙΚΗΣ & ΗΛΕΚΤΡΟΝΙΚΩΝ ΣΥΣΤΗΜΑΤΩΝ",
    subject: "Γλώσσες & Τεχν. Ιστού",
    subject_type: "ΕΡΓΑΣΤΗΡΙΟ & ΘΕΩΡΙΑ",
    difficulty: "MEDIUM",
    rating: "5(10 ratings)",
    enrolled: "500",
  },
  {
    school: "ΜΗΧ. ΠΛΗΡΟΦΟΡΙΚΗΣ & ΗΛΕΚΤΡΟΝΙΚΩΝ ΣΥΣΤΗΜΑΤΩΝ",
    subject: "Ανάπτυξη Πληρ/κών Συστημάτων",
    subject_type: "ΘΕΩΡΙΑ",
    difficulty: "HARD",
    rating: "5(10 ratings)",
    enrolled: "500",
  },
  {
    school: "ΜΗΧ. ΠΛΗΡΟΦΟΡΙΚΗΣ & ΗΛΕΚΤΡΟΝΙΚΩΝ ΣΥΣΤΗΜΑΤΩΝ",
    subject: "Ανάκτηση Πληροφοριών",
    subject_type: "ΘΕΩΡΙΑ",
    difficulty: "HARD",
    rating: "5(10 ratings)",
    enrolled: "500",
  },
  {
    school: "ΜΗΧ. ΠΛΗΡΟΦΟΡΙΚΗΣ & ΗΛΕΚΤΡΟΝΙΚΩΝ ΣΥΣΤΗΜΑΤΩΝ",
    subject: "Μηχανές Αναζήτησης",
    subject_type: "ΘΕΩΡΙΑ",
    difficulty: "HARD",
    rating: "5(10 ratings)",
    enrolled: "500",
  },
  {
    school: "ΜΗΧ. ΠΛΗΡΟΦΟΡΙΚΗΣ & ΗΛΕΚΤΡΟΝΙΚΩΝ ΣΥΣΤΗΜΑΤΩΝ",
    subject: "Ευφυή Συστήματα",
    subject_type: "ΘΕΩΡΙΑ",
    difficulty: "HARD",
    rating: "5(10 ratings)",
    enrolled: "311",
  },
  // Add more events as needed
];

function Modules(props) {
  // const [activeIndex, setActiveIndex] = useState(0);
  const eventsToShow = events.slice(0, 0 + 10);

  return (
    <div className="modules">
      {eventsToShow.map((event, index) => (
        <Module
          key={index}
          school={event.school}
          subject={event.subject}
          subject_type={event.subject_type}
          difficulty={event.difficulty}
          rating={event.rating}
          enrolled={event.enrolled}
        />
      ))}
    </div>
  );
}

export default Modules;
