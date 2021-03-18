<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InterIndicadores.aspx.cs"
    Inherits="Fonade.FONADE.interventoria.InterIndicadores" %>

<%@ Register Src="~/Controles/Post_It.ascx" TagPrefix="uc1" TagName="Post_It" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../../Scripts/common.js" type="text/javascript"></script>
    <style type="text/css">
        .sinlinea
        {
            border: none;
            border-collapse: collapse;
            border-bottom-color: none;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <table>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <div class="help_container">
                    <div onclick="textoAyuda({titulo: 'Indicadores Específicos', texto: 'IndicadoresInter'});">
                        <img alt="help_Objetivos" border="0" src="../../Images/imgAyuda.gif" />
                    </div>
                    <div>
                        &nbsp;<strong>Indicadores Específicos</strong>
                    </div>
                    <div style="margin-left: 50px;">
                        <asp:Label ID="lblrisgostotal" runat="server" Text="Indicadores Pendientes de Aprobar: " />
                        <%--Riesgos Pendientes de Aprobar: --%>
                        <asp:Label ID="lblRiesgosAprobar" runat="server" />
                    </div>
                    <div id="post_it_show" runat="server" visible="false" style="margin-left: 50px;">
                        <uc1:Post_It runat="server" ID="Post_It" _txtTab="1" _txtCampo="IndicadoresInter" _mostrarPost="true"/>
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <asp:ImageButton ID="IB_AgregarIndicador" runat="server" ImageUrl="~/Images/icoAdicionarUsuario.gif"
                    OnClick="IB_AgregarIndicador_Click" Style="height: 14px" />
                &nbsp;
                <asp:LinkButton ID="btn_agregar" runat="server" OnClick="btn_agregar_Click" Text="Agregar Indicador" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:GridView ID="GV_Indicador" runat="server" AutoGenerateColumns="False" DataKeyNames="Id_IndicadorInter"
                    CssClass="Grilla" OnRowCommand="GV_Indicador_RowCommand" 
                    ShowHeaderWhenEmpty="true" EmptyDataText="No hay datos." OnRowDataBound="GV_Indicador_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="Id_IndicadorInter" Visible="false" HeaderStyle-Width="3%" />
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" OnClientClick="return confirm('¿Desea eliminar este indicador?');"
                                    CssClass="sinlinea" CommandArgument='<%# Eval("id_indicadorInter") %>' CommandName="eliminarGV">
                                    <asp:Image ID="I_EliminarIndicador" runat="server" ImageUrl="~/Images/icoBorrar.gif"
                                        CssClass="sinlinea" />
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="False" HeaderText="Aspecto" HeaderStyle-Width="25%"
                            HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:LinkButton ID="LB_Aspecto" runat="server" CausesValidation="False" CommandName="Edit_Inter"
                                    Text='<%# Eval("Aspecto") %>' CommandArgument='<%# Eval("Id_IndicadorInter") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Fecha de Seguimiento" HeaderStyle-Width="8%" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Text='<%# Eval("FechaSeguimiento") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:TemplateField HeaderText="Tipo de Indicador" HeaderStyle-Width="20%" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("tipoInidicador") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="Indicador" HeaderStyle-Width="20%" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <div class="auto-style2" style="text-align: center;">
                                    <asp:Label ID="hl_Numerador" runat="server" Text='<%# Eval("Numerador") %>' CommandArgument='<%# Eval("Numerador") %>'
                                        CssClass="boton_Link_Grid" />
                                    <br />
                                        <asp:Panel ID="pnlLinea" runat="server">
                                            <hr  <%# !Convert.IsDBNull(DataBinder.GetPropertyValue(GetDataItem(),"TipoIndi"))? Equals(DataBinder.GetPropertyValue(GetDataItem(),"TipoIndi").ToString(),"Indicadores Cualitativos y de Cumplimiento")?string.Format("style={0}visibility:hidden{0}","'"):string.Empty : string.Empty %> />
                                        </asp:Panel>
                                    <br />
                                    <asp:Label ID="hl_Denominador" runat="server" Text='<%# Eval("Denominador") %>' CommandArgument='<%# Eval("Denominador") %>'
                                        CssClass="boton_Link_Grid" />
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Descripción del Indicador" HeaderStyle-Width="20%"
                            HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Label ID="Label4" runat="server" Text='<%# Bind("Descripcion") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Rango aceptable" HeaderStyle-Width="9%" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Eval("RangoAceptable") + "%" %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Observación" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Label ID="lblObservacion" runat="server" Text='<%# Eval("Observacion") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="ODS_Indicador" runat="server" TypeName="Fonade.FONADE.interventoria.InterIndicadores"
                    UpdateMethod="actualizar" SelectMethod="llenarGriView">
                    <UpdateParameters>
                        <asp:Parameter Name="Id_IndicadorInter" Type="String" />
                        <asp:Parameter Name="Aspecto" Type="String" />
                        <asp:Parameter Name="FechaSeguimiento" Type="String" />
                        <asp:Parameter Name="tipoInidicador" Type="String" />
                        <asp:Parameter Name="Numerador" Type="String" />
                        <asp:Parameter Name="Denominador" Type="String" />
                        <asp:Parameter Name="Descripcion" Type="String" />
                        <asp:Parameter Name="RangoAceptable" Type="String" />
                        <asp:Parameter Name="Observacion" Type="String" />
                    </UpdateParameters>
                </asp:ObjectDataSource>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
