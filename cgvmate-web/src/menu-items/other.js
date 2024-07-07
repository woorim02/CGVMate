// assets
import { IconBell } from '@tabler/icons-react';
import Constants from '../constants'

// constant
const icons = { IconBell };

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
    }
  ]
};

export default other;