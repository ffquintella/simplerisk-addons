<?php
require_once "functions.php";

function display_authentication(){

global $escaper, $lang;
?>
    <link rel="stylesheet" href="css/auth.css">
    <form name="authentication_settings" method="post" action="">
        <input class="hidden-checkbox" readonly type="checkbox" id="enabled" name="enabled"<?php if (get_setting('custom_auth') == 'true') echo " checked" ?> /><label for="enabled">&nbsp;&nbsp;<?php echo $escaper->escapeHtml($lang['Enabled']); ?></label>
        <table>
        <tr><td colspan=2>
        <div class="title">
            Service Provider Info:
        </div>
        </td></tr>
        <?php 
                if (!empty(get_setting('custom_auth_sp_url'))){
                    $custom_auth_sp_url = get_setting('custom_auth_sp_url');
                }
            ?>
        <tr>
        <td>URL:</td> <td><input name="custom_auth_sp_url" type="text" maxlength="50" size="20" value="<?php echo isset($custom_auth_sp_url) ? $escaper->escapeHtml($custom_auth_sp_url) : "" ?>" />
        </td></tr>
        </table>
        <input type="submit" value="<?php echo $escaper->escapeHtml($lang['Update']); ?>" name="authentication_settings_update" />

    <br\><br\>
        <!--input type="submit" name="activate" value="<?php echo $escaper->escapeHtml($lang['Activate']); ?>" name="activate" /-->
        <input type="submit" name="deactivate" value="<?php echo $escaper->escapeHtml($lang['Deactivate']); ?>" name="deactivate" />
        </form>
<?php
}