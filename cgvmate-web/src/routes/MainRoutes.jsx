import { lazy } from 'react';

// project imports
import MainLayout from 'layout/MainLayout';
import Home from 'views/Home';
import CgvEventPage from 'views/cgv/event/EventPage';
import CgvGiveawayPage from 'views/cgv/event/giveaway/GiveawayPage';
import CgvGiveawayDetailPage from 'views/cgv/event/giveaway/detail/DetailPage';
import SpeedCuponPage from 'views/cgv/event/cupon/SpeedCuponPage';
import SurpriseCuponPage from 'views/cgv/event/cupon/SurpriseCuponPage';
import LotteEventPage from 'views/lotte/event/EventPage';
import LotteGiveawayPage from 'views/lotte/event/giveaway/GiveawayPage';
import LotteGiveawayDetailPage from 'views/lotte/event/giveaway/detail/DetailPage';
import MegaboxGiveawayPage from 'views/megabox/event/giveaway/GiveawayPage';
import MegaboxGiveawayDetailPage from 'views/megabox/event/giveaway/detail/DetailPage';
import PromoListPage from 'views/promo/ListPage';



// ==============================|| MAIN ROUTING ||============================== //

const MainRoutes = {
  path: '/',
  element: <MainLayout />,
  children: [
    {
      path: '/',
      element: <Home />
    },
    {
      path: 'cgv/event/cupon/speed',
      element: <SpeedCuponPage/>
    },
    {
      path: 'cgv/event/cupon/surprise',
      element: <SurpriseCuponPage/>
    },
    {
      path: 'cgv/event',
      element: <CgvEventPage/>
    },
    {
      path: 'cgv/event/giveaway',
      element: <CgvGiveawayPage/>,
    },
    {
      path: 'cgv/event/giveaway/detail',
      element: <CgvGiveawayDetailPage/>,
    },
    {
      path: 'lotte/event',
      element: <LotteEventPage/>
    },
    {
      path: 'lotte/event/giveaway',
      element: <LotteGiveawayPage/>,
    },
    {
      path: 'lotte/event/giveaway/detail',
      element: <LotteGiveawayDetailPage/>,
    },
    {
      path: 'megabox/event/giveaway',
      element: <MegaboxGiveawayPage/>,
    },
    {
      path: 'megabox/event/giveaway/detail',
      element: <MegaboxGiveawayDetailPage/>,
    },
    {
      path: 'promo/list',
      element: <PromoListPage/>
    }
  ]
};

export default MainRoutes;
