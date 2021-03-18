<%@ Page Language="C#" MasterPageFile="~/Master.master"  EnableEventValidation="false" AutoEventWireup="true" CodeBehind="PagosActividadCoordFiduciaria.aspx.cs" Inherits="Fonade.FONADE.interventoria.PagosActividadCoordFiduciaria" %>

 <asp:Content  ID="head1"   ContentPlaceHolderID="head" runat="server">
     <style type="text/css">
         table {
             width: 100%;
         }
         .ddlcon {
             width:400px;
         }
     </style>
    </asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="bodyContentPlace">
    <asp:Panel ID="pnlprincipal" runat="server">
        <h1>
            <label>APROBACION DE SOLICITUDES DE PAGO</label>
        </h1>
        <br />
        <br />
        <table>
            <tr>
                <td>Fiduciaria :</td>
                <td><asp:DropDownList ID="ddlcontacto" runat="server" DataTextField="Email" PostBackUrl="PagosActividadCoord.aspx"
                    DataValueField="Id_Contacto" CssClass="ddlcon" DataSourceID="odscontacto"></asp:DropDownList>
                    <asp:ObjectDataSource ID="odscontacto" runat="server" SelectMethod="lstcontacto" TypeName="Fonade.FONADE.interventoria.PagosActividadCoordFiduciaria"></asp:ObjectDataSource>
                </td>
            </tr>
        </table>
        <br />
        <table>
            <tr>
                <td>
                    <asp:Button ID="btnenviarOLD" runat="server" Visible="false" Text="Enviar" OnClick="btnenviar_Click" />
                    <asp:Button ID="btnEnviar" runat="server" Text="Enviar" PostBackUrl="PagosActividadCoord.aspx" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>