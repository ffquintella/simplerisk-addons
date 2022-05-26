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

Analog::log ('SimpleRisk API Addon Activated', Analog::INFO);

function get_api_keys_cvs(){
    $db = db_open();
    $stmt = $db->prepare("SELECT * FROM addons_api_keys WHERE status='enabled';");
    $stmt->execute();

    $keys = $stmt->fetchAll();
    db_close($db);

    $result = "";

    foreach($keys as $key) {
        $result = $result.",".$key["name"].":".decrypt($key["value"]);
    }

    return $result;
}

function UniqueMachineID($salt = "") {
    if (strtoupper(substr(PHP_OS, 0, 3)) === 'WIN') {
        $temp = sys_get_temp_dir().DIRECTORY_SEPARATOR."diskpartscript.txt";
        if(!file_exists($temp) && !is_file($temp)) file_put_contents($temp, "select disk 0\ndetail disk");
        $output = shell_exec("diskpart /s ".$temp);
        $lines = explode("\n",$output);
        $result = array_filter($lines,function($line) {
            return stripos($line,"ID:")!==false;
        });
        if(count($result)>0) {
            $result = array_shift(array_values($result));
            $result = explode(":",$result);
            $result = trim(end($result));
        } else $result = $output;
    } else {
        $result = shell_exec("blkid -o value -s UUID");
        if(stripos($result,"blkid")!==false) {
            $result = $_SERVER['HTTP_HOST'];
        }
    }
    return md5($salt.md5($result));
}

function encrypt($plaintext = "") {
    $key=UniqueMachineID("kjhsdfD387sdft");

    $encrypted_string=openssl_encrypt($plaintext,"aes-192-cbc",$key);

    return $encrypted_string;
}

function decrypt($ciphertext = ""){
    $key=UniqueMachineID("kjhsdfD387sdft");
    $decrypted_string=openssl_decrypt($ciphertext,"aes-192-cbc",$key);
    return $decrypted_string;
}


function list_api_keys(){
    $db = db_open();
    $stmt = $db->prepare("SELECT * FROM addons_api_keys;");
    $stmt->execute();

    $result = $stmt->fetchAll();
    db_close($db);

    return $result;
}

function delete_api_key($id){
    Analog::log ('Deleting api with ID:'.$id, Analog::DEBUG);
    $db = db_open();

    $stmt = $db->prepare("SELECT count(*) as count_values FROM  addons_api_keys WHERE id=:id ;");
    $stmt->bindParam(":id", $id, PDO::PARAM_INT);
    $stmt->execute();
    // Store the list in the array
    $result = $stmt->fetch();

    if($result["count_values"] > 0){
        $stmt = $db->prepare("DELETE FROM addons_api_keys WHERE id=:id;");
        $stmt->bindParam(":id", $id, PDO::PARAM_INT);
        $stmt->execute(); 
    }

    db_close($db);
}

function disable_api_key($id){
    Analog::log ('Disabling api with ID:'.$id, Analog::DEBUG);
    $db = db_open();

    $stmt = $db->prepare("SELECT count(*) as count_values FROM  addons_api_keys WHERE id=:id ;");
    $stmt->bindParam(":id", $id, PDO::PARAM_INT);
    $stmt->execute();
    // Store the list in the array
    $result = $stmt->fetch();

    if($result["count_values"] > 0){
        $stmt = $db->prepare("UPDATE addons_api_keys SET status='disabled' WHERE id=:id;");
        $stmt->bindParam(":id", $id, PDO::PARAM_INT);
        $stmt->execute(); 
    }

    db_close($db);
}

function enable_api_key($id){

    Analog::log ('Enabling api with ID:'.$id, Analog::DEBUG);
    $db = db_open();

    $stmt = $db->prepare("SELECT count(*) as count_values FROM  addons_api_keys WHERE id=:id ;");
    $stmt->bindParam(":id", $id, PDO::PARAM_INT);
    $stmt->execute();
    // Store the list in the array
    $result = $stmt->fetch();

    if($result["count_values"] > 0){
        $stmt = $db->prepare("UPDATE addons_api_keys SET status='enabled' WHERE id=:id;");
        $stmt->bindParam(":id", $id, PDO::PARAM_INT);
        $stmt->execute(); 
    }

    db_close($db);
}

function update_api_key($name, $value){
    $db = db_open();

    $stmt = $db->prepare("SELECT count(*) as count_values FROM  addons_api_keys WHERE name=:api_name ;");
    $stmt->bindParam(":api_name", $name, PDO::PARAM_STR, 30);
    $stmt->execute();
    // Store the list in the array
    $result = $stmt->fetch();

    if($result["count_values"] > 0){
        $stmt = $db->prepare("UPDATE addons_api_keys SET VALUE=:api_value WHERE NAME=:api_name;");
        $stmt->bindParam(":api_name", $name, PDO::PARAM_STR, 30);
        $stmt->bindParam(":api_value", encrypt($value), PDO::PARAM_STR, 50);
        $stmt->execute(); 
    }else{
        $stmt = $db->prepare("INSERT INTO addons_api_keys(name, value, status) VALUES(:api_name,:api_value,'enabled');");
        $stmt->bindParam(":api_name", $name, PDO::PARAM_STR, 30);
        $stmt->bindParam(":api_value", encrypt($value), PDO::PARAM_STR, 50);
        $stmt->execute(); 
    }

    db_close($db);
}

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
            status VARCHAR(10) NOT NULL,
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