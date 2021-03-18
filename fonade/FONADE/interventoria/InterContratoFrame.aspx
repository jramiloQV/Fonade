<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InterContratoFrame.aspx.cs" EnableEventValidation="false"
    Inherits="Fonade.FONADE.interventoria.InterContratoFrame" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.9.1.js"></script>
    <link href="../../Styles/Site.css" rel="stylesheet" />
    <script src="../../Scripts/ScriptsGenerales.js"></script>
    <script src="../../Scripts/common.js" type="text/javascript"></script>
    <style type="text/css">
        html, body {
            background-image: none !important;
        }

        table {
            width: 80%;
        }

        .title {
            font-size: 16px;
        }

        input.boton_Link_Grid_2 {
            width: 0px;
            height: 0px;
            border: 0px;
            background-image: none;
            background: none;
            text-decoration: underline;
            cursor: pointer;
            padding: 0px;
        }

            input.boton_Link_Grid_2:hover {
                width: 0px;
                height: 0px;
                border: 0px;
                text-decoration: none;
                background-image: none;
                background: none;
                padding: 0px;
                background: none !important;
            }
    </style>

    <script type="text/javascript">
        function alerta() {
            return confirm('¿Está seguro de eliminar este archivo?');
        }

        function confirmarCarga() {
            return confirm('En mi calidad de Emprendedor beneficiario del Fondo Emprender, '+
                'declaro bajo juramento y certifico con mi firma digitalizada, '+
                'de manera libre y voluntaria que cada uno de los hechos, '+
                'comentarios o precisiones sobre la ejecución del plan de negocios '+
                'y los documentos de soporte presentados durante la visita de seguimiento virtual, '+
                'en el marco del contrato suscrito con el SENA, se ajustan a la verdad, '+
                'son ciertos y veraces frente a la realidad empresarial y mi actuar como empresario');
        }
    </script>
   
