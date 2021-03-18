<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InterRiesgos.aspx.cs" Inherits="Fonade.FONADE.interventoria.InterRiesgos" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../../Scripts/common.js" type="text/javascript"></script>
    <style type="text/css">
        table
        {
            width: 100%;
        }
        h1
        {
            text-align: center;
        }
        #neConte
        {
            margin-left: 10%;
            margin-right: 10%;
        }
        .sinlinea
        {
            border: none;
            border-collapse: collapse;
            border-bottom-color: none;
        }
    </style>
    <script type="text/javascript">
        function ValidNum(e) {
            var tecla = document.all ? tecla = e.keyCode : tecla = e.which;
            return (tecla != 13);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="ContentInfo" style="width: 900px; height: auto;">
        <table>
            <tr>
                <td>
                    <div class="help_container">
                        <div onclick="textoAyuda({titulo: 'Riesgos Identificados y Mitigación', texto: 'RiesgoInter'});">
                            <img alt="help_Objetivos" border="0" src="../../Images/imgAyuda.gif" />
                        </div>
                        <div>
                            &nbsp; <strong>Riesgos Identificados y Mitigación</strong>
                        </div>
                    </div>
                </td>
                <td>
                    <asp:Label ID="lblrisgostotal" runat="server" Text="Riesgos Pendientes de Aprobar: "></asp:Label>
                    <asp:Label ID="lblRiesgosAprobar" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <br />
                    <br />
                    &nbsp;&nbsp;
                    <asp:ImageButton ID="IB_AgregarIndicador" runat="server" ImageUrl="~/Images/icoAdicionarUsuario.gif"
                        OnClick="IB_AgregarIndicador_Click" />
                    &nbsp;&nbsp;
                    <asp:LinkButton ID="btn_agregar" runat="server" OnClick="btn_agregar_Click" Text="Agregar Riesgo"></asp:LinkButton>
                </td>
                <td>
                    <br />
                    <br />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="Grilla"
                        DataSourceID="ObjectDataSource1" DataKeyNames="id_Riesgo" OnRowEditing="GridView1_RowEditing"
                        OnRowDataBound="GridView1_RowDataBound" OnRowUpdated="GridView1_RowUpdated" AllowPaging="True"
                        Width="100%" ForeColor="#666666">
                        <Columns>
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkBLButton2" runat="server" CausesValidation="False" CommandName="Delete"
                                        Text="" OnClientClick="return confirm('desea eliminar el riesgo de la lista?')"
                                        CssClass="sinlinea">
                                        <asp:Image ID="LB_eliminar" runat="server" ImageUrl="~/Images/icoBorrar.gif" CssClass="sinlinea" />
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Eje Funcional">
                                <ItemTemplate>
                                    <asp:Label ID="lblejefunconal" runat="server" Text='<%# Eval("NomejeFuncional") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False" HeaderText="Riesgo">
                                <EditItemTemplate>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Update"
                                        Text="Actualizar">
                                    </asp:LinkButton>
                                    &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel"
                                        Text="Cancelar"></asp:LinkButton>
                                    &nbsp;<asp:TextBox ID="TextBox1" CssClass="validar" runat="server" Text='<%# Bind("Riesgo") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Edit"
                                        Text="">
                                        <asp:Label ID="hl_convocatoria" runat="server" Text='<%# Eval("Riesgo") %>' CommandArgument='<%# Eval("id_Riesgo") %>'
                                            CssClass="boton_Link_Grid" CommandName="Modificar">
                                        </asp:Label>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Mitigacion" HeaderText="Mitigación" />
                            <asp:TemplateField HeaderText="Observación">
                                <EditItemTemplate>
                                    Eje Funcional:
                                    <br />
                                    <asp:DropDownList ID="ddlejefuncional" runat="server" DataTextField="NomEjeFuncional"
                                        DataValueField="Id_EjeFuncional" DataSourceID="osdejefuncional" Width="250px"
                                        OnSelectedIndexChanged="ddlejefuncional_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                    <asp:ObjectDataSource ID="osdejefuncional" runat="server" SelectMethod="ejefuncional"
                                        TypeName="Fonade.FONADE.interventoria.InterRiesgos"></asp:ObjectDataSource>
                                    <br />
                                    Observación:
                                    <br />
                                    <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("Observacion") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("Observacion") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="resultado"
                        TypeName="Fonade.FONADE.interventoria.InterRiesgos" DeleteMethod="eliminar" UpdateMethod="modificar">
                        <DeleteParameters>
                            <asp:Parameter Name="id_Riesgo" Type="Int32" />
                        </DeleteParameters>
                        <UpdateParameters>
                            <asp:Parameter Name="id_Riesgo" Type="Int32" />
                            <asp:Parameter Name="Riesgo" Type="String" />
                            <asp:Parameter Name="Mitigacion" Type="String" />
                            <asp:Parameter Name="Observacion" Type="String" />
                            <asp:Parameter Name="CodEjeFuncional" Type="String" />
                        </UpdateParameters>
                    </asp:ObjectDataSource>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
