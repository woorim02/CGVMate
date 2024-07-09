import { lazy } from 'react';


// project imports
import AdminMainLayout from 'layout/AdminMainLayout';
import Home from 'views/admin/Home';
import AuthLogin from 'views/admin/Login';



// ==============================|| MAIN ROUTING ||============================== //

const AdminRoutes = {
  path: '/admin',
  element: <AdminMainLayout />,
  children: [
    {
      path: '',
      element: <Home />
    },
    {
      path: 'login',
      element: <AuthLogin />
    },
  ]
};

export default AdminRoutes;
