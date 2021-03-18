<%@ Page Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="CatalogoEvaluacionConceptos.aspx.cs" Inherits="Fonade.FONADE.evaluacion.CatalogoEvaluacionConceptos" %>

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
</style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    
    <table class="auto-style1" >
        <thead>
            <tr>
                <th colspan="2" style="background-color:#00468f; text-align:center; padding-left:50px">
                    <asp:Label ID="L_ReportesEvaluacion" runat="server" ForeColor="White" Text="TIPO DE ÁMBITO" Width="260px"></asp:Label>
                </th>
            </tr>
        </thead>
        
        <tr>
                <td>
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:ImageButton ID="IB_AgregarIndicador" runat="server" ImageUrl="~/Images/icoAdicionarUsuario.gif" OnClick="IB_AgregarIndicador_Click" />
                    &nbsp;&nbsp;
                    <asp:LinkButton ID="btn_agregar" runat="server" Text="Adicionar Evaluacion Conceptos" 
                         onclick="btn_agregar_Click"></asp:LinkButton>
                </td>
            </tr>
        <tr>
            <td>
                <br />
                <asp:GridView ID="GV_Reporte" runat="server" CssClass="Grilla" AutoGenerateColumns="False" AllowPaging="True" DataSourceID="ODS_Contacto" DataKeyNames="Id" Width="600">
                    <Columns>

                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="LB_Eliminar001" runat="server" CausesValidation="False" CommandName="Delete" Text="" OnClientClick="return confirm('Desea borrar el tipo de ambito ?')">
                                    <asp:Image ID="LB_eliminar" runat="server" ImageUrl="~/Images/icoBorrar.gif"  CssClass="sinlinea" />
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Id">
                            <EditItemTemplate>
                                <asp:Label ID="TextBox1" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField ShowHeader="False" HeaderText="Nombre">
                            <EditItemTemplate>
                                <asp:LinkButton ID="LB_BotonActualizar" runat="server" CausesValidation="True" CommandName="Update" Text="Actualizar" ForeColor="#009900">

                                    </asp:LinkButton>
                                    &nbsp;<asp:LinkButton ID="LB_Boton_Cancelar" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancelar" ForeColor="Red"></asp:LinkButton>
                                    &nbsp;<asp:TextBox ID="TB_Nombre001" CssClass ="validar" Width="450" runat="server" Text='<%# Bind("Nombre") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="LB_Editar_Nombre" runat="server" CausesValidation="False" CommandName="Edit" Text="">
                                        <asp:Label ID="hl_convocatoria" runat="server" Text='<%# Eval("Nombre") %>'
                                            CommandArgument='<%# Eval("Id") %>' CssClass="boton_Link_Grid" CommandName="Modificar">
                                        </asp:Label> 
                                                
                                    </asp:LinkButton>
                                </ItemTemplate>
                        </asp:TemplateField>
                        
                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="ODS_Contacto" runat="server" SelectMethod="contacto" DeleteMethod="eliminar" UpdateMethod="modificar" TypeName="Fonade.FONADE.evaluacion.CatalogoEvaluacionConceptos">
                    <DeleteParameters>
                            <asp:Parameter Name="Id" Type="Int32" />
                        </DeleteParameters>
                    <UpdateParameters>
                            <asp:Parameter Name="Nombre" Type="String" />
                        </UpdateParameters>
                </asp:ObjectDataSource>
                <br />
            </td>
        </tr>
    </table>
    
</asp:Content>