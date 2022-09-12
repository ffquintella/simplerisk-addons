





# Starting jira
exec {'Starting Jira':
  path  => '/bin:/sbin:/usr/bin:/usr/sbin',
  command => "${real_appdir}/bin/start-jira.sh"
}
