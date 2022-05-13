<?php
require_once "functions.php";
require_once "../../includes/authenticate.php";

//define("TOOLKIT_PATH", 'vendor/onelogin/php-saml/');
//require_once('vendor/onelogin/php-saml/_toolkit_loader.php');
require_once "settings.php";


$needsAuth = empty($_SESSION['samlUserdata']);

if ($needsAuth) {
    
    $auth = new \OneLogin\Saml2\Auth($settings);

    if (!empty($_REQUEST['SAMLResponse']) && !empty($_REQUEST['RelayState'])) {
        $auth->processResponse(null);
        $errors = $auth->getErrors();
        if (empty($errors)) {
            // user has authenticated successfully
            $needsAuth = false;
            $_SESSION['samlUserdata'] = $auth->getAttributes();

            $userName = $auth->getNameId();
            $_SESSION['samlUsername'] = $userName;
            
            if (is_valid_saml_user($userName)){
                set_user_permissions($userName);
                grant_access();
            }else{
                Redirect("/index.php", false);
            }



        }
    }

    if ($needsAuth) {
        $auth->login();
    }
}

Redirect("/reports/index.php", false);

//var_dump($auth);