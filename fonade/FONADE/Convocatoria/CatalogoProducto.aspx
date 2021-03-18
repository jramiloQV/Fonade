<%@ Page Title="FONDO EMPRENDER" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="CatalogoProducto.aspx.cs" Inherits="Fonade.FONADE.Convocatoria.CatalogoProducto" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style10
        {
            width: 100%;
        }
        .style11
        {
            width: 299px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <style type="text/css">
        .titulos
        {
            text-align: center;
        }
        .celda
        {
            text-align:center;
         }
    </style>
    <table class="style10">
        <tr>
            <td>
                <asp:Label ID="L_NombreProductoServicio" runat="server" Text="Nombre del Producto o Servicio:"></asp:Label>
            </td>
            <td colspan="3">
                <asp:TextBox ID="TB_NombreProductoServicio" runat="server" Width="300px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="L_PosicionArancelaria" runat="server" 
                    Text="Posición Arancelaria:"></asp:Label>
            </td>
            <td class="style11">
                <asp:TextBox ID="TB_PosicionArancelariacodigo" runat="server" Width="99px"></asp:TextBox>&nbsp;&nbsp;
                <asp:TextBox ID="TB_PosicionArancelariadescripcion" runat="server" Width="185px"></asp:TextBox>
            </td>
            <td colspan="2" rowspan="2">
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="L_PrecioLanzamiento" runat="server" 
                    Text="Precio de Lanzamiento:"></asp:Label>
            </td>
            <td class="style11">
                <asp:TextBox ID="TB_PrecioLanzamiento" runat="server" 
                    Width="185px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="L_IVA" runat="server" 
                    Text="%IVA:"></asp:Label>
            </td>
            <td class="style11">
                <asp:TextBox ID="TB_IVA" runat="server" Width="99px"></asp:TextBox></td>
            <td>
                <asp:Label ID="L_Retencionfuente" runat="server" 
                    Text="%Retencion en la fuente:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="TB_Retencionfuente" runat="server" Width="99px"></asp:TextBox></td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="L_VentasCredito" runat="server" 
                    Text="%Ventas a Crédito:"></asp:Label>
            </td>
            <td class="style11">
                <asp:TextBox ID="TB_VentasCredito" runat="server" Width="99px"></asp:TextBox></td>
            <td colspan="2">
                &nbsp;</td>
        </tr>
        <tr>
            <td colspan="4">
            <br />
            <br />
                <asp:Label ID="L_titulo" runat="server" Text="PROYECCION DE VENTAS" Width="100%" Font-Bold="true" BackColor="Blue" ForeColor="White" BorderStyle="NotSet" CssClass="titulos"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="celda" style="font-weight: bold">
                <asp:Label ID="L_periodo" runat="server" 
                    Text="PERIODOS" Font-Bold="true"></asp:Label>
            </td>
            <td class="celda" style="font-weight: bold">
                <asp:Label ID="L_anio1" runat="server" 
                    Text="Año 1" Font-Bold="true"></asp:Label>
            </td>
            <td class="celda" style="font-weight: bold">
                <asp:Label ID="L_anio2" runat="server" 
                    Text="Año 2" Font-Bold="true"></asp:Label>
            </td>
            <td class="celda" style="font-weight: bold">
                <asp:Label ID="L_anio3" runat="server" 
                    Text="Año 3" Font-Bold="true"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="celda">
                &nbsp;</td>
            <td class="celda">
                &nbsp;</td>
            <td class="celda">
                &nbsp;</td>
            <td class="celda">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="celda">
                &nbsp;</td>
            <td class="celda">
                &nbsp;</td>
            <td class="celda">
                &nbsp;</td>
            <td class="celda">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="celda">
                &nbsp;</td>
            <td class="celda">
                &nbsp;</td>
            <td class="celda">
                &nbsp;</td>
            <td class="celda">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="celda">
                &nbsp;</td>
            <td class="celda">
                &nbsp;</td>
            <td class="celda">
                &nbsp;</td>
            <td class="celda">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="celda">
                &nbsp;</td>
            <td class="celda">
                &nbsp;</td>
            <td class="celda">
                &nbsp;</td>
            <td class="celda">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="celda">
                &nbsp;</td>
            <td class="celda">
                &nbsp;</td>
            <td class="celda">
                &nbsp;</td>
            <td class="celda">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="celda">
                &nbsp;</td>
            <td class="celda">
                &nbsp;</td>
            <td class="celda">
                &nbsp;</td>
            <td class="celda">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="celda">
                &nbsp;</td>
            <td class="celda">
                &nbsp;</td>
            <td class="celda">
                &nbsp;</td>
            <td class="celda">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="celda">
                &nbsp;</td>
            <td class="celda">
                &nbsp;</td>
            <td class="celda">
                &nbsp;</td>
            <td class="celda">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="celda">
                &nbsp;</td>
            <td class="celda">
                &nbsp;</td>
            <td class="celda">
                &nbsp;</td>
            <td class="celda">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="celda">
                &nbsp;</td>
            <td class="celda">
                &nbsp;</td>
            <td class="celda">
                &nbsp;</td>
            <td class="celda">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="celda">
                &nbsp;</td>
            <td class="celda">
                &nbsp;</td>
            <td class="celda">
                &nbsp;</td>
            <td class="celda">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="celda">
                &nbsp;</td>
            <td class="celda">
                &nbsp;</td>
            <td class="celda">
                &nbsp;</td>
            <td class="celda">
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td class="style11">
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td class="style11">
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
    
</asp:Content>
