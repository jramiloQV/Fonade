CREATE VIEW dbo.VW_C1_DATOSVARIOSSINEMPRESA
AS
SELECT        T6.NomProyecto, T1.NomCiudad, T2.Nombres, T2.Apellidos, T2.Email, T2.Direccion, T2.Telefono, T4.NomDepartamento, T5.NomUnidad, T6.Id_Proyecto, T6.Sumario, T8.NomSector, T9.NomSubSector
FROM            dbo.Ciudad AS T1 INNER JOIN
                         dbo.Contacto AS T2 ON T1.Id_Ciudad = T2.CodCiudad INNER JOIN
                         dbo.departamento AS T4 ON T1.CodDepartamento = T4.Id_Departamento INNER JOIN
                         dbo.ProyectoContacto AS T7 ON T2.Id_Contacto = T7.CodContacto INNER JOIN
                         dbo.ConvocatoriaProyecto AS T3 INNER JOIN
                         dbo.Proyecto AS T6 ON T3.CodProyecto = T6.Id_Proyecto INNER JOIN
                         dbo.Institucion AS T5 ON T6.CodInstitucion = T5.Id_Institucion ON T7.CodProyecto = T6.Id_Proyecto INNER JOIN
                         dbo.SubSector AS T9 ON T6.CodSubSector = T9.Id_SubSector INNER JOIN
                         dbo.Sector AS T8 ON T9.CodSector = T8.Id_Sector
WHERE        (T3.CodConvocatoria = 1) AND (T6.CodEstado >= 7) AND (T7.CodRol = 3) AND (T7.Inactivo = 0)
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
         Begin Table = "T1"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 225
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T2"
            Begin Extent = 
               Top = 6
               Left = 263
               Bottom = 136
               Right = 477
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T4"
            Begin Extent = 
               Top = 138
               Left = 38
               Bottom = 251
               Right = 230
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T7"
            Begin Extent = 
               Top = 138
               Left = 268
               Bottom = 268
               Right = 468
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T3"
            Begin Extent = 
               Top = 252
               Left = 38
               Bottom = 382
               Right = 259
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T6"
            Begin Extent = 
               Top = 384
               Left = 38
               Bottom = 514
               Right = 238
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T5"
            Begin Extent = 
               Top = 384
               Left = 276
               Bottom = 514
               Right = 467
            End
            DisplayFlags = 280
            TopColumn = 0
        ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'VW_C1_DATOSVARIOSSINEMPRESA';




GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N' End
         Begin Table = "T9"
            Begin Extent = 
               Top = 516
               Left = 38
               Bottom = 646
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T8"
            Begin Extent = 
               Top = 516
               Left = 246
               Bottom = 646
               Right = 416
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'VW_C1_DATOSVARIOSSINEMPRESA';




GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'VW_C1_DATOSVARIOSSINEMPRESA';

