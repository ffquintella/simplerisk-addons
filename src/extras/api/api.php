<?php

require_once __DIR__.'/vendor/analog/analog/lib/Analog.php';
require_once __DIR__.'/vendor/mevdschee/php-crud-api/api.include.php';
require_once __DIR__.'/../../includes/functions.php';
require_once __DIR__.'/../../includes/config.php';
require_once __DIR__.'/functions.php';

use Tqdev\PhpCrudApi\Api;
use Tqdev\PhpCrudApi\Config;
use Tqdev\PhpCrudApi\RequestFactory;
use Tqdev\PhpCrudApi\ResponseUtils;

$config = new Config([
    'driver' => 'mysql',
    'address' => DB_HOSTNAME,
    'port' => DB_PORT,
    'username' => DB_USERNAME,
    'password' => DB_PASSWORD,
    'database' => DB_DATABASE,
    'middlewares' => 'apiKeyAuth',
    'apiKeyAuth.keys' => get_api_keys_cvs(),
    // 'debug' => false
]);
$request = RequestFactory::fromGlobals();
$api = new Api($config);
$response = $api->handle($request);
ResponseUtils::output($response);

//file_put_contents('request.log',RequestUtils::toString($request)."===\n",FILE_APPEND);
//file_put_contents('request.log',ResponseUtils::toString($response)."===\n",FILE_APPEND);
