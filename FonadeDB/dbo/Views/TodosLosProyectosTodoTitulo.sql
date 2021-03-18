CREATE VIEW dbo.TodosLosProyectosTodoTitulo
AS
SELECT     TOP (100) PERCENT PC.CodProyecto, dbo.Proyecto.NomProyecto, C.Nombres, C.Apellidos, C.Identificacion, CE.TituloObtenido, CE.AnoTitulo, CE.Institucion, 
                      dbo.Ciudad.NomCiudad, dbo.NivelEstudio.NomNivelEstudio, dbo.ProgramaAcademico.NomProgramaAcademico, CE.Finalizado, CE.FechaGrado, CE.FechaUltimoCorte, 
                      CE.SemestresCursados, CE.FechaInicio, CE.fechaCreacion, CE.fechaCreacion AS Expr1, CE.fechaActualizacion, CE.FechaFinMaterias
FROM         dbo.ProyectoContacto AS PC INNER JOIN
                      dbo.Proyecto ON dbo.Proyecto.Id_Proyecto = PC.CodProyecto INNER JOIN
                      dbo.Contacto AS C ON C.Id_Contacto = PC.CodContacto LEFT OUTER JOIN
                      dbo.ContactoEstudio AS CE ON CE.CodContacto = PC.CodContacto LEFT OUTER JOIN
                      dbo.Ciudad ON dbo.Ciudad.Id_Ciudad = CE.CodCiudad LEFT OUTER JOIN
                      dbo.NivelEstudio ON dbo.NivelEstudio.Id_NivelEstudio = CE.CodNivelEstudio LEFT OUTER JOIN
                      dbo.ProgramaAcademico ON dbo.ProgramaAcademico.Id_ProgramaAcademico = CE.CodProgramaAcademico
WHERE     (PC.CodRol = 3) AND (PC.CodProyecto > 49767)
ORDER BY PC.CodProyecto, dbo.Proyecto.NomProyecto, C.Nombres, C.Apellidos, CE.AnoTitulo
GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "PC"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 114
               Right = 221
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Proyecto"
            Begin Extent = 
               Top = 6
               Left = 259
               Bottom = 114
               Right = 437
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "C"
            Begin Extent = 
               Top = 6
               Left = 475
               Bottom = 114
               Right = 665
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "CE"
            Begin Extent = 
               Top = 6
               Left = 703
               Bottom = 114
               Right = 899
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Ciudad"
            Begin Extent = 
               Top = 114
               Left = 38
               Bottom = 222
               Right = 206
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "NivelEstudio"
            Begin Extent = 
               Top = 114
               Left = 244
               Bottom = 222
               Right = 464
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ProgramaAcademico"
            Begin Extent = 
               Top = 114
               Left = 502
               Bottom = 222
               Right = 700
            End
            DisplayFlags = 280
   ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'TodosLosProyectosTodoTitulo';




GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'         TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 11
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'TodosLosProyectosTodoTitulo';




GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'TodosLosProyectosTodoTitulo';

