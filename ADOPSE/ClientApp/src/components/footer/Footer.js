import React from "react";
import "./footer.scss";

const Footer = ({ footerRef = null }) => {
  const currentYear = new Date().getFullYear();

  return (
    <div className="footer" ref={footerRef}>
      <p>Copyright © {currentYear}</p>
    </div>
  );
};

export default Footer;
