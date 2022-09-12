# This file will do the initial configuration of srnet and start the service

class { 'srnet':
  base_url => $base_url,
}


