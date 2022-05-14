<?php
require_once __DIR__."/vendor/autoload.php";
require_once __DIR__.'/vendor/analog/analog/lib/Analog.php';
require_once __DIR__.'/../../includes/functions.php';

// Include the SimpleRisk language file
require_once(language_file());
// Include Laminas Escaper for HTML Output Encoding
$escaper = new Laminas\Escaper\Escaper('utf-8');


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

function enable_authentication_extra(){
    // Open the database connection
    $db = db_open();

    // Store the file in the database
    $stmt = $db->prepare("UPDATE settings SET VALUE='true' WHERE NAME='custom_auth';");
    $stmt->execute();

    // Close the database connection
    db_close($db);
    return;
}

function disable_authentication_extra(){
        // Open the database connection
        $db = db_open();

        // Store the file in the database
        $stmt = $db->prepare("UPDATE settings SET VALUE='false' WHERE NAME='custom_auth';");
        $stmt->execute();
    
        // Close the database connection
        db_close($db);
    return;
}

