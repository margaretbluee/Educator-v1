import React from "react";
import "./footer.css";

const Footer = ({ footerRef = null }) => {
  return (
    <div className="footer" ref={footerRef}>
      <p>Copyright © 2023</p>
    </div>
  );
};

export default Footer;
