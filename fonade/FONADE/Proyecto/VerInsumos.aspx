<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VerInsumos.aspx.cs" Inherits="Fonade.FONADE.Proyecto.VerInsumos" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style10
        {
            height: 47px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">

     <asp:LinqDataSource ID="lds_Insumos" runat="server" 
        ContextTypeName="Datos.FonadeDBDataContext" AutoPage="true" 
        onselecting="lds_Insumos_Selecting" >
    </asp:LinqDataSource>

    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>

    <asp:UpdatePanel ID="UP_lista" runat="server" Visible="true" Width="100%" UpdateMode="Conditional">
    <ContentTemplate>

        <table width="600">
          <tr>
            <td class="style11">
                <asp:DropDownList ID="ddl_tipoinsumo" runat="server" AutoPostBack="True" 
                    Height="24px" Width="201px" 
                    onselectedindexchanged="ddl_tipoinsumo_SelectedIndexChanged">
                </asp:DropDownList>
              </td>
          </tr>
          <tr>
            <td class="style12">
                <asp:ImageButton ID="ibtn_adicionarinsumo" runat="server" 
                    ImageUrl="~/Images/add.png" />
                <asp:Button ID="btn_addinsumo" runat="server" Text="Adicionar Insumo" 
                    CssClass="boton_Link_Grid" onclick="btn_addinsumo_Click" Width="104px" />
              </td>
          </tr>
          <tr>
            <td>

            <asp:GridView ID="gvInsumos"  CssClass="Grilla" runat="server" DataSourceID="lds_Insumos" 
                   AllowPaging="false" AutoGenerateColumns="false" Width="100%" 
                    EmptyDataText="No hay insumos para el tipo seleccionado" >
                        <Columns>
                           <asp:TemplateField>
                                <ItemTemplate>                    
                                    <asp:ImageButton ID="btnEliminarInsumo" Visible="true" runat="server" ImageUrl="/Images/icoBorrar.gif" />
                                        <ajaxToolkit:ConfirmButtonExtender ID="cbe" runat="server"
                                            TargetControlID="btnEliminarInsumo"
                                            ConfirmText=" ¿Está seguro que desea eliminar este insumo?"
                                        />
                                </ItemTemplate>
                            </asp:TemplateField> 
                            <asp:TemplateField>
                                <ItemTemplate>                    
                                    <asp:CheckBox ID="chbinsumos" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Nombre" >
                                <ItemTemplate>
                                    <asp:HiddenField ID="hiddenidInsumo" runat="server" Value='<%# Eval("id_Insumo") %>' />
                                    <asp:Label ID="lnombreInsumo" runat="server" Text='<%# Eval("nomInsumo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tipo"  ItemStyle-Font-Bold="true">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hiddenidTipoinsumo" runat="server" Value='<%# Eval("id_TipoInsumo") %>' />
                                    <asp:Label ID="ltipoinsumo" runat="server" Text='<%# Eval("nomTipoInsumo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Presentación" >
                                <ItemTemplate>
                                    <asp:Label ID="l_presentacion" runat="server" Text='<%# Eval("Presentacion") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Unidad" >
                                <ItemTemplate>
                                    <asp:Label ID="lunidad" runat="server" Text='<%# Eval("Unidad") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>
                    </asp:GridView>
            
            </td>
          </tr>
          <tr>
            <td class="style10" align="center">
                <asp:Button ID="btn_agregarprod" runat="server" Text="Agregar Producto" 
                    onclick="btn_agregarprod_Click" /></td>
          </tr>
          <tr>
            <td>&nbsp;</td>
          </tr>
        </table>

    </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>