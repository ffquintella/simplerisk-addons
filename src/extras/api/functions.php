<?php

require_once __DIR__.'/vendor/analog/analog/lib/Analog.php';
require_once __DIR__.'/../../includes/functions.php';

// Include the SimpleRisk language file
require_once(language_file());
// Include Laminas Escaper for HTML Output Encoding
$escaper = new Laminas\Escaper\Escaper('utf-8');

use Analog\Analog;

Analog::handler (\Analog\Handler\FirePHP::init ());

Analog::log ('SimpleRisk API Addon Activated', Analog::INFO);

function enable_api_extra(){
    // Open the database connection
    $db = db_open();


    // Check if the apiKey table exists
    $stmt = $db->prepare("SELECT count(*) as tables FROM information_schema.TABLES WHERE TABLE_NAME = 'addons_api_keys' AND TABLE_SCHEMA in (SELECT DATABASE());");
    $stmt->execute();
    // Store the list in the array
    $result = $stmt->fetch();

    Analog::log ('API Key database '.strval($result["tables"]), Analog::DEBUG);

    if($result["tables"] == 0) {
        $stmt = $db->prepare("CREATE TABLE addons_api_keys (
            id INT(6) UNSIGNED AUTO_INCREMENT PRIMARY KEY,
            name VARCHAR(30) NOT NULL,
            value VARCHAR(50) NOT NULL,
            creation_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
            );");
        $stmt->execute();
        Analog::log ('Table addons_api_keys created', Analog::DEBUG);
    }

    if (!empty(get_setting('api'))){
        // Store the file in the database
        $stmt = $db->prepare("UPDATE settings SET VALUE='true' WHERE NAME='api';");
        $stmt->execute();

    }else{

        // Store the value in the database
        $stmt = $db->prepare("INSERT INTO settings VALUES('api','true');");
        $stmt->execute();
    }



    // Close the database connection
    db_close($db);

    //refresh the page
    header("Refresh:0");

    return;
}

function disable_api_extra(){
    // Open the database connection
    $db = db_open();

    if (!empty(get_setting('api'))){
        // Store the file in the database
        $stmt = $db->prepare("UPDATE settings SET VALUE='false' WHERE NAME='api';");
        $stmt->execute();

    }else{

        // Store the value in the database
        $stmt = $db->prepare("INSERT INTO settings VALUES('api','false');");
        $stmt->execute();
    }

    // Close the database connection
    db_close($db);

    //refresh the page
    header("Refresh:0");

    return;
}

// So far we will ignore this. 
function check_encryption_level(){
    return true;
}

// we will have to implement this on the future
function authenticate_key(){
    Analog::log ('Checking authentication key', Analog::DEBUG);
    return false;
}