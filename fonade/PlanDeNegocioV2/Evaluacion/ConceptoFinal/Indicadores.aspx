<%@ Page Title="" Language="C#" MasterPageFile="~/PlanDeNegocioV2/Evaluacion/Master/EvaluacionSite.Master" AutoEventWireup="true" CodeBehind="Indicadores.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Evaluacion.ConceptoFinal.Indicadores" %>
<%@ Register Src="~/Controles/Post_It.ascx" TagPrefix="uc1" TagName="Post_It" %>
<%@ Register Src="~/PlanDeNegocioV2/Evaluacion/Controles/EncabezadoEval.ascx" TagPrefix="uc1" TagName="EncabezadoEval" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyHolder" runat="server">
    <div id="#principal">
        <table class="auto-style1">
            <tr>
                <td>                    
                    <uc1:EncabezadoEval runat="server" id="EncabezadoEval" />  
                    <table>                      
                        <tr>
                            <td style="width: 70%">
                                <div class="help_container">
                                    <div onclick="textoAyuda({titulo: 'Proyección de ventas', texto: 'IndicadoresGestion'});">
                                        <img src="../../../Images/imgAyuda.gif" border="0" alt="help_Objetivos" />&nbsp; <strong>
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
                                   
                                    <div class="auto-style2" style="text-align: center;">
                                        <asp:Label ID="hl_Numerador" runat="server" Text='<%# Eval("Numerador") %>' CommandArgument='<%# Eval("Numerador") %>'
                                            CssClass="boton_Link_Grid">
                                        </asp:Label>
                                        
                                        <asp:Label ID="lbl_hr_1" Text="<br /><hr /><br />" runat="server" />
                                       
                                        <asp:Label ID="hl_Denominador" runat="server" Text='<%# Eval("Denominador") %>' CommandArgument='<%# Eval("Denominador") %>'
                                            CssClass="boton_Link_Grid">
                                        </asp:Label>
                                    </div>
                                    
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
</asp:Content>
