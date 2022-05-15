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

    if(empty(get_setting('custom_auth_sp_entity_id'))) {
        echo "SAML not configured";
        return;
    }

    //Updating the variables we need
    $settings['sp']['entityId'] = get_setting('custom_auth_sp_entity_id');
    $settings['sp']['assertionConsumerService']['url'] = get_setting('custom_auth_sp_assertion_consumer_service_url');
    $settings['sp']['singleLogoutService']['url'] = get_setting('custom_auth_sp_single_logout_service_url');
    $settings['idp']['entityId'] = get_setting('custom_auth_ip_entity_id');
    $settings['idp']['singleSignOnService']['url'] = get_setting('custom_auth_ip_single_signOn_service_url');
    $settings['idp']['singleLogoutService']['url'] = get_setting('custom_auth_ip_single_logout_service_url');
    $settings['idp']['singleLogoutService']['responseUrl'] = get_setting('custom_auth_ip_single_logout_service_response_url');
    $settings['idp']['certFingerprint'] = get_setting('custom_auth_ip_cert_fingerprint');
    $settings['idp']['certFingerprintAlgorithm'] = get_setting('custom_auth_ip_cert_fingerprint_algorithm');
    
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
                Redirect("/reports/index.php", false);
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



