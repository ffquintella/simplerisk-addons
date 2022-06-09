<?php


require_once __DIR__.'/../../includes/functions.php';
require_once __DIR__.'/../../includes/governance.php';
require_once __DIR__.'/../../includes/mail.php';


// THIS SCRIPT SHOULD NOT BE DISTRIBUTED TO PRODUCTION ENVIROMENTS


echo "Disabling compliance forge .... <br\>" ;

$db = db_open();


// Store the file in the database
$stmt = $db->prepare("UPDATE settings SET VALUE='false' WHERE NAME='complianceforge_scf';");
$stmt->execute();



echo "Reseting finished. <br\>" ;

db_close($db);


