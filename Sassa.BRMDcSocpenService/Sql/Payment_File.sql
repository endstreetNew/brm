--1. Invalidate Active grants.
UPDATE DC_SOCPEN
SET STATUS_CODE = 'INACTIVE'
WHERE STATUS_CODE = 'ACTIVE';

COMMIT;

--2. Set new Active grants from In-Payment Table
UPDATE DC_SOCPEN
SET STATUS_CODE = 'ACTIVE'
WHERE (beneficiary_id,GRANT_TYPE) in (SELECT ID_NUMBER, GRANT_TYPE FROM CUST_PAYMENT);

COMMIT;

--3. Migrate “INACTIVE” DG capture/scan data for each existing  OAG

--4. Remove migrated Inactive DG records.