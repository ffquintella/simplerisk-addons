<?php
require_once "functions.php";

function display_authentication(){

global $escaper, $lang;
?>
    <link rel="stylesheet" href="css/auth.css">
    <form name="authentication_settings" method="post" action="">
        <input class="hidden-checkbox" readonly type="checkbox" id="enabled" name="enabled"<?php if (get_setting('custom_auth') == 'true') echo " checked" ?> /><label for="enabled">&nbsp;&nbsp;<?php echo $escaper->escapeHtml($lang['Enabled']); ?></label>
        <div class="title">
            Service Provider Info:
        </div>
        <br\><br\>
        <input type="submit" value="<?php echo $escaper->escapeHtml($lang['Update']); ?>" name="authentication_settings_update" />
    </form>
    <br\><br\>
        <!--input type="submit" name="activate" value="<?php echo $escaper->escapeHtml($lang['Activate']); ?>" name="activate" /-->
        <input type="submit" name="deactivate" value="<?php echo $escaper->escapeHtml($lang['Deactivate']); ?>" name="deactivate" />
<?php
}