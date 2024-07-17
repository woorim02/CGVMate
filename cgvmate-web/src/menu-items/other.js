// assets
import { IconBell, IconChalkboard } from '@tabler/icons-react';
import Constants from '../constants'

// constant
const icons = { IconBell, IconChalkboard };

// ==============================|| SAMPLE PAGE & DOCUMENTATION MENU ITEMS ||============================== //

const other = {
  id:'other',
  title:'Other',
  type: 'group',
  children: [
    {
      id:'promo',
      title: '프로모션 쿠폰 목록',
      type: 'item',
      url: Constants.promo_list,
      icon: icons.IconBell,
      breadcrumbs: false
    },
    {
      id:'board',
      title: '커뮤니티',
      type: 'item',
      url: '/board',
      icon: icons.IconChalkboard,
      breadcrumbs: false
    }
  ]
};

export default other;