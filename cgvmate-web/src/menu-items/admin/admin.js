// assets
import { IconStack2, IconDashboard  } from '@tabler/icons-react';
import Constants from '../../constants'

// constant
const icons = { IconStack2, IconDashboard };

// ==============================|| DASHBOARD MENU ITEMS ||============================== //

const dashboard = {
  id: 'admin',
  title: '대시보드',
  type: 'group',
  children: [
    {
      id: 'adminDashboard',
      title: '현황',
      type: 'item',
      url: Constants.admin_dashboard,
      icon: icons.IconDashboard,
      breadcrumbs: false
    },
  ]
};

export default dashboard;
