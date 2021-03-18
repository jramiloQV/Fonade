<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="UpdateContrato.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Administracion.Abogado.VisorContrato.UpdateContrato" %>
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
            return confirm('¿ Esta seguro de actualizar este contrato ?');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="true" EnableScriptLocalization="true" ></asp:ToolkitScriptManager>    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always" >               
        <ContentTemplate>
            <h1>
                <asp:Label Text="Actualizar contrato" runat="server" ID="lblMainTitle" Visible="true" />
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
                        <asp:Label ID="Label4" Font-Bold="true" runat="server" Text="Fecha de firma del contrato"></asp:Label>
                        <br />                   
                        <asp:TextBox ID="txtFechaFirmaContrato" runat="server" BackColor="White" Width="100px" />
                        <asp:Image ID="btnDateFirma" runat="server" AlternateText="cal1" ImageAlign="AbsBottom" ImageUrl="~/Images/calendar.png" Height="21px" Width="20px" />
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MM/yyyy" CssClass="ajax__calendar" PopupButtonID="btnDateFirma" TargetControlID="txtFechaFirmaContrato" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3" >
                        <asp:Label ID="Label11" Font-Bold="true" runat="server" Text="Fecha de inicio"></asp:Label>
                        <br />                        
                        <asp:Label ID="lblFechaInicio"  runat="server" Text="Fecha de inicio"></asp:Label>                        
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3" >
                        <asp:Label ID="Label3" Font-Bold="true" runat="server" Text="Objeto del contrato"></asp:Label>
                        <br />                        
                        <asp:Label ID="lblObjeto" runat="server" ></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3" >
                        <asp:Label ID="Label5" Font-Bold="true" runat="server" Text="Certificado de disponibilidad"></asp:Label>
                        <br />
                        <asp:TextBox ID="txtCertificadoDisponibilidad" Width="422px"  runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3" >
                        <asp:Label ID="Label6" Font-Bold="true" runat="server" Text="Fecha de certificado de disponibilidad"></asp:Label>
                        <br />                        
                        <asp:TextBox ID="txtFechaCertificadoDisponibilidad" runat="server" BackColor="White" Width="100px" />
                        <asp:Image ID="btnDateFechaCertificadoDisponibilidad" runat="server" AlternateText="cal1" ImageAlign="AbsBottom" ImageUrl="~/Images/calendar.png" Height="21px" Width="20px" />
                        <asp:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd/MM/yyyy" CssClass="ajax__calendar" PopupButtonID="btnDateFechaCertificadoDisponibilidad" TargetControlID="txtFechaCertificadoDisponibilidad" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3" >
                        <asp:Label ID="Label7" Font-Bold="true" runat="server" Text="Plazo inicial del contrato en meses"></asp:Label>
                        <br />                        
                        <asp:Label ID="lblPlazoInicialContrato" runat="server" ></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3" >
                        <asp:Label ID="Label8" Font-Bold="true" runat="server" Text="Número de acta concejo directivo"></asp:Label>
                        <br />
                        <asp:TextBox ID="txtNumeroActaConcejoDirectivo" Width="422px"  runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3" >
                        <asp:Label ID="Label9" Font-Bold="true" runat="server" Text="Fecha de acta concejo directivo"></asp:Label>
                        <br />                        
                        <asp:TextBox ID="txtFechaActaConcejoDirectivo" runat="server" BackColor="White" Width="100px" />
                        <asp:Image ID="btnDateActaConcejoDirectivo" runat="server" AlternateText="cal1" ImageAlign="AbsBottom" ImageUrl="~/Images/calendar.png" Height="21px" Width="20px" />
                        <asp:CalendarExtender ID="CalendarExtender4" runat="server" Format="dd/MM/yyyy" CssClass="ajax__calendar" PopupButtonID="btnDateActaConcejoDirectivo" TargetControlID="txtFechaActaConcejoDirectivo" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3" >
                        <asp:Label ID="Label10" Font-Bold="true" runat="server" Text="Valor Ente"></asp:Label>
                        <br />
                        <asp:TextBox ID="txtValorEnte" Width="422px"  runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3" >
                        <asp:Label ID="Label12" Font-Bold="true" runat="server" Text="Valor Sena"></asp:Label>
                        <br />
                        <asp:TextBox ID="txtValorSena" Width="422px"  runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3" >
                        <asp:Label ID="Label13" Font-Bold="true" runat="server" Text="Número poliza seguro de vida"></asp:Label>
                        <br />
                        <asp:TextBox ID="txtNumeroPoliza" Width="422px"  runat="server" />
                    </td>
                </tr>                                              
                <tr>
                    <td class="auto-style3" >
                        <asp:Label ID="Label17" Font-Bold="true" runat="server" Text="Valor inicial en pesos"></asp:Label>
                        <br />
                        <asp:TextBox ID="txtValorInicial" Width="422px"  runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3" >
                        <asp:Label ID="Label18" Font-Bold="true" runat="server" Text="Tipo de contrato"></asp:Label>
                        <br />
                        <asp:TextBox ID="txtTipoContrato" Width="422px"  runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3" >
                        <asp:Label ID="Label19" Font-Bold="true" runat="server" Text="Estado"></asp:Label>
                        <br />
                        <asp:TextBox ID="txtEstado" Width="422px"  runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3" >
                        <asp:Label ID="Label2" Font-Bold="true" runat="server" Text="Emprendedores"></asp:Label>
                        <br />
                        <asp:Label ID="lblEmprendedores" runat="server" ></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3" >
                        <asp:Label ID="Label15" Font-Bold="true" runat="server" Text="Teléfono emprendedor"></asp:Label>
                        <br />
                        <asp:Label ID="lblTelefono" runat="server" ></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3" >
                        <asp:Label ID="Label16" Font-Bold="true" runat="server" Text="Email emprendedor"></asp:Label>
                        <br />
                        <asp:Label ID="lblEmail" runat="server" ></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3" >
                        <asp:Label ID="Label20" Font-Bold="true" runat="server" Text="Compañia seguros - Prórroga"></asp:Label>
                        <br />
                        <asp:Label ID="lblProrroga" runat="server" ></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3" >
                        <asp:Label ID="Label21" Font-Bold="true" runat="server" Text="Fecha final del contrato"></asp:Label>
                        <br />
                        <asp:Label ID="lblFechaFinalContrato" runat="server" ></asp:Label>
                    </td>
                </tr>

                <tr>
                    <td >
                        <asp:Label ID="lblError" runat="server" ForeColor="Red" Text="Sucedio un error." Visible="False"></asp:Label>
                        <br />
                        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" ></asp:Button>            
                        <asp:Button ID="btnAdicionar" runat="server" Text="Actualizar contrato" OnClientClick="return alerta();" OnClick="btnAdd_Click"></asp:Button>            
                    </td>
                </tr>               
            </table>
            <br />                                 
        </ContentTemplate>          
    </asp:UpdatePanel>
</asp:Content>
