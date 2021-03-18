<%@ Page Language="C#" MasterPageFile="~/Master.master" AutoEventWireup="true" CodeBehind="VerProyectoEvaluacion.aspx.cs" Inherits="Fonade.FONADE.AdministrarPerfiles.VerProyecto" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="bodyContentPlace">
     <asp:LinqDataSource ID="lds_proyectos" runat="server" ContextTypeName="Datos.FonadeDBDataContext"
        AutoPage="false" onselecting="lds_proyectos_Selecting">
    </asp:LinqDataSource>
    <asp:Panel ID="pnlPrincipal" runat="server">
        <asp:GridView ID="gw_proyectos" runat="server" Width="100%" AutoGenerateColumns="False"
            DataKeyNames="IdProyecto" CssClass="Grilla" AllowPaging="True" DataSourceID="lds_proyectos"
            PageSize="<%# PAGE_SIZE %>" AllowSorting="True" OnRowDataBound="gw_proyectos_RowDataBound"
            OnDataBound="gw_proyectos_DataBound" PagerStyle-CssClass="Paginador" OnRowCommand="gw_proyectos_RowCommand">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton ID="ibtn_Inactivar" runat="server" CommandArgument='<%# Bind("IdProyecto") %>'
                            CommandName="InActivar" ImageUrl="~/Images/icoBorrar.gif" Visible="false" />
                        <asp:ImageButton ID="ibtn_Activar" runat="server" CommandArgument='<%# Bind("IdProyecto") %>'
                            CommandName="Activar" ImageUrl="~/Images/icoActivar.gif" Visible="false" />
                    </ItemTemplate>
                </asp:TemplateField>
                <%--2--%>
                <asp:TemplateField HeaderText="Nombre" SortExpression="NombreProyecto">
                    <ItemTemplate>
                        <asp:HyperLink ID="hl_proyecto" runat="server" NavigateUrl='<%# "ProyectoFrameSet.aspx?CodProyecto=" + Eval("IdProyecto") %>'
                            Text='<%# Eval("IdProyecto") + " - " + Eval("NombreProyecto") %>'></asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Evaluar">
                    <ItemTemplate>
                        <asp:HyperLink ID="hl_evaluacion" runat="server" NavigateUrl="" Text="Evaluar"></asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Ciudad" SortExpression="NombreCiudad">
                    <ItemTemplate>
                        <asp:Label ID="lbl_ciudad" runat="server" Text='<%# Eval("NombreCiudad") + " (" + Eval("NombreDepartamento") + ")" %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
              
            </Columns>
        </asp:GridView>
    </asp:Panel>
    <asp:Panel ID="pnlInActivar" runat="server" Visible="false">
        <asp:Label ID="lblTitulo" runat="server"></asp:Label>
        Motivo de Inactivación:<br />
        <asp:TextBox id="txtMotivoInactivacion" runat="server" Width="500px" Height="80px" TextMode="MultiLine"></asp:TextBox>
        <br />
        <asp:Button ID="btnCerrar" runat="server" Text="Cerrar" 
            onclick="btnCerrar_Click" />
        <asp:Button ID="btnInActivar" runat="server" Text="Inactivar" 
            onclick="btnInActivar_Click" />
            <asp:HiddenField ID="hddIdProyecto" runat="server" />
    </asp:Panel>
</asp:Content>