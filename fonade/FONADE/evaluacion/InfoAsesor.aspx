<%@ Page Title="FONDO EMPRENDER" Language="C#" MasterPageFile="~/Emergente.Master" AutoEventWireup="true" CodeBehind="InfoAsesor.aspx.cs" Inherits="Fonade.FONADE.evaluacion.InfoAsesor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style10
        {
            width: 152px;
        }
    </style>
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <title>Información De Asesor</title>
    <br/>
     <table width="470" border="0" style="margin-left: 20px;">
          <tr>
            <td colspan="2">
                <h1><asp:Label runat="server" ID="lbl_Titulo" style="font-weight: 700">Información De Asesor</asp:Label></h1>
              </td>
            <td colspan="2" align="right">
                <asp:Label ID="l_fechaActual" runat="server" style="font-weight: 700"></asp:Label>
              </td>
          </tr>
        </table>
    <table width="470" cellpadding="0" cellspacing="4" style="margin-left: 20px;">
			    <tr>
			        <td class="style10">Nombre:</td>
			        <td><asp:Label runat="server" ID="nombre"/></td>
			    </tr>
			    <tr>
			        <td class="style10">Correo Electr&oacute;nico:</td>
			        <td> <asp:Label runat="server" ID="email"/></td>
			    </tr>
			    <tr>
			        <td class="style10">N&uacute;mero Telef&oacute;nico</td>
			        <td> <asp:Label runat="server" ID="telefono"/></td>
			    </tr>
			    <tr>
			        <td class="style10">&nbsp;</td>
			        <td> &nbsp;</td>
			    </tr>
			    <tr><td align="center" colspan="2"> 
                    <asp:Button ID="Button1" runat="server" Text="Cerrar" onclick="Button1_Click" />
                    </td></tr>
			</table>
</asp:Content>
