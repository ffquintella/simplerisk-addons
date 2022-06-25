#!/bin/bash

run_sql_command() {
	# $1: Password
	# $2: Command
	MYSQL_PWD=$1 mysql -uroot -e "$2"
}

run_sql_dump_command() {
	# $1: Password
	# $2: Command
	MYSQL_PWD=$1 mysqldump -uroot simplerisk "$2"
}


password=$(cat /passwords/pass_mysql_root.txt)

run_sql_dump_command "$password" "settings" > /backups/restore/local_settings.sql

run_sql_command "$password" "use simplerisk; \. /backups/restore/$1" 

run_sql_command "$password" "use simplerisk; \. /backups/restore/local_settings.sql" 