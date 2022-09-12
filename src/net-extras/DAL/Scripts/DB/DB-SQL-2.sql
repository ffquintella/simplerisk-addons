ALTER TABLE `simplerisk`.`addons_client_registration`
    ADD INDEX `Ind_ExternalID`(`ExternalId`) USING BTREE;

ALTER TABLE `simplerisk`.`addons_client_registration`
    MODIFY COLUMN `Id` int(11) NOT NULL AUTO_INCREMENT FIRST;