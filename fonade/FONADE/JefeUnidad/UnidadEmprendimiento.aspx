<%@ Page Title="" Language="C#" MasterPageFile="~/Emergente.Master" AutoEventWireup="true" CodeBehind="UnidadEmprendimiento.aspx.cs" Inherits="Fonade.FONADE.JefeUnidad.UnidadEmprendimiento" Culture="es-CO" UICulture="es-CO" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   <link rel="stylesheet" type="text/css" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css"/>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
   <% Page.DataBind(); %>
   <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="true" EnableScriptLocalization="true"></asp:ToolkitScriptManager>
   <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
      <ContentTemplate>                                                  
         <div style="width:1000px;margin:0 auto;">
            <div class="form-horizontal no-margin center-block">
               <div class="form-group center-block">                  
                  <div class="col-xs-4">
                    </div>
                   <div class="col-xs-6">
                    <h3>
                        <asp:Label Text="Crear unidad de emprendimiento" runat="server" ID="lblTituloCrear" Visible="false" />
                        <asp:Label Text="Actualizar plan de negocio" runat="server" ID="lblTituloActualizar" Visible="false" />         
                    </h3>
                  </div>
               </div>                                
                <div class="form-group center-block">
                  <label class="control-label col-xs-4"> Tipo unidad : </label>
                  <div class="col-xs-4">                                         
                     <asp:DropDownList ID="cmbTipoInstitucion" runat="server" Width="179px" Style="height: 18px" DataSourceID="dsTipoUnidad" DataTextField="NomTipoInstitucion" DataValueField="Id_TipoInstitucion" AppendDataBoundItems="true" >
                        <Items>
                            <asp:ListItem Text="Seleccione" Value="" />
                        </Items>
                     </asp:DropDownList>     
                  </div>
               </div>
               <div class="form-group center-block">
                  <label class="control-label col-xs-4"> Nombre unidad : </label>
                  <div class="col-xs-4">
                     <asp:TextBox ID="txtNombreUnidad" runat="server" MaxLength="80" Width="219px" />
                  </div>
               </div>
               <div class="form-group center-block">
                  <label class="control-label col-xs-4"> Nombre Centro o Institución : </label>
                  <div class="col-xs-4">
                     <asp:TextBox ID="txtNombreInstitucion" runat="server" MaxLength="80" Width="219px" />
                  </div>
               </div>
               <div class="form-group center-block">
                  <label class="control-label col-xs-4"> NIT Centro o Institución : </label>
                  <div class="col-xs-4">
                     <asp:TextBox ID="txtNitInstitucion" runat="server" MaxLength="80" Width="219px" />
                  </div>
               </div>
               <div class="form-group center-block">
                  <label class="control-label col-xs-4"> Departamento :  </label>
                  <div class="col-xs-4">
                     <asp:DropDownList ID="cmbDepartamento" runat="server" Width="179px" Style="height: 18px" AutoPostBack="true" DataSourceID="dsDepartamento" DataValueField="Id" DataTextField="Nombre" >
                        <Items>
                            <asp:ListItem Text="Seleccione" Value="" />
                        </Items>
                    </asp:DropDownList> 
                  </div>
               </div>
               <div class="form-group center-block">
                  <label class="control-label col-xs-4"> Ciudad : </label>
                  <div class="col-xs-4">
                     <asp:DropDownList ID="cmbCiudad" runat="server" Width="179px" Style="height: 18px" DataSourceID="dsCiudad" DataTextField="Nombre" DataValueField="Id" >
                        <Items>
                            <asp:ListItem Text="Seleccione" Value="" />
                        </Items>
                    </asp:DropDownList> 
                  </div>
               </div>
               <div class="form-group center-block">
                  <label class="control-label col-xs-4"> Dirección de Correpondencia: </label>
                  <div class="col-xs-4">
                     <asp:TextBox ID="txtDireccion" runat="server" MaxLength="100" Width="219px" />
                  </div>
               </div>
                <div class="form-group center-block">
                  <label class="control-label col-xs-4"> Criterios de Selección : </label>
                  <div class="col-xs-4">
                     <asp:TextBox ID="txtCriterio" runat="server" TextMode="MultiLine" Rows="6" Columns="35" />
                  </div>
               </div>
               <div class="form-group center-block" >
                  <asp:HiddenField ID="hfCodigoJefeDeUnidad" runat="server" Value="" Visible="false" />
                  <label class="control-label col-xs-4"> Jefe de Unidad : </label>
                  <div class="col-xs-4">
                     <b><asp:Label ID="lblJefeUnidad" runat="server" Enabled ="false" Width="219px" Text="Sin jefe de unidad" ForeColor="Blue" /></b>
                     <asp:Button ID="btnBuscarJefeUnidad" Text="Buscar" runat="server" BackColor="#6699FF" OnClick="btnBuscarJefeUnidad_Click" />                      
                      <br />
                      <asp:Button ID="btnCambiarDatos" Text="Cambiar Datos de jefe de unidad" runat="server" BackColor="#6699FF" Visible="false" /> 
                  </div>
               </div>
               <div id="divMotivoCambio" class="form-group center-block" runat="server" Visible="false" >
                  <label class="control-label col-xs-4"> Razón de cambio de Jefe de Unidad : </label>
                  <div class="col-xs-4">
                     <asp:TextBox ID="txtMotivoCambioJefeUnidad" runat="server" Columns="35" Rows="5" TextMode="MultiLine" />                      
                  </div>
               </div>                                                                                                                                                   
                                                                                          
               <div class="form-group center-block">
                  <asp:Label ID="lblError" CssClass="control-label col-xs-8" ForeColor="Red" Text="Sucedio un error" runat="server" Visible="false" />  
               </div>
               <div class="form-group center-block">
                  <div class="col-xs-4">
                  </div>
                  <div class="col-xs-4">
                     <asp:Button ID="btnCrearUnidad" runat="server" Text="Crear unidad de emprendimiento" OnClick="btnCrear_Click" Visible="false" />
                     <asp:Button ID="btnActualizarUnidad" runat="server" Text="Actualizar unidad de emprendimiento" OnClick="btnActualizar_Click" Visible="false" />
                     <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" Visible="true" OnClick="btnCancelar_Click" />
                  </div>
               </div>
            </div>
         </div>

         <asp:ObjectDataSource ID="dsTipoUnidad" runat="server" EnablePaging="false" SelectMethod="getTipoUnidad"
            TypeName="Fonade.FONADE.JefeUnidad.UnidadEmprendimiento" >
         </asp:ObjectDataSource>

         <asp:ObjectDataSource 
            ID="dsDepartamento"
            runat="server"
            TypeName="Fonade.FONADE.JefeUnidad.UnidadEmprendimiento"
            SelectMethod="getDepartamentos" >
         </asp:ObjectDataSource>

         <asp:ObjectDataSource 
            ID="dsCiudad" 
            runat="server" 
            SelectMethod="getCiudades" 
            TypeName="Fonade.FONADE.JefeUnidad.UnidadEmprendimiento">
            <SelectParameters>
               <asp:ControlParameter ControlID="cmbDepartamento" DefaultValue="0" Name="codigoDepartamento" PropertyName="SelectedValue" Type="Int32" />
            </SelectParameters>
         </asp:ObjectDataSource>
                                                                
      </ContentTemplate>
   </asp:UpdatePanel>
     
      <asp:Panel id="ModalBuscarJefe" CssClass="modal fade" runat="server" role="dialog">
      <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
          <div class="modal-dialog modal-lg">
         <!-- Modal content-->
         <div class="modal-content">
            <div class="modal-header">
               <button type="button" class="close" data-dismiss="modal">&times;</button>
               <h4 class="modal-title">Buscar jefe de la unidad</h4>
            </div>
            
            <div class="modal-body">
                       <div id="buscador" runat="server" visible="true">    
                           <div class="form-group center-block">
                              <label id="lblTituloBusqueda" > Buscar: </label>
                              <asp:TextBox runat="server" ID="txtBusqueda" Enabled="true" AutoPostBack="false" />
                              <asp:Button ID="btnBuscarJefe" runat="server" Text="Buscar" Visible="true" OnClick="btnBuscarJefe_Click"  />
                              <asp:Button ID="btnCrearJefe" runat="server" Text="Crear nuevo jefe de unidad" Visible="true" OnClick="btnCrearJefe_Click" />
                           </div>
                       </div>
                       <div id="nuevoJefeUnidad" runat="server" visible="false">
                            <div class="form-horizontal no-margin center-block">       
                            <div class="form-group center-block">
                                <div class="col-xs-4">
                                </div>
                                <div class="col-xs-6">
                                    <h3>
                                        <asp:Label Text="Crear jefe de unidad" runat="server" ID="lblTitleJefeUnidad" Visible="true" />
                                    </h3>
                                </div>
                            </div>                                                                              
                            <div class="form-group left-block">                                
                                <h5>
                                    <label class="control-label col-xs-4"> Adicionar jefe a la unidad </label>
                                </h5>
                            </div>
                            <div class="form-group center-block">
                                <label class="control-label col-xs-4"> Nombre: </label>
                                <div class="col-xs-4">
                                    <asp:TextBox runat="server" ID="txtNombreJefeUnidad" Text="" Width="200px" />
                                </div>
                            </div>
                            <div id="formNuevaInstitucion" class="form-group center-block" runat="server" >
                                <label class="control-label col-xs-4"> Apellidos: </label>
                                <div class="col-xs-4">
                                    <asp:TextBox runat="server" ID="txtApellidoJefeUnidad" Text="" Width="200px" />
                                </div>
                            </div>                            
                            <div class="form-group center-block">
                                <label class="control-label col-xs-4"> Tipo de identificación: </label>
                                <div class="col-xs-4">
                                    <asp:DropDownList runat="server" ID="cmbTipoDocumento" nombre="Tipo documento" DataSourceID="dataTiposIdentificacion" AutoPostBack="true" DataValueField="Id" DataTextField="Nombre" />
                                </div>
                            </div>
                            <div id="Div1" class="form-group center-block" runat="server" >
                                <label class="control-label col-xs-4"> Identificación: </label>
                                <div class="col-xs-4">
                                    <asp:TextBox runat="server" ID="txtIdentificacionJefeUnidad" Text="" Width="200px" />
                                </div>
                            </div>                            
                            <div class="form-group center-block">
                                <label class="control-label col-xs-4"> Email: </label>
                                <div class="col-xs-4">
                                    <asp:TextBox runat="server" ID="txtEmailJefeUnidad" Text="" Width="200px" />
                                </div>
                            </div>
                            <div class="form-group left-block">                                
                                <h5>
                                    <label class="control-label col-xs-4"> Información de la unidad </label>
                                </h5>
                            </div>
                            <div class="form-group center-block">
                                <label class="control-label col-xs-4"> Departamento: </label>
                                <div class="col-xs-4">
                                    <asp:DropDownList runat="server" ID="cmbDepartamentoUnidad" nombre="Departamento" DataSourceID="dsDepartamentoJefeUnidad" DataTextField="Nombre" DataValueField="Id" AutoPostBack="true" Width="200px" />
                                </div>
                            </div>
                           <div class="form-group center-block">
                                <label class="control-label col-xs-4"> Ciudad: </label>
                                <div class="col-xs-4">
                                    <asp:DropDownList runat="server" ID="cmbCiudadUnidad" nombre="Ciudad" DataSourceID="dsCiudadJefeUnidad" DataTextField="Nombre" DataValueField="Id" Width="200px" />
                                </div>
                            </div>
                            <div class="form-group center-block">
                                <label class="control-label col-xs-4"> Telefono: </label>
                                <div class="col-xs-4">
                                    <asp:TextBox runat="server" ID="txtTelefonoUnidad" Text="" Width="200px" />
                                </div>
                            </div>
                            <div class="form-group center-block">
                                <label class="control-label col-xs-4"> Fax: </label>
                                <div class="col-xs-4">
                                    <asp:TextBox runat="server" ID="txtFaxUnidad" Text="" Width="200px" />
                                </div>
                            </div>
                            <div class="form-group center-block">
                                <label class="control-label col-xs-4"> Sitio web: </label>
                                <div class="col-xs-4">
                                    <asp:TextBox runat="server" ID="txtSitioWebUnidad" Text="" Width="200px" />
                                </div>
                            </div>
                            <div class="form-group left-block">                                
                                <h5>
                                    <label class="control-label col-xs-4"> Información personal </label>
                                </h5>
                            </div>
                            <div class="form-group center-block">
                                <label class="control-label col-xs-4"> Cargo: </label>
                                <div class="col-xs-4">
                                    <asp:TextBox runat="server" ID="txtCargoJefeUnidad" Text="" Width="200px" />
                                </div>
                            </div>
                            <div class="form-group center-block">
                                <label class="control-label col-xs-4"> Telefono: </label>
                                <div class="col-xs-4">
                                    <asp:TextBox runat="server" ID="txtTelefonoPersonalJefeUnidad" Text="" Width="200px" />
                                </div>
                            </div>
                            <div class="form-group center-block">
                                <label class="control-label col-xs-4"> Fax: </label>
                                <div class="col-xs-4">
                                    <asp:TextBox runat="server" ID="txtFaxPersonalJefeUnidad" Text="" Width="200px" />
                                </div>
                            </div>
                           <asp:updateprogress id="UpdateProgress2" runat="server" associatedupdatepanelid="UpdatePanel2" dynamiclayout="true" >
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
                               <asp:Label ID="lblErrorCrearJefeUnidad" CssClass="control-label col-xs-8" ForeColor="Red" Text="Sucedio un error" runat="server" Visible="false" />                                                                
                            </div>
                           <div class="form-group center-block">                                
                                <div class="col-xs-8">
                                  <asp:Button ID="btnCrearJefeUnidad" runat="server" Text="Crear jefe de unidad" Visible="true"  OnClick="btnCrearJefeUnidad_Click" />
                                  <asp:Button ID="btnCancelarCrearJefeUnidad" runat="server" Text="Cancelar" Visible="true" OnClick="btnCancelarCrearJefeUnidad_Click" />            
                                </div>
                            </div>
                        </div>
                       </div>
                       <div id="listadoUsuarios" runat="server" visible="true">
                           <asp:GridView ID="gvUsuarios" runat="server" AutoGenerateColumns="false" CssClass="Grilla" HeaderStyle-HorizontalAlign="Left" RowStyle-HorizontalAlign="Left" Width="100%" PageSize="10" AllowPaging="true" DataSourceID="dsBusquedaJefe" EmptyDataText="0 registros para mostrar." OnRowCommand="gvUsuarios_RowCommand" >
                                    <Columns>
                                        <asp:TemplateField HeaderText="Identificación ">
                                            <ItemTemplate>                                                
                                                <asp:Label ID="lblIdentificacionContacto" Text='<%# Eval("Identificacion")%>' runat="server" CausesValidation="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Nombre">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNombreContacto" Text='<%# Eval("NombreCompleto")%>' runat="server" CausesValidation="false" />                                                
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Rol">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRolContacto" Text='<%# Eval("NombreGrupo")%>' runat="server" CausesValidation="false" />                                                
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Asignar usuario">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkAsignarRol" runat="server" CausesValidation="false" CommandArgument='<%# Eval("Id") + ";" + Eval("NombreCompleto") %>' Enabled='<%# Eval("AllowAsignarJefe") %>'
                                                    CommandName="asignar" Text='<%# Eval("linkAsignar")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <asp:updateprogress id="UpdateProgress3" runat="server" associatedupdatepanelid="UpdatePanel1" dynamiclayout="true" >
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
                       </div>                                                                                                                      
            </div>          
                                               
            <asp:ObjectDataSource
                ID="dsBusquedaJefe"
                runat="server"
                EnablePaging="false"
                SelectMethod="getBusquedaJefe"
                TypeName="Fonade.FONADE.JefeUnidad.UnidadEmprendimiento" >
              <SelectParameters>                
                <asp:ControlParameter ControlID="txtBusqueda" DefaultValue="" Name="identificacion" PropertyName="Text" Type="String" />
              </SelectParameters>                
            </asp:ObjectDataSource>
                       
             <asp:ObjectDataSource 
             ID="dsCiudadJefeUnidad" 
             runat="server" 
             SelectMethod="getCiudades" 
             TypeName="Fonade.FONADE.JefeUnidad.UnidadEmprendimiento">
             <SelectParameters>
               <asp:ControlParameter ControlID="cmbDepartamentoUnidad" DefaultValue="0" Name="codigoDepartamento" PropertyName="SelectedValue" Type="Int32" />
             </SelectParameters>
             </asp:ObjectDataSource>

             <asp:ObjectDataSource 
                ID="dsDepartamentoJefeUnidad"
                runat="server"
                TypeName="Fonade.FONADE.JefeUnidad.UnidadEmprendimiento"
                SelectMethod="getDepartamentos"
                >
             </asp:ObjectDataSource>            

             <asp:ObjectDataSource 
                ID="dataTiposIdentificacion"
                runat="server"
                TypeName="Fonade.FONADE.JefeUnidad.UnidadEmprendimiento"
                SelectMethod="getTiposIdentificacion"
                >
             </asp:ObjectDataSource>

            <div class="modal-footer">                
                <asp:Button ID="btnCerrarBuscarJefe" runat="server" Text="Cerrar" OnClick="btnCerrarBuscarJefe_Click" Visible="true" />                
            </div>
         </div>
      </div>
          </ContentTemplate>
            </asp:UpdatePanel>
   </asp:Panel>

   <script src="https://code.jquery.com/jquery-1.9.1.min.js" type="text/javascript" ></script>
   <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/js/bootstrap.min.js" type="text/javascript" ></script>                    
</asp:Content>