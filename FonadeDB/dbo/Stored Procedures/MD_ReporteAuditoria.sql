
CREATE PROCEDURE [dbo].[MD_ReporteAuditoria]
     @tabla varchar(128)
	,@fechaInicio datetime
	,@fechaFin datetime

AS

BEGIN

	SELECT case [Type] 
		when 'I' then 'Inserción'
		when 'U' then 'Actualización'
		when 'D' then 'Eliminación'
	end as tipo
	,FieldName
	,OldValue
	,NewValue
	,UpdateDate
	,UserName
	, IpUser
	, Nombres + ' ' + Apellidos as nombres
	FROM Audit 
	left join Contacto on STR(LEN(UserName)) = Id_Contacto
	where UserName COLLATE Modern_Spanish_CI_AS not in (SELECT name COLLATE Modern_Spanish_CI_AS FROM sys.server_principals)
	and TableName=@tabla and 
	UpdateDate BETWEEN  @fechaInicio and @fechaFin

	union all

	SELECT case [Type] 
		when 'I' then 'Inserción'
		when 'U' then 'Actualización'
		when 'D' then 'Eliminación'
	end as tipo
	,FieldName
	,OldValue
	,NewValue
	,UpdateDate
	,UserName
	, IpUser
	, UserName as nombres
	FROM Audit 
	where UserName COLLATE Modern_Spanish_CI_AS in (SELECT name COLLATE Modern_Spanish_CI_AS FROM sys.server_principals)
	and TableName=@tabla and 
	UpdateDate BETWEEN  @fechaInicio and @fechaFin

	order by UpdateDate desc
END