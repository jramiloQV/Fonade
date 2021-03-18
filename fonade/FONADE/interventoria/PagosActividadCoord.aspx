<%@ Page Language="C#" MasterPageFile="~/Master.master" EnableEventValidation="false"
    AutoEventWireup="true" CodeBehind="PagosActividadCoord.aspx.cs" Inherits="Fonade.FONADE.interventoria.PagosActividadCoord" ValidateRequest="false" %>

<asp:Content ID="head1" ContentPlaceHolderID="head" runat="server"> 
 <style>
        .btnv{
            display:none;
        }
        table
        {
            width: 100%;
        }
 </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="bodyContentPlace">    
    <asp:ScriptManager ID="scrmanager1" runat="server" EnablePageMethods="true" />
    <asp:Panel ID="pnlprincipal" runat="server">
        <h1>
            <span>APROBACION DE SOLICITUDES DE PAGO</span>
        </h1>
        <asp:GridView ID="gvsolicitudes" runat="server" CssClass="Grilla" AutoGenerateColumns="False"
            OnRowCommand="gvsolicitudes_RowCommand" OnRowDataBound="gvsolicitudes_RowDataBound"
            AllowPaging="True" OnPageIndexChanging="gvsolicitudes_PageIndexChanging" EmptyDataText="No hay Solicitudes de pago registradas">
            <Columns>
                <asp:TemplateField HeaderText="Solicitud No.">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnk_btn_Id_PagoActividad" Text='<%# Eval("Id_PagoActividad") %>'
                            runat="server" CausesValidation="false" CommandName="mostrar_coordinadorPago"
                            CommandArgument='<%# Eval("Id_PagoActividad") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Fecha">
                    <ItemTemplate>
                        <asp:Label ID="lbl_fecha" Text='<%# Eval("Fecha") %>' runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Empresa" DataField="RazonSocial" />
                <asp:TemplateField HeaderText="Agendó">
                    <ItemTemplate>
                        <asp:Label ID="lbl_Intervemtor" runat="server" Text='<%# Eval("Agendo") %>' />
                        <asp:HiddenField ID="hdf_RazonSocial" runat="server" Value='<%# Eval("RazonSocial") %>' />
                        <asp:HiddenField ID="hdf_codProyecto" runat="server" Value='<%# Eval("CodProyecto") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Valor" HeaderStyle-Width="100px">
                    <ItemTemplate>
                        <asp:Label ID="lbl_valor" Text='<%# Eval("Valor") %>' runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Observación Interventor">
                    <ItemTemplate>
                        <asp:Label ID="lbl_observ_interv" Text='<%# Eval("ObservaInterventor") %>' runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Aprobado">
                    <ItemTemplate>
                        <asp:HiddenField ID="hdf_codactafonade" runat="server" Value='<%# Eval("codactafonade") %>' />
                        <asp:HiddenField ID="hdf_CodBeneficiario" runat="server" Value='<%# Eval("numIdentificacion") %>' />
                        <asp:HiddenField ID="hdf_empresa" runat="server" Value='<%# Eval("Id_Empresa") %>' />
                        <asp:Label ID="lbl_displayText" Text="" runat="server" Visible="false" />
                        <asp:RadioButtonList ID="rb_lst_aprobado" runat="server" RepeatDirection="Vertical">
                            <asp:ListItem Text="Si" Value="opcion_SI" />
                            <asp:ListItem Text="No" Value="opcion_NO" />
                            <asp:ListItem Text="Pendiente" Value="opcion_Pendiente" />
                        </asp:RadioButtonList>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Observaciones">
                    <ItemTemplate>
                        <asp:TextBox ID="txt_observ" runat="server" TextMode="MultiLine" Columns="18" Rows="10" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                No hay Solicitudes de pago registradas.</EmptyDataTemplate>
        </asp:GridView>
        <asp:Panel ID="resTotal" runat="server">
            <table>
                <tr>
                    <td style="text-align: center;">
                        <br />
                        <p>
                            Al hacer clic sobre el botón Verifica Certificado, el programa le solicitará ingresar la
                            firma digital, y por medio de esta acción usted estará adquiriendo la responsabilidad
                            legal sobre los datos consignados en el formulario.  Luego hará clic sobre el botón 
                            Enviar Datos para confirmar el proceso de aprobación.
                        </p>
                    </td>
                </tr>
                <tr>
                   
                <td style="text-align: center;">
                        <br />
                        <br />
                        <asp:Button ID="btnverficaCertficado"  runat="server" Text="Verificar Certificado" 
                            OnClick="btnverficaCertficado_Click" Visible ="false" />   <%-- 31/DIC/2014 WAFS ==> Se incluyo este nuevo boton--%>
                        <br />
                        <asp:Button ID="btnenviardatos" runat="server" Text="Enviar  Datos" OnClick="btnenviardatos_Click" class ="btnv" Visible="true" />                                                                  
                        <asp:Button ID="VerificarToken" runat="server" Text="Verificar Certificado" Visible="true" OnClick="ObtenerXml_Click" />
                    </td>
                </tr>
            </table>
            
        </asp:Panel>       
    </asp:Panel>
    <object id="oCAPICOM" codebase="../Styles/capicom.cab" > </object>
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
        'Para pruebas colocar en true, para produccion colocar en false
	    ValidateRoot = false 'false

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
            var btnID = '<%=btnverficaCertficado.ClientID %>';
            __doPostBack('firmaDigital', datosProcesados);
        }
    </script>


</asp:Content>
