<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="TareasEspecialesGerencia.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Administracion.Interventoria.TareaEspecial.TareasEspecialesGerencia" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <asp:Label ID="lblMainTitle" runat="server" Font-Bold="True" Text="Tareas especiales de interventoria"></asp:Label>
    <br />        
    <br />
    <asp:LinkButton ID="lnkSeleccionarPago" Visible="false" Text="Crear tarea especial de interventoria" PostBackUrl="~/PlanDeNegocioV2/Administracion/Interventoria/TareaEspecial/SeleccionarPago.aspx" runat="server"></asp:LinkButton>
    <br />
    <asp:DropDownList ID="cmbEstado" runat="server" AutoPostBack="false">
            <asp:ListItem Text="Pendientes" Value="pending" Selected="True" />
            <asp:ListItem Text="Todas" Value="all" />
    </asp:DropDownList>  
    &nbsp;  
    <asp:DropDownList ID="cmbTipo" runat="server" AutoPostBack="false">
            <asp:ListItem Text="Código de proyecto" Value="proyecto" Selected="True" />
            <asp:ListItem Text="Código de pago" Value="pago" />
    </asp:DropDownList>
    
    <asp:TextBox ID="txtCodigo" runat="server"></asp:TextBox>        
    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click"/>
    <br />     
    <br />   
    <asp:Label ID="Label3" runat="server" Font-Bold="True" Text="Listado de tareas especiales"></asp:Label>
    <br />     
    <br />
    <asp:Label ID="lblError" runat="server" ForeColor="Red" Text="Sucedio un error." Visible="False"></asp:Label>
    <asp:GridView ID="gvMain" runat="server" AllowPaging="true" AutoGenerateColumns="False" DataSourceID="data" EmptyDataText="No se encontro información." Width="98%" BorderWidth="0" CellSpacing="1" CellPadding="4" CssClass="Grilla" HeaderStyle-HorizontalAlign="Left"  OnRowCommand="gvMain_RowCommand" >
        <Columns>
            <asp:BoundField HeaderText="Código de tarea" DataField="Id" HtmlEncode="false" />
            <asp:BoundField HeaderText="Código de pago" DataField="CodigoPago" HtmlEncode="false" />
            <asp:BoundField HeaderText="Proyecto" DataField="CodigoYNombreProyecto" HtmlEncode="false" />
            <asp:BoundField HeaderText="Descripción" DataField="Descripcion" HtmlEncode="false" />
            <asp:BoundField HeaderText="Fecha inicio" DataField="FechaReintegroWithFormat" HtmlEncode="false" />
            <asp:BoundField HeaderText="Remitente" DataField="NombreRemitente" HtmlEncode="false" />
            <asp:BoundField HeaderText="Destinatario" DataField="NombreDestinatario" HtmlEncode="false" />
            <asp:BoundField HeaderText="Estado" DataField="NombreEstado" HtmlEncode="false" />            
            <asp:TemplateField HeaderText="Mensajes nuevos">
                <ItemTemplate>
                    <asp:Image runat="server" ID="imgHasUpdates" Height="34" Width="34" ImageUrl="~/Images/PlanNegocioV2/Tareas/NotificationV3.png" Visible='<%# (bool)Eval("HasUpdates") %>' />
                    <asp:Image runat="server" ID="imgNoHasUpdates" Height="34" Width="34" ImageUrl="~/Images/PlanNegocioV2/Tareas/NotificationV4.png" Visible='<%# !(bool)Eval("HasUpdates") %>' />
                </ItemTemplate>
            </asp:TemplateField>            
            <asp:TemplateField HeaderText="Acción">
                <ItemTemplate>
                    <asp:LinkButton ID="lnk" CommandArgument='<%# Eval("Id") %>'
                        CommandName="Ver"  CausesValidation="False" Text='Ver tarea' runat="server"/>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
        </asp:GridView>
    
        <asp:ObjectDataSource
                    ID="data"
                    runat="server"
                    TypeName="Fonade.PlanDeNegocioV2.Administracion.Interventoria.TareaEspecial.TareasEspecialesGerencia"
                    SelectMethod="Get"
                    EnablePaging="true"
                    SelectCountMethod="Count"
                    MaximumRowsParameterName="maxRows"
                    StartRowIndexParameterName="startIndex">
                <SelectParameters>
                    <asp:ControlParameter ControlID="txtCodigo" Name="codigo" PropertyName="Text" Type="String" DefaultValue="" />
                    <asp:ControlParameter ControlID="cmbTipo" Name="tipo" PropertyName="SelectedValue" Type="String" DefaultValue="" />
                    <asp:ControlParameter ControlID="cmbEstado" Name="estado" PropertyName="SelectedValue" Type="String" DefaultValue="" />
                </SelectParameters>
        </asp:ObjectDataSource>      
</asp:Content>
