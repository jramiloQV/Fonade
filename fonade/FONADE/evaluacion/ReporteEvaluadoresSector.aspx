<%@ Page Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="ReporteEvaluadoresSector.aspx.cs" Inherits="Fonade.FONADE.evaluacion.ReporteEvaluadoresSector" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
    .auto-style1 {
        width: 100%;
    }
</style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <table class="auto-style1">
        <thead>
            <tr>
                <th colspan="2" style="background-color:#00468f; text-align:left; padding-left:50px">
                    <asp:Label ID="L_ReportesEvaluacion" runat="server" ForeColor="White" Text="" Width="260px"></asp:Label>
                </th>
            </tr>
        </thead>
       
        <tr>
            <td>
                <br />
                <br />
                <asp:GridView ID="GV_Datos" runat="server" AutoGenerateColumns="False" CssClass="Grilla" Width="100%">
                    <Columns>
                        <asp:BoundField DataField="Codigo" HeaderText="CIIU" />
                        <asp:BoundField DataField="NomSubSector" HeaderText="Descripcion" />
                        <asp:BoundField DataField="Lugar" HeaderText="Ciudad del Plan" />
                        <asp:BoundField DataField="Cuantos" HeaderText="#Evaluadores" />
                    </Columns>
                </asp:GridView>
                <br />
                <br />
                <br />
                <br />
            </td>
        </tr>
    </table>
</asp:Content>