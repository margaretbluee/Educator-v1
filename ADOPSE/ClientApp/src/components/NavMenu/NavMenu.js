import React, { useState, useCallback } from "react";
import {
  Collapse,
  Navbar,
  NavbarBrand,
  NavbarToggler,
  NavItem,
  NavLink,
} from "reactstrap";
import { Link, useNavigate, useLocation } from "react-router-dom";
import "./NavMenu.scss";
import { hasJWT, removeJWT } from "../authentication/authentication";
import { useEffect } from "react";
function NavMenu({ navbarRef }) {
  const [collapsed, setCollapsed] = useState(true);
  const toggleNavbar = () => setCollapsed(!collapsed);

  const navigate = useNavigate();

  const location = useLocation();

  const getNavTitle = useCallback(() => {
    switch (location.pathname) {
      case "/":
        return "Dashboard";
      case "/modules":
        return "Modules";
      case "/module":
        return "Module Info";
      case "/lecturer":
        return "Lecturer";
      case "/myLearning":
        return "My Learning";
      case "/googleCalendar":
        return "Google Calendar";
      case "/login":
        return "Login";
      case "/register":
        return "Register";
      default:
        return "";
    }
  }, [location.pathname]);

  const [navTitle, setNavTitle] = useState(getNavTitle());

  useEffect(() => {
    setNavTitle(getNavTitle());
  }, [location.pathname, getNavTitle]);

  return (
    <header ref={navbarRef} className="fixed-top nav-menu">
      <Navbar
        className="navbar-expand-sm bg-white navbar-toggleable-sm ng-white border-bottom box-shadow"
        light
      >
        <div className="d-flex align-items-center">
          <NavbarBrand className="ms-3 navbar-brand" tag={Link} to="/">
            Educator <span className="nav-title">{navTitle}</span>
          </NavbarBrand>
        </div>

        <NavbarToggler onClick={toggleNavbar} className="me-2" />
        <Collapse
          className="d-sm-inline-flex flex-sm-row-reverse me-2"
          isOpen={!collapsed}
          navbar
        >
          <ul className="navbar-nav flex-grow">
            <NavItem>
              <NavLink tag={Link} className="text-dark" to="/">
                Dashboard
              </NavLink>
            </NavItem>
            <NavItem>
              <NavLink tag={Link} className="text-dark" to="/modules">
                Modules
              </NavLink>
            </NavItem>
            <NavItem>
              {hasJWT() ? (
                <NavLink tag={Link} className="text-dark" to="/myLearning">
                  My Learning
                </NavLink>
              ) : (
                <div></div>
              )}
            </NavItem>
            <NavItem>
              {hasJWT() ? (
                <NavLink tag={Link} className="text-dark" to="/googleCalendar">
                  GoogleCalendar
                </NavLink>
              ) : (
                <div></div>
              )}
            </NavItem>
            <NavItem className="login-register">
              {hasJWT() ? (
                <div className="logout-div">
                  <button
                    className="logout-button"
                    onClick={() => {
                      removeJWT();
                      navigate("/login");
                    }}
                  >
                    Logout
                  </button>
                </div>
              ) : (
                <div>
                  <NavLink tag={Link} to="/login" className="text-dark">
                    Login
                  </NavLink>
                  <span className="text-dark"> / </span>
                  <NavLink tag={Link} to="/register" className="text-dark">
                    Register
                  </NavLink>
                </div>
              )}
            </NavItem>
          </ul>
        </Collapse>
      </Navbar>
    </header>
  );
}

NavMenu.displayName = "NavMenu";

export default NavMenu;
