<%@ Page Title="FONDO EMPRENDER" Language="C#" MasterPageFile="~/Emergente.Master" AutoEventWireup="true" CodeBehind="CatalogoCriterio.aspx.cs" Inherits="Fonade.CatalogoCriterio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function cerrar() {
            window.parent.opener.focus();
            window.close();
        }
    </script>

    <style type="text/css">
         body, html {
            background-image: none !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <div style="width: 860px; height: auto; padding: 35px;">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:LinqDataSource ID="lds_ciudades" runat="server" ContextTypeName="Datos.FonadeDBDataContext" OnSelecting="lds_ciudades_Selecting"></asp:LinqDataSource>
        <asp:LinqDataSource ID="lds_ciudadesseleccionadas" runat="server" ContextTypeName="Datos.FonadeDBDataContext" OnSelecting="lds_ciudadesseleccionadas_Selecting"></asp:LinqDataSource>
        <asp:LinqDataSource ID="lds_departamento" runat="server" ContextTypeName="Datos.FonadeDBDataContext" OnSelecting="lds_departamento_Selecting"></asp:LinqDataSource>
        <asp:LinqDataSource ID="lds_sectores" runat="server" ContextTypeName="Datos.FonadeDBDataContext" OnSelecting="lds_sectores_Selecting"></asp:LinqDataSource>
        <asp:LinqDataSource ID="lds_sectoresseleccionados" runat="server" ContextTypeName="Datos.FonadeDBDataContext" OnSelecting="lds_sectoresseleccionados_Selecting"></asp:LinqDataSource>
        <h1>
            <asp:Label ID="lbl_titulo" runat="server" Text=""></asp:Label>
        </h1>
        <br />
        <table style="width: 100%;">
            <tr>
                <td style="text-align: right;">Nombre:</td>
                <td colspan="2">
                    <asp:TextBox ID="txtnombrecriterio" runat="server" Text="" Width="400px" ValidationGroup="accion"></asp:TextBox>
                    <br />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtnombrecriterio" ErrorMessage="RequiredFieldValidator" ForeColor="Red" ValidationGroup="accion">Este Campo es requerido</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <h2>Ámbito Geográfico</h2>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <br />
                    Departamentos:</td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:DropDownList ID="ddldepartamento" runat="server" Width="400px" DataSourceID="lds_departamento" DataTextField="NomDepartamento" DataValueField="Id_Departamento" OnChange="javascript:WebForm_DoPostBackWithOptions(new WebForm_PostBackOptions(this.name, '', true, '', '', false, true))" OnSelectedIndexChanged="ddldepartamento_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <br />
                    Municipios:
                </td>
                <td>
                    <br />
                </td>
                <td>
                    <br />
                    Seleccionados Ciudad:
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="upnlmunicipios" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                        <ContentTemplate>
                            <asp:ListBox ID="lbx_ciudades" runat="server" Width="400px" SelectionMode="Multiple" DataSourceID="lds_ciudades" DataTextField="NomCiudad" DataValueField="Id_Ciudad" Height="150px"></asp:ListBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddldepartamento" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
                <td style="text-align: center;">
                    <asp:Button ID="btn_agregarciudad" runat="server" Text=">>" OnClick="btn_agregarciudad_Click" />
                    <br />
                    <br />
                    <asp:Button ID="btn_quitarciudad" runat="server" Text="<<" OnClick="btn_quitarciudad_Click" />
                </td>
                <td>
                    <asp:ListBox ID="lbx_ciudadesseleccionadas" runat="server" Width="400px" Height="150px" DataTextField="NomCiudad" DataValueField="Id_Ciudad" SelectionMode="Multiple" DataSourceID="lds_ciudadesseleccionadas"></asp:ListBox>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <h2>Ámbito Económico</h2>
                </td>
            </tr>
            <tr>
                <td>
                    <br />
                    Sectores:
                </td>
                <td>
                    <br />
                </td>
                <td>
                    <br />
                    Seleccionados Ciudad:
                </td>
            </tr>
            <tr>
                <td>
                    <asp:ListBox ID="lbx_sectores" runat="server" Width="400px" SelectionMode="Multiple" DataSourceID="lds_sectores" DataTextField="NomSector" DataValueField="Id_Sector" Height="150px">
                        <asp:ListItem Value="0" Text="(Todos los sectores)" Selected="True" Enabled="true"></asp:ListItem>
                    </asp:ListBox>
                </td>
                <td style="text-align: center;">
                    <asp:Button ID="btn_agregarsector" runat="server" Text=">>" OnClick="btn_agregarsector_Click" />
                    <br />
                    <br />
                    <asp:Button ID="btn_quitarsector" runat="server" Text="<<" OnClick="btn_quitarsector_Click" />
                </td>
                <td>
                    <asp:ListBox ID="lbx_sectoresseleccionados" runat="server" Width="400px" Height="150px" DataTextField="nomSector" DataValueField="Id_Sector" SelectionMode="Multiple" DataSourceID="lds_sectoresseleccionados"></asp:ListBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: center;">
                    <br />
                    <asp:Button ID="btnaccion" runat="server" Text="" OnClick="btnaccion_Click" ValidationGroup="accion" />
                </td>
                <td></td>
                <td style="text-align: center;">
                    <br />
                    <asp:Button ID="btncerrar" runat="server" Text="Cerrar" OnClientClick="cerrar();" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
