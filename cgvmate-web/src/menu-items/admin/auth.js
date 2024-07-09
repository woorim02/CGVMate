// assets
import { IconStack2, IconGift } from '@tabler/icons-react';
import Constants from '../../constants'

// constant
const icons = { IconStack2, IconGift };

// ==============================|| DASHBOARD MENU ITEMS ||============================== //

const auth = {
  id: 'adminAuth',
  title: 'Auth',
  type: 'group',
  children: [
    {
      id: 'authLogin',
      title: '로그인',
      type: 'item',
      url: Constants.admin_login,
      icon: icons.IconGift,
      breadcrumbs: false
    },
  ]
};

export default auth;
