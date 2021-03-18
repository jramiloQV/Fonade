<%@ Page Title="FONDO EMPRENDER" Language="C#" MasterPageFile="~/Emergente.Master" AutoEventWireup="true" CodeBehind="CatalogoCofinanciacion.aspx.cs" Inherits="Fonade.CatalogoCofinanciacion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function cerrar() {
            window.parent.opener.focus();
            window.close();
        }

        function ValidNum(e) {
            var tecla = document.all ? tecla = e.keyCode : tecla = e.which;
            return (tecla > 47 && tecla < 58);
        }
    </script>
    <style type="text/css">        
         body, html {
            overflow: hidden;
            background-image: none !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <div style="width: 600px; height: 220px;">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:LinqDataSource ID="lds_departamento" runat="server" ContextTypeName="Datos.FonadeDBDataContext" OnSelecting="lds_departamento_Selecting"></asp:LinqDataSource>
        <asp:LinqDataSource ID="lds_municipio" runat="server" ContextTypeName="Datos.FonadeDBDataContext" OnSelecting="lds_municipio_Selecting"></asp:LinqDataSource>
        <h1>
            <asp:Label ID="lbl_titulo" runat="server" Text=""></asp:Label>
        </h1>
        <br />
        <table width="100%">
            <tr>
                <td>Departamento:</td>
                <td>
                    <asp:DropDownList ID="ddldepartamento" runat="server" Width="400px" OnChange="javascript:WebForm_DoPostBackWithOptions(new WebForm_PostBackOptions(this.name, '', true, '', '', false, true))" OnSelectedIndexChanged="ddldepartamento_SelectedIndexChanged" DataSourceID="lds_departamento" DataTextField="NomDepartamento" DataValueField="Id_Departamento">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Municipio:</td>
                <td>
                    <asp:UpdatePanel ID="upnlmunicipios" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlmunicipio" runat="server" Width="400px" DataSourceID="lds_municipio" DataTextField="NomCiudad" DataValueField="Id_Ciudad">
                            </asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddldepartamento" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>Cofinanciación:</td>
                <td>
                    <asp:TextBox ID="txtcofinanciacion" runat="server" Width="400px" ValidationGroup="accion"></asp:TextBox>
                    <br />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtcofinanciacion" ErrorMessage="RequiredFieldValidator" ForeColor="Red" ValidationGroup="accion">Este campo es requerido</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <br />
                </td>
            </tr>
            <tr>
                <td style="text-align: right;" colspan="2">
                    <asp:Button ID="btnaccion" runat="server" Text="" OnClick="btnaccion_Click" ValidationGroup="accion" />
                    &nbsp;&nbsp;
                    <asp:Button ID="btncerrar" runat="server" Text="Cerrar" OnClientClick="cerrar();" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