</head>
<body style="overflow: auto;"  >
 
    <form id="form1" runat="server">

        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server">
        </ajaxToolkit:ToolkitScriptManager>

        <div class="ContentInfo" style="width: 995px; height: 100%; margin-top: 0px;"">

            <div>
                <asp:Panel ID="panexosagre" runat="server">
                    <h1>
                        <label>
                            INFORMACIÓN DEL CONTRATO</label>
                    </h1>
                    <br />
                    <table class="Grilla">
                        <tr>
                            <td style="width: 25%;">No Contrato de Colaboración Empresarial:
                            </td>
                            <td style="width: 25%;">
                                <asp:Label ID="lblNumContrato" runat="server"></asp:Label>
                            </td>
                            <td style="width: 25%;">Plazo en meses del ctto:
                            </td>
                            <td style="width: 25%;">
                                <asp:Label ID="lblplazoMeses" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Fecha de Acta de Inicio:
                            </td>
                            <td>
                                <asp:Label ID="lblFechaActa" runat="server"></asp:Label>
                            </td>
                            <td>Numero del ap presupuestal:
                            </td>
                            <td>
                                <asp:Label ID="lblNumAppresupuestal" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Objeto:
                            </td>
                            <td colspan="3">
                                <asp:Label ID="lblObjeto" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Fecha del ap:
                            </td>
                            <td>
                                <asp:Label ID="lblFechaAp" runat="server"></asp:Label>
                            </td>
                            <td>Fecha Firma Del Contrato:
                            </td>
                            <td>
                                <asp:Label ID="lblFechaFirmaContrato" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>No Póliza de Seguro de Vida:
                            </td>
                            <td>
                                <asp:Label ID="lblPolizaSeguro" runat="server"></asp:Label>
                            </td>
                            <td>Compañía Seguro de Vida:
                            </td>
                            <td>
                                <asp:Label ID="lblCompaniaSeguroVida" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Valor inicial en pesos:
                            </td>
                            <td>
                                <asp:Label ID="lblValorInicial" runat="server"></asp:Label>
                            </td>
                            <td colspan="2"></td>
                        </tr>
                    </table>
                    <p>
                        Cargue de archivos adjuntos :
                    </p>
                    <p>
                        <asp:Button ID="lnkSuberArchivo" runat="server" Text="Cargar un archivo" OnClick="lnkSuberArchivo_Click1" CssClass="boton_Link_Grid"></asp:Button>
                    </p>

                    <p>

                        <%-- <asp:DataList ID="DataList1" runat="server" OnSelectedIndexChanged="btn_Click" Width="70%" Height="50%">
                                <ItemTemplate>
                                    <asp:Button ID="Button2" runat="server" CssClass="boton_Link_Grid" OnClick="btn_Click" Text="Eliminar " OnClientClick="var mnb = confirm('¿ Eliminar archivo adjunto  ?'); return mnb;" CommandArgument='<%# Eval("ruta") %>' Visible='<%# Convert.ToBoolean(DataBinder.GetPropertyValue(this, "perfilVisibilidad")) %>' />
                                    <asp:Button ID="Button1" runat="server" CssClass="boton_Link_Grid" OnClick="btn_Click" Text='<%# Eval("NombreArchivo") %>' Enabled="false" />
                                    <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/Images/buscarrr.png" Width="20px" OnClientClick='<%# string.Format("window.open(|{0}|)", Eval("filePath","http://www.fondoemprender.com:8080/{0}").ToString()).Replace(char.ConvertFromUtf32(124), char.ConvertFromUtf32(39))%>' ImageAlign="Right" />
                                </ItemTemplate>
                            </asp:DataList>--%>
                    </p>
                    <br />
                    
                    <!--Descarga de multiples archivos-->
                    <div style="width: 70%; height: auto">
                        <asp:GridView ID="gvDescargaArchivos" runat="server"
                            CssClass="Grilla" DataKeyNames="idContratoArchivoAnexo"
                            AutoGenerateColumns="false" EmptyDataText="No se han cargado archivos" 
                            OnRowCommand="gvDescargaArchivos_RowCommand"
                            >
                            <Columns>
                                <asp:TemplateField HeaderText="Listado de archivos Adjuntos">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSelect" runat="server" Text='<%# Eval("NombreArchivo") %>' />
                                        <asp:Label ID="lblFilePath" runat="server" Text='<%# Eval("filePath") %>' Visible="false"></asp:Label>
                                         
                                    </ItemTemplate>
                                </asp:TemplateField>                                
                                <asp:TemplateField HeaderText="Accion">
                                    <ItemTemplate>                                       
                                        <asp:ImageButton ID="imgBtnEliminar" runat="server" ImageUrl="~/Images/icoBorrar.gif" 
                                            Width="20px"                                               
                                           CommandName="Borrar" CommandArgument='<%# Eval("idContratoArchivoAnexo") %>'
                                             Visible='<%# Convert.ToBoolean(DataBinder.GetPropertyValue(this, "mostrarEliminar")) %>'
                                            ImageAlign="Right" />

                                        <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/Images/buscarrr.png" Width="20px" 
                                            OnClientClick='<%# string.Format("window.open(|{0}|)"
                                            , Eval("filePath","http://www.fondoemprender.com:8080/{0}")
                                            .ToString()).Replace(char.ConvertFromUtf32(124)
                                            , char.ConvertFromUtf32(39)).Replace("\\\\10.3.3.118\\","")%>' 
                                            ImageAlign="Right" />
                                         
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <br />
                        <asp:Button ID="btnDownload" runat="server" Text="Descargar Archivos" OnClick="btnDownload_Click" />
                    </div>
                    <!--FIN ... Descarga de multiples archivos-->
                </asp:Panel>         
                
                <!--CArgar archivos EMPRENDEDOR-->
                <div id="panelCargaArchivosEmprendedor" runat="server">
                    <!--Cargue de Actas de Terminación -->
                    <fieldset style="Width:90%">
                        <legend>Subir archivo firmado</legend>
                        <div>            
                             <div style="padding:5px">
                               <asp:Label ID="lblTipoArchivo" runat="server" Text="Tipo archivo a cargar: "></asp:Label>
                               <asp:DropDownList ID="ddlTipoArchivo"
                                   DataValueField="idtipo"
                                   DataTextField="tipo"
                                   runat="server">
                               </asp:DropDownList>
                           </div>
                            <div style="padding:5px">
                                <div style="padding:5px">
                                    <asp:FileUpload ID="FUArchivoFirmado" runat="server" Width="100%"  accept="application/pdf"/>
                                    <br />
                                </div>
                                <div style="padding:5px">
                                     <asp:Button ID="btnSubirFirmados" runat="server" 
                                         OnClientClick="return confirmarCarga();"    
                                          OnClick="btnSubirFirmados_Click" 
                                             Text="Subir Archivo Firmado"/>
                                        <br />
                                </div>
                                <div style="padding:5px">
                                    <asp:Label ID="Label1" runat="server" 
                                    Text="Solo puede realizar la carga de un archivo por tipo de archivo." 
                                    ForeColor="Maroon"></asp:Label>
                                    <br />
                                    <asp:Label ID="lblMensajeArchivo" runat="server" 
                                    Text="Verifique que el nombre del archivo no lleve caracteres especiales, 
                                    ni espacios, en caso de tenerlos reemplacelos por guion bajo (_)." 
                                    ForeColor="Maroon"></asp:Label>
                                </div>
                                
                            </div>    

                            <br />
                            <div>
                                <asp:Label ID="lblErrorSubirArchivoFirmado" runat="server" 
                                ForeColor="Red" Text="Sucedio un error." Visible="False"></asp:Label>
                            </div>
                        </div>
                    </fieldset>
                </div>
                <!--FIN CArgar archivos EMPRENDEDOR-->

                <!--Archivos Especiales-->
                <fieldset id="archivosEspeciales" runat="server" style="width:90%">
                        <legend>Archivos</legend>
                        <asp:GridView ID="gvArchivosEspeciales" runat="server" AllowPaging="false" AutoGenerateColumns="False"
                            EmptyDataText="No hay archivos que mostrar." Width="98%" BorderWidth="0" CellSpacing="1"
                            CellPadding="4" CssClass="Grilla" HeaderStyle-HorizontalAlign="Left" 
                            DataKeyNames="idArchivo"
                            OnRowCommand="gvArchivosEspeciales_RowCommand">
                            <Columns>
                                <asp:TemplateField HeaderText="Accion">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgBtnEliminar" runat="server" ImageUrl="~/Images/icoBorrar.gif"
                                            Width="20px"
                                            CommandName="Borrar" CommandArgument='<%# Eval("idArchivo") %>'
                                            Visible='<%# Convert.ToBoolean(DataBinder.GetPropertyValue(this, "mostrarEliminar")) %>'
                                            OnClientClick="return alerta()"
                                            ImageAlign="Left" />

                                        <asp:ImageButton ID="imgBtnDescargar" runat="server" ImageUrl="~/Images/buscarrr.png" Width="20px"
                                            CommandName="VerArchivo" CausesValidation="False"
                                            CommandArgument='<%#  Eval("NombreArchivo") + ";" + Eval("ruta")+ ";" + Eval("idArchivo") %>'
                                            ImageAlign="Right" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Nombre" DataField="NombreArchivo" HtmlEncode="false" />
                                <asp:BoundField HeaderText="Fecha Carga" DataField="FechaIngreso" HtmlEncode="false" />
                                <asp:BoundField HeaderText="Cargado por" DataField="NombreContacto" HtmlEncode="false" />
                                <asp:BoundField HeaderText="Tipo" DataField="NombreTipoArchivo" HtmlEncode="false" />
                            </Columns>
                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        </asp:GridView>
                    </fieldset>
                <!--Archivos Especiales-->

                


                <asp:Panel ID="Adjunto" runat="server">
                    <h3 class="title">Carga Archivo</h3>
                    <table>
                        <tr>
                            <td>Seleccione el archivo:
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblErrorDocumento" runat="server" CssClass="failureNotification"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:FileUpload ID="fuArchivo" runat="server" Width="240px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnSubirDocumento" runat="server" Text="Cargar Archivo" OnClick="btnSubirDocumento_Click" />
                                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>Por favor use click en examinar para escoger los archivos Adjuntos
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
        </div>

         <asp:Label ID="lblModalEliminar" runat="server" Text=""></asp:Label>
        <!--MODAL EDITAR COMPROMISO-->
        
        <asp:ModalPopupExtender ID="ModalEliminarArchivo" runat="server"
            CancelControlID="btnCerrarModalEdit"
            TargetControlID="lblModalEliminar" PopupControlID="pnlEliminarArchivo"
            PopupDragHandleControlID="PopupHeader" Drag="true"
            BackgroundCssClass="modalBackground">
        </asp:ModalPopupExtender>
        <!--Style="display: none"-->
        <asp:Panel ID="pnlEliminarArchivo" runat="server" Style="display: none"
            Width="600px" Height="330px" BackColor="White">
            <div class="EliminarArchivoPopup">
                <%--Popup--%>
                <div class="Controls" style="text-align: right; height: 0%">
                    <input type="submit" value="X" id="btnCerrarModalEdit" />
                </div>
                <div class="PopupBody" style="max-height: 500px; overflow: auto;">
                    
                   <asp:Label ID="lblIdArchivoContrato" runat="server" Text="Label" ForeColor="White"></asp:Label>
                   <div id="cuerpoEliminarArchivo" style="height:0%; padding-left: 20px;">
                        <h1>Eliminar Archivo</h1>
                        <hr />
                        <h3>Nombre Archivo: </h3>
                        <h2><asp:Label ID="lblNombreArchivo" runat="server" Text="Nombre Archivo"></asp:Label></h2>
                        <h3>Motivo de eliminación del archivo (Obligatorio): </h3>
                        <asp:TextBox ID="txtMotivoEliminar" runat="server"
                            TextMode="MultiLine" style="width: 520px;margin: 0px;height: 120px;"></asp:TextBox>
                        <div style="text-align:center;height: 0%;">
                             <asp:Button ID="btnEliminarArchivo" runat="server" 
                                 Text="Eliminar Archivo" OnClick="btnEliminarArchivo_Click" />
                        </div>
                   </div>
                </div>
            </div>
        </asp:Panel>
                       
    </form>
</body>
</html>
