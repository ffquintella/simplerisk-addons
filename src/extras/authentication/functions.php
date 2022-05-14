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

// We do not implement MFA here it's on charge of the SAML
function multi_factor_authentication_options($type)
{
    return;
}

function is_valid_saml_user($user){
    $needsAuth = empty($_SESSION['samlUsername']);
    if ($needsAuth) {
        Redirect('extras/authentication/login.php', false);
    }

    if($user != $_SESSION['samlUsername'] ) return false;

    $type = get_user_type($user);
    if($type == "saml") return true;

    return false;
}

function Redirect($url, $permanent = false)
{
    header('Location: ' . $url, true, $permanent ? 301 : 302);

    exit();
}

