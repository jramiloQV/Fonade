CREATE VIEW dbo.Vista_Ingreso_Asesores_UnidadesExternas
AS
SELECT     TOP (100) PERCENT i.Id_Institucion, i.NomInstitucion + ' (' + i.NomUnidad + ')' AS Institucion, c.Nombres + ' ' + c.Apellidos AS Asesor, c.Email, 
                      li.FechaUltimoIngreso, li.NoLogins
FROM         dbo.Institucion AS i LEFT OUTER JOIN
                      dbo.Contacto AS c ON i.Id_Institucion = c.CodInstitucion LEFT OUTER JOIN
                      dbo.LogIngreso AS li ON c.Id_Contacto = li.CodContacto INNER JOIN
                      dbo.GrupoContacto AS gc ON c.Id_Contacto = gc.CodContacto INNER JOIN
                      dbo.Grupo ON dbo.Grupo.Id_Grupo = gc.CodGrupo
WHERE     (i.CodTipoInstitucion = 3) AND (i.Inactivo = 0) AND (dbo.Grupo.Id_Grupo = 5)
ORDER BY i.Id_Institucion
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
         Begin Table = "i"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 114
               Right = 211
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "c"
            Begin Extent = 
               Top = 6
               Left = 249
               Bottom = 114
               Right = 439
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "li"
            Begin Extent = 
               Top = 6
               Left = 477
               Bottom = 114
               Right = 652
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "gc"
            Begin Extent = 
               Top = 6
               Left = 690
               Bottom = 84
               Right = 841
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Grupo"
            Begin Extent = 
               Top = 6
               Left = 879
               Bottom = 99
               Right = 1030
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
         Output', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'Vista_Ingreso_Asesores_UnidadesExternas';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N' = 720
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'Vista_Ingreso_Asesores_UnidadesExternas';




GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'Vista_Ingreso_Asesores_UnidadesExternas';

