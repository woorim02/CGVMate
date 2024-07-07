// assets
import { IconStack2, IconBell, IconGift } from '@tabler/icons-react';
import Constants from '../constants'

// constant
const icons = { IconStack2, IconBell, IconGift };

// ==============================|| DASHBOARD MENU ITEMS ||============================== //

const dashboard = {
  id: 'cgvDashboard',
  title: 'CGV',
  type: 'group',
  children: [
    {
      id: 'cgvEventList',
      title: '이벤트 목록',
      type: 'item',
      url: Constants.event_,
      icon: icons.IconStack2,
      breadcrumbs: false
    },
    {
      id: 'cgvGiveawayEventList',
      title: '경품 이벤트 현황',
      type: 'item',
      url: Constants.event_giveaway,
      icon: icons.IconGift,
      breadcrumbs: false
    },
    {
      id: 'speed',
      title: '스피드쿠폰 조회',
      type: 'item',
      url: Constants.event_cupon_speed,
      icon: icons.IconBell,
      breadcrumbs: false
    },
    {
      id: 'surprise',
      title: '서프라이즈 쿠폰 조회',
      type: 'item',
      url: Constants.event_cupon_surprise,
      icon: icons.IconBell,
      breadcrumbs: false
    },
  ]
};

export default dashboard;
