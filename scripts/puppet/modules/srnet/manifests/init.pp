class srnet (
  # SRNet Settings
  $base_url     = '',

  # Database Settings
  $dbserver     = '127.0.0.1',
  $dbuser       = 'srnet',
  $dport        = '3306',
  $dbpassword   = '',
  $dbschema     = 'simplerisk',

  #SAML Settings
  $enable_saml  = false,

) inherits srnet::params {

$dbpwd = file('/passwords/pass_simplerisk.txt')

if ( $dbpassword == '') {
  $dbpassword = $dbpwd
}

#notice($dbpwd)

file{'/srnet/SRNET-ConsoleClient/appsettings.json': 
  ensure  => file,
  content => epp('srnet/consoleClient/appsettings.epp', {
    'db_server'   => $dbserver,
    'db_user'     => $dbuser,
    'db_port'     => $dport ,
    'db_password' => $dbpassword ,
    'db_schema'   => $dbschema
  })
}



}
