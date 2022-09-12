# This file will do the initial configuration of srnet and start the service

class { 'srnet':
  srnet_url => $srnet_url,
}


