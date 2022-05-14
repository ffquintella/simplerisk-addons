<?php
require_once "functions.php";

function display_authentication(){

global $escaper, $lang;
?>
    <form name="authentication_settings" method="post" action="">
        <input class="hidden-checkbox" type="checkbox" id="enabled" name="enabled"<?php if (get_setting('custom_auth') == 'true') echo " checked" ?> /><label for="enabled">&nbsp;&nbsp;<?php echo $escaper->escapeHtml($lang['Enabled']); ?></label>
        <br\><br\>
        <input type="submit" value="<?php echo $escaper->escapeHtml($lang['Update']); ?>" name="authentication_settings_update" />
    </form>
<?php
}