/*   
 Autor  alberto Palencia  
 Creado 25-02-2014  
 Obtengo los APORTES  
  
 exec MD_GetAportes  47548,147  
  
*/  
CREATE procedure MD_GetAportes(  
 @codProyecto varchar(50),  
 @codConvocatoria varchar(50)   
)  
  
as  
begin  
  
    SELECT  
   nomTipoIndicador  
   , id_TipoIndicador  
   ,sum(Isnull(solicitado,0)) as TotalSolicitado  
  , isnull(sum(isnull(Recomendado,0)),0) as TotalRecomendado   
    FROM TipoIndicadorGestion T JOIN  EvaluacionProyectoAporte E ON  T.id_tipoindicador = E.CodTipoIndicador  
    WHERE E.codProyecto=@codProyecto AND E.codConvocatoria=@codConvocatoria    
    GROUP BY t.nomTipoIndicador,t.Id_TipoIndicador  
    ORDER BY t.Id_TipoIndicador  
  
  
  
end