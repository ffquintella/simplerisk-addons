<?php
session_start(); 

require_once "functions.php";

//define("TOOLKIT_PATH", 'vendor/onelogin/php-saml/');
//require_once('vendor/onelogin/php-saml/_toolkit_loader.php');
require_once "settings.php";

$auth = new \OneLogin\Saml2\Auth($settings);


if (isset($_SESSION) && isset($_SESSION['AuthNRequestID'])) {
    $requestID = $_SESSION['AuthNRequestID'];
} else {
    $requestID = null;
}

$auth->processResponse($requestID);
unset($_SESSION['AuthNRequestID']);

$errors = $auth->getErrors();

if (!empty($errors)) {
    echo '<p>', implode(', ', $errors), '</p>';
    exit();
}

if (!$auth->isAuthenticated()) {
    echo "<p>Not authenticated</p>";
    exit();
}


//var_dump($auth);