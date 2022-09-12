class srnet (
  # SRNet Settings
  $base_url     = '',

  # Database Settings
  $server       = '127.0.0.1',
  $dbuser       = 'srnet',
  $dbpassword   = '',
  $dbschema     = 'simplerisk',

  #SAML Settings
  $enable_saml  = false,

) inherits srnet::params {


}
