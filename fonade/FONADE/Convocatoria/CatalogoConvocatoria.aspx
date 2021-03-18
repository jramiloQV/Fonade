<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CatalogoConvocatoria.aspx.cs" Inherits="Fonade.FONADE.Convocatoria.CatalogoConvocatoria1" MasterPageFile="~/Master.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="BodyContent"  runat="server" ContentPlaceHolderID="bodyContentPlace">
        
        <table width="100%">
          <tr>
            <td colspan="2">
                <h1><asp:Label runat="server" ID="lbl_Titulo" style="font-weight: 700"></asp:Label></h1>
            </td>
          </tr>
          <tr>
            <td class="style10">
                
                <asp:ImageButton ID="ibtn_crearConvoct" runat="server" ImageAlign="AbsBottom" 
                    ImageUrl="~/Images/add.png" />
                <asp:LinkButton ID="LinkButton1" runat="server" onclick="LinkButton1_Click">
                    Crear Convocatoria</asp:LinkButton>
                
            </td>
            
            <td class="style10">
                <asp:Label ID="lblRegistro" runat="server" Text="Registros por página:" 
                    ForeColor="#00468F" Font-Italic="True" Font-Bold="True"></asp:Label>
              <asp:DropDownList ID="cmbRegistrosPorPagina" runat="server" AutoPostBack="true" 
                  OnSelectedIndexChanged="cmbRegistrosPorPagina_SelectedIndexChanged" >
                <asp:ListItem Text="10" Value="10" />
                <asp:ListItem Text="20" Value="20"  Selected="True"/>
                <asp:ListItem Text="30" Value="30" />
                <asp:ListItem Text="40" Value="40" />
                <asp:ListItem Text="50" Value="50" />
                <asp:ListItem Text="60" Value="60" />
                <asp:ListItem Text="70" Value="70" />
                <asp:ListItem Text="80" Value="80" />
                <asp:ListItem Text="90" Value="90" />
                <asp:ListItem Text="100" Value="100" />                
              </asp:DropDownList>
            
          </tr>
          <tr>
            <td colspan="2" style="text-align:left">
                <asp:GridView ID="gvConvocatorias"  CssClass="Grilla" runat="server"  
                    AllowSorting="True" PageSize="20"
                    AutoGenerateColumns="false" DataSourceID="dsConvocatorias" 
                    AllowPaging="true"
                    EmptyDataText="No hay información disponible." Width="100%" 
                    onrowcommand="GridViewConvoct_RowCommand">        
                    <Columns>
                        <asp:TemplateField HeaderText="Nombre"  
                            SortExpression="NomConvocatoria">
                            <ItemTemplate>
                                <asp:Button ID="btnConvocatoria"
                                    Text='<%# Eval("NomConvocatoria") %>' 
                                    CommandArgument='<%# Eval("Id_Convocatoria") %>' CommandName="VerConvocatoria" CssClass="boton_Link_Grid" runat="server"  Width="300px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Fecha Inicio" SortExpression="FechaInicio">
                            <ItemTemplate>
                                <asp:Label ID="lblFechaInicio" runat="server" Text='<%# Eval("FechaInicio") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Fecha Fin" SortExpression="FechaFin">
                            <ItemTemplate>
                                <asp:Label ID="lblFechaFin" runat="server" Text='<%# Eval("FechaFin") %>'></asp:Label>
                            </ItemTemplate> 
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Publicado" SortExpression="Publicado" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>                    
                                <asp:ImageButton ID="btnPublicado" CommandArgument='<%# Eval("Id_Convocatoria") %>'  Visible='<%# (Boolean)Eval("Publicado") %>' CommandName="VerProyectosConvatoria" runat="server" ImageUrl="~/Images/check.png" />
                            </ItemTemplate>
                        </asp:TemplateField> 
                        <asp:TemplateField HeaderText="Aspectos Evaluados" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Button ID="btnAspectos" runat="server" Text="Ver" CommandArgument='<%# Eval("Id_Convocatoria") %>' IdVersionProyecto='<%# Eval("IdVersionProyecto") %>' CommandName="VerEvalConvatoria" CssClass="boton_Link_Grid" />
                            </ItemTemplate> 
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Operador" SortExpression="NomOperador">
                            <ItemTemplate>
                                <asp:Label ID="lblOperador" runat="server" Text='<%# Eval("NomOperador") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
          </tr>
        </table>
        <asp:ObjectDataSource ID="dsConvocatorias" runat="server" EnablePaging="true" SelectMethod="getConvocatorias" SortParameterName="orderBy"
                      SelectCountMethod="getConvocatoriasCount" TypeName="Fonade.FONADE.Convocatoria.CatalogoConvocatoria1" MaximumRowsParameterName="maxRows"
                      StartRowIndexParameterName="startIndex">            
        </asp:ObjectDataSource>

</asp:Content>
<asp:Content ID="Content1" runat="server" contentplaceholderid="head">
    <style type="text/css">
        .style10
        {
            height: 30px;
        }
    </style>
</asp:Content>

