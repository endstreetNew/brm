--ALL DG must be converted to Oag if DG and OAG exists
--1 Set all 3 to x where 0 exists
--2 Set tdw_rec where x to tdw_rec where 0 tdw_rec is not null


--Set tdw_rec from Dg to Oag
Update DC_Socpen s
Set (tdw_rec) = (
	SELECT f.tdw_rec
	FROM DC_Socpen f
	WHERE f.Beneficiary_id =  s.Beneficiary_id
	and f.Grant_TYPE = '3'
	and s.tdw_rec is not null
	AND ROWNUM = 1
)
where s.tdw_rec is null