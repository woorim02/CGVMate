// assets
import { IconStack2, IconGift } from '@tabler/icons-react';
import Constants from '../constants'

// constant
const icons = { IconStack2, IconGift };

// ==============================|| DASHBOARD MENU ITEMS ||============================== //

const megabox = {
  id: 'megabox',
  title: '메가박스',
  type: 'group',
  children: [
    {
      id: 'megaboxGiveawayEventList',
      title: '경품 이벤트 현황',
      type: 'item',
      url: Constants.megabox_event_giveaway,
      icon: icons.IconGift,
      breadcrumbs: false
    },
  ]
};

export default megabox;
