<?php

function checkNeedsUpgrade($version): bool {

    $setting = get_setting('notifications_version');

    if($setting != false){
        if($version != $setting) return true;
        return false;
    } else return true;
    
}

function doUpgrade(string $toVersion){

    $fromVersion = get_setting('notifications_version');
    if($fromVersion == false){
        $fromVersion = "1.0.1";
    }

    if ($toVersion == $fromVersion) return;

    // taking it to 1.0.2
    if(version_compare($fromVersion, '1.0.2', '<')){
        // DO the 1.0.2 updates
        // Open the database connection
        $db = db_open();
        $stmt = $db->prepare("INSERT INTO addons_notification_messages (name,value, status) VALUES('document_update','A document was updated. %event_details%', 'enabled');");
        $stmt->execute();
        db_close($db);
        update_setting("notifications_version", '1.0.2');
    }

    return;

}


