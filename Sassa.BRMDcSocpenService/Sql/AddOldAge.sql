﻿--If there is an disability grant convert it to old age grant

INSERT INTO DC_SOCPEN (BENEFICIARY_ID, CHILD_ID, NAME, SURNAME,  GRANT_TYPE, REGION_ID, APPLICATION_DATE, STATUS_CODE, PAYPOINT) 
SELECT     
    DISTINCT 
    LPAD(A.PENSION_NO,13,0) AS BENEFICIARY_ID,
    NULL AS CHILD_ID,
    B.NAME_EXT,
    B.SURNAME_EXT,
    A.GRANT_TYPE AS GRANT_TYPE,
    D.REGION_CODE AS REGION_ID,
    A.APPLICATION_DATE AS APPLICATION_DATE,
    CASE
        WHEN A.PRIM_STATUS IN ('B','A','9') AND A.SEC_STATUS IN ('2') THEN 'ACTIVE'
        ELSE 'INACTIVE'
    END AS STATUS_CODE,
    B.secondary_paypoint as paypoint  
FROM SASSA.socpen_personal_grants A
LEFT JOIN SASSA.SOCPEN_PERSONAL B ON  A.PENSION_NO = B.PENSION_NO
LEFT JOIN SASSA.cust_rescodes D ON b.secondary_paypoint = d.res_code
where NOT exists(SELECT 1 FROM DC_Socpen x WHERE x.Beneficiary_id = LPAD(A.PENSION_NO,13,0) AND x.grant_type = A.GRANT_TYPE)
and A.GRANT_TYPE in('0','1','3','7','8','4')
and A.Application_date is not null
