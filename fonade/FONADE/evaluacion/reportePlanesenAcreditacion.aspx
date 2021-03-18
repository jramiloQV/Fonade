<%@ Page Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="reportePlanesenAcreditacion.aspx.cs" Inherits="Fonade.FONADE.evaluacion.reportePlanesenAcreditacion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
    .auto-style1 {
        width: 100%;
    }
</style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <asp:ObjectDataSource ID="ODS_Convocatoria" runat="server" SelectMethod="Convocatoria" TypeName="Fonade.FONADE.evaluacion.reportePlanesenAcreditacion"></asp:ObjectDataSource>
    <table class="auto-style1">
        <thead>
            <tr>
                <th colspan="2" style="background-color:#00468f; text-align:left; padding-left:50px">
                    <asp:Label ID="L_ReportesEvaluacion" runat="server" ForeColor="White" Text="Reporte Planes de acreditación" Width="260px"></asp:Label>
                </th>
            </tr>
        </thead>
        
        <tr>
            <td style="text-align:center;">
                <br />
                <br />
                <br />
                <asp:Label ID="L_Seleccionar" runat="server" Text="Seleccione la convocatoria"></asp:Label>
            &nbsp;
                <asp:DropDownList ID="DDL_Convocatoria" runat="server" Width="400px" DataSourceID="ODS_Convocatoria" DataTextField="NomConvocatoria" DataValueField="Id_Convocatoria" ValidationGroup="item1" OnSelectedIndexChanged="DDL_Convocatoria_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                <br />
                <asp:Button ID="B_VerDetalles" runat="server" Text="Ver Detalles" OnClick="B_VerDetalles_Click" />
                <br />
                <br />
            </td>
        </tr>
    </table>

</asp:Content>