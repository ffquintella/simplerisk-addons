<?php

require_once __DIR__.'/vendor/analog/analog/lib/Analog.php';
require_once __DIR__.'/../../includes/functions.php';
require_once __DIR__.'/../../includes/api.php';

// Include the SimpleRisk language file
require_once(language_file());
// Include Laminas Escaper for HTML Output Encoding
$escaper = new Laminas\Escaper\Escaper('utf-8');

use Analog\Analog;

Analog::handler (\Analog\Handler\FirePHP::init ());

Analog::log ('SimpleRisk API Addon Activated', Analog::INFO);

if (is_authenticated())
{

    Analog::log ('Creating API Key', Analog::INFO);

    if($_POST["api-name"])

    $api_name = isset($_POST['api-name']) ? $_POST['api-name'] : '';
    $api_value = isset($_POST['api-value']) ? $_POST['api-value'] : '';


    if(empty($api_name)) {
         Analog::log ('Invalid empty api-name', Analog::ERROR);
         $_SESSION["validation-error"] = true;
         $_SESSION["validation-message"] = 'Invalid empty api-name';
         header("Location: /admin/api.php");
         exit();    
    }
    if(empty($api_value)) {
        Analog::log ('Invalid empty api-value', Analog::ERROR);
        $_SESSION["validation-error"] = true;
        $_SESSION["validation-message"] = 'Invalid empty api-value';
        header("Location: /admin/api.php");
        exit();    
    }
    if(strlen($api_value) < 12) {
        Analog::log ('API VALUE too short', Analog::ERROR);
        $_SESSION["validation-error"] = true;
        $_SESSION["validation-message"] = 'API VALUE too short';
        header("Location: /admin/api.php");
        exit();    
    }

    update_api_key($api_name, $api_value);


    header("Location: /admin/api.php");
    exit();

}else{
    Analog::log ('USER NOT AUTHENTICATED', Analog::ERROR);
    echo "NOT AUTHENTICATED";
}

