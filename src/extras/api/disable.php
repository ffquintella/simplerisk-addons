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


if (is_authenticated())
{
    // Add the session
    $permissions = array(
        "check_access" => true,
        "check_admin" => true,
    );
    add_session_check($permissions);


    if(isset($_GET['id'])){
        $id = $_GET['id'];

        disable_api_key($id);
    }

    header("Location: /admin/api.php");
    exit();

}else{
    Analog::log ('USER NOT AUTHENTICATED', Analog::ERROR);
    echo "NOT AUTHENTICATED";
}