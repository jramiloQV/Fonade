<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProyectosPorConvocatoria.aspx.cs" Inherits="Fonade.FONADE.Convocatoria.ProyectosXConvocatoria"  MasterPageFile="~/Master.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="BodyContent"  runat="server" ContentPlaceHolderID="bodyContentPlace">

    <asp:LinqDataSource ID="lds_listadoProyXConvoct" runat="server" 
    ContextTypeName="Datos.FonadeDBDataContext" AutoPage="false" 
    onselecting="lds_listadoProyXConvoct_Selecting" >
    </asp:LinqDataSource>

    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"> </asp:ToolkitScriptManager>
    <table width="100%">
      <tr>
        <td colspan="2"><h1><asp:Label runat="server" ID="lbl_Titulo" style="font-weight: 700"></asp:Label></h1></td>
      </tr>
      <tr>
        <td class="style13" valign="top">Apertura Número:</td>
        <td class="style14">
            <asp:Label ID="l_numeroapertura" runat="server" Text="" style="color: #FF3300"></asp:Label>
          </td>
      </tr>
      <tr>
        <td class="style13" valign="top">Nombre:</td>
        <td class="style14">
            <asp:Label ID="l_nombre" runat="server" Text=""></asp:Label>
          </td>
      </tr>
      <tr>
        <td class="style13" valign="top">Descripción:</td>
        <td class="style14">
            <asp:Label ID="l_descipcion" runat="server" Text=""></asp:Label>
          </td>
      </tr>
      <tr>
        <td class="style13" valign="top">Fecha de Inicio:</td>
        <td class="style14">
            <asp:Label ID="l_fechainicio" runat="server" Text=""></asp:Label>
          </td>
      </tr>
      <tr>
        <td class="style13" valign="top">Fecha de Finalización:</td>
        <td class="style14">
            <asp:Label ID="l_fechafin" runat="server" Text=""></asp:Label>
          </td>
      </tr>
      <tr>
        <td class="style13" valign="top">Presupuesto:</td>
        <td class="style14">
            <asp:Label ID="l_presupuesto" runat="server" Text=""></asp:Label>
          </td>
      </tr>
      <tr>
        <td class="style11"><b>Valor mínimo para aprobar<br />
            Plan de Negocio:</b></td>
        <td>
            <asp:Label ID="l_valorminimo" runat="server" Text=""></asp:Label>
          </td>
      </tr>
      <tr>
        <td colspan="2" align="center"><asp:Button ID="btn_IraConvoct" runat="server" 
                Text="Volver a la Convocatoria" onclick="btn_IraConvoct_Click" /></td>

      </tr>
    </table>
    <div style="width:100%; overflow-x:scroll">
    <table width="100%">
        <tr>
        <td class="style15" >
            &nbsp;&nbsp;</td>
      </tr>
      <tr>
        <td style="background-color:#00468F" align="center" class="style10">
            <strong>Proyectos
        </strong>
        </td>
      </tr>
      <tr>
        <td>
            
            <asp:GridView ID="GridViewConvoct"  CssClass="Grilla" runat="server"  AllowSorting="True"
                AutoGenerateColumns="false" DataSourceID="lds_listadoProyXConvoct" AllowPaging="true"
                EmptyDataText="No hay información disponible." Width="100%" 
                ondatabound="GridViewConvoct_DataBound" 
                onrowcommand="GridViewConvoct_RowCommand" >
        
                <Columns>
                    <asp:TemplateField HeaderText="Proyecto">
                        <ItemTemplate>
                            <asp:HiddenField ID="hiddenIdProy" runat="server" Value='<%# Eval("id_Proyecto") %>'  />
                            <asp:Button ID="hl_proyecto" Text='<%# Eval("nomProyecto") %>' CommandArgument='<%# Eval("id_Proyecto") %>' CommandName="VerConvocatoria" CssClass="boton_Link_Grid" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Presentado Por">
                        <ItemTemplate>
                            <asp:Label ID="hl_presentado" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Asesor Lider">
                        <ItemTemplate>
                            <asp:Label ID="hl_AsesorL" runat="server" Text='<%# Eval("Lider") %>'></asp:Label>
                        </ItemTemplate> 
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Unidad de Emprendimiento">
                        <ItemTemplate>                    
                           <asp:Label ID="hl_unidad" runat="server" Text='<%# Eval("Unidad") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField> 
                    <asp:TemplateField HeaderText="Sector">
                        <ItemTemplate>                    
                           <asp:Label ID="hl_sector" runat="server" Text='<%# Eval("nomSector") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField> 
                    <asp:TemplateField HeaderText="Ciudad">
                        <ItemTemplate>                    
                           <asp:Label ID="hl_ciudad" runat="server" Text='<%# Eval("ciudad") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField> 
                    <asp:TemplateField HeaderText="Solicitado al Fondo">
                        <ItemTemplate>                    
                           <asp:Label ID="hl_recursos" runat="server" Text='<%# Eval("Recursos") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField> 
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>                    
                           <asp:ImageButton ID="btnPrint" CommandArgument='<%# Eval("id_Proyecto") %>' CommandName="ImprResumenProyecto" runat="server" ImageUrl="~/Images/print.png" ToolTip="Imprimir resumen ejecutivo" />
                        </ItemTemplate>
                    </asp:TemplateField> 
                    
                </Columns>
            </asp:GridView>

        </td>
      </tr>
    </table>
        </div>
</asp:Content>
<asp:Content ID="Content1" runat="server" contentplaceholderid="head">
    <style type="text/css">
        .style10
        {
            color: #FFFFFF;
            height: 28px;
        }
        .style11
        {
            width: 185px;
        }
        .style13
        {
            width: 185px;
            font-weight: bold;
            height: 24px;
        }
        .style14
        {
            height: 24px;
        }
        .style15
        {
            height: 30px;
        }
    </style>
</asp:Content>
