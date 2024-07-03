import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min';
import './App.css';
import Navbar from './components/Navbar/Navbar';
import Sidebar from './components/Sidebar/Sidebar';
import Offcanvas from './components/Offcanvas/Offcanvas';

function App() {
  return (
    <div className="App">
      <Offcanvas />
      <div className="navbar-container">
        <Navbar />
        <div className="cgv-gradient"></div>
      </div>
      <div className="body-container">
        <div className="sidebar-container shadow-sm">
          <Sidebar className="sidebar" isOpen={true} />
        </div>
        <main>
        </main>
      </div>
    </div>
  );
}

export default App;
