<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IngresarInformacionAcademica.aspx.cs" Inherits="Fonade.FONADE.MiPerfil.IngresarInformacionAcademica" MasterPageFile="~/Emergente.Master" Culture="es-CO" UICulture="es-CO" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css"/>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="true" EnableScriptLocalization="true"></asp:ToolkitScriptManager>
   <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
      <ContentTemplate>
                                                  
         <div style="width:1000px;margin:0 auto; background-color: #FFFFFF;">
            <div class="form-horizontal no-margin center-block">
               <div class="form-group center-block">                  
                  <div class="col-xs-4">
                    </div>
                   <div class="col-xs-4">
                    <h3>
                        <asp:Label Text="Programa academico" runat="server" ID="lblTituloCrearEmprendedor" Visible="false" />
                        <asp:Label Text="Actualizar programa academico" runat="server" ID="lblTituloActualizarEmprendedor" Visible="False"  />
                    </h3>
                  </div>
               </div>
                <asp:HiddenField ID="hfCodigoEmprendedor" runat="server" Value="" Visible="false" />
                <asp:HiddenField ID="hfCodigoContactoEstudio" runat="server" Value="" Visible="false" />
               <div class="form-group center-block">
                  <label class="control-label col-xs-4"> Nivel de estudio : </label>
                  <div class="col-xs-4">
                     <asp:DropDownList ID="cmbNivelEstudio" runat="server" nombre="Nivel estudio" DataSourceID="dataNivelEstudio" DataTextField="Nombre" DataValueField="Id" AutoPostBack="true" />
                  </div>
               </div>
               <div class="form-group center-block">
                  <label class="control-label col-xs-4"> Programa Realizado: </label>
                  <div class="col-xs-4">
                     <asp:HiddenField ID="hfcodigoProgramaRealizado" runat="server" Value="" Visible="false" />
                     <asp:TextBox ID="txtProgramaRealizado" runat="server" Enabled="false" />
                     <asp:ImageButton ID="imbtn_institucion" ImageUrl="~/Images/icoComentario.gif"  runat="server" OnClick="imbtn_institucion_Click" />
                  </div>
               </div>
               <div class="form-group center-block">
                  <label class="control-label col-xs-4"> Institución: </label>
                  <div class="col-xs-4">
                      <asp:HiddenField ID="hfCodigoInstitucionEducativa" runat="server" Value="" Visible="false" />
                      <asp:TextBox ID="txtInstitucionEducativa" runat="server" Enabled="false" />
                     <asp:ImageButton ID="imbtn_nivel" ImageUrl="~/Images/icoComentario.gif" runat="server" OnClick="imbtn_nivel_Click" />
                  </div>
               </div>
               <div class="form-group center-block">
                  <label class="control-label col-xs-4"> Ciudad Institución: </label>
                  <div class="col-xs-4">
                     <asp:HiddenField ID="hfCodigoCiudadInstitucionEducativa" runat="server" Value="" Visible="false" />                   
                     <asp:TextBox runat="server" ID="txtCiudadInstitucion" Enabled="false" />
                  </div>
               </div>
               <div class="form-group center-block">
                  <label class="control-label col-xs-4"> Estado: </label>
                  <div class="col-xs-4">
                     <asp:DropDownList runat="server" ID="cmbEstadoEstudio" nombre="Estado" AutoPostBack="true" OnTextChanged="cmbEstadoEstudio_TextChanged" OnLoad="cmbEstadoEstudio_Load" >        
                        <asp:ListItem Text="Actualmente cursando" Value="0" />
                        <asp:ListItem Text="Finalizado" Value="1" />
                     </asp:DropDownList>
                  </div>
               </div>
               <div class="form-group center-block">
                  <label class="control-label col-xs-4"> Fecha inicio: </label>
                  <div class="col-xs-4">
                     <asp:TextBox runat="server" ID="txtFechaInicioEstudio" Enabled="false" placeholder="dd/mm/aaaa" />
                     <asp:Image runat="server" ID="img_dateInicio" AlternateText="Calendario fecha de inicio estudio" ImageUrl="~/images/icomodificar.gif" />
                     <ajaxToolkit:CalendarExtender runat="server" ID="Calendarextender1" PopupButtonID="img_dateInicio"
                        CssClass="ajax__calendar" TargetControlID="txtFechaInicioEstudio" Format="dd/MM/yyyy" ClearTime="True" />
                  </div>
               </div>
               <div id="formFechaFinalizacion" runat="server" visible="false" class="form-group center-block">
                  <label class="control-label col-xs-4"> Fecha finalización: </label>
                  <div class="col-xs-4">
                     <asp:TextBox runat="server" ID="txtFechaFinalizacionEstudio" Enabled="false" placeholder="dd/mm/aaaa" />
                     <asp:Image runat="server" ID="Image3" AlternateText="Calendario de fecha de finalizacion de estudio" ImageUrl="~/images/icomodificar.gif" />
                     <ajaxToolkit:CalendarExtender runat="server" ID="Calendarextender4" PopupButtonID="Image3"
                        CssClass="ajax__calendar" TargetControlID="txtFechaFinalizacionEstudio" Format="dd/MM/yyyy" ClearTime="True" />
                  </div>
               </div>
               <div id="formFechaGraduacion" runat="server" visible="false" class="form-group center-block">
                  <label class="control-label col-xs-4"> Fecha graduación: </label>
                  <div class="col-xs-4">
                     <asp:TextBox runat="server" ID="txtFechaGraduacionEstudio" Enabled="false" placeholder="dd/mm/aaaa" />
                     <asp:Image runat="server" ID="Image2" AlternateText="Calendario de fecha de graduación" ImageUrl="~/images/icomodificar.gif" />
                     <ajaxToolkit:CalendarExtender runat="server" ID="Calendarextender3" PopupButtonID="Image2"
                        CssClass="ajax__calendar" TargetControlID="txtFechaGraduacionEstudio" Format="dd/MM/yyyy" DefaultView="Days" ClearTime="True" />
                  </div>
               </div>
               <div id="formHorasDedicadas" runat="server" visible="false" class="form-group center-block">
                  <label class="control-label col-xs-4"> Semestre actual u horas dedicadas: </label>
                  <div class="col-xs-4">
                     <asp:TextBox runat="server" ID="txtHorasDedicadas" Enabled="true" Visible="true"/>
                  </div>
               </div>
               <asp:updateprogress id="UpdateProgress1" runat="server" associatedupdatepanelid="UpdatePanel1" dynamiclayout="true" >
                <progresstemplate>
                    <div class="form-group center-block">                                                                 
                        <div class="col-xs-4">
                        </div>
                        <div class="col-xs-4">
                            <label class="control-label"> <b>Procesando información</b> </label>
                            <img class="control-label" src="http://www.bba-reman.com/images/fbloader.gif" />                                  
                        </div>
                    </div>                                                                
                 </progresstemplate>
               </asp:updateprogress>           
               <div class="form-group center-block">
                  <asp:Label ID="lblErrorCrearEmprendedor" CssClass="control-label col-xs-8" ForeColor="Red" Text="Sucedio un error" runat="server" Visible="false" />  
               </div>
               <div class="form-group center-block">
                  <div class="col-xs-4">                      
                  </div>
                  <div class="col-xs-4">
                     <asp:Button ID="btnCrearEmprendedor" runat="server" Text="Crear programa academico" OnClick="btnCrearEmprendedor_Click" Visible="false" />
                     <asp:Button ID="btnActualizarEmprendedor" runat="server" Text="Actualizar programa academico" OnClick="btnActualizarEmprendedor_Click" Visible="False" />
                     <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" Visible="true" OnClick="btnCancelar_Click" />
                  </div>
               </div>
            </div>
         </div>
     
         <asp:ObjectDataSource 
            ID="dataNivelEstudio"
            runat="server"
            TypeName="Fonade.FONADE.PlandeNegocio.CrearEmprendedor"
            SelectMethod="getNivelesEstudio"
            >
         </asp:ObjectDataSource>
                       
      </ContentTemplate>
   </asp:UpdatePanel>
     
      <asp:Panel id="ModalEstudios" CssClass="modal fade" runat="server" role="dialog">
      <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
          <div class="modal-dialog modal-lg">
         <!-- Modal content-->
         <div class="modal-content">
            <div class="modal-header">
               <button type="button" class="close" data-dismiss="modal">&times;</button>
               <h4 class="modal-title">Programas academicos</h4>
            </div>
            
            <div class="modal-body">
                       <div id="buscador" runat="server" visible="true">    
                           <div class="form-group center-block">
                              <label id="lblTituloBusqueda" > Buscar: </label>
                              <asp:TextBox runat="server" ID="txtBusquedaPorNombre" Enabled="true" AutoPostBack="false" />                                                                                        
                              <label id="lblTituloSeleccionarCiudad" > Ciudad: </label>
                              <asp:DropDownList ID="cmbAllCiudades" runat="server" nombre="Ciudades" DataSourceID="dataAllCiudades" AutoPostBack="true" DataValueField="Id" DataTextField="Nombre" />                             
                              <asp:Button ID="btnBuscarEstudio" runat="server" Text="Buscar" Visible="true" OnClick="btnBuscarEstudio_Click" />
                              <asp:Button ID="btnCrearPrograma" runat="server" Text="Crear programa academico" Visible="true" OnClick="btnCrearPrograma_Click" />
                           </div>
                       </div>
                       <div id="nuevoProgramaAcademico" runat="server" visible="false">
                            <div class="form-horizontal no-margin center-block">                                                                                        
                            <div class="form-group center-block">
                                <label class="control-label col-xs-4"> Institución educativa: </label>
                                <div class="col-xs-4">
                                    <asp:DropDownList runat="server" ID="cmbInstitucionEducativa" nombre="Institucion educativa" DataSourceID="dataInstitucionesEducativas" DataTextField="NombreYLocacion" DataValueField="Id" Width="200px" OnTextChanged="cmbInstitucionEducativa_TextChanged" OnDataBound="cmbInstitucionEducativa_DataBound" AutoPostBack="true"/>                                        
                                </div>
                            </div>
                            <div id="formNuevaInstitucion" class="form-group center-block" runat="server" visible="false">
                                <label class="control-label col-xs-4"> Nueva institución educativa: </label>
                                <div class="col-xs-4">
                                    <asp:TextBox runat="server" ID="txtNuevaInstitucion" Text="" Width="200px" />
                                </div>
                            </div>
                            <div class="form-group center-block">
                                <label class="control-label col-xs-4"> Nombre del nuevo programa academico: </label>
                                <div class="col-xs-4">
                                    <asp:TextBox runat="server" ID="txtNuevoPrograma" Text="" Width="200px" />
                                </div>
                            </div>
                            <div class="form-group center-block">
                                <label class="control-label col-xs-4"> Departamento de institución: </label>
                                <div class="col-xs-4">
                                    <asp:DropDownList runat="server" ID="cmbDepartamentoInstitucion" nombre="Departamento institución" DataSourceID="dataDepartamentoInstituciones" DataTextField="Nombre" DataValueField="Id" AutoPostBack="true" Width="200px" />
                                </div>
                            </div>
                           <div class="form-group center-block">
                                <label class="control-label col-xs-4"> Ciudad de institución: </label>
                                <div class="col-xs-4">
                                    <asp:DropDownList runat="server" ID="cmbCiudadInstitucion" nombre="Ciudad institución" DataSourceID="dataCiudadInstituciones" DataTextField="Nombre" DataValueField="Id" Width="200px" />
                                </div>
                            </div>
                           <div class="form-group center-block">
                               <asp:Label ID="lblErrorProgramaAcademico" CssClass="control-label col-xs-8" ForeColor="Red" Text="Sucedio un error" runat="server" Visible="false" />                                                                
                            </div>
                           <div class="form-group center-block">                                
                                <div class="col-xs-8">
                                  <asp:Button ID="btnNuevoProgramaAcademico" runat="server" Text="Crear programa academico" Visible="true" OnClick="btnNuevoProgramaAcademico_Click" />
                                  <asp:Button ID="btnCancelarNuevoProgramaAcademico" runat="server" Text="Cancelar" OnClick="btnCancelarNuevoProgramaAcademico_Click" Visible="true" />            
                                </div>
                            </div>
                        </div>
                       </div>
                       <div id="listadoProgramas" runat="server" visible="true">
                           <asp:GridView ID="gvProgramaAcademico" runat="server" AutoGenerateColumns="false" CssClass="Grilla" OnRowCommand="gvProgramaAcademico_RowCommand"
                                    HeaderStyle-HorizontalAlign="Left" RowStyle-HorizontalAlign="Left" Width="100%"
                                    PageSize="10" AllowPaging="true" DataSourceID="dataProgramasAcademicos" >
                                    <Columns>
                                        <asp:TemplateField HeaderText="Programa Académico">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="linkPrograma" runat="server" CausesValidation="false"
                                                    CommandArgument='<%# Eval("Id") +";"+ Eval("Nombre") +";"+ Eval("Ciudad")+";"+ Eval("CodigoInstitucionEducativa")+";"+ Eval("InstitucionEducativa") + ";" + Eval("CodigoCiudad") %>'
                                                    CommandName="seleccionarPrograma" Text='<%# Eval("Nombre")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Institución Educativa">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="linkInstitucion" runat="server" CausesValidation="false" CommandArgument='<%# Eval("Id") +";"+ Eval("Nombre") +";"+ Eval("Ciudad")+";"+ Eval("CodigoInstitucionEducativa")+";"+ Eval("InstitucionEducativa") + ";" + Eval("CodigoCiudad") %>'
                                                    CommandName="seleccionarPrograma" Text='<%# Eval("InstitucionEducativa")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Ciudad">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="linkCiudad" runat="server" CausesValidation="false" CommandArgument='<%# Eval("Id") +";"+ Eval("Nombre") +";"+ Eval("Ciudad")+";"+ Eval("CodigoInstitucionEducativa")+";"+ Eval("InstitucionEducativa") + ";" + Eval("CodigoCiudad") %>'
                                                    CommandName="seleccionarPrograma" Text='<%# Eval("Ciudad")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                       </div>                                                                                                                      
            </div>          
            <asp:ObjectDataSource 
                ID="dataAllCiudades"
                runat="server"
                TypeName="Fonade.FONADE.PlandeNegocio.CrearEmprendedor"
                SelectMethod="getAllCiudades">
            </asp:ObjectDataSource> 
            
            <asp:ObjectDataSource 
                ID="dataProgramasAcademicos" 
                runat="server" 
                EnablePaging="true" 
                SelectMethod="getProgramasAcademicos"
                SelectCountMethod="getProgramasAcademicosCount" 
                TypeName="Fonade.FONADE.PlandeNegocio.CrearEmprendedor"
                MaximumRowsParameterName="maxRows"
                StartRowIndexParameterName="startIndex" >
              <SelectParameters>
                <asp:ControlParameter ControlID="cmbNivelEstudio" DefaultValue="0" Name="codigoNivelEstudio" PropertyName="SelectedValue" Type="Int32" />               
                <asp:ControlParameter ControlID="cmbAllCiudades" DefaultValue="0" Name="codigoCiudad" PropertyName="SelectedValue" Type="Int32" />                
                <asp:ControlParameter ControlID="txtBusquedaPorNombre" DefaultValue="" Name="nombrePrograma" PropertyName="Text" Type="String" />
              </SelectParameters>                
            </asp:ObjectDataSource>
            
            <asp:ObjectDataSource 
                ID="dataInstitucionesEducativas" 
                runat="server"                 
                SelectMethod="getInstitucionesAcademicas"                
                TypeName="Fonade.FONADE.PlandeNegocio.CrearEmprendedor"
              >
              <SelectParameters>
                <asp:ControlParameter ControlID="cmbNivelEstudio" DefaultValue="0" Name="codigoNivelEstudio" PropertyName="SelectedValue" Type="Int32" />                
              </SelectParameters>                
            </asp:ObjectDataSource>

             <asp:ObjectDataSource 
             ID="dataCiudadInstituciones" 
             runat="server" 
             SelectMethod="getCiudades" 
             TypeName="Fonade.FONADE.PlandeNegocio.CrearEmprendedor">
             <SelectParameters>
               <asp:ControlParameter ControlID="cmbDepartamentoInstitucion" DefaultValue="0" Name="codigoDepartamento" PropertyName="SelectedValue" Type="Int32" />
             </SelectParameters>
             </asp:ObjectDataSource>
             <asp:ObjectDataSource 
                ID="dataDepartamentoInstituciones"
                runat="server"
                TypeName="Fonade.FONADE.PlandeNegocio.CrearEmprendedor"
                SelectMethod="getDepartamentos"
                >
             </asp:ObjectDataSource>            
            <div class="modal-footer">                
                <asp:Button ID="btnCerrarModalProgramaAcademico" runat="server" Text="Cerrar" OnClick="btnCerrarModalProgramaAcademico_Click" Visible="true" />                
            </div>
         </div>
      </div>
          </ContentTemplate>
            </asp:UpdatePanel>
   </asp:Panel>

   <script src="https://code.jquery.com/jquery-1.9.1.min.js" type="text/javascript" ></script>
   <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/js/bootstrap.min.js" type="text/javascript" ></script>                    
</asp:Content>
