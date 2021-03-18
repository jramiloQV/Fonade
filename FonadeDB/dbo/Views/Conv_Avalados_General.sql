CREATE VIEW dbo.Conv_Avalados_General
AS
SELECT     T1.NomCiudad, d.NomDepartamento, T2.Nombres, T2.Apellidos, T2.Identificacion, c2.NomCiudad AS LugarExpedicion, T2.FechaNacimiento, T2.Email, 
                      T2.Telefono, R.Nombre, T3.NomInstitucion, T3.NomUnidad, T4.Id_Proyecto, T4.NomProyecto, T4.Sumario, T5.CodRol, T5.Inactivo, T6.Fecha, 
                      T6.CodConvocatoria, T7.NomSector, T8.NomSubSector, c.NomConvocatoria, PFI.Recursos
FROM         dbo.ProyectoContacto AS T5 INNER JOIN
                      dbo.Contacto AS T2 ON T5.CodContacto = T2.Id_Contacto INNER JOIN
                      dbo.Ciudad AS T1 INNER JOIN
                      dbo.Proyecto AS T4 ON T1.Id_Ciudad = T4.CodCiudad INNER JOIN
                      dbo.Institucion AS T3 ON T4.CodInstitucion = T3.Id_Institucion ON T5.CodProyecto = T4.Id_Proyecto INNER JOIN
                      dbo.ProyectoFormalizacion AS T6 ON T4.Id_Proyecto = T6.codProyecto INNER JOIN
                      dbo.SubSector AS T8 ON T4.CodSubSector = T8.Id_SubSector INNER JOIN
                      dbo.Sector AS T7 ON T8.CodSector = T7.Id_Sector INNER JOIN
                      dbo.Convocatoria AS c ON c.Id_Convocatoria = T6.CodConvocatoria INNER JOIN
                      dbo.Rol AS R ON R.Id_Rol = T5.CodRol LEFT OUTER JOIN
                      dbo.ProyectoFinanzasIngresos AS PFI ON T4.Id_Proyecto = PFI.CodProyecto INNER JOIN
                      dbo.departamento AS d ON d.Id_Departamento = T1.CodDepartamento LEFT OUTER JOIN
                      dbo.Ciudad AS c2 ON c2.Id_Ciudad = T2.LugarExpedicionDI
WHERE     (T5.CodRol <= 3)
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
         Begin Table = "T5"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 114
               Right = 221
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T2"
            Begin Extent = 
               Top = 6
               Left = 259
               Bottom = 114
               Right = 449
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T1"
            Begin Extent = 
               Top = 6
               Left = 487
               Bottom = 114
               Right = 655
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T4"
            Begin Extent = 
               Top = 114
               Left = 38
               Bottom = 222
               Right = 216
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T3"
            Begin Extent = 
               Top = 114
               Left = 254
               Bottom = 222
               Right = 427
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T6"
            Begin Extent = 
               Top = 114
               Left = 465
               Bottom = 222
               Right = 668
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T8"
            Begin Extent = 
               Top = 222
               Left = 38
               Bottom = 330
               Right = 189
            End
            DisplayFlags = 280
            TopColumn = 0
         ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'Conv_Avalados_General';




GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'End
         Begin Table = "T7"
            Begin Extent = 
               Top = 222
               Left = 227
               Bottom = 330
               Right = 378
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "c"
            Begin Extent = 
               Top = 6
               Left = 693
               Bottom = 114
               Right = 858
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "R"
            Begin Extent = 
               Top = 114
               Left = 706
               Bottom = 222
               Right = 857
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "PFI"
            Begin Extent = 
               Top = 222
               Left = 416
               Bottom = 300
               Right = 567
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "d"
            Begin Extent = 
               Top = 222
               Left = 605
               Bottom = 315
               Right = 775
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "c2"
            Begin Extent = 
               Top = 222
               Left = 813
               Bottom = 330
               Right = 981
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'Conv_Avalados_General';




GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'Conv_Avalados_General';

