<?php
require_once "vendor/autoload.php";
require_once "settings.php";

$auth = new \OneLogin\Saml2\Auth($settings);


var_dump($auth);