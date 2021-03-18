<%@ Page Title="FONDO EMPRENDER" Language="C#" MasterPageFile="~/Emergente.Master"
    AutoEventWireup="true" CodeBehind="CoordinadorPago.aspx.cs" Inherits="Fonade.FONADE.Fiduciaria.CoordinadorPago" %>

<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>--%>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <table width="100%" class="Login">
        <tr>
            <td>
                <asp:Label ID="NombrePagina" runat="server" Text="Label" />
            </td>
            <td>
                <asp:Label ID="NombreUsuario" runat="server" Text="Label" />
            </td>
            <td>
                <asp:Label ID="FechaActual" runat="server" Text="Label" />
            </td>
        </tr>
    </table>
    <table width="100%" border="0" cellpadding="2" align="center">
        <tbody>
            <tr>
                <td width="200">
                    <b>Número Solicitud</b>
                </td>
                <td>
                    <asp:Label ID="NumeroSolicitud" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <b>Número Proyecto</b>
                </td>
                <td>
                    <asp:Label ID="NumeroProyecto" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <b>Nombre Proyecto</b>
                </td>
                <td>
                    <asp:Label ID="NombreProyecto" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <b>Valor Solicitud</b>
                </td>
                <td>
                    <asp:Label ID="ValorSolicitud" runat="server" DataFormatString="{0:C}"/>
                </td>
            </tr>
            <tr>
                <td>
                    <b>Concepto Solicitud</b>
                </td>
                <td>
                    <asp:Label ID="ConceptoSolicitud" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <b>Fecha Solicitud</b>
                </td>
                <td>
                    <asp:Label ID="FechaSolicitud" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <b>Archivos Adjuntos</b>
                </td>
                <td>
                    <asp:HyperLink ID="ArchivosAdjuntos" runat="server" />
                    <%--<table>

					<tbody><tr>
						<td><a href="Documentos/Pagos/51/Pagos_102258/planilla mes octubre_2013.pdf" target="_blank">Planilla mes octubre de 2013</a></td>
					</tr>

				</tbody></table>--%>
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
    <style type="text/css">
        .style11
        {
            height: 65px;
        }
        .style13
        {
            width: 145px;
            height: 27px;
        }
        .style16
        {
            height: 65px;
            width: 104px;
        }
        .style19
        {
            height: 65px;
            width: 160px;
        }
        .style20
        {
            width: 104px;
            height: 63px;
        }
        .style21
        {
            height: 63px;
        }
        .style22
        {
            width: 160px;
            height: 63px;
        }
        .style24
        {
            height: 60px;
        }
        .style26
        {
            width: 104px;
            height: 27px;
        }
        .style27
        {
            height: 27px;
        }
        .style28
        {
            width: 160px;
            height: 27px;
        }
        .style29
        {
            width: 145px;
            font-weight: bold;
            height: 30px;
        }
        .style30
        {
            width: 104px;
            height: 30px;
        }
        .style31
        {
            height: 30px;
        }
        .style32
        {
            width: 160px;
            height: 30px;
        }
        .style33
        {
            width: 104px;
            height: 19px;
        }
        .style34
        {
            height: 19px;
        }
        .style35
        {
            width: 160px;
            height: 19px;
        }
    </style>
</asp:Content>
