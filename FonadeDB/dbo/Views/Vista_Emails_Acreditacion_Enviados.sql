CREATE VIEW dbo.Vista_Emails_Acreditacion_Enviados
AS
SELECT DISTINCT 
                      p.Id_Proyecto, p.NomProyecto, hea.Email, hea.Fecha, c.Nombres + ' ' + c.Apellidos AS UsuarioRecibe, c3.Nombres + ' ' + c3.Apellidos AS UsuarioEnvia,
                       hea.CodConvocatoria, con.NomConvocatoria
FROM         dbo.HistoricoEmailAcreditacion AS hea INNER JOIN
                      dbo.ContactoHistoricoEmailAcreditacion AS chea ON chea.CodHistoricoEmailAcreditacion = hea.Id_HistoricoEmailAcreditacion INNER JOIN
                      dbo.Contacto AS c ON hea.CodContacto = c.Id_Contacto INNER JOIN
                      dbo.Contacto AS c2 ON chea.CodContacto = c2.Id_Contacto INNER JOIN
                      dbo.Contacto AS c3 ON hea.CodContactoEnvia = c3.Id_Contacto INNER JOIN
                      dbo.Proyecto AS p ON hea.CodProyecto = p.Id_Proyecto INNER JOIN
                      dbo.Convocatoria AS con ON hea.CodConvocatoria = con.Id_Convocatoria
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
         Begin Table = "hea"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 114
               Right = 258
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "chea"
            Begin Extent = 
               Top = 6
               Left = 296
               Bottom = 84
               Right = 519
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "c"
            Begin Extent = 
               Top = 6
               Left = 557
               Bottom = 114
               Right = 747
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "c2"
            Begin Extent = 
               Top = 6
               Left = 785
               Bottom = 114
               Right = 975
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "c3"
            Begin Extent = 
               Top = 84
               Left = 296
               Bottom = 192
               Right = 486
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "p"
            Begin Extent = 
               Top = 146
               Left = 42
               Bottom = 254
               Right = 220
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "con"
            Begin Extent = 
               Top = 114
               Left = 524
               Bottom = 222
               Right = 689
            End
            DisplayFlags = 280
            TopColumn = 0
         E', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'Vista_Emails_Acreditacion_Enviados';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'nd
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'Vista_Emails_Acreditacion_Enviados';




GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'Vista_Emails_Acreditacion_Enviados';

