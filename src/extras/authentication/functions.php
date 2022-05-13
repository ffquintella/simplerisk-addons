<?php
require_once __DIR__."/vendor/autoload.php";
require_once __DIR__.'/vendor/analog/analog/lib/Analog.php';

use Analog\Analog;


Analog::handler (\Analog\Handler\Syslog::init ('SR-ADDONS', 'www-data'));

Analog::log ('Error message', Analog::WARNING);

// Check if the user has multi factor authentication
function enabled_auth($user){
    return 1;
}
