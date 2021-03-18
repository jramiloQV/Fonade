<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="SolicitudesDePagoDescargadas.aspx.cs" Inherits="Fonade.FONADE.Fiduciaria.SolicitudesDePagoDescargadas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Styles/Site.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <table width="98%" border="0">
        <tr>
            <td class="style50">
                <h1>
                    <asp:Label runat="server" ID="lblTitulo" Text="Solicitudes de pago descargadas" Style="font-weight: 700"></asp:Label>
                </h1>
            </td>
            <td align="right">
            </td>            
        </tr>       
    </table>
    <table>
        <tr>
            <td class="auto-style3">
                <asp:Label ID="lblNumeroActa" runat="server" Text="Numero de acta"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtNumeroActa" onkeydown = "return (event.keyCode!=13);" runat="server" ></asp:TextBox>
            </td>
            <td>
                <asp:Button ID="btnBuscarActaPorNumero" runat="server" Text="Buscar acta" OnClick="btnBuscarActaPorNumero_Click" />
            </td>
        </tr>
    </table>
    <br />
    <asp:GridView ID="gvSolicitudesDePagoDescargadas" runat="server"
        Width="98%" BorderWidth="0" CellSpacing="1" CellPadding="4" AllowPaging="true" 
        OnRowCommand="detalleSolicitud_RowCommand"
        PageSize="30" AutoGenerateColumns="False" CssClass="Grilla" HeaderStyle-HorizontalAlign="Left" 
        DataSourceID="dsSolicitudesDePago" EmptyDataText="No hay solicitudes pendientes por procesar.">
        <Columns>
            <asp:TemplateField HeaderText="Número solicitud">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkmostrar" CommandArgument='<%# Eval("codigoActaFiduciaria") %>'
                        CommandName="verDetalleSolicitud" CausesValidation="False" Text='<%#Eval("codigoActaFiduciaria") %>'
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
            <asp:BoundField HeaderText="Estado" DataField="descargado" HtmlEncode="false" />
        </Columns>
        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
    </asp:GridView>
    <asp:ObjectDataSource ID="dsSolicitudesDePago" runat="server" EnablePaging="true" 
        SelectMethod="getSolicitudesDePago"
        SelectCountMethod="getSolicitudesDePagoCount" 
        TypeName="Fonade.FONADE.Fiduciaria.SolicitudesDePagoDescargadas"  MaximumRowsParameterName="maxRows"
        StartRowIndexParameterName="startIndex" >
        <SelectParameters> 
        <asp:ControlParameter ControlID="txtNumeroActa" Name="numeroActa" PropertyName="Text" Type="String" DefaultValue="" />
        </SelectParameters>         
    </asp:ObjectDataSource>
</asp:Content>
