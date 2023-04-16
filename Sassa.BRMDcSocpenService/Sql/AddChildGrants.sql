INSERT INTO DC_SOCPEN (BENEFICIARY_ID, CHILD_ID, NAME, SURNAME,  GRANT_TYPE, REGION_ID, APPLICATION_DATE, STATUS_CODE, PAYPOINT) 
SELECT     
    DISTINCT LPAD(c.PENSION_NO,13,0)  AS BENEFICIARY_ID,
    LPAD(c.ID_NO,13,0) AS CHILD_ID,
    B.NAME_EXT,
    B.SURNAME_EXT,
    C.GRANT_TYPE AS GRANT_TYPE,
    D.REGION_CODE AS REGION_ID,
    C.APPLICATION_DATE AS APPLICATION_DATE,
    C.STATUS_CODE,
    B.secondary_paypoint as paypoint  
FROM SASSA.socpen_personal_grants a
LEFT JOIN SASSA.SOCPEN_PERSONAL B ON  LPAD(a.PENSION_NO,13,0) = LPAD(b.PENSION_NO,13,0)
INNER JOIN VW_P12_CHILDREN c ON LPAD(c.PENSION_NO,13,0) = LPAD(a.PENSION_NO,13,0)
LEFT JOIN SASSA.cust_rescodes D ON b.secondary_paypoint = d.res_code
where NOT exists(SELECT 1 FROM DC_Socpen x WHERE x.Beneficiary_id = LPAD(c.PENSION_NO,13,0) AND x.grant_type = c.GRANT_TYPE and x.Child_Id = LPAD(c.ID_NO,13,0))