<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="UpdateEntidad.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Administracion.Interventoria.Entidad.UpdateEntidad" %>
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
            return confirm('¿ Esta seguro de actualizar esta entidad ?');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" ></asp:ToolkitScriptManager>    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" >               
        <ContentTemplate>
            <h1>
                <asp:Label Text="Actualizar entidad" runat="server" ID="lblMainTitle" Visible="true" />
            </h1            
            <br />
            <table id="gvMain" class="auto-style2" runat="server">
                <tr>
                    <td class="auto-style3" >
                        <asp:Label ID="Label1" Font-Bold="true" runat="server" Text="Nombre"></asp:Label>
                        <br />                        
                        <asp:TextBox ID="txtNombre" Width="422px"  runat="server" />            
                    </td>                    
                </tr>
                <tr>
                    <td class="auto-style3" colspan="6">
                        <asp:Label ID="Label2" Font-Bold="true" runat="server" Text="Nombre corto"></asp:Label>
                        <br />                        
                        <asp:TextBox ID="txtNombreCorto" Width="422px"  runat="server" />            
                    </td>                    
                </tr>
                <tr>
                    <td class="auto-style3" >
                        <asp:Label ID="Label3" Font-Bold="true" runat="server" Text="Número de poliza"></asp:Label>
                        <br />                        
                        <asp:TextBox ID="txtNumeroPoliza" Width="422px"  runat="server" />            
                    </td>                    
                </tr>
                <tr>
                    <td class="auto-style3" >
                        <asp:Label ID="Label11" Font-Bold="true" runat="server" Text="Fecha de poliza"></asp:Label>
                        <br />                        
                        <asp:TextBox ID="txtFechaPoliza" runat="server" BackColor="White" Width="100px" />
                        <asp:Image ID="btnDatePicker" runat="server" AlternateText="cal1" ImageAlign="AbsBottom" ImageUrl="~/Images/calendar.png" Height="21px" Width="20px" />
                        <asp:CalendarExtender ID="CalendarfechaI" runat="server" Format="dd/MM/yyyy" CssClass="ajax__calendar" PopupButtonID="btnDatePicker" TargetControlID="txtFechaPoliza" />                                        
                    </td>                    
                </tr>
                <tr>
                    <td class="auto-style3" >
                        <asp:Label ID="Label4" Font-Bold="true" runat="server" Text="Persona a cargo"></asp:Label>
                        <br />                        
                        <asp:TextBox ID="txtPersonaACargo" Width="422px"  runat="server" />            
                    </td>                    
                </tr>
                <tr>
                    <td class="auto-style3" >
                        <asp:Label ID="Label5" Font-Bold="true" runat="server" Text="Telefono oficina"></asp:Label>
                        <br />                        
                        <asp:TextBox ID="txtTelefono" Width="422px"  runat="server" />            
                    </td>                    
                </tr>
                <tr>
                    <td class="auto-style3" >
                        <asp:Label ID="Label6" Font-Bold="true" runat="server" Text="Telefono celular"></asp:Label>
                        <br />                        
                        <asp:TextBox ID="txtCelular" Width="422px"  runat="server" />            
                    </td>                    
                </tr>
                <tr>
                    <td class="auto-style3" >
                        <asp:Label ID="Label7" Font-Bold="true" runat="server" Text="Dirección"></asp:Label>
                        <br />                        
                        <asp:TextBox ID="txtDireccion" Width="422px"  runat="server" />            
                    </td>                    
                </tr>
                <tr>
                    <td class="auto-style3" >
                        <asp:Label ID="Label8" Font-Bold="true" runat="server" Text="Dependencia"></asp:Label>
                        <br />                        
                        <asp:TextBox ID="txtDependencia" Width="422px"  runat="server" />            
                    </td>                    
                </tr>
                <tr>
                    <td class="auto-style3" >
                        <asp:Label ID="Label9" Font-Bold="true" runat="server" Text="Email"></asp:Label>
                        <br />                        
                        <asp:TextBox ID="txtEmail" Width="422px"  runat="server" />            
                    </td>                    
                </tr>
                <tr>
                    <td class="auto-style3" >
                        <asp:Label ID="Label13" Font-Bold="true" runat="server" Text="Operador"></asp:Label>
                        <br />                        
                        <asp:TextBox ID="txtOperador" Width="422px"  runat="server" Enabled="false" />            
                    </td>                    
                </tr>
                <tr>
                    <td class="auto-style3" >
                        <asp:Label ID="Label10" Font-Bold="true" runat="server" Text="Cambiar logo - 100px - 100px"></asp:Label>
                        <br /> 
                        <asp:Image ID="imgLogo" ImageUrl="image0url" runat="server" Visible="true" Width="100px" Height="100px" />
                        <br />                        
                        <asp:FileUpload ID="fuArchivo" runat="server" Width="422px" Visible="true" ToolTip="Reemplazar archivo" />                                  
                        <br />                        
                        <br />  
                    </td>                    
                </tr>
                <tr>
                    <td class="auto-style3" >
                        <asp:Label ID="Label12" Font-Bold="true" runat="server" Text="Seleccione los departamentos:"></asp:Label>
                        <br />   
                        <br />   
                        <asp:GridView ID="gvDepartamentos" runat="server" AllowPaging="false"   
                            AutoGenerateColumns="False" EmptyDataText="No hay datos que mostrar." 
                            Width="98%" BorderWidth="0" CellSpacing="1" CellPadding="4" CssClass="Grilla" 
                            HeaderStyle-HorizontalAlign="Left" ShowHeaderWhenEmpty="true" Visible="true" 
                            EnableViewState="true" DataSourceID="data" OnRowDataBound="gvProyectos_RowDataBound">
                                <Columns>   
                                    <asp:TemplateField HeaderText="#">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chkDepartamento"  Checked='<%# (bool)Eval("CheckDepartamento") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>         
                                    <asp:TemplateField HeaderText="Departamentos" >
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("NombreDepartamento") %>' runat="server" Font-Bold="true" />
                                            <br />                                            
                                            <asp:HiddenField runat="server" ID="hdCodigoDepartamento" Value='<%# Eval("Id") %>' />                                                                                        
                                            <asp:HiddenField runat="server" ID="hdCodigoZona" Value='<%# Eval("ZonaDepartamento") %>' />                                                                                        
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Zona" >
                                        <ItemTemplate>  
                                            <asp:DropDownList ID="DropDownList1" runat="server" DataValueField="Id" DataTextField="Nombre">  
                                            </asp:DropDownList>  
                                        </ItemTemplate>                  
                                    </asp:TemplateField>                                    
                                </Columns>
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        </asp:GridView>
                    </td>                    
                </tr>    
                <tr>
                    <td >
                        <asp:Label ID="lblError" runat="server" ForeColor="Red" Text="Sucedio un error." Visible="False"></asp:Label>
                        <br />
                        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" PostBackUrl="~/PlanDeNegocioV2/Administracion/Interventoria/Entidad/Entidades.aspx" ></asp:Button>            
                        <asp:Button ID="btnAdicionar" runat="server" Text="Actualizar entidad" 
                            OnClientClick="return alerta();" OnClick="btnAdd_Click"></asp:Button>            
                    </td>
                </tr>               
            </table>
            <br />                
            <asp:ObjectDataSource
                    ID="data"
                    runat="server"
                    TypeName="Fonade.PlanDeNegocioV2.Administracion.Interventoria.Entidad.UpdateEntidad"
                    SelectMethod="GetDepartamentos"
                    EnablePaging="false">    
                <SelectParameters> 
                    <asp:QueryStringParameter Name="idEntidad" Type="String" DefaultValue="0" QueryStringField="codigo" />                    
                </SelectParameters>
            </asp:ObjectDataSource>
        </ContentTemplate>   
    <Triggers>
                  <asp:PostBackTrigger ControlID="btnAdicionar" />
              </Triggers>
    </asp:UpdatePanel>
</asp:Content>
