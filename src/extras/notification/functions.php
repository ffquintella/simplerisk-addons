<?php

require_once __DIR__.'/vendor/autoload.php';
require_once __DIR__.'/vendor/analog/analog/lib/Analog.php';
require_once __DIR__.'/../../includes/functions.php';
require_once __DIR__.'/../../includes/mail.php';

// Include the SimpleRisk language file
require_once(language_file());
require_once(notification_language_file());


// Include Laminas Escaper for HTML Output Encoding
$escaper = new Laminas\Escaper\Escaper('utf-8');

use Analog\Analog;

Analog::handler (\Analog\Handler\FirePHP::init ());

Analog::log ('SimpleRisk Notification Addon Activated', Analog::INFO);

function update_notification_config(){

    //Analog::log ('POST Variables:'.var_export($_POST, true), Analog::DEBUG);

    $db = db_open();

    $stmt = $db->prepare("UPDATE addons_notification_messages SET VALUE=:value WHERE ID=1;");
    $stmt->bindParam(":value", $_POST['newrisk'], PDO::PARAM_STR, 1000);
    $stmt->execute(); 
    db_close($db);
    


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

    // TODO IMPLEMENT
    return;
}

function notify_new_risk($risk_id, $risk_name){
    global $escaper ;

    if(get_notification_message_status("new_risk") == "enabled"){

        $risk = get_risk_by_id($risk_id + 1000)[0];

        Analog::log ('Sending notification for risk:'.$risk_id + 1000, Analog::INFO);

        // Set up the test email
        $name = "[SR] New risk - ".$escaper->escapeHtml($risk_name);
        
        $subject = "[SR] New risk - ".$escaper->escapeHtml($risk_name);
        $full_message = replace_notification_variables(get_notification_message("new_risk"), $risk);


        $emails = get_risk_notified_emails($risk);

        foreach($emails as $email){
            // Send the e-mail
            send_email($name, $email, $subject, $full_message);
        }

    }
    return;
}

function get_risk_notified_emails($risk){

    $emails = array();
    $owner_id = $risk["owner"];
    $owner = get_user_by_id($owner_id);

    array_push($emails, $owner["email"]);

    if(isset($risk["manager"]) && $risk["manager"] > 0){
        $manager_id = $risk["manager"];
        $manager = get_user_by_id($manager_id);
        array_push($emails, $manager["email"]);
    }

    return $emails ;
}

function replace_notification_variables($message, $risk){

    foreach(get_notification_variables() as $key => $value){
        if(str_contains($message, $key)){
            $message = str_replace($key, get_notification_risk_variable_value($key, $risk), $message);
        }
    }

    return $message;
}

function get_notification_risk_variable_value($variable, $risk){
    switch($variable){
        case "%risk_name%":
            return $risk["subject"];
            break;
        case "%risk_responsible%":
            $owner = get_user_by_id($risk["owner"]);
            return $owner["name"];
            break;
        default:
           echo "";
    }
}

function get_notification_variables(){
    global $lang, $lang_not;

    $variables = [
        "%risk_name%" => $lang_not['Risk name description'],
        "%risk_responsible%" => $lang_not['Risk responsible description'],
    ];

    return $variables;
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

function enable_notification($id){
    $db = db_open();

    $stmt = $db->prepare("UPDATE addons_notification_messages SET STATUS='enabled' WHERE ID=:id;");
    $stmt->bindParam(":id", $id, PDO::PARAM_INT);
    $stmt->execute(); 
    db_close($db);

}

function disable_notification($id){
    $db = db_open();

    $stmt = $db->prepare("UPDATE addons_notification_messages SET STATUS='disabled' WHERE ID=:id;");
    $stmt->bindParam(":id", $id, PDO::PARAM_INT);
    $stmt->execute(); 
    db_close($db);

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
    
    // Check if the notifications control table exists
    $stmt = $db->prepare("SELECT count(*) as tables FROM information_schema.TABLES WHERE TABLE_NAME = 'addons_notification_control' AND TABLE_SCHEMA in (SELECT DATABASE());");
    $stmt->execute();
    // Store the list in the array
    $result = $stmt->fetch();

    Analog::log ('Notification Message database '.strval($result["tables"]), Analog::DEBUG);
    
    if($result["tables"] == 0) {
        $stmt = $db->prepare("CREATE TABLE addons_notification_control (
            id INT(6) UNSIGNED AUTO_INCREMENT PRIMARY KEY,
            risk_id INT NOT NULL,
            notified_id INT NOT NULL,
            sent_date DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
            INDEX rsk_ind (risk_id),
            INDEX ntf_ind (notified_id),
            FOREIGN KEY (risk_id) REFERENCES risks(id) ON DELETE CASCADE,
            FOREIGN KEY (notified_id) REFERENCES user(value) ON DELETE CASCADE
            );");
        $stmt->execute();
        Analog::log ('Table addons_notification_control created', Analog::DEBUG);

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