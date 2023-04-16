import React, { useState } from "react";
import {
  Collapse,
  Navbar,
  NavbarBrand,
  NavbarToggler,
  NavItem,
  NavLink,
} from "reactstrap";
import { Link } from "react-router-dom";
import "./NavMenu.scss";

function NavMenu({ navbarRef }) {
  const [collapsed, setCollapsed] = useState(true);
  const toggleNavbar = () => setCollapsed(!collapsed);

  return (
    <header ref={navbarRef} className="fixed-top navMenu">
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
              <NavLink tag={Link} className="text-dark" to="/counter">
                Modules
              </NavLink>
            </NavItem>
            <NavItem>
              <NavLink tag={Link} className="text-dark" to="/fetch-data">
                My Learning
              </NavLink>
            </NavItem>
            <NavItem className="login-register">
              <div>
                <NavLink tag={Link} to="/login" className="text-dark">
                  Login
                </NavLink>
                <span className="text-dark"> / </span>
                <NavLink tag={Link} to="/register" className="text-dark">
                  Register
                </NavLink>
              </div>
            </NavItem>
          </ul>
        </Collapse>
      </Navbar>
    </header>
  );
}

NavMenu.displayName = "NavMenu";

export default NavMenu;
