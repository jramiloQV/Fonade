-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE sp_InsertarIntervRiesgo
	@riesgo varchar(200),
	@mitigacion varchar(200),
	@tipo int,
	@pro int, @id int = null
AS
BEGIN
if(@tipo = 1)
begin 
insert into interventorriesgo (riesgo, mitigacion, codproyecto)
values (@riesgo, @mitigacion, @pro)
end
if(@tipo = 2)
begin 
update interventorriesgo
set mitigacion= @mitigacion,
riesgo= @riesgo
where id_riesgo= @id
end	
END