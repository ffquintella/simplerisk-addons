<?php
require_once __DIR__."/vendor/autoload.php";
require_once __DIR__.'/vendor/analog/analog/lib/Analog.php';

use Analog\Analog;


Analog::handler (\Analog\Handler\Syslog::init ('SR-ADDONS', 'user'));

Analog::log ('SimpleRisk Addons Activated', Analog::INFO);

// Check if the user has multi factor authentication
function enabled_auth($user){
    return 1;
}
