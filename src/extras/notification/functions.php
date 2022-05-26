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