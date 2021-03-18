<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EvaluacionProductos.aspx.cs"
    Inherits="Fonade.FONADE.evaluacion.EvaluacionProductos" %>

<%@ Register Src="~/Controles/Post_It.ascx" TagPrefix="uc1" TagName="Post_It" %>
<%--<%@ Register Src="../../Controles/CtrlCheckedProyecto.ascx" TagName="CtrlCheckedProyecto"
    TagPrefix="uc2" %>--%>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1
        {
            width: 100%;
        }
        #principal
        {
            background-color: white;
        }
        .border
        {
            border: none;
            border-collapse: collapse;
            border-style: none;
        }
        .auto-style2
        {
            width: 100%;
        }
    </style>
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../../Scripts/common.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="#principal">
        <table class="auto-style1">
            <tr>
                <td>
                    <table border="0" width="100%" style="background-color: White">
                        <tr>
                            <td>
                                <table>
                                    <tbody>
                                        <tr>
                                            <td>
                                                ULTIMA ACTUALIZACIÓN:&nbsp;
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_nombre_user_ult_act" Text="" runat="server" ForeColor="#CC0000" />&nbsp;&nbsp;&nbsp;
                                                <asp:Label ID="lbl_fecha_formateada" Text="" runat="server" ForeColor="#CC0000" />
                                            </td>
                                            <td style="width: 20px;">
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chk_realizado" Text="MARCAR COMO REALIZADO:&nbsp;&nbsp;&nbsp;&nbsp;"
                                                    runat="server" TextAlign="Left" />
                                                &nbsp;<asp:Button ID="btn_guardar_ultima_actualizacion" Text="Guardar" runat="server"
                                                    ToolTip="Guardar" OnClick="btn_guardar_ultima_actualizacion_Click" Visible="False" />
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <table style="width: 100%">
                                    <%--<tr>
                                <td style="width: 50%">
                                    <uc2:CtrlCheckedProyecto ID="CtrlCheckedProyecto1" runat="server" />
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>--%>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <%--<tr>
                            <td colspan="2">
                                <uc2:ctrlcheckedproyecto id="CtrlCheckedProyecto1" runat="server" />
                            </td>
                        </tr>--%>
                        <tr>
                            <td style="width: 70%">
                                <div class="help_container">
                                    <div onclick="textoAyuda({titulo: 'Proyección de ventas', texto: 'IndicadoresGestion'});">
                                        <img src="../../Images/imgAyuda.gif" border="0" alt="help_Objetivos" />&nbsp; <strong>
                                            Indicadores de Gestión y Cumplimiento</strong>
                                    </div>
                                </div>
                            </td>
                            <td>
                                <div id="div_Post_It1" runat="server" visible="false">
                                    <uc1:Post_It ID="Post_It1" runat="server" _txtCampo="IndicadoresGestion" _txtTab="1" _mostrarPost="false"/>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;&nbsp;
                    <asp:ImageButton ID="IB_AgregarIndicador" runat="server" ImageUrl="~/Images/icoAdicionarUsuario.gif"
                        OnClick="IB_AgregarIndicador_Click" />
                    &nbsp;<asp:LinkButton ID="B_AgregarIndicador" runat="server" Text="Agregar Indicador"
                        Font-Bold="true" OnClick="B_AgregarIndicador_Click" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="GV_Indicador" runat="server" Width="100%" AutoGenerateColumns="False"
                        DataSourceID="ODS_Indicador" DataKeyNames="Id_IndicadorGestion" CssClass="Grilla"
                        Font-Bold="True" ForeColor="#666666" OnRowDataBound="GV_Indicador_RowDataBound"
                        OnRowCommand="GV_Indicador_RowCommand" EmptyDataText="No hay datos para mostrar.">
                        <Columns>
                            <asp:BoundField DataField="Id_IndicadorGestion" Visible="false" />
                            <asp:BoundField DataField="CodProyecto" Visible="false" />
                            <asp:BoundField DataField="CodConvocatoria" Visible="false" />
                            <asp:TemplateField ShowHeader="False" HeaderStyle-Width="20px" ItemStyle-Width="20px">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" Text=""
                                        OnClientClick="return confirm('¿Desea eliminar este indicador?');" OnClick="LinkButton1_Click"
                                        CssClass="border">
                                        <asp:Image ID="I_EliminarIndicador" runat="server" ImageUrl="~/Images/icoBorrar.gif"
                                            CssClass="border" />
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False" HeaderText="Aspecto" HeaderStyle-Width="30%"
                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LB_Aspecto" runat="server" CausesValidation="False" Text='<%# Eval("Aspecto") %>'
                                        CommandName="editar" CommandArgument='<%# Eval("Id_IndicadorGestion") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Fecha de Seguimiento" HeaderStyle-Width="8%" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="Label3" runat="server" Text='<%# Bind("FechaSeguimiento") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tipo De Indicador" HeaderStyle-Width="200px">
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("TipoDeIndicador") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Indicador" HeaderStyle-Width="25%" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <!--<table class="auto-style2" style="text-align:center;">-->
                                    <!--    <tr>-->
                                    <!--    <td>-->
                                    <div class="auto-style2" style="text-align: center;">
                                        <asp:Label ID="hl_Numerador" runat="server" Text='<%# Eval("Numerador") %>' CommandArgument='<%# Eval("Numerador") %>'
                                            CssClass="boton_Link_Grid">
                                        </asp:Label>
                                        <!--  </td>-->
                                        <!--  </tr>-->
                                        <!--  <tr>-->
                                        <!--     <td>-->
                                        <%--<br /><hr /><br />--%>
                                        <asp:Label ID="lbl_hr_1" Text="<br /><hr /><br />" runat="server" />
                                        <!--         </td>-->
                                        <!--       </tr>-->
                                        <!--       <tr>-->
                                        <!--<td>-->
                                        <asp:Label ID="hl_Denominador" runat="server" Text='<%# Eval("Denominador") %>' CommandArgument='<%# Eval("Denominador") %>'
                                            CssClass="boton_Link_Grid">
                                        </asp:Label>
                                    </div>
                                    <!--            </td>-->
                                    <!--        </tr>-->
                                    <!--     </table>-->
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Descripción del Indicador" HeaderStyle-Width="25%"
                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="Label4" runat="server" Text='<%# Bind("Descripcion") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Rango aceptable" HeaderStyle-Width="9%" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="Label2" runat="server" Text='<%# Bind("RangoAceptable") %>'></asp:Label>
                                    <asp:Label ID="Label5" runat="server" Text="%"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:ObjectDataSource ID="ODS_Indicador" runat="server" TypeName="Fonade.FONADE.evaluacion.EvaluacionProductos"
                        SelectMethod="llenarGriView"></asp:ObjectDataSource>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
