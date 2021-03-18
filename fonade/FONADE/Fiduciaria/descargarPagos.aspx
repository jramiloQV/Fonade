<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="descargarPagos.aspx.cs" Inherits="Fonade.FONADE.Fiduciaria.descargarPagos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Styles/Site.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <table width="98%" border="0">
        <tr>
            <td class="style50">
                <h1>
                    <asp:Label runat="server" ID="lblTitulo" Text="Detalle de solicitudes de pago" Style="font-weight: 700"></asp:Label>
                </h1>
            </td>
            <td align="right">
            </td>
        </tr>
    </table>
    <b> <asp:Label ID="lblNumeroDeSolicitud" Text="Numero de solicitud : " runat="server" /></b>
    <asp:GridView ID="gvSolicitudesDePago" runat="server"
        Width="98%" BorderWidth="0" CellSpacing="1" CellPadding="4" AllowPaging="False" OnRowCommand="detalleSolicitud_RowCommand"
        PageSize="5" AutoGenerateColumns="False" CssClass="Grilla" HeaderStyle-HorizontalAlign="Left" EmptyDataText="No hay solicitudes pendientes por procesar.">
        <Columns>
            <asp:TemplateField HeaderText="Número solicitud">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkmostrar" CommandArgument='<%# Eval("idPagoActividad") %>'
                        CommandName="verDetallePago" CausesValidation="False" Text='<%#Eval("idPagoActividad") %>'
                        runat="server" Font-Bold="true" ForeColor="Black" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Fecha envío">
                <ItemTemplate>
                    <asp:Label ID="lbl_FechEnvi" Text='<%# Eval("fechaConFormato") %>' runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="Empresa" DataField="razonSocial" HtmlEncode="false" />
            <asp:BoundField HeaderText="Valor" DataField="cantidadDineroConFormato" DataFormatString="{0:C0}"  ItemStyle-HorizontalAlign="Right" runat="server" > 
                <ItemStyle HorizontalAlign="Right"></ItemStyle>
            </asp:BoundField>           
        </Columns>
        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
    </asp:GridView>
    <asp:Panel ID="pnlVerificarToken" runat="server" HorizontalAlign="Center">
        <p>
            Al hacer clic sobre el botón Descargar usted estará adquiriendo la responsabilidad legal 
            sobre los datos que se estan descargando. En la Base de datos quedan registrados la fecha 
            y hora de descarga del archivo plano. 
        </p>
        <asp:Button ID="btnDescargarPagos" runat="server" Text="Descargar Directo" OnClick="btnDescargarPagos_Click" 
            Visible="false"/>
        <br />        
        <asp:Button ID="VerificarToken" runat="server" Text="Descargar" Visible="true" OnClick="ObtenerXml_Click" />
    </asp:Panel>
    <br />    
    <asp:Panel ID="pnlDescargarArchivosPagosYTerceros" Visible="false" runat="server" HorizontalAlign="Center">
    </asp:Panel>
    <br />
    <asp:Panel ID="pnlDescargarArchivosAdjuntos" Visible="false" runat="server" HorizontalAlign="Center">
    </asp:Panel> 
    
    <!--Esta instrucción detecta si capicom se encuentra instalada en la máquina. Si no lo está la instala de manera automática-->
    <object id="oCAPICOM" codebase="../Styles/capicom.cab" >
    </object>    
    <script language="vbscript">
        Sub GenerarDatos_OnClick(xmlParaFirmar)
        Dim Firma
		Dim DatosFirmados
		Dim Datos
		Dim j
		Dim Cadena
		Dim PerfilFonade
		Dim ValidaCRL
		Dim ValidaRoot
        Dim DatosFirmante

		DatosFirmados = False

		'Aqui es donde se concatenan los datos para firmar
		Datos = ""

		Datos = xmlParaFirmar
        
        Firma = ""
        DatosFirmantes = ""

		DatosFirmados = SignData(Datos, Firma)
        
		If DatosFirmados Then
            If VerifySign(Datos, Firma) Then
	            If ValidateRoot(Datos, Firma) Then
                    If ValidateTime(Datos, Firma) Then
                        Dim Verifier
	                    Dim Certificate

	                    'Inicializar el objeto Capicom que gestiona las firmas digitales
	                    Set Verifier = CreateObject("CAPICOM.SignedData")

	                    Verifier.Content = Datos

	                    On Error Resume Next

	                    'Verificar la firma digital
	                    Verifier.Verify Firma, True, CAPICOM_VERIFY_SIGNATURE_ONLY

	                    For Each Certificate in Verifier.Certificates
		                    DatosFirmante = DatosFirmante & Certificate.subjectName
	                    Next
                        
                        procesarDatos(Firma & "[FirmaSplitter]" & DatosFirmante)
                    Else
                        MsgBox "Error: El Certificado utilizado no esta vigente, por lo tanto no es válido.", vbCritical
			        End If
                Else
			        MsgBox "Error: El Certificado utilizado no fue emitido por una entidad certificadora, por lo tanto no es válido.", vbCritical			        
		        End If                
            End If
		End If

    End Sub

    'Función para detectar si Capicom se encuentra instalado en la máquina
    Function isCapicomAvailable()
	    Dim oStore
	    On Error Resume Next

	    Set oStore = CreateObject("CAPICOM.Store")
	    oStore.Open CAPICOM_LOCAL_MACHINE_STORE, "Root", CAPICOM_STORE_OPEN_READ_ONLY
	    If Err.Number <> 0 Then
		    isCapicomAvailable = False
		    Exit Function
	    End If

	    isCapicomAvailable = True
	    Set oStore = Nothing
    End Function

    Function SignData(Datos, Firma)
	    Dim SignedData
	    Dim FirmaDigital

	    'Verificar que Capicom se encuentra instalada en la máquina
	    If NOT isCapicomAvailable Then
		    MsgBox "CAPICOM No esta Instalado.", vbCritical, "::CAPICOM::"
		    SignData = false
   		    Exit Function
  	    End If

	    'Crear el objeto Capicom que gestiona firmas digitales
	    Set SignedData = CreateObject("CAPICOM.SignedData")

	    'Capturar los datos que van a ser firmados
 	    SignedData.Content = Datos

	    On Error Resume Next

	    'Crear la firma digital
 	    FirmaDigital = SignedData.Sign(Nothing, true)

 	    'Manejo de errores
 	    If Err.Number <> 0 Then

		    If Hex(Err.Number) = "8010000C" Then
			    MsgBox "Error: usted ha seleccionado un certificado que no posee una llave privada asociada. Verifique que el Token se encuentra conectado a la máquina", vbCritical
		    End If

		    If Hex(Err.Number) = "80090016" Then
			    MsgBox "Error: usted ha seleccionado un certificado que no posee una llave privada asociada. Verifique que el Token se encuentra conectado a la máquina", vbCritical
		    End If

		    If Hex(Err.Number) = "80880902" Then
			    MsgBox "Error: el proceso de firma falló. La selección del certificado fue cancelada por el usuario", vbCritical
		    End If

		    If Hex(Err.Number) = "8009000D" Then
			    MsgBox "Error: el proceso de firma falló. La selección del certificado fue cancelada por el usuario", vbCritical
		    End If

		    If Hex(Err.Number) = "80880231" Then
			    MsgBox "Error: no hay ningún certificado registrado en el almacén de certificados de Windows. Verifique que el Token se encuentra conectado a la máquina", vbCritical
		    End If

		    SignData = false

	    Else
		    Firma = FirmaDigital
		    SignData = true
   	    End If

	    'Inicializar el objeto Capicom que gestiona las firmas digitales
 	    Set SignedData = Nothing
    End Function

    Function VerifySign(Datos, Firma)

	    Dim Verifier

	    'Crear el objeto Capicom que gestiona firmas digitales
	    Set Verifier = CreateObject("CAPICOM.SignedData")

	    'Capturar los datos que fueron firmados digitalmente
	    Verifier.Content = Datos

	    On Error Resume Next

	    'Verificar la firma digital
	    Verifier.Verify Firma, True

	    'Manejo de error
	    If Err.Number <> 0 Then

		    If Hex(Err.Number) = "80090006" Then
			    MsgBox "Error: Firma Invalida. Se efectuó un cambio sobre el mensaje de datos con base en el cual se generó la firma", vbCritical
		    End If

		    If Hex(Err.Number) = "80093102" Then
			    MsgBox "Error: Firma Invalida. Se efectuó una modificación sobre el valor de la firma digital eliminando caracteres", vbCritical
		    End If
        
		    If Hex(Err.Number) = "80091004" Then
			    MsgBox "Error: Firma Invalida. Se efectuó una modificación sobre el valor de la firma digital cambiando caracteres", vbCritical
		    End If

		    If Hex(Err.Number) = "80880251" Then
			    MsgBox "Error: Firma Invalida. Se efectuó una modificación sobre el valor de la firma digital y no es posible identificar la información del firmante sobre la firma", vbCritical
		    End If

		    If Hex(Err.Number) = "800B0109" Then
			    MsgBox "Error: la firma no puede ser validada. No está instalado el certificado raíz de la CA en la  máquina en la cual se está efectuando la verificación de la firma", vbCritical
		    End If

		    VerifySign = true
	    Else
		    VerifySign = true
	    End If

    End Function

    Function Validatecrl(Datos, Firma)
	    Dim cert
	    Dim SingData

	    Const CAPICOM_VERIFY_SIGNATURE_ONLY = 0

	    'Inicializamos la constante CAPICOM_CHECK_ONLINE_REVOCATION_STATUS que permite hacer la verificación siempre en línea
	    Const CAPICOM_CHECK_ONLINE_REVOCATION_STATUS = &H8

	    'Crear el objeto que gestiona las firmas digitales
	    Set SingData = CreateObject("CAPICOM.SignedData")

	    'Capturar los datos que fueron firmados digitalmente
	    SingData.Content = Datos

	    'Inicializar el valor por defecto de la verificación de CRL
	    ValidateCRL = false

	    On Error resume next

	    'Verificar la firma digital
	    SingData.Verify  Firma, True, CAPICOM_VERIFY_SIGNATURE_ONLY

	    For Each cert In SingData.Certificates

	       'Realizar la validación de la CRL para el certificado seleccionado
	       cert.IsValid.CheckFlag = CAPICOM_CHECK_ONLINE_REVOCATION_STATUS

	       If cert.IsValid.Result Then
		     'MsgBox "El certificado digital no se encuentra la CRL. El certificado es valido", vbInformation
		     ValidateCRL = true
	       Else
	         'MsgBox "El certificado digital se encuentra la CRL. El certificado no es valido", vbCritical
	       End If

	    Next

    End Function

    Function ValidateRoot(Datos, Firma)
	    Dim SignedData
	    Dim resultado
	    resultado = 0

	    'Inicializar la constante CAPICOM_CHECK_TRUSTED_ROOT
	    Const CAPICOM_CHECK_TRUSTED_ROOT = &H1

	    'Inicializar el valor por defecto de la verificación de cadena de certificación
	    ValidateRoot = false

        'Crear el objeto Capicom que gestiona las firmas digitales
	    Set SignedData = CreateObject("CAPICOM.SignedData")

	    'Capturar los datos que fueron firmados
        SignedData.Content = Datos

	    On Error resume next

	    'Validar la firma digital
	    SignedData.Verify Firma, True, CAPICOM_VERIFY_SIGNATURE_ONLY

	    'Validar la cadena de certificación para cada uno de los certificados de firma
	    For Each certificado In SignedData.Certificates
		    certificado.IsValid.CheckFlag = CAPICOM_CHECK_TRUSTED_ROOT

		    If Not certificado.IsValid.Result Then
			    resultado = resultado + 1
		    End If
	    Next

	    If resultado <= 0 Then
		    ValidateRoot = true
	    End If

     End Function

    Function ValidateTime(Datos, Firma)
	    Dim SignedData
	    Dim resultado
	    resultado = 0

	    'Inicializar la constante CAPICOM_CHECK_TRUSTED_ROOT
	    Const CAPICOM_CHECK_TIME_VALIDITY = &H2

	    'Inicializar el valor por defecto de la verificación de cadena de certificación
	    ValidateTime = false

        'Crear el objeto Capicom que gestiona las firmas digitales
	    Set SignedData = CreateObject("CAPICOM.SignedData")

	    'Capturar los datos que fueron firmados
        SignedData.Content = Datos

	    On Error resume next

	    'Validar la firma digital
	    SignedData.Verify Firma, True, CAPICOM_VERIFY_SIGNATURE_ONLY

	    'Validar la vigencia de cada uno de los certificados de firma
	    For Each certificado In SignedData.Certificates
		    certificado.IsValid.CheckFlag = CAPICOM_CHECK_TIME_VALIDITY

		    If Not certificado.IsValid.Result Then
			    resultado = resultado + 1
		    End If
	    Next

	    If resultado <= 0 Then
		    ValidateTime = true
	    End If

     End Function


    Function ViewCertificate(Datos, Firma)
        Dim Verifier
	    Dim Certificate
	    Dim count

	    'Crear el objeto que gestiona las firmas digitales
	    Set Verifier = CreateObject("CAPICOM.SignedData")

	    'Capturar los datos que fueron firmados digitalmente
	    Verifier.Content = Datos.value

	    On Error Resume Next

	    'Verificar la firma digital
	    Verifier.Verify Firma.value, True, CAPICOM_VERIFY_SIGNATURE_ONLY

	    'Desplegar los certificados empleados para el proceso de firma
	    For Each Certificate in Verifier.Certificates
		    Certificate.Display()
	    Next

    End Function

    Function GetDatosFirmante(Datos, Firma)
        Dim Verifier
	    Dim Certificate

	    'Inicializar el objeto Capicom que gestiona las firmas digitales
	    Set Verifier = CreateObject("CAPICOM.SignedData")

	    Verifier.Content = Datos

	    On Error Resume Next

	    'Verificar la firma digital
	    Verifier.Verify Firma, True, CAPICOM_VERIFY_SIGNATURE_ONLY

	    For Each Certificate in Verifier.Certificates
		    Msgbox "Los datos del fimante del certificado son: " & Certificate.subjectName
	    Next
    End Function

    </script>
    <script  type="text/javascript">
        function procesarDatos(datosProcesados) {            
            __doPostBack('firmaDigital', datosProcesados);
        }
    </script>

</asp:Content>
