
package{'zip':
  ensure => present
}

file{'/srnet/SRNET-ConsoleClient/ConsoleClient':
  mode => 755
}

file{'/srnet/SRNET-Server/API':
  mode => 755
}

file{'/srnet/SRNET-GUIClient-lin/GUIClient':
  mode => 755
}


# Cleaning unused packages to decrease image size
exec {'erase installer':
  path  => '/bin:/sbin:/usr/bin:/usr/sbin',
  command => 'rm -rf /tmp/*; rm -rf /opt/staging/*'
} ->

exec {'erase cache':
  path  => '/bin:/sbin:/usr/bin:/usr/sbin',
  command => 'rm -rf /var/cache/*'
} ->
exec {'erase logs':
  path  => '/bin:/sbin:/usr/bin:/usr/sbin',
  command => 'rm -rf /var/log/*'
}


package {'openssh': ensure => absent }
package {'openssh-clients': ensure => absent }
package {'openssh-server': ensure => absent }
package {'rhn-check': ensure => absent }
package {'rhn-client-tools': ensure => absent }
package {'rhn-setup': ensure => absent }
package {'rhnlib': ensure => absent }
