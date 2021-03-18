<%@ Page Language="C#" MasterPageFile="~/Master.master" EnableEventValidation="false"
    AutoEventWireup="true" CodeBehind="InformeConsolidadoInter.aspx.cs" Inherits="Fonade.FONADE.interventoria.InformeConsolidadoInter" %>

<asp:Content ID="head1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        table
        {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="bodyContentPlace">
    <asp:ScriptManager ID="updt_1" runat="server" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <h1>
                <asp:Label ID="L_titulo" runat="server" Text="INFORME CONSOLIDADO DE INTERVENTORIA"> </asp:Label>
            </h1>
            <div id="contenido">
                <label>
                    Buscar por letra:</label>
                <asp:DropDownList ID="ddlbuscar" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlbuscar_SelectedIndexChanged">
                    <asp:ListItem Value="" Text="Todo" Selected="True" />
                    <asp:ListItem Value="a" Text="A" />
                    <asp:ListItem Value="b" Text="B" />
                    <asp:ListItem Value="c" Text="C" />
                    <asp:ListItem Value="d" Text="D" />
                    <asp:ListItem Value="e" Text="E" />
                    <asp:ListItem Value="f" Text="F" />
                    <asp:ListItem Value="g" Text="G" />
                    <asp:ListItem Value="h" Text="H" />
                    <asp:ListItem Value="i" Text="I" />
                    <asp:ListItem Value="j" Text="J" />
                    <asp:ListItem Value="k" Text="K" />
                    <asp:ListItem Value="l" Text="L" />
                    <asp:ListItem Value="m" Text="M" />
                    <asp:ListItem Value="n" Text="N" />
                    <asp:ListItem Value="o" Text="O" />
                    <asp:ListItem Value="p" Text="P" />
                    <asp:ListItem Value="q" Text="Q" />
                    <asp:ListItem Value="r" Text="R" />
                    <asp:ListItem Value="s" Text="S" />
                    <asp:ListItem Value="t" Text="T" />
                    <asp:ListItem Value="u" Text="U" />
                    <asp:ListItem Value="v" Text="V" />
                    <asp:ListItem Value="w" Text="W" />
                    <asp:ListItem Value="x" Text="X" />
                    <asp:ListItem Value="y" Text="Y" />
                    <asp:ListItem Value="z" Text="Z" />
                </asp:DropDownList>
                <br />
                <br />
                <asp:LinqDataSource ID="lds_informes" runat="server" ContextTypeName="Datos.FonadeDBDataContext" AutoPage="true"
                        OnSelecting="lds_informes_Selecting">
                    </asp:LinqDataSource>
                <asp:GridView ID="gv_informesinterventoria" runat="server" CssClass="Grilla" EmptyDataText="Usted no ha generado Informes de Consolidado."
                    AutoGenerateColumns="False" OnRowCommand="gv_informesinterventoria_RowCommand" DataSourceID="lds_informes"
                    AllowSorting="True" OnSorting="gv_informesinterventoria_Sorting" AllowPaging="false"
                    PageSize="10" OnPageIndexChanging="gv_informesinterventoria_PageIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="Id_InterventorInformeFinal" Visible="false" />
                        <asp:TemplateField SortExpression="Id_InterventorInformeFinal">
                            <ItemTemplate>
                                <asp:Label ID="lbl_numero" runat="server" Text='<%# Eval("Id_InterventorInformeFinal") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Nombre del Informe" SortExpression="NomInterventorInformeFinal">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Eval("NomInterventorInformeFinal")+ ";" + Eval("Id_InterventorInformeFinal") %>' />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Button ID="btn_nombreinforme" runat="server" Text='<%# Eval("NomInterventorInformeFinal") %>'
                                    CssClass="boton_Link_Grid" CommandName="Editar" CommandArgument='<%# Eval("Id_InterventorInformeFinal") + ";" + Eval("CodEmpresa") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Fecha del Informe" SortExpression="FechaInforme">
                            <ItemTemplate>
                                <asp:Label ID="lblfecha" runat="server" Text='<%# Eval("FechaInforme") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Estado" SortExpression="EstadoVAL">
                            <ItemTemplate>
                                <asp:Label ID="lblestado" runat="server" Text='<%# Eval("EstadoVAL") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:Panel ID="pnl_adicionar_informe_visita" runat="server" HorizontalAlign="Center"
                    Visible="false">
                    <asp:LinqDataSource ID="lds_empresas" runat="server" ContextTypeName="Datos.FonadeDBDataContext"
                        OnSelecting="lds_empresas_Selecting">
                    </asp:LinqDataSource>
                    <asp:DropDownList ID="dd_Empresas" runat="server" Height="16px" DataSourceID="lds_empresas" DataTextField="razonsocial" DataValueField="id_empresa"
                        Width="449px" />
                    <br />
                    <asp:Button ID="btn_adicionar_informe_visita" Text="Ingresar Informe" runat="server"
                        OnClick="btn_adicionar_informe_visita_Click" />
                    <%--&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
                </asp:Panel>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btn_adicionar_informe_visita" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
