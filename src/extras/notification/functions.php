<?php

require_once __DIR__.'/vendor/autoload.php';
require_once __DIR__.'/vendor/analog/analog/lib/Analog.php';
require_once __DIR__.'/../../includes/functions.php';

// Include the SimpleRisk language file
require_once(language_file());
require_once(notification_language_file());


// Include Laminas Escaper for HTML Output Encoding
$escaper = new Laminas\Escaper\Escaper('utf-8');

use Analog\Analog;

Analog::handler (\Analog\Handler\FirePHP::init ());

Analog::log ('SimpleRisk Notification Addon Activated', Analog::INFO);

function update_notification_config(){
    //TODO IMPLEMENT
    return;
}

function notification_language_file($force_default=false)
{
    // If the session hasn't been defined yet
    // Making it fall through if called from the command line to load the default
    if (!isset($_SESSION) && PHP_SAPI !== 'cli' && !$force_default)
    {
        // Return an empty language file
        return __DIR__ . '/languages/empty.php';
    }
    // If the language is set for the user
    elseif (isset($_SESSION['lang']) && $_SESSION['lang'] != "")
    {
        // Use the users language
        return __DIR__ . '/languages/' . $_SESSION['lang'] . '/lang.' . $_SESSION['lang'] . '.php';
    }
    else
    {
        // Set the default language to null
        $default_language = null;

        // Try connecting to the database
        try
        {
            $db = new PDO("mysql:charset=UTF8;dbname=".DB_DATABASE.";host=".DB_HOSTNAME.";port=".DB_PORT,DB_USERNAME,DB_PASSWORD, array(PDO::ATTR_ERRMODE => PDO::ERRMODE_EXCEPTION));
        }
        catch (PDOException $e)
        {
            $default_language = "en";
        }

        // If we can connect to the database
        if (is_null($default_language))
        {
            // Get the default language
            $default_language = get_setting("default_language");
            if (!$default_language) $default_language = "en";
        }

        // If the default language is set
        if ($default_language != false)
        {
            // Use the default language
            return __DIR__ . '/languages/' . $default_language . '/lang.' . $default_language . '.php';
        }
        // Otherwise, use english
        else return __DIR__ . '/languages/en/lang.en.php';
    }
}


function process_run_now_notification(){
    // TODO IMPLEMENT
    return;

    // Set up the test email
    $name = "SimpleRisk Test";
    $email = $_POST['email'];
    $subject = "SimpleRisk Test Email";
    $full_message = "This is a test email from SimpleRisk.";

    // Send the e-mail
    send_email($name, $email, $subject, $full_message);

}

function run_notification_crons(){
    //TODO IMPLEMENT
    return;
}

function get_notification_message($name){
    // Open the database connection
    $db = db_open();


    // Check if the notifications message table exists
    $stmt = $db->prepare("SELECT value FROM addons_notification_messages WHERE name = :name ;");
    $stmt->bindParam(":name", $name, PDO::PARAM_STR, 50);
    $stmt->execute();
    // Store the list in the array
    $result = $stmt->fetch();

    db_close($db);

    if(!empty($result)) return $result['value'];
    return "";
}

function get_notification_message_status($name){
    // Open the database connection
    $db = db_open();


    // Check if the notifications message table exists
    $stmt = $db->prepare("SELECT status FROM addons_notification_messages WHERE name = :name ;");
    $stmt->bindParam(":name", $name, PDO::PARAM_STR, 50);
    $stmt->execute();
    // Store the list in the array
    $result = $stmt->fetch();

    db_close($db);

    if(!empty($result)) return $result['status'];
    return "";
}

function set_notification_message($name, $value){
    // Open the database connection
    $db = db_open();

    $stmt = $db->prepare("SELECT count(*) as count_values FROM  addons_notification_messages WHERE name=:name ;");
    $stmt->bindParam(":name", $name, PDO::PARAM_STR, 50);
    $stmt->execute();
    // Store the list in the array
    $result = $stmt->fetch();

    if($result["count_values"] > 0){
        $stmt = $db->prepare("UPDATE addons_notification_messages SET VALUE=:value WHERE NAME=:name;");
        $stmt->bindParam(":name", $name, PDO::PARAM_STR, 50);
        $stmt->bindParam(":value", $value, PDO::PARAM_STR, 1000);
        $stmt->execute(); 
    }else{
        $stmt = $db->prepare("INSERT INTO addons_notification_messages(name, value, status) VALUES(:name,:value,'enabled');");
        $stmt->bindParam(":name", $name, PDO::PARAM_STR, 50);
        $stmt->bindParam(":value", $value, PDO::PARAM_STR, 1000);
        $stmt->execute(); 
    }

    db_close($db);

    return;
}

function enable_notification_extra(){
    // Open the database connection
    $db = db_open();


    // Check if the notifications message table exists
    $stmt = $db->prepare("SELECT count(*) as tables FROM information_schema.TABLES WHERE TABLE_NAME = 'addons_notification_messages' AND TABLE_SCHEMA in (SELECT DATABASE());");
    $stmt->execute();
    // Store the list in the array
    $result = $stmt->fetch();

    Analog::log ('Notification Message database '.strval($result["tables"]), Analog::DEBUG);

    if($result["tables"] == 0) {
        $stmt = $db->prepare("CREATE TABLE addons_notification_messages (
            id INT(6) UNSIGNED AUTO_INCREMENT PRIMARY KEY,
            name VARCHAR(50) NOT NULL,
            value VARCHAR(32766) NOT NULL,
            status VARCHAR(10) NOT NULL
            );");
        $stmt->execute();
        Analog::log ('Table addons_notification_messages created', Analog::DEBUG);

        $stmt = $db->prepare("INSERT INTO addons_notification_messages (name,value, status) VALUES('new_risk','A new risk named %risk_name% was created.', 'enabled');");
        $stmt->execute();

    }



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