<?php


require_once __DIR__.'/../../includes/functions.php';
require_once __DIR__.'/../../includes/governance.php';
require_once __DIR__.'/../../includes/mail.php';


// THIS SCRIPT SHOULD NOT BE DISTRIBUTED TO PRODUCTION ENVIROMENTS


echo "Reseting notification database .... <br\>" ;

$db = db_open();


// Store the file in the database
$stmt = $db->prepare("UPDATE settings SET VALUE='false' WHERE NAME='notifications';");
$stmt->execute();

$stmt = $db->prepare("DROP TABLE addons_notification_messages;");
$stmt->execute();

$stmt = $db->prepare("DROP TABLE addons_notification_control;");
$stmt->execute();

echo "Reseting finished. <br\>" ;

db_close($db);