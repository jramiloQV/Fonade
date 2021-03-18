CREATE VIEW dbo.ListadoUsuariosCompleto
AS
SELECT DISTINCT 
                      dbo.Contacto.Id_Contacto, dbo.Contacto.Identificacion, dbo.Contacto.Nombres + ' ' + dbo.Contacto.Apellidos AS Nombres, 
                      (CASE WHEN Contacto.fechaActualizacion = '2014-01-14 09:53:51.293' OR
                      h.FechaAcualizacion = NULL THEN h.FechaAcualizacion ELSE Contacto.fechaActualizacion END) AS [Fecha ultimo ingreso], dbo.Contacto.Inactivo
FROM         dbo.Contacto INNER JOIN
                          (SELECT     CodContacto, MAX(fechaActualizacion) AS FechaAcualizacion
                            FROM          dbo.ContactoHistorico
                            GROUP BY CodContacto) AS h ON dbo.Contacto.Id_Contacto = h.CodContacto LEFT OUTER JOIN
                      dbo.GrupoContacto ON dbo.Contacto.Id_Contacto = dbo.GrupoContacto.CodContacto LEFT OUTER JOIN
                      dbo.Grupo ON dbo.Grupo.Id_Grupo = dbo.GrupoContacto.CodGrupo
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
         Begin Table = "Contacto"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 114
               Right = 228
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "h"
            Begin Extent = 
               Top = 6
               Left = 266
               Bottom = 84
               Right = 433
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "GrupoContacto"
            Begin Extent = 
               Top = 6
               Left = 471
               Bottom = 84
               Right = 622
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Grupo"
            Begin Extent = 
               Top = 6
               Left = 660
               Bottom = 99
               Right = 811
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
E', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'ListadoUsuariosCompleto';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'nd
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'ListadoUsuariosCompleto';




GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'ListadoUsuariosCompleto';

