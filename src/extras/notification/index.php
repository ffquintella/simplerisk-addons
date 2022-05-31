<?php

require_once "functions.php";

define('NOTIFICATION_EXTRA_VERSION', '1.0.1');

function display_notification(){
    global $escaper, $lang, $lang_not;
?>
<link rel="stylesheet" href="/extras/notification/css/notification.css">

<div class="not_title"> <?php echo $escaper->escapeHtml($lang_not['Notifications']); ?> </div>

<div class="not_text"> <?php echo $escaper->escapeHtml($lang_not['Notification_Explain_Text']); ?> </div>

<div class="not_text"> <?php echo $escaper->escapeHtml($lang_not['Variable_Table']); ?> </div>

<table class="var_table">
    <tr>
    <th><?php echo $escaper->escapeHtml($lang_not['Variable']); ?> </th><th><?php echo $escaper->escapeHtml($lang_not['Description']); ?> </th>
    </tr>
<?php
foreach (get_notification_variables() as $key => $value){ ?>
    <tr>
    <td class="not_enfasis"><?php echo $key; ?></td><td><?php echo $escaper->escapeHtml($value); ?> </td>
    </tr>
<?php
}
?>

</table>

<form name="notification_settings" method="post" action="">
    <table class="not_table" width="400">
        <tr>
            <?php 
            if(get_notification_message_status("new_risk") != "enabled"){
                $bt_img = "off-button.png";
                $action = "enable.php";
            }else {
                $bt_img = "on-button.png";
                $action = "disable.php";
            }
            ?>
        <td><?php echo $escaper->escapeHtml($lang_not['New risk']); ?><a href='/extras/notification/<?php echo $action; ?>?id=1'><img class='btimg' src='/extras/notification/imgs/<?php echo $bt_img; ?>' /></a></td>
        </tr>
        <tr>
        <td><textarea class="not_text" id="newrisk" name="newrisk" rows="4"  width="100%"><?php echo get_notification_message("new_risk");?></textarea></td>
        
        </tr>

    </table>
    <input class="uptdate_bt" type="submit" name="submit" value="<?php echo $escaper->escapeHtml($lang['Update']); ?>" />
    <input type="submit" name="deactivate" value="<?php echo $escaper->escapeHtml($lang['Deactivate']); ?>" name="deactivate" />
</form>
<?php
}