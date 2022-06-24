<?php

function checkNeedsUpgrade($version): bool {

    $setting = get_setting('custom_auth_version');

    if($setting != false){
        if($version != $setting) return true;
        return false;
    } else return true;
    
}

function doUpgrade(string $toVersion){

    $fromVersion = get_setting('custom_auth_version');
    if($fromVersion == false){
        $fromVersion = "1.0.1";
    }



}
