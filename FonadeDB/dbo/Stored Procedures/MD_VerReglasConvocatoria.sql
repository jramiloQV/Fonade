
CREATE PROCEDURE [dbo].[MD_VerReglasConvocatoria]
	@IdConvocatoria int
AS

BEGIN
	SELECT CASE WHEN ExpresionLogica = 'Entre' 
                     THEN 'Condición ' + CAST( NoRegla as varchar(200) ) 
					 + ': Si los empleos generados están entre ' 
					 + CAST( EmpleosGenerados1 as varchar(200) )
					  + ' y ' + CAST( EmpleosGenerados2 as varchar(200) ) + ' se prestarán: ' 
					  +  CAST( SalariosAPrestar as varchar(200) )  + ' (SMMLV)'
                  ELSE  
				  'Condición ' + CAST( NoRegla as varchar(200) ) + ': Si los empleos generados son ' 
				  +CAST( ExpresionLogica as varchar(200) )  + ' a ' + CAST( EmpleosGenerados1 as varchar(200) ) 
				  + ' se prestarán: ' + CAST( SalariosAPrestar as varchar(200) ) + ' (SMMLV)'
             END as condicion
			 , CodConvocatoria
      ,ExpresionLogica
      ,EmpleosGenerados1
      ,EmpleosGenerados2
      ,SalariosAPrestar
      ,NoRegla
  FROM ConvocatoriaReglaSalarios
  where CodConvocatoria=@IdConvocatoria
END