const Constants = {
  // Routes
  root: "/",

  admin_login: "/admin/login",
  admin_dashboard: "/admin/dashboard",

  promo_list: '/promo/list',

  event_: "/cgv/event",
  event_cupon_speed: "/cgv/event/cupon/speed",
  event_cupon_surprise: "/cgv/event/cupon/surprise",
  event_giveaway: "/cgv/event/giveaway",
  event_giveaway_detail: "/cgv/event/giveaway/detail",
  event_giveaway_autosignup: "/cgv/event/giveaway/autosignup",

  lotte_event: "/lotte/event",
  lotte_event_giveaway: "/lotte/event/giveaway",
  lotte_event_giveaway_detail: "/lotte/event/giveaway/detail",

  megabox_event_giveaway: "/megabox/event/giveaway",
  megabox_event_giveaway_detail: "/megabox/event/giveaway/detail",

  // API Host
   API_HOST: "https://api.cgvmate.com"
  // API_HOST: "http://localhost:8080"
};

// Export the constants for use in other modules
export default Constants;
