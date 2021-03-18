CREATE VIEW dbo.pagos
AS
SELECT     dbo.Empresa.razonsocial, dbo.PagoActividad.CodProyecto, dbo.PagoActividad.Id_PagoActividad, dbo.PagoActividad.CantidadDinero, 
                      dbo.PagoActividad.Estado, dbo.PagoBeneficiario.Id_PagoBeneficiario, dbo.TipoIdentificacion.NomTipoIdentificacion, 
                      dbo.PagoBeneficiario.NumIdentificacion, dbo.PagoBeneficiario.Nombre, dbo.PagoBeneficiario.Apellido, 
                      dbo.PagoBeneficiario.RazonSocial AS razonsocialbeneficiario, dbo.PagoBanco.NomBanco, dbo.TipoCuenta.NomTipoCuenta, 
                      dbo.PagoBeneficiario.NumCuenta, dbo.PagosActaSolicitudPagos.CodPagosActaSolicitudes AS numacta, 
                      dbo.PagosActaSolicitudPagos.CodPagoActividad
FROM         dbo.TipoIdentificacion INNER JOIN
                      dbo.PagoActividad INNER JOIN
                      dbo.Empresa ON dbo.PagoActividad.CodProyecto = dbo.Empresa.codproyecto INNER JOIN
                      dbo.PagosActaSolicitudPagos ON dbo.PagoActividad.Id_PagoActividad = dbo.PagosActaSolicitudPagos.CodPagoActividad INNER JOIN
                      dbo.PagoBeneficiario ON dbo.PagoActividad.CodPagoBeneficiario = dbo.PagoBeneficiario.Id_PagoBeneficiario INNER JOIN
                      dbo.PagoBanco ON dbo.PagoBeneficiario.CodPagoBanco = dbo.PagoBanco.Id_Banco ON 
                      dbo.TipoIdentificacion.Id_TipoIdentificacion = dbo.PagoBeneficiario.CodTipoIdentificacion INNER JOIN
                      dbo.TipoCuenta ON dbo.PagoBeneficiario.TipoCuenta = dbo.TipoCuenta.Id_TipoCuenta
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
         Begin Table = "TipoIdentificacion"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 114
               Right = 223
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "PagoActividad"
            Begin Extent = 
               Top = 114
               Left = 38
               Bottom = 222
               Right = 258
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Empresa"
            Begin Extent = 
               Top = 6
               Left = 261
               Bottom = 114
               Right = 437
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "PagosActaSolicitudPagos"
            Begin Extent = 
               Top = 222
               Left = 38
               Bottom = 330
               Right = 238
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "PagoBeneficiario"
            Begin Extent = 
               Top = 222
               Left = 276
               Bottom = 330
               Right = 467
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "PagoBanco"
            Begin Extent = 
               Top = 114
               Left = 296
               Bottom = 192
               Right = 447
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "TipoCuenta"
            Begin Extent = 
               Top = 330
               Left = 38
               Bottom = 408
               Right = 194
  ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'pagos';




GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'          End
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'pagos';




GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'pagos';

