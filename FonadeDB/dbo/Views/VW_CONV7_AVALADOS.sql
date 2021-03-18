CREATE VIEW dbo.VW_CONV7_AVALADOS
AS
SELECT     T1.NomCiudad, T4.NomDepartamento, T5.NomInstitucion, T5.NomUnidad, T6.Id_Proyecto, T6.NomProyecto, T7.Recursos, T8.Fecha, 
                      T8.CodConvocatoria, T9.NomSector, T10.NomSubSector
FROM         dbo.Convocatoria AS T2 INNER JOIN
                      dbo.ConvocatoriaProyecto AS T3 ON T2.Id_Convocatoria = T3.CodConvocatoria INNER JOIN
                      dbo.Ciudad AS T1 INNER JOIN
                      dbo.Proyecto AS T6 ON T1.Id_Ciudad = T6.CodCiudad INNER JOIN
                      dbo.departamento AS T4 ON T1.CodDepartamento = T4.Id_Departamento ON T3.CodProyecto = T6.Id_Proyecto INNER JOIN
                      dbo.Institucion AS T5 ON T6.CodInstitucion = T5.Id_Institucion LEFT OUTER JOIN
                      dbo.ProyectoFinanzasIngresos AS T7 ON T6.Id_Proyecto = T7.CodProyecto INNER JOIN
                      dbo.ProyectoFormalizacion AS T8 ON T6.Id_Proyecto = T8.codProyecto AND T2.Id_Convocatoria = T8.CodConvocatoria INNER JOIN
                      dbo.SubSector AS T10 ON T6.CodSubSector = T10.Id_SubSector INNER JOIN
                      dbo.Sector AS T9 ON T10.CodSector = T9.Id_Sector
WHERE     (T2.Id_Convocatoria = 80)
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
         Begin Table = "T2"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 114
               Right = 203
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T3"
            Begin Extent = 
               Top = 6
               Left = 241
               Bottom = 114
               Right = 438
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T1"
            Begin Extent = 
               Top = 114
               Left = 38
               Bottom = 222
               Right = 206
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T6"
            Begin Extent = 
               Top = 114
               Left = 244
               Bottom = 222
               Right = 422
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T4"
            Begin Extent = 
               Top = 222
               Left = 38
               Bottom = 315
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T5"
            Begin Extent = 
               Top = 222
               Left = 246
               Bottom = 330
               Right = 419
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T7"
            Begin Extent = 
               Top = 318
               Left = 38
               Bottom = 396
               Right = 189
            End
            DisplayFlags = 280
            TopColumn = 0
        ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'VW_CONV7_AVALADOS';




GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N' End
         Begin Table = "T8"
            Begin Extent = 
               Top = 330
               Left = 227
               Bottom = 438
               Right = 430
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T10"
            Begin Extent = 
               Top = 396
               Left = 38
               Bottom = 504
               Right = 189
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T9"
            Begin Extent = 
               Top = 438
               Left = 227
               Bottom = 546
               Right = 378
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'VW_CONV7_AVALADOS';




GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'VW_CONV7_AVALADOS';

