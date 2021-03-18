<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="SolicitudesDePagoSinDescargar.aspx.cs" Inherits="Fonade.FONADE.Fiduciaria.SolicitudesDePagoSinDescargar" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Styles/Site.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <table width="98%" border="0">
        <tr>
            <td class="style50">
                <h1>
                    <asp:Label runat="server" ID="lblTitulo" Text="Solicitudes de pago" Style="font-weight: 700"></asp:Label>
                </h1>
            </td>
            <td align="right">
            </td>
        </tr>
    </table>
    <asp:GridView ID="gvSolicitudesDePago" runat="server"
        Width="98%" BorderWidth="0" CellSpacing="1" CellPadding="4" AllowPaging="true" 
        OnRowCommand="detalleSolicitud_RowCommand"
        PageSize="5" AutoGenerateColumns="False" CssClass="Grilla" HeaderStyle-HorizontalAlign="Left" 
        DataSourceID="dsSolicitudesDePago" EmptyDataText="No se encontraron datos.">
        <Columns>
            <asp:TemplateField HeaderText="Número solicitud">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkmostrar" CommandArgument='<%# Eval("codigoActaPago") %>'
                        CommandName="verDetalleSolicitud" CausesValidation="False" Text='<%#Eval("codigoActaPago") %>'
                        runat="server" Font-Bold="true" ForeColor="Black" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Fecha de envío">
                <ItemTemplate>
                    <asp:Label ID="lbl_FechEnvi" Text='<%# Eval("fechaSolicitudPagoConFormato") %>' runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="Numero de solicitudes" DataField="numeroSolicitudesPago" HtmlEncode="false" />
            <asp:BoundField HeaderText="Firma" DataField="firma" HtmlEncode="false" />                       
        </Columns>
        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
    </asp:GridView>
    <asp:ObjectDataSource ID="dsSolicitudesDePago" runat="server" EnablePaging="true" 
        SelectMethod="getSolicitudesDePago"
        SelectCountMethod="getSolicitudesDePagoCount" 
        TypeName="Fonade.FONADE.Fiduciaria.SolicitudesDePagoSinDescargar"  MaximumRowsParameterName="maxRows"
        StartRowIndexParameterName="startIndex"></asp:ObjectDataSource>
</asp:Content>
