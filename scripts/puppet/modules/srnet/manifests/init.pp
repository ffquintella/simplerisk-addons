class srnet (
  # SRNet Settings
  $base_url     = '',

  # Database Settings
  $dbserver     = '127.0.0.1',
  $dbuser       = 'simplerisk',
  $dport        = '3306',
  $dbpassword   = '',
  $dbschema     = 'simplerisk',

  #SAML Settings
  $enable_saml  = false,

) inherits srnet::params {

# UPDATE THIS EVERY RELEASE
$srnetmaxdbver = 2

$dbpwd = String(file('/passwords/pass_simplerisk.txt'), "%t")
$srnetdbver = String(file('/configurations/srnetdb.version'), "%t")

$n_srvdbver = 0 + $srnetdbver

Integer[$n_srvdbver, $srnetmaxdbver].each |$x| {
  notice("updating DB version ${x}")
}


if ( $dbpassword == '') {
  $dbpw_fin = $dbpwd
}else{
  $dbpw_fin = $dbpassword
}

#notice($dbpwd)

file{'/srnet/SRNET-ConsoleClient/appsettings.json': 
  ensure  => file,
  content => epp('srnet/consoleClient/appsettings.epp', {
    'db_server'   => $dbserver,
    'db_user'     => $dbuser,
    'db_port'     => $dport ,
    'db_password' => $dbpw_fin ,
    'db_schema'   => $dbschema
  })
}



}
