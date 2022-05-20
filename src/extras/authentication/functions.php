<?php
require_once __DIR__."/vendor/autoload.php";
require_once __DIR__.'/vendor/analog/analog/lib/Analog.php';
require_once __DIR__.'/../../includes/functions.php';

// Include the SimpleRisk language file
require_once(language_file());
// Include Laminas Escaper for HTML Output Encoding
$escaper = new Laminas\Escaper\Escaper('utf-8');

use Analog\Analog;

Analog::handler (\Analog\Handler\FirePHP::init ());

// debug-level message
//Analog::log (array ('A debug message', __FILE__, __LINE__), Analog::DEBUG);
//Analog::handler (\Analog\Handler\Syslog::init ('SR-ADDONS', 'user'));

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

    if (!empty(get_setting('custom_auth'))){
        // Store the file in the database
        $stmt = $db->prepare("UPDATE settings SET VALUE='true' WHERE NAME='custom_auth';");
        $stmt->execute();

    }else{

        // Store the value in the database
        $stmt = $db->prepare("INSERT INTO settings VALUES('custom_auth','true');");
        $stmt->execute();
    }



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

function update_authentication_config(){
    // Open the database connection
    $db = db_open();


    if(!empty($_POST['custom_auth_sp_entity_id'])){
        if (!empty(get_setting('custom_auth_sp_entity_id'))){
            // Update the value in the database
            $stmt = $db->prepare("UPDATE settings SET VALUE=:custom_auth_sp_entity_id WHERE NAME='custom_auth_sp_entity_id';");
            
            $stmt->bindParam(":custom_auth_sp_entity_id", $_POST['custom_auth_sp_entity_id'], PDO::PARAM_STR, 100);
            
            $stmt->execute();
        }else{
            // Store the value in the database
            $stmt = $db->prepare("INSERT INTO settings VALUES('custom_auth_sp_entity_id',:custom_auth_sp_entity_id);");
            
            $stmt->bindParam(":custom_auth_sp_entity_id", $_POST['custom_auth_sp_entity_id'], PDO::PARAM_STR, 100);
            
            $stmt->execute();
        }
    }

    if(!empty($_POST['custom_auth_sp_assertion_consumer_service_url'])){
        if (!empty(get_setting('custom_auth_sp_assertion_consumer_service_url'))){
            // Update the value in the database
            $stmt = $db->prepare("UPDATE settings SET VALUE=:custom_auth_sp_assertion_consumer_service_url WHERE NAME='custom_auth_sp_assertion_consumer_service_url';");
            
            $stmt->bindParam(":custom_auth_sp_assertion_consumer_service_url", $_POST['custom_auth_sp_assertion_consumer_service_url'], PDO::PARAM_STR, 100);
            
            $stmt->execute();
        }else{
            // Store the value in the database
            $stmt = $db->prepare("INSERT INTO settings VALUES('custom_auth_sp_assertion_consumer_service_url',:custom_auth_sp_assertion_consumer_service_url);");
            
            $stmt->bindParam(":custom_auth_sp_assertion_consumer_service_url", $_POST['custom_auth_sp_assertion_consumer_service_url'], PDO::PARAM_STR, 100);
            
            $stmt->execute();
        }
    }

    if(!empty($_POST['custom_auth_sp_single_logout_service_url'])){
        if (!empty(get_setting('custom_auth_sp_single_logout_service_url'))){
            // Update the value in the database
            $stmt = $db->prepare("UPDATE settings SET VALUE=:custom_auth_sp_single_logout_service_url WHERE NAME='custom_auth_sp_single_logout_service_url';");
            
            $stmt->bindParam(":custom_auth_sp_single_logout_service_url", $_POST['custom_auth_sp_single_logout_service_url'], PDO::PARAM_STR, 100);
            
            $stmt->execute();
        }else{
            // Store the value in the database
            $stmt = $db->prepare("INSERT INTO settings VALUES('custom_auth_sp_single_logout_service_url',:custom_auth_sp_single_logout_service_url);");
            
            $stmt->bindParam(":custom_auth_sp_single_logout_service_url", $_POST['custom_auth_sp_single_logout_service_url'], PDO::PARAM_STR, 100);
            
            $stmt->execute();
        }
    }

    if(!empty($_POST['custom_auth_ip_entity_id'])){
        if (!empty(get_setting('custom_auth_ip_entity_id'))){
            // Update the value in the database
            $stmt = $db->prepare("UPDATE settings SET VALUE=:custom_auth_ip_entity_id WHERE NAME='custom_auth_ip_entity_id';");
            
            $stmt->bindParam(":custom_auth_ip_entity_id", $_POST['custom_auth_ip_entity_id'], PDO::PARAM_STR, 100);
            
            $stmt->execute();
        }else{
            // Store the value in the database
            $stmt = $db->prepare("INSERT INTO settings VALUES('custom_auth_ip_entity_id',:custom_auth_ip_entity_id);");
            
            $stmt->bindParam(":custom_auth_ip_entity_id", $_POST['custom_auth_ip_entity_id'], PDO::PARAM_STR, 100);
            
            $stmt->execute();
        }
    }

    if(!empty($_POST['custom_auth_ip_single_signOn_service_url'])){
        if (!empty(get_setting('custom_auth_ip_single_signOn_service_url'))){
            // Update the value in the database
            $stmt = $db->prepare("UPDATE settings SET VALUE=:custom_auth_ip_single_signOn_service_url WHERE NAME='custom_auth_ip_single_signOn_service_url';");
            
            $stmt->bindParam(":custom_auth_ip_single_signOn_service_url", $_POST['custom_auth_ip_single_signOn_service_url'], PDO::PARAM_STR, 100);
            
            $stmt->execute();
        }else{
            // Store the value in the database
            $stmt = $db->prepare("INSERT INTO settings VALUES('custom_auth_ip_single_signOn_service_url',:custom_auth_ip_single_signOn_service_url);");
            
            $stmt->bindParam(":custom_auth_ip_single_signOn_service_url", $_POST['custom_auth_ip_single_signOn_service_url'], PDO::PARAM_STR, 100);
            
            $stmt->execute();
        }
    }

    if(!empty($_POST['custom_auth_ip_single_logout_service_url'])){
        if (!empty(get_setting('custom_auth_ip_single_logout_service_url'))){
            // Update the value in the database
            $stmt = $db->prepare("UPDATE settings SET VALUE=:custom_auth_ip_single_logout_service_url WHERE NAME='custom_auth_ip_single_logout_service_url';");
            
            $stmt->bindParam(":custom_auth_ip_single_logout_service_url", $_POST['custom_auth_ip_single_logout_service_url'], PDO::PARAM_STR, 100);
            
            $stmt->execute();
        }else{
            // Store the value in the database
            $stmt = $db->prepare("INSERT INTO settings VALUES('custom_auth_ip_single_logout_service_url',:custom_auth_ip_single_logout_service_url);");
            
            $stmt->bindParam(":custom_auth_ip_single_logout_service_url", $_POST['custom_auth_ip_single_logout_service_url'], PDO::PARAM_STR, 100);
            
            $stmt->execute();
        }
    }

    if(!empty($_POST['custom_auth_ip_single_logout_service_response_url'])){
        if (!empty(get_setting('custom_auth_ip_single_logout_service_response_url'))){
            // Update the value in the database
            $stmt = $db->prepare("UPDATE settings SET VALUE=:custom_auth_ip_single_logout_service_response_url WHERE NAME='custom_auth_ip_single_logout_service_response_url';");
            
            $stmt->bindParam(":custom_auth_ip_single_logout_service_response_url", $_POST['custom_auth_ip_single_logout_service_response_url'], PDO::PARAM_STR, 100);
            
            $stmt->execute();
        }else{
            // Store the value in the database
            $stmt = $db->prepare("INSERT INTO settings VALUES('custom_auth_ip_single_logout_service_response_url',:custom_auth_ip_single_logout_service_response_url);");
            
            $stmt->bindParam(":custom_auth_ip_single_logout_service_response_url", $_POST['custom_auth_ip_single_logout_service_response_url'], PDO::PARAM_STR, 100);
            
            $stmt->execute();
        }
    }
    if(!empty($_POST['custom_auth_ip_cert_fingerprint'])){
        if (!empty(get_setting('custom_auth_ip_cert_fingerprint'))){
            // Update the value in the database
            $stmt = $db->prepare("UPDATE settings SET VALUE=:custom_auth_ip_cert_fingerprint WHERE NAME='custom_auth_ip_cert_fingerprint';");
            
            $stmt->bindParam(":custom_auth_ip_cert_fingerprint", $_POST['custom_auth_ip_cert_fingerprint'], PDO::PARAM_STR, 100);
            
            $stmt->execute();
        }else{
            // Store the value in the database
            $stmt = $db->prepare("INSERT INTO settings VALUES('custom_auth_ip_cert_fingerprint',:custom_auth_ip_cert_fingerprint);");
            
            $stmt->bindParam(":custom_auth_ip_cert_fingerprint", $_POST['custom_auth_ip_cert_fingerprint'], PDO::PARAM_STR, 100);
            
            $stmt->execute();
        }
    }
    if(!empty($_POST['custom_auth_ip_cert_fingerprint_algorithm'])){
        if (!empty(get_setting('custom_auth_ip_cert_fingerprint_algorithm'))){
            // Update the value in the database
            $stmt = $db->prepare("UPDATE settings SET VALUE=:custom_auth_ip_cert_fingerprint_algorithm WHERE NAME='custom_auth_ip_cert_fingerprint_algorithm';");
            
            $stmt->bindParam(":custom_auth_ip_cert_fingerprint_algorithm", $_POST['custom_auth_ip_cert_fingerprint_algorithm'], PDO::PARAM_STR, 100);
            
            $stmt->execute();
        }else{
            // Store the value in the database
            $stmt = $db->prepare("INSERT INTO settings VALUES('custom_auth_ip_cert_fingerprint_algorithm',:custom_auth_ip_cert_fingerprint_algorithm);");
            
            $stmt->bindParam(":custom_auth_ip_cert_fingerprint_algorithm", $_POST['custom_auth_ip_cert_fingerprint_algorithm'], PDO::PARAM_STR, 100);
            
            $stmt->execute();
        }
    }


    // Close the database connection
    db_close($db);
    return;
}

