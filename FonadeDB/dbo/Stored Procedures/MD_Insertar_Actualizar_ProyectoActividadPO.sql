-- Batch submitted through debugger: SQLQuery2.sql|7|0|C:\Users\TEMPAD~1\AppData\Local\Temp\1\~vs1974.sql
-- Batch submitted through debugger: SQLQuery22.sql|7|0|C:\Users\TEMPAD~1\AppData\Local\Temp\2\~vs3748.sql

CREATE PROCEDURE [dbo].[MD_Insertar_Actualizar_ProyectoActividadPO]
 	
 @_CodProyecto int,
 @_Id_Actividad int,
 @_CodGrupo int ,
 @_NomActividad varchar (250),
 @_caso varchar(50) 

AS
Declare @FilasAfectadas int 
Declare @Valor int
Declare @Mes int
Declare @CodTipoFinanciacion int
Declare @_CodContacto int
Declare @Observaciones Nvarchar(max)
Declare @NomActividad varchar(200)
Declare @Metas Varchar(200)
Declare @item SMALLINT
Declare @i int
Declare @j int 
Declare @CodGrupo int
Declare @cons_interventor int
Declare @Const_GerenteInterventor int
		
--BEGIN Transaction
	begin


	

	--begin 
	begin
	Declare @query Nvarchar(max)
	Declare @query1 Nvarchar(max)
	Declare @query2 Nvarchar(max)
	Declare @query3 Nvarchar(max)
	Declare @query4 Nvarchar(max)
	set @cons_interventor = 12



       if (@_CodGrupo=@cons_interventor and @_caso ='Delete' )
	   begin
		set @query = 'Insert into proyectoactividadPOInterventorTMP (id_actividad,CodProyecto,Tarea) 
			values (@_Id_Actividad,@_CodProyecto,@_caso)'

		set @query1= 'Insert into proyectoactividadPOMesInterventorTMP (CodActividad)
			values (@_Id_Actividad)'
	    
		set @query4 = 'INSERT INTO AvanceActividadPOMes (CodActividad,CodTipoFinanciacion,Valor,Observaciones,CodContacto) 
					  values (@_Id_Actividad,@CodTipoFinanciacion,@valor,@Observaciones,@_CodContacto)'
		
		set @query2= 'Delete ProyectoactividadPOMes where CodActividad=@_Id_Actividad'

		set @query3= 'Delete ProyectoactividadPO where Id_Actividad=@_Id_Actividad'

		set @query = @query + @query1 + @query2 + @query3

		
		Execute sp_executesql @query
				if (@@Rowcount >0)

				PRINT N'Datos Eliminados Satisfactoriamente'
			


				if @@rowcount = 0 goto error
	    
				   error: rollback
				  PRINT N'Error al Eliminar los Datos'
					return(2) 
			end 
		end

		if (@_caso ='create' and @_CodGrupo=@cons_interventor)

		begin 
	    
		set nocount on;
		
		Declare @querys  Nvarchar(max)
		Declare @querys1 Nvarchar(max)
		Declare @querys2 Nvarchar(max)
		Declare @insert Nvarchar(max)
		declare @trancount int

		
		begin try
		--set @trancount = @@TRANCOUNT
		set @insert = 'Insert into proyectoactividadPOInterventorTMP (Id_Actividad,CodProyecto,Item,NomActividad,Metas)
		values ( @_Id_Actividad,@_CodProyecto,@item,@_NomActividad,@Metas)'

		
		EXECUTE sp_executesql @insert

		SELECT 'Registros Insertados:', @@ROWCOUNT
		commit;

		if (@@ROWCOUNT >0)
		

		set @i = 1
	    set @j = 1
	    set @Mes= 1
		set @CodTipoFinanciacion = 1
						
				while(@CodTipoFinanciacion <= 2)
					begin
						while(@Mes <= 12)
							begin
								set @querys1 ='INSERT INTO ProyectoactividadPOMesInterventorTMP(id_Actividad,Mes,CodTipoFinanciacion,Valor) 
											   VALUES(@_Id_Actividad,@mes,@CodTipoFinanciacion,@Valor)'

								SET @querys ='INSERT INTO AvanceActividadPOMes (id_Actividad,Mes,CodTipoFinanciacion,Valor,Observaciones,CodContacto) 
											   values (@_Id_Actividad,@Mes,@CodTipoFinanciacion,@Observaciones,@_CodContacto)'
							  
								SET @querys = @querys +  @querys1 
								Print @querys
								EXECUTE sp_executesql @querys

								if (@@Rowcount >0)
									PRINT N'Datos Insertados Satisfactoriamente'
									return(1)
								if @@rowcount = 0 goto exc
									 --exc: rollback
									 exc: print 'Los datos no fueron Insertados Correctamente'
									 return(2) 
									 PRINT N'los Datos No fueron Insertados Satisfactoriamente'

								SET @Mes += 1
							end 
							SET @CodTipoFinanciacion += 1 
						end 
				end try

				begin catch
				declare @error int, @message varchar(4000), @xstate int;
				select @error = ERROR_NUMBER(), @message = ERROR_MESSAGE(), @xstate = XACT_STATE();
				if @xstate = -1
					rollback;
				if @xstate = 1 and @trancount = 0
					rollback
				if @xstate = 1 and @trancount > 0
					rollback transaction usp_my_procedure_name;

				raiserror ('MD_Insertar_Actualizar_ProyectoActividadPO: %d: %s', 16, 1, @error, @message) ;
						end catch
				end 
					
						
	--			begin
	-- /***Consulta De Nuevo Avance para una actividad********/
	-- SELECT EmpresaInterventor.CodContacto, Proyecto.NomProyecto 
	-- FROM EmpresaInterventor 
	-- INNER JOIN Empresa  ON EmpresaInterventor.CodEmpresa = Empresa.id_empresa 
	-- INNER JOIN Proyecto ON Empresa.codproyecto = Proyecto.Id_Proyecto 
	-- WHERE Empresa.codproyecto = @_CodProyecto AND EmpresaInterventor.Inactivo = 0

	
	--Select distinct codcontacto from ProyectoContacto where CodProyecto = @_CodProyecto and codrol in (1,2,3) and inactivo=0
	--   end
		   
		  if @_caso = 'Update'
		    begin
			Declare @select Nvarchar(max)
			Declare @select1 Nvarchar(max)
			Declare @select2 Nvarchar(max)
			Declare @select3 Nvarchar(max)
			Declare @select4 Nvarchar(max)
		  
			set @select1 = 'Insert into proyectoactividadPOInterventor (CodProyecto,Item,NomActividad,Metas) 
		    values (@_CodProyecto,@Item,@NomActividad,@Metas)'

			PRINT @select1
			execute sp_executesql @select1

			           if (@@rowcount > 0)

							 set @select2 = 'DELETE proyectoactividadPOInterventorTMP 
							  where CodProyecto=@_CodProyecto AND Id_Actividad = @_Id_Actividad'
             
			      select @select = Id_Actividad from proyectoactividadPOInterventor ORDER BY Id_Actividad DESC                  
	               
				         if @select <> null
						  
						   set @select3 = 'update ProyectoActividadPOMesInterventor set CodTipoFinanciacion=@CodTipoFinanciacion, Valor=@Valor
						   where codactividad=@_Id_Actividad and mes=@Mes'

						   set @select4 ='Update AvanceActividadPOMes set Mes = @Mes
							,CodTipoFinanciacion = @CodTipoFinanciacion
							,Valor = @Valor
							,Observaciones = @Observaciones
							,CodContacto = @_CodContacto
						 	WHERE CodActividad= @_Id_Actividad AND mes=@Mes AND 
							CodTipoFinanciacion= @CodTipoFinanciacion'
				           
						   --print @select2
						   SET @select2 = @select2 +  @select3 + @select4
	                       EXECUTE sp_executesql @select2 
							
							if (@@ROWCOUNT> 0 and @CodGrupo =  @cons_interventor)
							          begin
							         Insert into ProyectoActividadPOMesInterventor (CodActividad,Mes,CodTipoFinanciacion,Valor) 
							         values (@_Id_Actividad,@Mes,@CodTipoFinanciacion,@Valor)

							         if @@rowcount = 0 goto exception
	                                 --exception: rollback
									  exception: print 'Los datos no fueron Insertados Correctamente'
	                                 return(0)
										
									 end 
                             else 
							 
							    DELETE ProyectoActividadPOMesInterventorTMP
								where CodActividad=@_Id_Actividad

								DELETE FROM proyectoactividadPOInterventorTMP
								where CodProyecto=@_CodProyecto and Id_Actividad=@_Id_Actividad


								DELETE FROM ProyectoactividadPOMesInterventorTMP 
								where CodActividad=@_Id_Actividad

								if @@rowcount = 0 goto exception_
	                                 exception_: rollback
	                                 return(0)
									  
							if (@@ROWCOUNT> 0 and @CodGrupo = @Const_GerenteInterventor)

							  begin
									  set @select = '
									  Update proyectoactividadPOInterventor set CodProyecto = @_CodProyecto
						 			  ,item = @item
						 			  ,NomActividad = @_NomActividad 
						 			  ,Metas = @Metas
									  WHERE CodProyecto=@_CodProyecto and Id_Actividad= @_Id_Actividad'

									  set @select1 = 'DELETE FROM proyectoactividadPOInterventorTMP 
								      where CodProyecto= @_CodProyecto and Id_Actividad=@_Id_Actividad'

									  set @select2 = 'DELETE ProyectoActividadPOMesInterventor 
								      where CodActividad=@_Id_Actividad'

									 
									  set @select = @select + @select +@select2

									   EXECUTE sp_executesql @select

									   if (@@ROWCOUNT>0)

									   update ProyectoActividadPOMesInterventor set CodTipoFinanciacion= @CodTipoFinanciacion,Valor=@Valor
							           where codactividad=@_Id_Actividad and mes=@Mes
									 
									  Insert into ProyectoActividadPOMesInterventor (CodActividad,Mes,CodTipoFinanciacion,Valor)
								      values (@_Id_Actividad,@Mes,@CodTipoFinanciacion,@Valor)

									  DELETE ProyectoActividadPOMesInterventorTMP
								      where CodActividad=@_Id_Actividad										
							 end 

			end




	end