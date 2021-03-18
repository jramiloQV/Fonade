<%@ Page Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="ReporteFechas.aspx.cs" Inherits="Fonade.FONADE.evaluacion.ReporteFechas" %>

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
                <asp:GridView ID="GV_Datos" runat="server" AutoGenerateColumns="False" CssClass="Grilla" Width="100%" EmptyDataText="No hay Tareas Pendientes" OnRowDataBound="GV_Datos_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="FechaInicio" HeaderText="Fecha de Asignación" />
                        <asp:BoundField DataField="DIAS" HeaderText="#Días en estudio" />
                        <asp:BoundField DataField="Evaluador" HeaderText="Evaluador" />
                        <asp:BoundField DataField="NomProyecto" HeaderText="Plan de Negocio" />
                        <asp:BoundField DataField="CIIU" HeaderText="CIIU" />
                        <asp:BoundField DataField="Tarea1" HeaderText="Tarea >= 24h" />
                        <asp:BoundField DataField="Tarea2" HeaderText="Tarea >= 48h" />
                        <asp:BoundField DataField="Tarea3" HeaderText="Tarea >= 72h" />
                        <asp:BoundField DataField="Agendo" HeaderText="Agendó" />
                        <asp:BoundField DataField="Responsable" HeaderText="Responsable" />
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