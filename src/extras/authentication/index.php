<?php
require_once "functions.php";

function display_authentication(){

    
if (!empty(get_setting('custom_auth_sp_entity_id'))){
    $custom_auth_sp_entity_id = get_setting('custom_auth_sp_entity_id');
}
if (!empty(get_setting('custom_auth_sp_assertion_consumer_service_url'))){
    $custom_auth_sp_assertion_consumer_service_url = get_setting('custom_auth_sp_assertion_consumer_service_url');
}
if (!empty(get_setting('custom_auth_sp_single_logout_service_url'))){
    $custom_auth_sp_single_logout_service_url = get_setting('custom_auth_sp_single_logout_service_url');
}
if (!empty(get_setting('custom_auth_ip_entity_id'))){
    $custom_auth_ip_entity_id = get_setting('custom_auth_ip_entity_id');
}
if (!empty(get_setting('custom_auth_ip_single_signOn_service_url'))){
    $custom_auth_ip_single_signOn_service_url = get_setting('custom_auth_ip_single_signOn_service_url');
}
if (!empty(get_setting('custom_auth_ip_single_logout_service_url'))){
    $custom_auth_ip_single_logout_service_url = get_setting('custom_auth_ip_single_logout_service_url');
}
if (!empty(get_setting('custom_auth_ip_single_logout_service_response_url'))){
    $custom_auth_ip_single_logout_service_response_url = get_setting('custom_auth_ip_single_logout_service_response_url');
}
if (!empty(get_setting('custom_auth_ip_cert_fingerprint'))){
    $custom_auth_ip_cert_fingerprint = get_setting('custom_auth_ip_cert_fingerprint');
}
if (!empty(get_setting('custom_auth_ip_cert_fingerprint_algorithm'))){
    $custom_auth_ip_cert_fingerprint_algorithm = get_setting('custom_auth_ip_cert_fingerprint_algorithm');
}

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
        <tr>
        <td>Entity ID:</td> <td><input name="custom_auth_sp_entity_id" type="text" maxlength="100" size="50" value="<?php echo isset($custom_auth_sp_entity_id) ? $escaper->escapeHtml($custom_auth_sp_entity_id) : "" ?>" /></td>
        </tr>
        <tr>
        <td>Assertion Consumer Service URL:</td> <td><input name="custom_auth_sp_assertion_consumer_service_url" type="text" maxlength="100" size="50" value="<?php echo isset($custom_auth_sp_assertion_consumer_service_url) ? $escaper->escapeHtml($custom_auth_sp_assertion_consumer_service_url) : "" ?>" /></td>
        </tr>
        <tr>
        <td>Single Logout Service URL:</td> <td><input name="custom_auth_sp_single_logout_service_url" type="text" maxlength="100" size="50" value="<?php echo isset($custom_auth_sp_single_logout_service_url) ? $escaper->escapeHtml($custom_auth_sp_single_logout_service_url) : "" ?>" /></td>
        </tr>
        <tr class="sub_title"><td colspan=2>
                Identity Provider:
        </td></tr>
        <tr>
        <tr>
        <td>Entity ID:</td> <td><input name="custom_auth_ip_entity_id" type="text" maxlength="50" size="50" value="<?php echo isset($custom_auth_ip_entity_id) ? $escaper->escapeHtml($custom_auth_ip_entity_id) : "" ?>" /></td>
        </tr>
        <tr>
        <tr>
        <td>Single SignOn Service URL:</td> <td><input name="custom_auth_ip_single_signOn_service_url" type="text" maxlength="50" size="50" value="<?php echo isset($custom_auth_ip_single_signOn_service_url) ? $escaper->escapeHtml($custom_auth_ip_single_signOn_service_url) : "" ?>" /></td>
        </tr>
        <tr>
        <td>Single Logout Service URL:</td> <td><input name="custom_auth_ip_single_logout_service_url" type="text" maxlength="50" size="50" value="<?php echo isset($custom_auth_ip_single_logout_service_url) ? $escaper->escapeHtml($custom_auth_ip_single_logout_service_url) : "" ?>" /></td>
        </tr>
        <tr>
        <td>Single Logout Service Response URL:</td> <td><input name="custom_auth_ip_single_logout_service_response_url" type="text" maxlength="50" size="50" value="<?php echo isset($custom_auth_ip_single_logout_service_response_url) ? $escaper->escapeHtml($custom_auth_ip_single_logout_service_response_url) : "" ?>" /></td>
        </tr>
        <tr>
        <td>Certificate Fingerprint:</td> <td><input name="custom_auth_ip_cert_fingerprint" type="text" maxlength="50" size="50" value="<?php echo isset($custom_auth_ip_cert_fingerprint) ? $escaper->escapeHtml($custom_auth_ip_cert_fingerprint) : "" ?>" /></td>
        </tr>
        <tr>
        <td>Certificate Fingerprint Algorithm:</td> <td><input name="custom_auth_ip_cert_fingerprint_algorithm" type="text" maxlength="50" size="50" value="<?php echo isset($custom_auth_ip_cert_fingerprint_algorithm) ? $escaper->escapeHtml($custom_auth_ip_cert_fingerprint_algorithm) : "" ?>" /></td>
        </tr>
        
        </table>
        
        <input type="submit" value="<?php echo $escaper->escapeHtml($lang['Update']); ?>" name="update_saml" />

    <br\><br\>
        <!--input type="submit" name="activate" value="<?php echo $escaper->escapeHtml($lang['Activate']); ?>" name="activate" /-->
        <input type="submit" name="deactivate" value="<?php echo $escaper->escapeHtml($lang['Deactivate']); ?>" name="deactivate" />
        </form>
<?php
}