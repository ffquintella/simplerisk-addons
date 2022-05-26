<?php

require_once "functions.php";

define('NOTIFICATION_EXTRA_VERSION', '1.0.1');

function display_notification(){
    global $escaper, $lang;
?>
<link rel="stylesheet" href="/extras/notification/css/notification.css">

<div class="title"> <?php echo $escaper->escapeHtml($lang_not['Notifications']); ?> </div>



<form name="notification_settings" method="post" action="">
    <table>
        <tr>
        <th></th>
        </tr>

    </table>

    <input type="submit" name="deactivate" value="<?php echo $escaper->escapeHtml($lang['Deactivate']); ?>" name="deactivate" />
</form>
<?php
}