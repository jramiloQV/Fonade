<%@ Page Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="CatalogoCierreEvaluacion.aspx.cs" Inherits="Fonade.FONADE.evaluacion.CatalogoCierreEvaluacion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
    .auto-style1 {
        width: 100%;
    }
    .sinlinea {
            border:none;
            border-collapse:collapse;
            border-bottom-color:none;
        }
        table {
            width:100%;
        }
</style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">

    <table>
        <thead>
            <tr>
                <td>
                    <h1>
                    <asp:Label ID="L_ReportesEvaluacion" runat="server" Text="CIERRE DE EVALUACION" ></asp:Label></h1>
                </td>
            </tr>
        </thead>
        
        <tr>
            <td>
                <div style="height: 700px; overflow:scroll; width:100%;">
                <br />
                <asp:GridView ID="GV_Reporte" runat="server" CssClass="Grilla" AutoGenerateColumns="False" DataSourceID="ODS_Contacto" DataKeyNames="id_Parametro" Width="100%">
                    <Columns>

                        <asp:TemplateField HeaderText="Id">
                            
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("id_Parametro") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField ShowHeader="False" HeaderText="Nombre">
                            
                                <ItemTemplate>
                                    <asp:LinkButton ID="LB_Editar_Nombre" runat="server" CausesValidation="False" CommandName="Edit" Text="" OnClick="LB_Editar_Nombre_Click">
                                        <asp:Label ID="hl_convocatoria" runat="server" Text='<%# Eval("nomParametro") %>' CssClass="boton_Link_Grid" CommandName="Modificar">
                                        </asp:Label>
                                    </asp:LinkButton>
                                </ItemTemplate>
                        </asp:TemplateField>
                        
                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="ODS_Contacto" runat="server" SelectMethod="contacto" TypeName="Fonade.FONADE.evaluacion.CatalogoCierreEvaluacion">
                                        
                </asp:ObjectDataSource>
                <br />
                    </div>
            </td>
        </tr>
    </table>

</asp:Content>