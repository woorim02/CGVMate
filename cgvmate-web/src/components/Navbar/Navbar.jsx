import React from 'react';
import './Navbar.css'

const Navbar = () => {
  return (
    <nav className="navbar">
      <button
        className="btn btn-primary navbar-toggler"
        type="button"
        data-bs-toggle="offcanvas"
        data-bs-target="#offcanvasExample"
        aria-controls="offcanvasExample"
      >
        <svg
          aria-hidden="true"
          height="24"
          width="24"
          viewBox="0 0 16 16"
          version="1.1"
          data-view-component="true"
          className="hamburger"
        >
          <path
            d="M1 2.75A.75.75 0 0 1 1.75 2h12.5a.75.75 0 0 1 0 1.5H1.75A.75.75 0 0 1 1 2.75Zm0 5A.75.75 0 0 1 1.75 7h12.5a.75.75 0 0 1 0 1.5H1.75A.75.75 0 0 1 1 7.75ZM1.75 12h12.5a.75.75 0 0 1 0 1.5H1.75a.75.75 0 0 1 0-1.5Z"
          />
        </svg>
      </button>
      <a className="navbar-brand text-black" href="/">
        <span>CGV 도우미</span>
      </a>
    </nav>
  );
};

export default Navbar;
