<?php

require_once __DIR__.'/vendor/autoload.php';
require_once __DIR__.'/vendor/analog/analog/lib/Analog.php';
require_once __DIR__.'/../../includes/functions.php';

// Include the SimpleRisk language file
require_once(language_file());
// Include Laminas Escaper for HTML Output Encoding
$escaper = new Laminas\Escaper\Escaper('utf-8');

use Analog\Analog;

Analog::handler (\Analog\Handler\FirePHP::init ());

Analog::log ('SimpleRisk Notification Addon Activated', Analog::INFO);

function update_notification_config(){
    //TODO IMPLEMENT
    return;
}

function process_run_now_notification(){
    // TODO IMPLEMENT
    return;
}

function run_notification_crons(){
    //TODO IMPLEMENT
    return;
}

function enable_notification_extra(){
    // Open the database connection
    $db = db_open();

    if (!empty(get_setting('notifications'))){
        // Store the file in the database
        $stmt = $db->prepare("UPDATE settings SET VALUE='true' WHERE NAME='notifications';");
        $stmt->execute();

    }else{

        // Store the value in the database
        $stmt = $db->prepare("INSERT INTO settings VALUES('notifications','true');");
        $stmt->execute();
    }



    // Close the database connection
    db_close($db);
    return;
}

function disable_notification_extra(){
    // Open the database connection
    $db = db_open();

    // Store the file in the database
    $stmt = $db->prepare("UPDATE settings SET VALUE='false' WHERE NAME='notifications';");
    $stmt->execute();

    // Close the database connection
    db_close($db);
    return;
}