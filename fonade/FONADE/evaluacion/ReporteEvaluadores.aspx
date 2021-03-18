<%@ Page Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="ReporteEvaluadores.aspx.cs" Inherits="Fonade.FONADE.evaluacion.ReporteEvaluadores" %>

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
                <asp:GridView ID="GV_Reporte" runat="server" CssClass="Grilla" AutoGenerateColumns="False" Width="100%">
                    <Columns>
                        <asp:TemplateField HeaderText="Departamento">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Departamento") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("Departamento") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Evaluador" HeaderText="Evaluador" />
                        <asp:BoundField DataField="Profesion" HeaderText="Profesión" />
                        <asp:TemplateField HeaderText="CIIU">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("CIIU") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("CIIU") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Planes" HeaderText="# Planes" />
                        <asp:BoundField DataField="Municipio" HeaderText="Municipio" />
                    </Columns>
                </asp:GridView>
                <br />
            </td>
        </tr>
        <tr>
            <td>
                <table class="Grilla">
                    
                    <tr>
                        <th style="text-align:right;">
                            <asp:Label ID="L_TituloTotalPlanes" runat="server" Text="Total Planes:" Width="150px"></asp:Label></th>
                        <th style="text-align:center;">
                            <asp:Label ID="L_textTotalPlanes" runat="server" Width="200px"></asp:Label></th>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

</asp:Content>