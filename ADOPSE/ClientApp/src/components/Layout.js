import React, { useRef, useState, useLayoutEffect } from "react";
import NavMenu from "./NavMenu";
import Footer from "./footer";

const Layout = (props) => {
  const headerRef = useRef(null);
  const footerRef = useRef(null);

  const [headerHeight, setHeaderHeight] = useState(0);
  const [footerHeight, setFooterHeight] = useState(0);

  const handleSize = () => {
    setHeaderHeight(headerRef?.current?.clientHeight ?? 0);
    setFooterHeight(footerRef?.current?.clientHeight ?? 0);
  };

  useLayoutEffect(() => {
    handleSize();
    window.addEventListener("resize", handleSize);
  }, [headerRef, footerRef]);

  const divStyle = {
    position: "relative",
    top: `${headerHeight ?? 0}px`,
  };

  const containerStyle = {
    minHeight: `calc(100vh - ${headerHeight ?? 0}px - ${footerHeight ?? 0}px)`,
    paddingTop: "20px",
    paddingLeft: "20px",
    paddingRight: "20px",
    height: "100%",
    width: "100%",
  };

  return (
    <div>
      <NavMenu navbarRef={headerRef} />
      <div style={divStyle}>
        <div style={containerStyle} tag="main">
          {props.children}
        </div>
        <Footer footerRef={footerRef} />
      </div>
    </div>
  );
};

Layout.displayName = "Layout";

export default Layout;
