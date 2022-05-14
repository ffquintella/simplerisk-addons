<?php
require_once "functions.php";
require_once "../../includes/authenticate.php";

//define("TOOLKIT_PATH", 'vendor/onelogin/php-saml/');
//require_once('vendor/onelogin/php-saml/_toolkit_loader.php');
require_once "settings.php";

if (!isset($_SESSION))
{
    // Session handler is database
    if (USE_DATABASE_FOR_SESSIONS == "true")
    {
        session_set_save_handler('sess_open', 'sess_close', 'sess_read', 'sess_write', 'sess_destroy', 'sess_gc');
    }

    // Start session
    session_set_cookie_params(0, '/', '', isset($_SERVER["HTTPS"]), true);

    sess_gc(1440);
    session_name('SimpleRisk');
    session_start();
}



$needsAuth = empty($_SESSION['samlUsername'] );

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
}else{

    if (is_valid_saml_user($_SESSION['samlUsername'])){
        set_user_permissions($_SESSION['samlUsername'] );
        grant_access();
    }

    Redirect("/reports/index.php", false);
}



