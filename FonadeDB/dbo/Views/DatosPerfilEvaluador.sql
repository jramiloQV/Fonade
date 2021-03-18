CREATE VIEW dbo.DatosPerfilEvaluador
AS
SELECT     c.Id_Contacto, c.Nombres + ' ' + c.Apellidos AS Evaluador, ti.NomTipoIdentificacion, c.Identificacion, c.Email, c.Direccion, c.Telefono, c.Fax, e.Cuenta, 
                      tc.NomTipoCuenta, b.nomBanco, e.MaximoPlanes, c.Experiencia, s.NomSector AS SectorPrincipal, c.Intereses, s1.NomSector AS SectorSecundario, 
                      c.HojaVida, ne.NomNivelEstudio, ce.TituloObtenido, ce.AnoTitulo, ce.Institucion, ciu.NomCiudad
FROM         dbo.Contacto AS c LEFT OUTER JOIN
                      dbo.Evaluador AS e ON c.Id_Contacto = e.CodContacto LEFT OUTER JOIN
                      dbo.TipoCuenta AS tc ON tc.Id_TipoCuenta = e.CodTipoCuenta LEFT OUTER JOIN
                      dbo.Banco AS b ON b.Id_Banco = e.CodBanco LEFT OUTER JOIN
                      dbo.EvaluadorSector AS es ON c.Id_Contacto = es.CodContacto INNER JOIN
                      dbo.Sector AS s ON s.Id_Sector = es.CodSector AND es.Experiencia = 'P' LEFT OUTER JOIN
                      dbo.Sector AS s1 ON s1.Id_Sector = es.CodSector AND es.Experiencia = 'S' LEFT OUTER JOIN
                      dbo.ContactoEstudio AS ce ON c.Id_Contacto = ce.CodContacto LEFT OUTER JOIN
                      dbo.NivelEstudio AS ne ON ne.Id_NivelEstudio = ce.CodNivelEstudio LEFT OUTER JOIN
                      dbo.Ciudad AS ciu ON ciu.Id_Ciudad = ce.CodCiudad INNER JOIN
                      dbo.TipoIdentificacion AS ti ON c.CodTipoIdentificacion = ti.Id_TipoIdentificacion INNER JOIN
                      dbo.GrupoContacto AS gc ON c.Id_Contacto = gc.CodContacto INNER JOIN
                      dbo.Grupo AS g ON gc.CodGrupo = g.Id_Grupo
WHERE     (g.Id_Grupo = 11)
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
         Begin Table = "c"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 114
               Right = 228
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "e"
            Begin Extent = 
               Top = 6
               Left = 266
               Bottom = 114
               Right = 453
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tc"
            Begin Extent = 
               Top = 6
               Left = 491
               Bottom = 84
               Right = 647
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "b"
            Begin Extent = 
               Top = 6
               Left = 685
               Bottom = 99
               Right = 836
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "es"
            Begin Extent = 
               Top = 84
               Left = 491
               Bottom = 192
               Right = 660
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "s"
            Begin Extent = 
               Top = 102
               Left = 698
               Bottom = 210
               Right = 849
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "s1"
            Begin Extent = 
               Top = 114
               Left = 38
               Bottom = 222
               Right = 189
            End
            DisplayFlags = 280
            TopColumn = 0
         End
   ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'DatosPerfilEvaluador';




GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'      Begin Table = "ce"
            Begin Extent = 
               Top = 114
               Left = 227
               Bottom = 222
               Right = 423
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ne"
            Begin Extent = 
               Top = 192
               Left = 461
               Bottom = 300
               Right = 681
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ciu"
            Begin Extent = 
               Top = 210
               Left = 719
               Bottom = 318
               Right = 887
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ti"
            Begin Extent = 
               Top = 222
               Left = 38
               Bottom = 330
               Right = 223
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "gc"
            Begin Extent = 
               Top = 222
               Left = 261
               Bottom = 300
               Right = 412
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "g"
            Begin Extent = 
               Top = 300
               Left = 261
               Bottom = 393
               Right = 412
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'DatosPerfilEvaluador';




GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'DatosPerfilEvaluador';

