<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="NewContrato.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Administracion.Interventoria.Entidad.Contrato.NewContrato" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  <style type="text/css">
        .auto-style2 {
            width: 100%;
        }
        .auto-style3 {
            width: 197px;
        }
    </style>
    <script type="text/javascript">        
        function alerta() {
            return confirm('¿ Esta seguro de crear este contrato ?');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="true" EnableScriptLocalization="true" ></asp:ToolkitScriptManager>    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always" >               
        <ContentTemplate>
            <h1>
                <asp:Label Text="Crear contrato" runat="server" ID="lblMainTitle" Visible="true" />
            </h1>
            <br />
            <table id="gvMain" class="auto-style2" runat="server">
                <tr>
                    <td class="auto-style3" >
                        <asp:Label ID="Label1" Font-Bold="true" runat="server" Text="Número de contrato"></asp:Label>
                        <br />
                        <asp:TextBox ID="txtNumeroContrato" Width="422px"  runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3" >
                        <asp:Label ID="Label11" Font-Bold="true" runat="server" Text="Fecha de inicio"></asp:Label>
                        <br />                        
                        <asp:TextBox ID="txtFechaInicio" runat="server" BackColor="White" Width="100px" />
                        <asp:Image ID="btnDatePicker" runat="server" AlternateText="cal1" ImageAlign="AbsBottom" ImageUrl="~/Images/calendar.png" Height="21px" Width="20px" />
                        <asp:CalendarExtender ID="CalendarfechaI" runat="server" Format="dd/MM/yyyy" CssClass="ajax__calendar" PopupButtonID="btnDatePicker" TargetControlID="txtFechaInicio" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3" >
                        <asp:Label ID="Label2" Font-Bold="true" runat="server" Text="Fecha de finalización"></asp:Label>
                        <br />                        
                        <asp:TextBox ID="txtFechaFin" runat="server" BackColor="White" Width="100px" />
                        <asp:Image ID="Image1" runat="server" AlternateText="cal1" ImageAlign="AbsBottom" ImageUrl="~/Images/calendar.png" Height="21px" Width="20px" />
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy" CssClass="ajax__calendar" PopupButtonID="Image1" TargetControlID="txtFechaFin" />
                    </td>                    
                </tr>
                <tr>
                    <td >
                        <asp:Label ID="lblError" runat="server" ForeColor="Red" Text="Sucedio un error." Visible="False"></asp:Label>
                        <br />
                        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" ></asp:Button>            
                        <asp:Button ID="btnAdicionar" runat="server" Text="Guardar contrato" OnClientClick="return alerta();" OnClick="btnAdd_Click"></asp:Button>            
                    </td>
                </tr>               
            </table>
            <br />                                 
        </ContentTemplate>          
    </asp:UpdatePanel>
</asp:Content>
