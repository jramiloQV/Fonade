<%@ Page Language="C#" MasterPageFile="~/Master.master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="CargarContratos1.aspx.cs" Inherits="Fonade.FONADE.Administracion.CargarContratos1" %>

<asp:Content ID="head1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        table {
            width: 100%;
        }

        td {
            vertical-align: top;
        }

        .formatoTabla {
            text-align: center;
        }
    </style>
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="bodyContentPlace">
    <% Page.DataBind(); %>
    <h1>
        <label>
            CARGUE MASIVO DE CONTRATOS</label>
    </h1>
    <br />
    <br />
    <asp:Panel ID="pnlCargar" runat="server">
        <table>
            <tr>
                <td>Seleccione el archivo:</td>
            </tr>
            <tr>
                <td>
                    <asp:FileUpload ID="fld_cargar" runat="server" Width="400px" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btncargar" runat="server" OnClick="btncargar_Click" Text="Subir Archivo" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblInfo" runat="server" Text='<%# DataBinder.GetPropertyValue(this, "lblInfo_") %>'></asp:Label>
                </td>
            </tr>
        </table>
        <br />
        <br />
    </asp:Panel>
</asp:Content>
