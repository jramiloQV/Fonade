<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="CrearTarea.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Administracion.Interventoria.TareaEspecial.CrearTarea" %>
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
            return confirm('La tarea sera enviada al interventor ¿ Esta seguro de hacerlo ?');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="true" EnableScriptLocalization="true" ></asp:ToolkitScriptManager>    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always" >               
        <ContentTemplate>
            <h1>
                <asp:Label Text="Crear tarea de interventoria" runat="server" ID="lblMainTitle" Visible="true" />
            </h1            
            <br />
            <table id="gvMain" class="auto-style2" runat="server">
                <tr>
                    <td class="auto-style3"> <asp:Label ID="lblTitle1" Font-Bold="true" runat="server" Text="Remitente"></asp:Label></td>
                    <td class="auto-style3"> <asp:Label ID="lblTitle2" Font-Bold="true" runat="server" Text="Interventor destinatario"></asp:Label></td>                    
                </tr>
                <tr>                    
                    <td><asp:Label ID="lblRemitente" runat="server" Text="N/A"></asp:Label></td>
                    <td><asp:Label ID="lblDestinatario" runat="server" Text="N/A"></asp:Label></td>                    
                </tr>                  
                <tr>
                    <td class="auto-style3" colspan="6">
                        <asp:Label ID="lblTitle7" Font-Bold="true" runat="server" Text="Descripción de la tarea"></asp:Label>
                        <br />     
                        <asp:TextBox ID="txtDescripcion" TextMode="MultiLine" Width="100%" Height="50px" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3" colspan="6">
                        <asp:LinkButton ID="lnkSeleccionarPago" Text="+ Adjuntar Archivo (Opcional)" OnClick="lnkSeleccionarPago_Click" runat="server"></asp:LinkButton>                                                
                        <br />                        
                        <asp:FileUpload ID="fuArchivo" runat="server" Width="422px" Visible="false" />                        
                    </td>                    
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Label ID="lblError" runat="server" ForeColor="Red" Text="Sucedio un error." Visible="False"></asp:Label>
                    </td>
                    <td >
                        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" PostBackUrl="~/PlanDeNegocioV2/Administracion/Interventoria/TareaEspecial/SeleccionarPago.aspx" ></asp:Button>            
                    </td>
                    <td >
                        <asp:Button ID="btnAdicionar" runat="server" Text="Enviar tarea" OnClientClick="return alerta();" OnClick="btnAdd_Click"></asp:Button>            
                    </td>
                </tr>               
            </table>
            <br />                             
        </ContentTemplate>   
        <Triggers>
            <asp:PostBackTrigger ControlID="btnAdicionar"  />                 
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
