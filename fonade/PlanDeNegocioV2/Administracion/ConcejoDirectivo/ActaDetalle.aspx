<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="ActaDetalle.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Administracion.ConcejoDirectivo.ActaDetalle" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">        
        function alerta() {
            return confirm('¿ Está seguro que desea eliminar este registro ?');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <% Page.DataBind(); %>
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="true" EnableScriptLocalization="true"></asp:ToolkitScriptManager>

    <h1>
        <asp:Label Text="Crear acta de concejo directivo" runat="server" ID="lblTitleNew" Visible='<%# ((bool)DataBinder.GetPropertyValue(this, "IsCreate")) %>' />
        <asp:Label Text="Actualizar acta de concejo directivo" runat="server" ID="lblTitleUpdate" Visible='<%# !((bool)DataBinder.GetPropertyValue(this, "IsCreate")) %>' />
    </h1>
    <br />
    <asp:Table ID="formPlanDeNegocio" runat="server">
        <asp:TableRow>
            <asp:TableCell> Número: </asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="txtNumero" runat="server" Width="400px" Enabled='<%# ((bool)DataBinder.GetPropertyValue(this, "IsCreate")) %>' />
            </asp:TableCell></asp:TableRow><asp:TableRow>
            <asp:TableCell> Nombre: </asp:TableCell><asp:TableCell>
                <asp:TextBox ID="txtNombre" runat="server" Width="400px" Enabled='<%# ((bool)DataBinder.GetPropertyValue(this, "IsCreate")) %>' />
            </asp:TableCell></asp:TableRow><asp:TableRow>
            <asp:TableCell>Observación:</asp:TableCell><asp:TableCell>
                <asp:TextBox ID="txtObservacion" TextMode="MultiLine" Width="400px" Height="100px" runat="server" Enabled='<%# ((bool)DataBinder.GetPropertyValue(this, "AllowUpdate")) %>' />
            </asp:TableCell></asp:TableRow><asp:TableRow>
            <asp:TableCell>
                <asp:Label ID="lblConvccatoria" runat="server" Text="Convocatoria" Width="150"></asp:Label>
            </asp:TableCell><asp:TableCell>
                <asp:DropDownList ID="cmbConvocatoria" runat="server" Width="400px" DataSourceID="data" AutoPostBack="false" DataValueField="Id_convocatoria" DataTextField="NomConvocatoria" Enabled='<%# ((bool)DataBinder.GetPropertyValue(this, "IsCreate")) %>' AppendDataBoundItems="true">
                    <asp:ListItem Text="Seleccione" Value=""></asp:ListItem>
                </asp:DropDownList>

            </asp:TableCell></asp:TableRow><asp:TableRow>
            <asp:TableCell> Fecha: </asp:TableCell><asp:TableCell>
                <asp:TextBox ID="txtfecha" runat="server" Width="108px" Enabled='<%# ((bool)DataBinder.GetPropertyValue(this, "IsCreate")) %>' />
                <asp:Image ID="btnDate2" runat="server" AlternateText="cal2" ImageUrl="/images/icoModificar.gif" Visible='<%# ((bool)DataBinder.GetPropertyValue(this, "IsCreate")) %>' />
                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" Format='dd/MM/yyyy'
                    PopupButtonID="btnDate2" TargetControlID="txtfecha" />
            </asp:TableCell></asp:TableRow><asp:TableRow>
            <asp:TableCell runat="server" Visible='<%# !((bool)DataBinder.GetPropertyValue(this, "IsCreate")) %>'> Publicado: </asp:TableCell><asp:TableCell>
                <asp:CheckBox ID="chkPublicado" runat="server" Visible='<%# !((bool)DataBinder.GetPropertyValue(this, "IsCreate")) %>' />
            </asp:TableCell></asp:TableRow></asp:Table><br />
    <asp:Label ID="lblError" Text="Sucedio un error inesperado" Visible="False" runat="server" ForeColor="Red" />
    <br />

    <asp:ImageButton ID="imgAddProject" runat="server" ImageUrl="~/Images/icoAdicionarUsuario.gif"  OnClick="imgAddProject_Click" Visible='<%# !((bool)DataBinder.GetPropertyValue(this, "IsCreate")) && (bool)DataBinder.GetPropertyValue(this, "AllowUpdate")  %>'/>
    <asp:LinkButton ID="linkAddProject" runat="server" Text=" Adicionar proyecto"  OnClick="linkAddProject_Click" Visible='<%# !((bool)DataBinder.GetPropertyValue(this, "IsCreate")) && (bool)DataBinder.GetPropertyValue(this, "AllowUpdate")  %>' /><br />
    <br />
    <asp:GridView ID="gvMain" runat="server" Width="100%" AutoGenerateColumns="False" Visible='<%# !((bool)DataBinder.GetPropertyValue(this, "IsCreate")) %>' OnRowCommand="gvMain_RowCommand"
        CssClass="Grilla" AllowPaging="false" AllowSorting="false" EmptyDataText="No existen proyectos que mostrar.">
        <Columns>
            <asp:TemplateField HeaderText="Eliminar">
                <ItemTemplate>
                    <asp:LinkButton ID="imgborrar" runat="server" CommandArgument='<%# Eval("IdActa") + ";" + Eval("CodigoProyecto") %>' CommandName="Eliminar" CausesValidation="false" OnClientClick="return alerta();" Visible='<%# !((bool)DataBinder.GetPropertyValue(this, "IsCreate")) && (bool)DataBinder.GetPropertyValue(this, "AllowUpdate")  %>'>
                        <asp:Image ID="btnBorrar" ImageUrl="../../../Images/icoBorrar.gif" runat="server" Style="cursor: pointer;" ToolTip="Eliminar acta" />
                    </asp:LinkButton></ItemTemplate></asp:TemplateField>
            <asp:BoundField HeaderText="Código proyecto" DataField="CodigoProyecto" HtmlEncode="false" />
            <asp:BoundField HeaderText="Nombre proyecto" DataField="NombreProyecto" HtmlEncode="false" />
        </Columns>
    </asp:GridView>

    <asp:Button ID="btnNew" runat="server" Text="Crear acta" OnClick="btnNew_Click" Visible='<%# ((bool)DataBinder.GetPropertyValue(this, "IsCreate"))  %>' />
    <asp:Button ID="btnUpdate" runat="server" Text="Actualizar acta" OnClick="btnUpdate_Click" Visible='<%# !((bool)DataBinder.GetPropertyValue(this, "IsCreate")) && (bool)DataBinder.GetPropertyValue(this, "AllowUpdate") %>' />
    <asp:Button ID="btnCancel" runat="server" Text="Cancelar" PostBackUrl="~/PlanDeNegocioV2/Administracion/ConcejoDirectivo/ActaConcejoDirectivo.aspx" Visible="true" />

    <asp:ObjectDataSource
        ID="data"
        runat="server"
        TypeName="Fonade.PlanDeNegocioV2.Administracion.ConcejoDirectivo.ActaDetalle"
        SelectMethod="GetConvocatorias"
        EnablePaging="false"></asp:ObjectDataSource>
</asp:Content>
