<?php

require_once "functions.php";

define('NOTIFICATION_EXTRA_VERSION', '1.0.2');

if(checkNeedsUpgrade(NOTIFICATION_EXTRA_VERSION)) doUpgrade(NOTIFICATION_EXTRA_VERSION);

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
        <tr>
            <?php 
            if(get_notification_message_status("risk_update") != "enabled"){
                $bt_img = "off-button.png";
                $action = "enable.php";
            }else {
                $bt_img = "on-button.png";
                $action = "disable.php";
            }
            ?>
        <td><?php echo $escaper->escapeHtml($lang_not['Risk update']); ?><a href='/extras/notification/<?php echo $action; ?>?id=2'><img class='btimg' src='/extras/notification/imgs/<?php echo $bt_img; ?>' /></a></td>
        </tr>
        <tr>
        <td><textarea class="not_text" id="riskupdate" name="riskupdate" rows="4"  width="100%"><?php echo get_notification_message("risk_update");?></textarea></td>
        
        </tr>
        <tr>
            <?php 
            if(get_notification_message_status("new_review") != "enabled"){
                $bt_img = "off-button.png";
                $action = "enable.php";
            }else {
                $bt_img = "on-button.png";
                $action = "disable.php";
            }
            ?>
        <td><?php echo $escaper->escapeHtml($lang_not['New review']); ?><a href='/extras/notification/<?php echo $action; ?>?id=3'><img class='btimg' src='/extras/notification/imgs/<?php echo $bt_img; ?>' /></a></td>
        </tr>
        <tr>
        <td><textarea class="not_text" id="newreview" name="newreview" rows="4"  width="100%"><?php echo get_notification_message("new_review");?></textarea></td>
        
        </tr>
        <tr>
            <?php 
            if(get_notification_message_status("new_mitigation") != "enabled"){
                $bt_img = "off-button.png";
                $action = "enable.php";
            }else {
                $bt_img = "on-button.png";
                $action = "disable.php";
            }
            ?>
        <td><?php echo $escaper->escapeHtml($lang_not['New mitigation']); ?><a href='/extras/notification/<?php echo $action; ?>?id=4'><img class='btimg' src='/extras/notification/imgs/<?php echo $bt_img; ?>' /></a></td>
        </tr>
        <tr>
        <td><textarea class="not_text" id="newmitigation" name="newmitigation" rows="4"  width="100%"><?php echo get_notification_message("new_mitigation");?></textarea></td>
        
        </tr>
        <tr>
            <?php 
            if(get_notification_message_status("mitigation_update") != "enabled"){
                $bt_img = "off-button.png";
                $action = "enable.php";
            }else {
                $bt_img = "on-button.png";
                $action = "disable.php";
            }
            ?>
        <td><?php echo $escaper->escapeHtml($lang_not['Mitigation update']); ?><a href='/extras/notification/<?php echo $action; ?>?id=5'><img class='btimg' src='/extras/notification/imgs/<?php echo $bt_img; ?>' /></a></td>
        </tr>
        <tr>
        <td><textarea class="not_text" id="mitigationupdate" name="mitigationupdate" rows="4"  width="100%"><?php echo get_notification_message("mitigation_update");?></textarea></td>
        
        </tr>
        <tr>
            <?php 
            if(get_notification_message_status("new_document") != "enabled"){
                $bt_img = "off-button.png";
                $action = "enable.php";
            }else {
                $bt_img = "on-button.png";
                $action = "disable.php";
            }
            ?>
        <td><?php echo $escaper->escapeHtml($lang_not['New document']); ?><a href='/extras/notification/<?php echo $action; ?>?id=6'><img class='btimg' src='/extras/notification/imgs/<?php echo $bt_img; ?>' /></a></td>
        </tr>
        <tr>
        <td><textarea class="not_text" id="newdocument" name="newdocument" rows="4"  width="100%"><?php echo get_notification_message("new_document");?></textarea></td>
        
        </tr>
        <tr>
            <?php 
            if(get_notification_message_status("document_update") != "enabled"){
                $bt_img = "off-button.png";
                $action = "enable.php";
            }else {
                $bt_img = "on-button.png";
                $action = "disable.php";
            }
            ?>
        <td><?php echo $escaper->escapeHtml($lang_not['Document update']); ?><a href='/extras/notification/<?php echo $action; ?>?id=13'><img class='btimg' src='/extras/notification/imgs/<?php echo $bt_img; ?>' /></a></td>
        </tr>
        <tr>
        <td><textarea class="not_text" id="documentupdate" name="documentupdate" rows="4"  width="100%"><?php echo get_notification_message("document_update");?></textarea></td>
        
        </tr>
        <tr>
            <?php 
            if(get_notification_message_status("new_audit_comment") != "enabled"){
                $bt_img = "off-button.png";
                $action = "enable.php";
            }else {
                $bt_img = "on-button.png";
                $action = "disable.php";
            }
            ?>
        <td><?php echo $escaper->escapeHtml($lang_not['New audit comment']); ?><a href='/extras/notification/<?php echo $action; ?>?id=7'><img class='btimg' src='/extras/notification/imgs/<?php echo $bt_img; ?>' /></a></td>
        </tr>
        <tr>
        <td><textarea class="not_text" id="newauditcomment" name="newauditcomment" rows="4"  width="100%"><?php echo get_notification_message("new_audit_comment");?></textarea></td>
        
        </tr>
        <tr>
            <?php 
            if(get_notification_message_status("new_audit_status_change") != "enabled"){
                $bt_img = "off-button.png";
                $action = "enable.php";
            }else {
                $bt_img = "on-button.png";
                $action = "disable.php";
            }
            ?>
        <td><?php echo $escaper->escapeHtml($lang_not['New audit status change']); ?><a href='/extras/notification/<?php echo $action; ?>?id=8'><img class='btimg' src='/extras/notification/imgs/<?php echo $bt_img; ?>' /></a></td>
        </tr>
        <tr>
        <td><textarea class="not_text" id="newauditstatuschange" name="newauditstatuschange" rows="4"  width="100%"><?php echo get_notification_message("new_audit_status_change");?></textarea></td>
        
        </tr>
        <tr>
            <?php 
            if(get_notification_message_status("risk_close") != "enabled"){
                $bt_img = "off-button.png";
                $action = "enable.php";
            }else {
                $bt_img = "on-button.png";
                $action = "disable.php";
            }
            ?>
        <td><?php echo $escaper->escapeHtml($lang_not['Risk closed']); ?><a href='/extras/notification/<?php echo $action; ?>?id=9'><img class='btimg' src='/extras/notification/imgs/<?php echo $bt_img; ?>' /></a></td>
        </tr>
        <tr>
        <td><textarea class="not_text" id="riskclosed" name="riskclosed" rows="4"  width="100%"><?php echo get_notification_message("risk_close");?></textarea></td>
        
        </tr>
        <tr>
            <?php 
            if(get_notification_message_status("risk_comment") != "enabled"){
                $bt_img = "off-button.png";
                $action = "enable.php";
            }else {
                $bt_img = "on-button.png";
                $action = "disable.php";
            }
            ?>
        <td><?php echo $escaper->escapeHtml($lang_not['Risk commented']); ?><a href='/extras/notification/<?php echo $action; ?>?id=10'><img class='btimg' src='/extras/notification/imgs/<?php echo $bt_img; ?>' /></a></td>
        </tr>
        <tr>
        <td><textarea class="not_text" id="riskcomment" name="riskcomment" rows="4"  width="100%"><?php echo get_notification_message("risk_comment");?></textarea></td>
        
        </tr>
        <tr>
            <?php 
            if(get_notification_message_status("review_mitigation_alert") != "enabled"){
                $bt_img = "off-button.png";
                $action = "enable.php";
            }else {
                $bt_img = "on-button.png";
                $action = "disable.php";
            }
            ?>
        <td><?php echo $escaper->escapeHtml($lang_not['Alert to review mitigation']); ?><a href='/extras/notification/<?php echo $action; ?>?id=11'><img class='btimg' src='/extras/notification/imgs/<?php echo $bt_img; ?>' /></a></td>
        </tr>
        <tr>
        <td><textarea class="not_text" id="reviewmitigationalert" name="reviewmitigationalert" rows="4"  width="100%"><?php echo get_notification_message("review_mitigation_alert");?></textarea></td>
        
        </tr>
        <tr>
            <?php 
            if(get_notification_message_status("review_analysis_alert") != "enabled"){
                $bt_img = "off-button.png";
                $action = "enable.php";
            }else {
                $bt_img = "on-button.png";
                $action = "disable.php";
            }
            ?>
        <td><?php echo $escaper->escapeHtml($lang_not['Alert to review analysis']); ?><a href='/extras/notification/<?php echo $action; ?>?id=12'><img class='btimg' src='/extras/notification/imgs/<?php echo $bt_img; ?>' /></a></td>
        </tr>
        <tr>
        <td><textarea class="not_text" id="reviewanalysisalert" name="reviewanalysisalert" rows="4"  width="100%"><?php echo get_notification_message("review_analysis_alert");?></textarea></td>
        
        </tr>
    </table>
    <input class="uptdate_bt" type="submit" name="submit" value="<?php echo $escaper->escapeHtml($lang['Update']); ?>" />
    <input type="submit" name="deactivate" value="<?php echo $escaper->escapeHtml($lang['Deactivate']); ?>" name="deactivate" />
</form>
<?php
}