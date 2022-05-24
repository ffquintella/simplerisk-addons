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

Analog::log ('SimpleRisk API Addon Activated', Analog::INFO);

function enable_api_extra(){
    // Open the database connection
    $db = db_open();

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
    return;
}