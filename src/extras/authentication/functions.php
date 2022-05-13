<?php
require_once __DIR__."/vendor/autoload.php";
require_once __DIR__.'/vendor/analog/analog/lib/Analog.php';

use Analog\Analog;

Analog::handler (Analog\Handler\Stderr::init ());

Analog::log ('TESTE LOG');

// Check if the user has multi factor authentication
function enabled_auth($user){
    return 1;
}
