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
    <tr>
    <td class="not_enfasis">%risk_name%</td><td><?php echo $escaper->escapeHtml($lang_not['Risk name description']); ?> </td>
    </tr>
    <tr>
    <td class="not_enfasis">%risk_responsible%</td><td><?php echo $escaper->escapeHtml($lang_not['Risk responsible description']); ?> </td>
    </tr>
</table>

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