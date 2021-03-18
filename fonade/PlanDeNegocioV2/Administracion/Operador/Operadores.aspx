<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Operadores.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Administracion.Operador.Operadores" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .espacio {
            padding-top: 10px;
            padding-bottom: 10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <script type="text/javascript">
        function alerta() {
            return confirm("Está seguro que desea borrar el operador seleccionado?");
        }
    </script>


    <div>
        <asp:Label runat="server" ID="lbl_Titulo" Text="OPERADORES" Font-Size="Large" />
        <hr />

        <div id="linkADD" class="espacio">

            <asp:LinkButton ID="lnk" OnClick="lnk_Click" CssClass="addicionar"
                Text="Adicionar Operador" runat="server" />
        </div>
        <div>
            <asp:GridView ID="gvOperadores" runat="server" Width="100%" 
                AutoGenerateColumns="False" AllowSorting="True"
                DataKeyNames="" CssClass="Grilla" AllowPaging="false" 
                 SortedAscendingHeaderStyle-CssClass="sortasc-header"
                SortedDescendingHeaderStyle-CssClass="sortdesc-header"
                OnRowCommand="gvOperadores_RowCommand" OnSorting="gvOperadores_Sorting">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ID="btn_Inactivar" CommandArgument='<%# Eval("idOperador")%>' runat="server"
                                ImageUrl="/Images/icoBorrar.gif" Visible="true" CausesValidation="false" 
                                CommandName="Eliminar"
                                OnClientClick="return alerta();" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Acción">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnk" CommandArgument='<%# Eval("idOperador") %>'
                                CommandName="Editar" CausesValidation="False" Text='Editar' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Nombre" DataField="NombreOperador" 
                        SortExpression="NombreOperador"
                        HtmlEncode="false" />
                    <asp:BoundField HeaderText="Representante" DataField="NombreRepresentante" 
                        SortExpression="NombreRepresentante"
                        HtmlEncode="false" />
                    <asp:TemplateField HeaderText="Email Representante"
                        SortExpression="EmailRepresentante">
                        <ItemTemplate>
                            <asp:HyperLink ID="hl_Email" runat="server" NavigateUrl='<%# "mailto:{"+Eval("EmailRepresentante")+"}"  %> '
                                Text='<%# Eval("EmailRepresentante") %>'></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>


</asp:Content>
