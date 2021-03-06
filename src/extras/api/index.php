<?php

require_once "functions.php";

define('API_EXTRA_VERSION', '1.0.1');

function display_api(){
    global $escaper, $lang;

?>
   <link rel="stylesheet" href="/extras/api/css/api.css">

    <div class="hinfo">INFORMATION</div>

    <div class="info-text"> To use this addon you should use the 
        URL: https://yoursite/extras/api/api.php/records/endpoint. 
        All endpoints can be listet at https://yoursite/extras/api/api.php/openapi </div>
<?php

if(isset($_SESSION["validation-error"]) && $_SESSION["validation-error"]  == true){
    $_SESSION["validation-error"] = false;
    echo "<div class='warning'>" . $escaper->escapeHtml($_SESSION["validation-message"]) . "</div>";
}

?>


    <div class="sheader">API KEYS</div>

    API Keys should be placed on a header called X-API-Key with the following format |name|:|value|

    <table>
        <tr> <th> Name </th></tr>
<?php
    $keys = list_api_keys();

    foreach($keys as $key){
        if($key["status"] == 'enabled') {
            $img = "on-button.png";
            $action = "disable.php";
        }
        else {
            $img = "off-button.png";
            $action = "enable.php";
        }
        echo "<tr><td>".$key["name"]."</td> 
        <td> <a href='/extras/api/".$action."?id=".$key["id"]."'><img class='btimg' src='/extras/api/imgs/".$img."' /></a></td> 
        <td> <a href='/extras/api/delete.php?id=".$key["id"]."'><img class='btimg2' src='/extras/api/imgs/delete.png' /></a> </td></tr>";
    }

?>
    </table>

    <div class="sheader">Create API Key</div>
    <form name="api_keys" method="post" action="/extras/api/create.php">
        <table>
            <tr><td>Name</td><td>Value</td></tr>
            <tr><td><input type="texto" name="api-name" value="" /></td><td><input type="texto" name="api-value" value="" /></td></tr>
            <tr><td colspan="2"><input type="submit" name="deactivate" value="<?php echo $escaper->escapeHtml($lang['Send']); ?>" name="send" /></td></tr>
        </table>
    </form>

    <form name="api_settings" method="post" action="">
        <input type="submit" name="deactivate" value="<?php echo $escaper->escapeHtml($lang['Deactivate']); ?>" name="deactivate" />
    </form>

<?php
}