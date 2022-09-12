

file{'/srnet/SRNET-ConsoleClient/ConsoleClient':
  mode => '755'
}

file{'/srnet/SRNET-Server/API':
  mode => '755'
}

file{'/srnet/SRNET-GUIClient-lin/GUIClient':
  mode => '755'
}

exec {'erase cache':
  path  => '/bin:/sbin:/usr/bin:/usr/sbin',
  command => 'rm -rf /var/cache/*'
} ->
exec {'erase logs':
  path  => '/bin:/sbin:/usr/bin:/usr/sbin',
  command => 'rm -rf /var/log/*'
}


