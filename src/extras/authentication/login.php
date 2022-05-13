<?php
require_once "functions.php";

//define("TOOLKIT_PATH", 'vendor/onelogin/php-saml/');
//require_once('vendor/onelogin/php-saml/_toolkit_loader.php');
require_once "settings.php";

//$auth = new \OneLogin\Saml2\Auth($settings);

session_start();
$needsAuth = empty($_SESSION['samlUserdata']);

if ($needsAuth) {
    // put SAML settings into an array to avoid placing files in the
    // composer vendor/ directories 
    $samlsettings = array(/*...config goes here...*/);
    
    $auth = new \OneLogin\Saml2\Auth($settings);

    if (!empty($_REQUEST['SAMLResponse']) && !empty($_REQUEST['RelayState'])) {
        $auth->processResponse(null);
        $errors = $auth->getErrors();
        if (empty($errors)) {
            // user has authenticated successfully
            $needsAuth = false;
            $_SESSION['samlUserdata'] = $auth->getAttributes();
        }
    }

    if ($needsAuth) {
        $auth->login();
    }
}



//var_dump($auth);