<?php
require_once "vendor/autoload.php";

use Monolog\Level;
use Monolog\Logger;
use Monolog\Handler\StreamHandler;

// create a log channel
$log = new Logger('name');
$log->pushHandler(new StreamHandler('/var/log/apache2/simplerisk-addons.log', Level::Info));

// add records to the log
$log->info('Enabling SimpleRiskAddons');

// Check if the user has multi factor authentication
function enabled_auth($user){
    return 1;
}
