INSERT INTO DC_SOCPEN (SRD_NO,BENEFICIARY_ID,NAME,SURNAME,GRANT_TYPE,REGION_ID,APPLICATION_DATE,STATUS_CODE,PAYPOINT)
SELECT DISTINCT
    A.SRD_NO,
    NVL(A.BENEFICIARY_ID,'S' || LPAD(A.SRD_NO,12,0)),
    A.NAME,
    A.SURNAME,
    'S' AS GRANT_TYPE,
    A.PROVINCE AS REGION_ID,
    A.APPLICATION_DATE,
    A.STATUS_CODE,
    A.PAYPOINT
FROM
    VW_MAX_SRD_TYPE A
where NOT exists(SELECT 1 FROM DC_Socpen x WHERE x.SRD_NO = A.SRD_NO)