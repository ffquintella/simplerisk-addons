<?php

require_once "functions.php";

define('API_EXTRA_VERSION', '1.0.1');


function display_api(){
    global $escaper, $lang;

?>
   <link rel="stylesheet" href="/extras/api/css/api.css">

    <div class="hinfo">INFORMATION</div>

    <form name="api_settings" method="post" action="">
    <input type="submit" name="deactivate" value="<?php echo $escaper->escapeHtml($lang['Deactivate']); ?>" name="deactivate" />
    </form>

<?php
}