CREATE FUNCTION PuntajeCampo 
(
	@idConvocatoria int,
	@idProyecto int,
	@idCAmpo int
)
RETURNS int
AS
Begin
	Declare @puntaje int

	Select @puntaje = (Select ec.Puntaje from evaluacioncampo ec
	Inner Join Campo c on c.id_Campo = ec.codCampo
	Inner Join Campo v on v.id_Campo = c.codCampo
	Inner Join Campo a on a.id_Campo = v.codCampo
	Where ec.codConvocatoria = @idConvocatoria and ec.codProyecto = @idProyecto And a.id_Campo = @idCAmpo)
	RETURN @puntaje
End