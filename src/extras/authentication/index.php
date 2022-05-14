<?php
require_once "functions.php";

function display_authentication(){

global $escaper, $lang;
?>
    <link rel="stylesheet" href="/extras/authentication/css/auth.css">
    <form name="authentication_settings" method="post" action="">
        <input class="hidden-checkbox" readonly type="checkbox" id="enabled" name="enabled"<?php if (get_setting('custom_auth') == 'true') echo " checked" ?> /><label for="enabled">&nbsp;&nbsp;<?php echo $escaper->escapeHtml($lang['Enabled']); ?></label>
        <table>
        <tr class="title"><td colspan=2>
            SAML Configuration:
        </td></tr>
        <tr class="sub_title"><td colspan=2>
                Service Provider
        </td></tr>
        <?php 
                if (!empty(get_setting('custom_auth_sp_entity_id'))){
                    $custom_auth_sp_url = get_setting('custom_auth_sp_entity_id');
                }
            ?>
        <tr>
        <td>Entity ID:</td> <td><input name="custom_auth_sp_entity_id" type="text" maxlength="100" size="30" value="<?php echo isset($custom_auth_sp_entity_id) ? $escaper->escapeHtml($custom_auth_sp_entity_id) : "" ?>" /></td>
        </tr>
        <tr>
        <td>Assertion Consumer Service URL:</td> <td><input name="custom_auth_sp_assertion_consumer_service_url" type="text" maxlength="100" size="30" value="<?php echo isset($custom_auth_sp_url) ? $escaper->escapeHtml($custom_auth_sp_url) : "" ?>" /></td>
        </tr>
        <tr>
        <td>Single Logout Service URL:</td> <td><input name="custom_auth_sp_single_logout_service_url" type="text" maxlength="100" size="30" value="<?php echo isset($custom_auth_sp_url) ? $escaper->escapeHtml($custom_auth_sp_url) : "" ?>" /></td>
        </tr>
        <tr>
        <td>Single Logout Service URL:</td> <td><input name="custom_auth_sp_single_logout_service_url" type="text" maxlength="100" size="30" value="<?php echo isset($custom_auth_sp_url) ? $escaper->escapeHtml($custom_auth_sp_url) : "" ?>" /></td>
        </tr>
        <tr class="sub_title"><td colspan=2>
                Identity Provider:
        </td></tr>
        <?php 
                if (!empty(get_setting('custom_auth_ip_entity_id'))){
                    $custom_auth_ip_entity_id = get_setting('custom_auth_ip_entity_id');
                }
            ?>
        <tr>
        <tr>
        <td>Entity ID:</td> <td><input name="custom_auth_ip_entity_id" type="text" maxlength="50" size="30" value="<?php echo isset($custom_auth_ip_entity_id) ? $escaper->escapeHtml($custom_auth_ip_entity_id) : "" ?>" /></td>
        </tr>
        
        <!--tr>
        <td>Attribute Consuming Service Name:</td> <td><input name="custom_auth_sp_attribute_consuming_service_name" type="text" maxlength="50" size="30" value="<?php echo isset($custom_auth_sp_url) ? $escaper->escapeHtml($custom_auth_sp_url) : "" ?>" /></td>
        </tr-->
        </table>
        
        <input type="submit" value="<?php echo $escaper->escapeHtml($lang['Update']); ?>" name="update_saml" />

    <br\><br\>
        <!--input type="submit" name="activate" value="<?php echo $escaper->escapeHtml($lang['Activate']); ?>" name="activate" /-->
        <input type="submit" name="deactivate" value="<?php echo $escaper->escapeHtml($lang['Deactivate']); ?>" name="deactivate" />
        </form>
<?php
}