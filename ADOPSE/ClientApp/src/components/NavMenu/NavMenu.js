import React, { Component } from "react";
import {
  Collapse,
  Navbar,
  NavbarBrand,
  NavbarToggler,
  NavItem,
  NavLink,
} from "reactstrap";
import { Link } from "react-router-dom";
import "./NavMenu.css";

export default class NavMenu extends Component {
  static displayName = NavMenu.name;

  constructor(props) {
    super(props);

    this.toggleNavbar = this.toggleNavbar.bind(this);
    this.state = {
      collapsed: true,
    };
  }

  toggleNavbar() {
    this.setState({
      collapsed: !this.state.collapsed,
    });
  }

  render() {
    return (
      <header>
        <Navbar
          className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3"
          light
        >
          <NavbarBrand className="ms-3" tag={Link} to="/">
            Educator
          </NavbarBrand>
          <NavbarToggler onClick={this.toggleNavbar} className="me-2" />
          <Collapse
            className="d-sm-inline-flex flex-sm-row-reverse me-2"
            isOpen={!this.state.collapsed}
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
              <NavItem >
                <NavLink tag={Link} className="text-dark" to="/fetch-data">
                  My Learning
                </NavLink>
              </NavItem>
              <NavItem className="login-register">
                <div>
                  <NavLink tag={Link} to="/login" className="text-dark">
                    Login
                  </NavLink>
                  <span> / </span>
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
}
