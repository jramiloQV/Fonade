-- =============================================
-- Author:		Orange Developer
-- Create date: 16/09/2016
-- Description:	Devuelve el saldo del proyecto
-- =============================================
CREATE PROCEDURE [dbo].[Proc_SaldoProyecto] 
	-- Add the parameters for the stored procedure here
	@idProyecto int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Declare @SalariosRecomendado int
	Declare @Anio int
	Declare @idConvocatoria int
	declare @SalarioMin int
	Declare @TotalProyecto int
	Declare @TotalDesembolso int

	Set @idConvocatoria = (SELECT top 1 CodConvocatoria FROM Evaluacionobservacion eo
	Inner Join Convocatoria c on c.Id_Convocatoria = eo.CodConvocatoria
	Where CodProyecto = @idProyecto ORDER BY CodConvocatoria DESC)
	Set @Anio = (SELECT top 1 DatePart(yyyy,fechaInicio) FROM Evaluacionobservacion eo
	Inner Join Convocatoria c on c.Id_Convocatoria = eo.CodConvocatoria
	Where CodProyecto = @idProyecto ORDER BY CodConvocatoria DESC)
	Set @SalariosRecomendado = (SELECT top 1 ValorRecomendado AS AnnoConvocatoria FROM Evaluacionobservacion eo
	Inner Join Convocatoria c on c.Id_Convocatoria = eo.CodConvocatoria
	Where CodProyecto = @idProyecto ORDER BY CodConvocatoria DESC)

	--Select @idConvocatoria convo, @Anio año, @SalariosRecomendado salarios

	-- Salario minimo x año
	Set @SalarioMin = (Select SalarioMinimo from SalariosMinimos where AñoSalario = @Anio)
	--Select @SalarioMin salarioAnio

	--Total Asignado
	Set @TotalProyecto = @SalarioMin * @SalariosRecomendado
	--Select @TotalProyecto TotalAsignado

	--Pagos en estado >= 1 y < 5
	Set @TotalDesembolso = (SELECT ISNULL(SUM(CantidadDinero), 0) AS Total FROM PagoActividad
	WHERE Estado >= 1 AND Estado < 5  AND CodProyecto = @idProyecto)

	--Select @TotalDesembolso TotalDesembolso

	Select @TotalProyecto - @TotalDesembolso Saldo
END