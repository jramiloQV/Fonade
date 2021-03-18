<%@ Page Language="C#" Title="Fondo Emprender - Menu" MasterPageFile="~/Master.master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="Fonade.FONADE.MiPerfil.Home" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .fonTab {
            background-image: url('/Images/bgrAnotacion2.gif');
        }

        .encima {
            background-image: url('/Images/bgrIcoPos2.gif');
            background-repeat: no-repeat;
            background-position: center;
            text-align: center;
            color: black;
            text-decoration: none;
        }
    </style>
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="bodyContentPlace">
    <!--
    <span style="margin: 5px;">Hasta</span>
    <asp:DropDownList AutoPostBack="true" runat="server" ID="cmbFechaTareas">
        <asp:ListItem Value="1">Hoy</asp:ListItem>
        <asp:ListItem Value="7">Dentro de 1 semana</asp:ListItem>
        <asp:ListItem Value="15">Dentro de 2 semanas</asp:ListItem>
        <asp:ListItem Value="30">Dentro de 1 mes</asp:ListItem>
        <asp:ListItem Value="180">Dentro de 6 meses</asp:ListItem>
        <asp:ListItem Value="*" Selected="True">Siempre</asp:ListItem>
    </asp:DropDownList>-->

 

    <div runat="server" id="divTareasEspeciales" visible="false">
        <asp:Label ID="lblTareasEspeciales" runat="server" Text="Atención, usted tiene tareas especiales de interventoria pendientes por gestionar: "></asp:Label>
        <asp:LinkButton Text="Ver pendientes" runat="server" PostBackUrl="~/PlanDeNegocioV2/Administracion/Interventoria/TareaEspecial/TareasEspecialesGerencia.aspx" />
        <asp:Image runat="server" ID="imgNoHasUpdates" Height="34" Width="34" ImageUrl="~/Images/PlanNegocioV2/Tareas/NotificationV3.png" />
    </div>

    <div>
        <asp:Label ID="lblFiltro" runat="server" Text="Proyecto: "></asp:Label>
        <asp:DropDownList ID="ddlProyecto" runat="server" AutoPostBack="true"
            OnSelectedIndexChanged="ddlProyecto_SelectedIndexChanged">
        </asp:DropDownList>
    </div>

    <div style="padding: 20px 0px;">
        <div style="background-color: #00468f; width: 25%; text-align: center">
            <asp:Label ID="lblTitulo" runat="server" Text="MIS TAREAS PENDIENTES " Font-Bold="true" ForeColor="White" BackColor="#00468f" />
        </div>
        <!--DataSourceID="dsTareas"-->
        <asp:GridView ID="gvTareas" runat="server" Width="98%" AutoGenerateColumns="False"
            CssClass="Grilla"
            AllowPaging="true" AllowSorting="true" RowStyle-VerticalAlign="Top"
            EmptyDataText="No hay tareas pendientes"
            OnRowCommand="gwTareas_RowCommand"
            PageSize="30" >
            <Columns>
                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" SortExpression="NivelUrgencia" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnUrgencia" runat="server" ImageUrl='<%# Eval("Urgencia") %>' Enabled="false" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="FechaFormated" HeaderText="Fecha" SortExpression="fecha" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="19%">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle Width="19%"></ItemStyle>
                </asp:BoundField>
                <asp:TemplateField SortExpression="proyecto" HeaderText='Plan de negocio'>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkProyecto" Text='<%# Eval("NombreProyecto") %>' Enabled='<%# Eval("AllowProyecto") %>' runat="server" CommandName="mostrarProyecto" Style="text-decoration: none;" Font-Bold="True" CommandArgument='<%# Eval("IdProyecto") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="NombreCompletoContactoAgendo" HeaderText="Agendó" SortExpression="usuarioAgendo" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:TemplateField SortExpression="tarea" HeaderText="Tema" HeaderStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkTarea" runat="server" Text='<%# Eval("Nombre") %>' Style="text-decoration: none;" Enabled='<%# Eval("AllowTarea") %>' CommandArgument='<%# Bind("IdTareaUsuarioRepeticion") %>' CommandName="mostrarTarea" ToolTip='<%# Eval("Descripcion") %>' />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:TemplateField>
            </Columns>
            <RowStyle VerticalAlign="Top"></RowStyle>
        </asp:GridView>

        <!--Paginado-->
        <div style="margin-top: 20px;">
            <table style="width: 600px;">
                <tr>
                    <td>
                        <asp:LinkButton ID="lbFirst" runat="server" OnClick="lbFirst_Click">Inicio</asp:LinkButton>
                    </td>
                    <td>
                        <asp:LinkButton ID="lbPrevious" runat="server" OnClick="lbPrevious_Click"><<</asp:LinkButton>
                    </td>
                    <td>
                        <asp:DataList ID="rptPaging" runat="server"
                            OnItemCommand="rptPaging_ItemCommand"
                            OnItemDataBound="rptPaging_ItemDataBound" RepeatDirection="Horizontal">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbPaging" runat="server"
                                    CommandArgument='<%# Eval("PageIndex") %>' CommandName="newPage"
                                    Text='<%# Eval("PageText") %> ' Width="20px"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:DataList>
                    </td>
                    <td>
                        <asp:LinkButton ID="lbNext" runat="server" OnClick="lbNext_Click">>></asp:LinkButton>
                    </td>
                    <td>
                        <asp:Label ID="lblpage" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </table>

        </div>
        <!--Fin Paginado-->

        <p style="text-align: right;">
            <asp:Label ID="Lbl_Resultados" CssClass="Indicador" runat="server" />
        </p>
    </div>

    <asp:ObjectDataSource ID="dsTareas" runat="server" EnablePaging="true"
        SelectMethod="getTareas" SortParameterName="orderBy"
        SelectCountMethod="getTareasCount" TypeName="Fonade.FONADE.MiPerfil.Home"
        MaximumRowsParameterName="maxRows"
        StartRowIndexParameterName="startIndex"></asp:ObjectDataSource>
</asp:Content>
