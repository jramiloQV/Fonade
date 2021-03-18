<%@ Page Language="C#" MasterPageFile="~/Master.master" EnableEventValidation="false"
    AutoEventWireup="true" CodeBehind="InformeVisitaInter.aspx.cs" Inherits="Fonade.FONADE.interventoria.InformeVisitaInter" %>

<asp:Content ID="head1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        table {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="bodyContentPlace">
    <asp:ScriptManager ID="updt_1" runat="server" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="principal" runat="server">
                <h1>
                    <asp:Label ID="L_titulo" runat="server" Text="VISITAS DE INTERVENTORÍA" />
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
                        <asp:ListItem Value="0" Text="0" />
                        <asp:ListItem Value="1" Text="1" />
                        <asp:ListItem Value="2" Text="2" />
                        <asp:ListItem Value="3" Text="3" />
                        <asp:ListItem Value="4" Text="4" />
                        <asp:ListItem Value="5" Text="5" />
                        <asp:ListItem Value="6" Text="6" />
                        <asp:ListItem Value="7" Text="7" />
                        <asp:ListItem Value="8" Text="8" />
                        <asp:ListItem Value="9" Text="9" />
                    </asp:DropDownList>
                    <asp:LinqDataSource ID="lds_informes" runat="server"
                        ContextTypeName="Datos.FonadeDBDataContext" AutoPage="true"
                        OnSelecting="lds_informes_Selecting">
                    </asp:LinqDataSource>
                    <asp:GridView ID="gv_informesinterventoria" runat="server" CssClass="Grilla" DataSourceID="lds_informes" 
                        EmptyDataText="Usted no ha generado Informes de Visitas."
                        AutoGenerateColumns="False" OnRowCommand="gv_informesinterventoria_RowCommand"
                        AllowPaging="false" OnPageIndexChanging="gv_informesinterventoria_PageIndexChanging"
                        AllowSorting="True" OnSorting="gv_informesinterventoria_Sorting"
                        OnDataBound="gv_informesinterventoria_DataBound">
                        <Columns>
                            <asp:BoundField DataField="Id_Informe" Visible="false" SortExpression="Id_Informe" />
                            <asp:TemplateField SortExpression="NUM">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_numero" runat="server" Text='<%# Eval("NUM") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Nombre del Informe" SortExpression="NombreInforme">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("NombreInforme") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="btn_nombreinforme" Text='<%# Eval("NombreInforme") %>' runat="server"
                                        CausesValidation="false" CommandName="MostrarInforme" CommandArgument='<%# Eval("Id_Informe") + ";" + Eval("NombreInforme") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            Usted no ha generado Informes de Visitas.
                        </EmptyDataTemplate>
                    </asp:GridView>
                    <asp:Panel ID="pnl_adicionar_informe_visita" runat="server" Visible="false" HorizontalAlign="Center">
                        <asp:DropDownList ID="dd_Empresas" runat="server" AutoPostBack="true" Height="16px"
                            Width="449px" />
                        <br />
                        <asp:Button ID="btn_adicionar_informe_visita" Text="Adicionar" runat="server" OnClick="btn_adicionar_informe_visita_Click" />

                    </asp:Panel>
                </div>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btn_adicionar_informe_visita" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
