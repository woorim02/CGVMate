// assets
import { IconBrandGithub, IconBrandMessenger } from '@tabler/icons-react';

// constant
const icons = { IconBrandGithub, IconBrandMessenger };

// ==============================|| SAMPLE PAGE & DOCUMENTATION MENU ITEMS ||============================== //

const contact = {
  id:'contact',
  title:'Contact',
  type: 'group',
  children: [
    {
      id:'githhub',
      title: 'Github',
      type: 'item',
      url: "https://github.com/woorim02/CGVMate",
      icon: icons.IconBrandGithub,
      breadcrumbs: false
    },
    {
      id:'kakao',
      title: 'Kakao',
      type: 'item',
      url: "https://open.kakao.com/o/sSS6JJsg",
      icon: icons.IconBrandMessenger,
      breadcrumbs: false
    }
  ]
};

export default contact;