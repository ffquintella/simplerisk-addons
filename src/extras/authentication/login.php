<?php
require_once "functions.php"

//define("TOOLKIT_PATH", 'vendor/onelogin/php-saml/');
//require_once('vendor/onelogin/php-saml/_toolkit_loader.php');
require_once "settings.php";

$auth = new \OneLogin\Saml2\Auth($settings);


var_dump($auth);