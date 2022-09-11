

exec {'Coping Configs':
  path    => '/bin:/sbin:/usr/bin:/usr/sbin',
  command => "echo \"Coping configs ...\"; cp -r /opt/jira-config/* ${real_appdir}/conf; chown -R jira:jira ${real_appdir}/conf ",
  creates => "${real_appdir}/conf/server.xml"
} ->
# Starting jira
exec {'Starting Jira':
  path  => '/bin:/sbin:/usr/bin:/usr/sbin',
  command => "${real_appdir}/bin/start-jira.sh"
}
