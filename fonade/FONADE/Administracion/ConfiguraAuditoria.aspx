<%@ Page Language="C#" MasterPageFile="~/Master.master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="ConfiguraAuditoria.aspx.cs" Inherits="Fonade.FONADE.Administracion.ConfiguraAuditoria" %>

<asp:Content ID="head1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        table {
            width: 100%;
        }

        td {
            vertical-align: top;
        }
    </style>
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="bodyContentPlace">
    <asp:ScriptManager ID="scrmanager1" runat="server" />
    <h1>
        <label>CONFIGURAR CAMPOS A AUDITAR</label>
    </h1>
    <br />
    <br />
    <asp:UpdatePanel ID="upnPrincipal" runat="server">
        <ContentTemplate>
            <table>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>Tabla:</td>
                    <td>
                        <asp:DropDownList ID="ddltablas" runat="server" Height="16px" Width="500px" AutoPostBack="true" OnSelectedIndexChanged="ddltablas_SelectedIndexChanged" />
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        *Seleccione los campos que servirán como criterios de busquedas en la tabla
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top;"></td>
                    <td>

                        <asp:Panel ID="pnlcampos" runat="server">
                            <%--<asp:CheckBoxList ID="chlCampos" runat="server" />--%>
                            <asp:GridView ID="grvCampos" runat="server" AutoGenerateColumns="false" CssClass="Grilla">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="ckbSeleccionar" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Campo">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCampo" Text='<%# Eval("COLUMN_NAME") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tipo dato"  >
                                        <ItemTemplate>
                                            <asp:Label ID="lblTipoDato" runat="server" Text=<%# Eval("Data_Type") %> />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: right;">
                        <asp:Button ID="btnguardar" runat="server" Text="Guardar Configuración" OnClick="btnguardar_Click" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>


    <script type="text/javascript">
        var setItm = function (eventSource) {
            var idfld = event.target.id;//.split('_')[1]
            var okm = document.getElementById('ItmHiddenField');
            if (okm != null) {
                okm.value = okm.value + '|' + idfld + event.target.checked + '|';
            }
        }
    </script>
</asp:Content>
