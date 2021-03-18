<%@ Page Title="" Language="C#" MasterPageFile="~/PlanDeNegocioV2/Evaluacion/Master/EvaluacionSite.Master" AutoEventWireup="true" CodeBehind="Insumo.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Evaluacion.PlanOperativo.Insumo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyHolder" runat="server">
    <div style="width: 100%; text-align: center;">

            <h1 style="text-align: left;">
                <b><asp:Label ID="lblNomProducto" runat="server"></asp:Label></b>
            </h1>
            <br />
            <br />
            <asp:GridView ID="gvInsumos" runat="server" AutoGenerateColumns="false" ShowHeader="false" CssClass="Grilla" DataKeyNames="Id_TipoInsumo" OnRowDataBound="gvInsumos_RowDataBound" DataSourceID="ldsInsumos">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <h2>
                                <asp:Label ID="lblNomTipoInsumo" runat="server" Text='<%# Eval("NomTipoInsumo") %>'></asp:Label>
                            </h2>
                            <br />
                            <asp:GridView ID="gvProyectoInsumo" runat="server" AutoGenerateColumns="false" ShowHeader="false">
                                <Columns>
                                    <asp:BoundField DataField="nomInsumo" />
                                </Columns>
                            </asp:GridView>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:LinqDataSource ID="ldsInsumos" runat="server" ContextTypeName="Datos.FonadeDBDataContext" OnSelecting="ldsInsumos_Selecting"></asp:LinqDataSource>            
        </div>
</asp:Content>
