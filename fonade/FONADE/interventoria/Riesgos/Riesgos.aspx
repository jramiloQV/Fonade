<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="Riesgos.aspx.cs" Inherits="Fonade.FONADE.interventoria.Riesgos.Riesgos" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <link href="../../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />
    <script src="../../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../../../Scripts/common.js" type="text/javascript"></script>
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
        function alerta() {
            return confirm('¿ Desea eliminar el riesgo de la lista ?, debe ser aprobada por el coordinador de interventoria.');
        }
    </script>
</head>
<body>
    <% Page.DataBind(); %>
    <form id="form1" runat="server">
    <div class="ContentInfo" style="width: 900px; height: auto;">
        <table>
            <tr>
                <td>
                    <div class="help_container">
                        <div onclick="textoAyuda({titulo: 'Riesgos Identificados y Mitigación', texto: 'RiesgoInter'});">
                            <img alt="help_Objetivos" border="0" src="../../../Images/imgAyuda.gif" />
                        </div>
                        <div>
                            &nbsp; <strong>Riesgos Identificados y Mitigación</strong>
                        </div>
                    </div>
                </td>               
            </tr>
            <tr>
                <td>
                    <br />
                    <br />
                    &nbsp;&nbsp;
                    <asp:ImageButton ID="imgNewRiesgo" runat="server" ImageUrl="~/Images/icoAdicionarUsuario.gif" OnClick="lnkNewRiesgo_Click" Visible='<%# ((bool)DataBinder.GetPropertyValue(this, "AllowUpdate")) %>'/> &nbsp;&nbsp;
                    <asp:LinkButton ID="lnkNewRiesgo" runat="server" Text="Agregar Riesgo" OnClick="lnkNewRiesgo_Click" Visible='<%# ((bool)DataBinder.GetPropertyValue(this, "AllowUpdate")) %>' ></asp:LinkButton>
                </td>
                <td>
                    <br />
                    <br />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:GridView ID="gvRiesgos" runat="server" AutoGenerateColumns="False" CssClass="Grilla"
                        DataSourceID="dsRiesgos" AllowPaging="false" OnRowCommand="gvRiesgos_RowCommand" Width="100%" ForeColor="#666666">
                        <Columns>                            
                            <asp:TemplateField ShowHeader="false" >
                                <ItemTemplate>
                                    <asp:LinkButton ID="linkDelete" CommandArgument='<%# Eval("Id") %>' runat="server" CommandName="deleteRiesgo" OnClientClick="return alerta();">
                                        <img src="/Images/icoBorrar.gif" alt="Eliminar riesgo" />
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Eje Funcional">
                                <ItemTemplate>
                                    <asp:Label ID="lblEjeFuncional" runat="server" Text='<%# Eval("EjeFuncional") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False" HeaderText="Riesgo">                                
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkUpdate" runat="server" CausesValidation="False" CommandName="updateRiesgo" CommandArgument='<%# Eval("Id") %>' Text='<%# Eval("Riesgos") %>' Enabled='<%# ((bool)DataBinder.GetPropertyValue(this, "AllowUpdate")) %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Mitigacion" HeaderText="Mitigación" />
                            <asp:TemplateField HeaderText="Observación">                                
                                <ItemTemplate>
                                    <asp:Label ID="lblObsercacion" runat="server" Text='<%# Bind("Observacion") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:ObjectDataSource
                        ID="dsRiesgos"
                        runat="server"
                        SelectMethod="getRiesgos"
                        TypeName="Fonade.FONADE.interventoria.Riesgos.Riesgos" >
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

