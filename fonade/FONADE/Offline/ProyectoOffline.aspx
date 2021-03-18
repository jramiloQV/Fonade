<%@ Page Language="C#" MasterPageFile="~/Master.master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="ProyectoOffline.aspx.cs" Inherits="Fonade.FONADE.Offline.ProyectoOffline" %>

<asp:Content ID="head1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        table
        {
            width: 100%;
        }
        td {
            text-align:center;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="bodyContentPlace">
    <h1>
        <label>Off-Line</label>
    </h1>
    <br />
    <table>
        <tr>
            <td>
                <asp:LinqDataSource ID="lds_proyectosoffline" runat="server" ContextTypeName="Datos.FonadeDBDataContext" OnSelecting="lds_proyectosoffline_Selecting" AutoPage="true"></asp:LinqDataSource>
                <asp:GridView ID="gvr_proyectooffline" runat="server" AutoGenerateColumns="false" CssClass="Grilla" DataSourceID="lds_proyectosoffline" AllowSorting="True" AllowPaging="true" OnPageIndexChanging="gvr_proyectooffline_PageIndexChanging" OnRowCommand="gvr_proyectooffline_RowCommand">
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="3%">
                            <ItemTemplate>
                                <asp:ImageButton ID="ingbtn_borrar" runat="server" ImageUrl="~/Images/icoBorrar.gif" AlternateText="Eliminar documento del proyecto" CommandArgument='<%# Eval("Id_Documento") %>' CommandName="BorrarDocumento" OnClientClick="return confirm('Esta seguro que desea borrar el documento seleccionado?')" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="20%" HeaderText="Bajar Backup">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgbtn_descargar" runat="server" ImageUrl='<%# "~/Images/" + Eval("Icono") %>' CommandArgument='<%# Eval("URL") + ";" + Eval("NomDocumentoFormato") %>' CommandName="DescargarDocumento" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="42%" HeaderText="Cargar Backup" SortExpression="NomDocumento">
                            <ItemTemplate>
                                <asp:Button ID="btn_carga" runat="server" Text='<%# Eval("NomDocumento") %>' CommandArgument='<%# Eval("URL") %>' CommandName="ProcesaCarga" CssClass="boton_Link_Grid" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField ItemStyle-Width="35%" HeaderText="Fecha" DataField="Fecha" SortExpression="Fecha" />
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td>
                <table>
                    <tr>
                        <td><asp:Button ID="btnSubir" runat="server" Text="Subir Archivo Off-Line" /></td>
                        <td><asp:Button ID="btnCrear" runat="server" Text="Crear Backup" /></td>
                        <td><asp:Button ID="btnBajar" runat="server" Text="Bajar Programa Off-Line" /></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br />
    <br />
</asp:Content>