
CREATE PROCEDURE MD_InsertActa

	@NomActa varchar(250),
	@NumActa varchar(20),
	@FechaActa smalldatetime,
	@Observaciones varchar(1500),
	@CodConvocatoria int

AS

BEGIN

Declare @codigoacta int

	INSERT INTO AsignacionActa (NomActa,NumActa,FechaActa,Observaciones,CodConvocatoria,Publicado)
	VALUES(@NomActa, @NumActa, @FechaActa, @Observaciones, @CodConvocatoria ,1)


	SELECT @codigoacta=Max(Id_Acta) FROM AsignacionActa
	WHERE NomActa = @NomActa
	AND NumActa = @NumActa
	AND FechaActa = @FechaActa
	AND Observaciones = @Observaciones
	AND CodConvocatoria = @CodConvocatoria

return @codigoacta

END