<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VerRiesgo.aspx.cs" MasterPageFile="~/Emergente.Master" Inherits="Fonade.FONADE.interventoria.Riesgos.VerRiesgo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .auto-style1 {
            width: 113px;
        }
        .auto-style2 {
            width: 253px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <% Page.DataBind(); %>
    <div>        
        <asp:Panel ID="pnlRiesgo" runat="server" Height="126px">            
             <h1>
                <asp:Label Text="Adicionar riesgos Identificados y Mitigación" runat="server" ID="lblNuevo" Visible='<%# (!(bool)DataBinder.GetPropertyValue(this, "esActualizacion")) %>' />
                <asp:Label Text="Modificar Riesgos Identificados y Mitigación" runat="server" ID="lblUpdate" Visible='<%# ((bool)DataBinder.GetPropertyValue(this, "esActualizacion")) %>' /> 
            </h1>            
            <table class="style1" style="width: 500px;">
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblRiesgo" runat="server" Text="Riesgo"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:TextBox ID="txtRiesgo" runat="server" Width="100%" Height="50px" TextMode="MultiLine"></asp:TextBox>                        
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblMitigacion" runat="server" Text="Mitigacion"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:TextBox ID="txtMitigacion" runat="server" Width="100%" Height="50px" TextMode="MultiLine"  ></asp:TextBox>                       
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblEjeFuncional" runat="server" Text=" Eje Funcional:" ></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:DropDownList ID="cmbEjeFuncional" runat="server" Width="250px" DataSourceID="dsEjeFuncional" DataValueField="Id_EjeFuncional" DataTextField="NomEjeFuncional" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblObsercacion" runat="server" Text="Observación:" ></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:TextBox ID="txtObservacion" runat="server" Width="100%" Height="50px" TextMode="MultiLine" ></asp:TextBox>
                    </td>
                </tr>                
                <tr>
                    <td style="text-align: center;">
                        <asp:Button ID="btnNew" runat="server" Text="Crear" OnClick="btnNew_Click" Visible='<%# (!(bool)DataBinder.GetPropertyValue(this, "esActualizacion")) %>' />
                        &nbsp;
                        <asp:Button ID="btnUpdate" runat="server" Text="Actualizar" OnClick="btnUpdate_Click" Visible='<%# ((bool)DataBinder.GetPropertyValue(this, "esActualizacion")) %>' />
                        &nbsp;
                        <asp:Button ID="btnClose" Text="Cerrar" runat="server" OnClick="btnClose_Click" />
                    </td>
                    <td></td>
                </tr>
            </table>
        </asp:Panel>
        <br />        
    </div>        
        <asp:ObjectDataSource
            ID="dsEjeFuncional"
            runat="server"
            SelectMethod="getEjeFuncional"
            TypeName="Fonade.FONADE.interventoria.Riesgos.VerRiesgo" >
        </asp:ObjectDataSource>
</asp:Content>
