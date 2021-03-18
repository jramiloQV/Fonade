<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="VerTareaEspecial.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Administracion.Interventoria.TareaEspecial.VerTareaEspecial" %>
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
            return confirm('Se enviara una notificación al destinatario. ¿ Desea continuar ?');
        }

        function cerrarTarea() {
            return confirm('La tarea se cerrara y no podra ser abierta nuevamente. ¿ Desea continuar ?');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="true" EnableScriptLocalization="true" ></asp:ToolkitScriptManager>    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always" >               
        <ContentTemplate>
            <h1>
                <asp:Label Text="Tarea de interventoria" runat="server" ID="lblMainTitle" Visible="true" />
            </h1            
            <br />
            <table id="gvMain" class="auto-style2" runat="server">
                <tr>
                    <td class="auto-style3" colspan="6">
                        <asp:Label ID="lbltitleDescripcionTarea" Font-Bold="true" runat="server" Text="Descripción de la tarea"></asp:Label>
                        <br />   
                        <br />   
                        <asp:Label ID="lblDescripcionTarea"  runat="server" Text="N/A"></asp:Label>
                        <br />   
                        <br />   
                        <asp:LinkButton ID="lnkArchivoDescripcionTarea" Text="Descargar archivo adjunto" runat="server" OnClick="lnkArchivoDescripcionTarea_Click"></asp:LinkButton>
                        <br /> 
                        <br /> 
                    </td>
                </tr>                
                <tr>
                    <td class="auto-style3"> <asp:Label ID="lblTitle1" Font-Bold="true" runat="server" Text="Fecha de creación"></asp:Label></td>
                    <td class="auto-style3"> <asp:Label ID="lblTitle2" Font-Bold="true" runat="server" Text="Remitente"></asp:Label></td>
                    <td class="auto-style3"> <asp:Label ID="lblTitle3" Font-Bold="true" runat="server" Text="Destinatario"></asp:Label></td>                    
                    <td class="auto-style3"> <asp:Label ID="lbltitle4" Font-Bold="true" runat="server" Text="Estado"></asp:Label></td>
                </tr>
                <tr>                    
                    <td><asp:Label ID="lblFechaCreacion" runat="server" Text="N/A"></asp:Label></td>
                    <td><asp:Label ID="lblRemitente" runat="server" Text="N/A"></asp:Label></td>
                    <td><asp:Label ID="lblDestinatario" runat="server" Text="N/A"></asp:Label></td>                    
                    <td><asp:Label ID="lblEstado" runat="server" Text="N/A"></asp:Label></td>
                </tr>                  
                <tr>
                    <td class="auto-style3" colspan="6">
                        <br /> 
                        <asp:Label ID="lblTitle7" Font-Bold="true" runat="server" Text="Añadir comentario"></asp:Label>
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
                    <td colspan="2">
                        <asp:Label ID="lblError" runat="server" ForeColor="Red" Text="Sucedio un error." Visible="False"></asp:Label>
                    </td>
                    <td >
                        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" PostBackUrl="~/PlanDeNegocioV2/Administracion/Interventoria/TareaEspecial/TareasEspecialesGerencia.aspx" ></asp:Button>            
                    </td>
                    <td >
                        <asp:Button ID="btnAdicionar" runat="server" Text="Añadir comentario" OnClientClick="return alerta();" OnClick="btnAdd_Click"></asp:Button>
                    </td>
                    <td >
                        <asp:Button ID="btnCerrarTarea" Visible="false" runat="server" Text="Cerrar tarea" OnClientClick="return cerrarTarea();" OnClick="btnCerrarTarea_Click"></asp:Button>
                    </td>                    
                </tr>               
            </table>
            <br />       
            <br />
            <h1>
                <asp:Label ID="lblTitle9" Text="Actividad de la tarea" runat="server" Visible="true" />               
            </h1            
            <br />

        <asp:GridView ID="gvHistoriaTarea" runat="server" AllowPaging="false"   AutoGenerateColumns="False" DataSourceID="data" EmptyDataText="No se encontro información." Width="98%" BorderWidth="0" CellSpacing="1" CellPadding="4" CssClass="Grilla" HeaderStyle-HorizontalAlign="Left" OnRowCommand="gvHistoriaTarea_RowCommand" >
        <Columns>                                   
            <asp:TemplateField HeaderText="Historico de comentarios" >
                <ItemTemplate>
                    <asp:Label Text="Comentario" runat="server" Font-Bold="true" />                     
                    <br />                                        
                    <asp:Label Text='<%# Eval("Observacion") %>' runat="server" />
                    <br />                                        
                    <br />                    
                    <asp:LinkButton ID="lnkDescargarArchivo" CommandArgument='<%# Eval("Archivo") %>' CommandName="VerDocumento" CausesValidation="False" Text='Descargar archivo' runat="server"  Font-Bold="true" Visible='<%# (bool)Eval("HasFile") %>' />                                        
                    <br />                    
                    <asp:Label Text='<%# "Autor: " + Eval("NombreRemitente") + "- " + Eval("FechaCreacionWithFormat") %>' runat="server"  />                                        
                    <asp:Image runat="server" ID="imgNoHasUpdates" Height="24" Width="24" ImageUrl="~/Images/PlanNegocioV2/Tareas/NotificationV3.png" Visible='<%# (bool)Eval("IsNewMessage") %>' />
                </ItemTemplate>
            </asp:TemplateField>   
        </Columns>
        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
        </asp:GridView>
            
        <asp:ObjectDataSource
                    ID="data"
                    runat="server"
                    TypeName="Fonade.PlanDeNegocioV2.Administracion.Interventoria.TareaEspecial.VerTareaEspecial"
                    SelectMethod="GetHistoria" >
                <SelectParameters> 
                    <asp:QueryStringParameter Name="tareaEspecial" Type="String" DefaultValue="0" QueryStringField="tareaEspecial" />                    
                </SelectParameters>  
        </asp:ObjectDataSource> 
        </ContentTemplate>   
        <Triggers>
            <asp:PostBackTrigger ControlID="btnAdicionar" />
            <asp:PostBackTrigger ControlID="gvHistoriaTarea" />
            <asp:PostBackTrigger ControlID="lnkArchivoDescripcionTarea" />            
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>