import React from 'react';
import Sidebar from '../Sidebar/Sidebar';
import './Offcanvas.css'

const Offcanvas = () => {
  return (
    <div
      className="offcanvas offcanvas-start"
      id="offcanvasExample"
    >
      <div className="offcanvas-header cgv-gradient">
        <div className="offcanvas-title" id="offcanvasExampleLabel">CGV 도우미</div>
        <button
          type="button"
          className="btn-close text-reset"
          data-bs-dismiss="offcanvas"
          aria-label="Close"
        ></button>
      </div>
      <div className="offcanvas-body">
        <Sidebar IsOpen={true} />
      </div>
    </div>
  );
};

export default Offcanvas;
