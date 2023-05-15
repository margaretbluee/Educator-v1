import React, { useState } from "react";
import {
  Collapse,
  Navbar,
  NavbarBrand,
  NavbarToggler,
  NavItem,
  NavLink,
} from "reactstrap";
import { Link, useNavigate } from "react-router-dom";
import "./NavMenu.scss";
import { hasJWT, removeJWT } from "../authentication/authentication";
function NavMenu({ navbarRef }) {
  const [collapsed, setCollapsed] = useState(true);
  const toggleNavbar = () => setCollapsed(!collapsed);

  const navigate = useNavigate();

  return (
    <header ref={navbarRef} className="fixed-top nav-menu">
      <Navbar
        className="navbar-expand-sm bg-white navbar-toggleable-sm ng-white border-bottom box-shadow"
        light
      >
        <NavbarBrand className="ms-3" tag={Link} to="/">
          Educator
        </NavbarBrand>
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
              <NavLink tag={Link} className="text-dark" to="/fetch-data">
                My Learning
              </NavLink>
            </NavItem>
            <NavItem>
              {
                hasJWT() ? (
                    <NavLink tag={Link} className="text-dark" to="/googleCalendar">
                      GoogleCalendar
                    </NavLink>
                ) : (
                    <div></div>
                )
              }
              
            </NavItem>
            <NavItem className="login-register">
              {hasJWT() ? (
                <div>
                  <button
                    className="text-dark"
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
