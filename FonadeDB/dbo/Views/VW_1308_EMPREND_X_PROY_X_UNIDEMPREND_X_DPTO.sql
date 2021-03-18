CREATE VIEW dbo.VW_1308_EMPREND_X_PROY_X_UNIDEMPREND_X_DPTO
AS
SELECT     TOP (100) PERCENT T3.CodInstitucion, T4.CodConvocatoria, MAX(T5.Id_ProyectoContacto) AS ProyectoContacto, T6.Nombres, T6.Apellidos, T6.Identificacion, 
                      T3.NomProyecto, T6.Direccion, T6.Telefono, T6.Email, T7.NomCiudad, T8.NomDepartamento
FROM         dbo.Institucion AS T1 INNER JOIN
                      dbo.Ciudad AS T2 ON T1.CodCiudad = T2.Id_Ciudad INNER JOIN
                      dbo.Proyecto AS T3 ON T3.CodInstitucion = T1.Id_Institucion INNER JOIN
                      dbo.ConvocatoriaProyecto AS T4 ON T4.CodProyecto = T3.Id_Proyecto INNER JOIN
                      dbo.ProyectoContacto AS T5 ON T5.CodProyecto = T3.Id_Proyecto INNER JOIN
                      dbo.Contacto AS T6 ON T5.CodContacto = T6.Id_Contacto INNER JOIN
                      dbo.Ciudad AS T7 ON T7.Id_Ciudad = T6.CodCiudad INNER JOIN
                      dbo.departamento AS T8 ON T8.Id_Departamento = T7.CodDepartamento
WHERE     (T5.CodRol = 3) AND (T2.CodDepartamento = 76)
GROUP BY T3.CodInstitucion, T4.CodConvocatoria, T6.Nombres, T6.Apellidos, T6.Identificacion, T3.NomProyecto, T6.Direccion, T6.Telefono, T6.Email, T7.NomCiudad, 
                      T8.NomDepartamento
ORDER BY T3.CodInstitucion, T4.CodConvocatoria
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
         Begin Table = "T3"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 114
               Right = 232
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T4"
            Begin Extent = 
               Top = 6
               Left = 270
               Bottom = 114
               Right = 483
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T5"
            Begin Extent = 
               Top = 6
               Left = 521
               Bottom = 114
               Right = 720
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T6"
            Begin Extent = 
               Top = 6
               Left = 758
               Bottom = 114
               Right = 964
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T2"
            Begin Extent = 
               Top = 6
               Left = 1002
               Bottom = 114
               Right = 1186
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T8"
            Begin Extent = 
               Top = 114
               Left = 38
               Bottom = 207
               Right = 224
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T1"
            Begin Extent = 
               Top = 114
               Left = 262
               Bottom = 222
               Right = 451
            End
            DisplayFlags = 280
            TopColumn = 0
         E', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'VW_1308_EMPREND_X_PROY_X_UNIDEMPREND_X_DPTO';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'nd
         Begin Table = "T7"
            Begin Extent = 
               Top = 114
               Left = 489
               Bottom = 222
               Right = 673
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
      Begin ColumnWidths = 12
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 13035
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'VW_1308_EMPREND_X_PROY_X_UNIDEMPREND_X_DPTO';




GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'VW_1308_EMPREND_X_PROY_X_UNIDEMPREND_X_DPTO';

