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


    <div class="sheader">API KEYS</div>
    <table>
        <tr> <th> Name </th> <th> Value </th></tr>
    </table>

    <form name="api_settings" method="post" action="">
    <input type="submit" name="deactivate" value="<?php echo $escaper->escapeHtml($lang['Deactivate']); ?>" name="deactivate" />
    </form>

<?php
}