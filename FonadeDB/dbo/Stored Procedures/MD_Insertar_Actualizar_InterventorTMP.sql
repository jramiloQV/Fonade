-- Batch submitted through debugger: SQLQuery2.sql|7|0|C:\Users\TEMPAD~1\AppData\Local\Temp\1\~vs1974.sql
-- Batch submitted through debugger: SQLQuery22.sql|7|0|C:\Users\TEMPAD~1\AppData\Local\Temp\2\~vs3748.sql

CREATE PROCEDURE [dbo].[MD_Insertar_Actualizar_InterventorTMP]
 @_caso Nvarchar(max),
 @_CodProyecto int ,
 @_cargo Varchar(100),
 @_tipo Varchar(100),
 @id_Nomina int,
 @mes int,
 @Valor int
AS
set nocount on;
--BEGIN Transaction
	begin
		select * from InterventorNominaTMP 
	
		if (@_caso ='create' )
		begin 
			Insert into InterventorNominaTMP (Id_Nomina,CodProyecto,Cargo,Tipo) 
			values (@id_Nomina,@_CodProyecto,@_cargo,@_tipo)
							
			select * from InterventorNominaTMP
			--EXECUTE sp_executesql @insert

			SELECT 'Registros Insertados:', @@ROWCOUNT

			if (@@ROWCOUNT >0)
			begin
				select * from InterventorNominaMesTMP
			
				INSERT INTO InterventorNominaMesTMP(CodCargo,Mes,Valor,Tipo) 
				VALUES(@_cargo,@Mes,@Valor,@_tipo)
			end
		end		   
end