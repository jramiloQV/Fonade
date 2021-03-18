<%@ Page Title="Log In" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Login.aspx.cs" Inherits="Fonade.Account.Login" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="HeadContent">


    <script type="text/javascript">
        function redireccionar() {
            window.location = "RecuerdaClave.aspx";
        }
    </script>
</asp:Content>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

    <asp:Panel ID="pnllogeo" runat="server" Visible="true">
        <p>
            <h1>ESTE INGRESO ESTÁ HABILITADO ÚNICAMENTE PARA LAS PERSONAS ADSCRITAS AL FONDO EMPRENDER</h1>
            <p>
            </p>
            <h2>Iniciar sesión </h2>
            <p>
                Por favor ingrese su usuario y contraseña. <%--<asp:HyperLink ID="RegisterHyperLink" runat="server" EnableViewState="false">Regístrese</asp:HyperLink> si no tiene una cuenta.--%>
            </p>
            <asp:Login ID="LoginUser" runat="server" EnableViewState="False" OnAuthenticate="LoginUser_Authenticate" RenderOuterTable="False">
                <LayoutTemplate>
                    <span class="failureNotification">
                        <asp:Literal ID="FailureText" runat="server"></asp:Literal>
                    </span><%--            <asp:ValidationSummary ID="LoginUserValidationSummary" runat="server" CssClass="failureNotification" 
                 ValidationGroup="LoginUserValidationGroup"/>--%>
                    <div class="accountInfo">
                        <fieldset class="login">
                            <legend>Información de la cuenta</legend>
                            <p>
                                <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">Usuario:</asp:Label>
                                <asp:TextBox ID="UserName" runat="server" CssClass="textEntry"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" CssClass="failureNotification" ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="LoginUserValidationGroup">*</asp:RequiredFieldValidator>
                            </p>
                            <p>
                                <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Contraseña:</asp:Label>
                                <asp:TextBox ID="Password" runat="server" CssClass="passwordEntry" TextMode="Password"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password" CssClass="failureNotification" ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="LoginUserValidationGroup">*</asp:RequiredFieldValidator>
                            </p>
                            <p>
                                <asp:CheckBox ID="RememberMe" runat="server" />
                                <asp:Label ID="RememberMeLabel" runat="server" AssociatedControlID="RememberMe" CssClass="inline">Recordar contraseña</asp:Label>
                            </p>
                        </fieldset>
                        <p class="LoginsubmitButton">
                            <asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="Ingresar" ValidationGroup="LoginUserValidationGroup" />
                        </p>
                    </div>
                </LayoutTemplate>
            </asp:Login>
            <asp:LinkButton ID="hlOlvidaClave" runat="server" EnableViewState="false" OnClick="hlOlvidaClave_Click" Text="¿Olvidó su clave?"></asp:LinkButton>
            <p>
            </p>
        </p>
    </asp:Panel>

    <asp:Panel ID="recuperaClave" runat="server" Visible="false">
    </asp:Panel>
    <asp:Panel ID="pnlolvidoClave" runat="server" Visible="false">
        <h1>RECORDAR CLAVE</h1>
        <br />
        <asp:Panel ID="recordar" runat="server" Visible="true">
            Email:
        <asp:TextBox ID="txtEmail" runat="server" Width="350px" ValidationGroup="VAlidar"></asp:TextBox>
            <br />
            <br />
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEmail" ErrorMessage="RegularExpressionValidator" ForeColor="Red" ValidationGroup="VAlidar" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">Correo no valido</asp:RegularExpressionValidator>
            <br />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEmail" ErrorMessage="RequiredFieldValidator" ForeColor="Red" ValidationGroup="VAlidar">El Campo Es Requerido</asp:RequiredFieldValidator>
            <br />
            <asp:Label ID="lblError" runat="server" Visible="False" ForeColor="Red"></asp:Label>
            <br />
            <br />
            <asp:Button ID="btnEnviar" runat="server" Text="Enviar" OnClick="btnEnviar_Click" ValidationGroup="VAlidar" />
        </asp:Panel>
        <asp:Panel ID="mensaje" runat="server" Visible="false">
            <h4>Reciba un cordial saludo del Fondo Emprender.</h4>
            <br />
            <br />
            <p>Con la presente comunicación damos a conocer los datos de acceso al sistema de información del Fondo Emprender (www.fondoemprender.com).</p>
            <br />
            <br />
            Nombre de usuario:
            <asp:TextBox ID="lblEmail" runat="server" Text="" Enabled="false" Width="200px"></asp:TextBox>
            <br />
            Contraseña:
            <asp:TextBox ID="txtClave" runat="server" Text="*********" TextMode="Password" Enabled="false" Width="200px"></asp:TextBox>
            <br />
            <br />
            <p>Consideramos importante anotar que la información relacionada en la presente comunicación no debe ser conocida por terceras personas y la clave de acceso debe ser cambiada la primera vez que ingrese a la plataforma del Fondo Emprender con el fin de garantizar la confidencialidad y seguridad de la misma.</p>
            <br />
            <br />
            Cordialmente,
        <br />
            <br />
            Fondo Emprender
        <br />
            Línea de soporte técnico (Bogotá): 4292670 
        <br />
            <br />
            <asp:Button ID="btnEnviarCorreo" runat="server" Text="Enviar Correo" OnClick="btnEnviarCorreo_Click" />
        </asp:Panel>
    </asp:Panel>
      <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
          <asp:UpdatePanel ID="PanelCambioDeClave" runat="server" Visible="false" Width="100%"
        UpdateMode="Conditional" >
        <contenttemplate>
       
            <asp:Label ID="l_fechaActual" runat="server" style="font-weight: 700"></asp:Label>
            
                <h1>CAMBIO DE CLAVE</h1>
              <p>
            <asp:Label ID="UserName" runat="server" AssociatedControlID="UserName">Clave Actual:<br /><span style="font-size:smaller;">(Máximo 20 caracteres)</span></asp:Label>
            <asp:TextBox ID="txt_claveActual" runat="server" TextMode="Password" MaxLength="20"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                    BackColor="White" ControlToValidate="txt_claveActual" 
                    ErrorMessage="* Este campo está vacío" Display="Dynamic"
                    style="font-size: small; color: #FF0000;"></asp:RequiredFieldValidator>
           
              </p>
              <p>
            <asp:Label ID="Label1" runat="server" AssociatedControlID="UserName">Nueva Clave:<br /><span style="font-size:smaller;">(Máximo 20 caracteres)</span></asp:Label>
                        
                <asp:TextBox ID="txt_nuevaclave" runat="server" Width="170px" 
                    TextMode="Password" MaxLength="20"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                    BackColor="White" ControlToValidate="txt_nuevaclave" 
                    ErrorMessage="* Este campo está vacío" Display="Dynamic"
                    style="font-size: small; color: #FF0000;">
                </asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidatora5" runat="server" display="dynamic"
                ControlToValidate="txt_nuevaclave"  
                ErrorMessage="* La clave debe tener al menos una mayúscula, una minúsculas, un número, un carácter y debe tener al menos 8 digitos, no es permitido ingresar el usuario en el password"
                style="font-size: small; color: #FF0000;"></asp:RegularExpressionValidator>
               </p>
              <p>
             <asp:Label ID="Label2" runat="server" AssociatedControlID="UserName">Confirmar Nueva Clave:<br /><span style="font-size:smaller;">(Máximo 20 caracteres)</span></asp:Label>
            
                <asp:TextBox ID="txt_confirmaNuevaClave" runat="server" Width="170px" 
                    TextMode="Password" MaxLength="20"></asp:TextBox>
           
                <asp:CompareValidator ID="CompareValidator1" runat="server" 
                    ControlToCompare="txt_nuevaclave" ControlToValidate="txt_confirmaNuevaClave" 
                    ErrorMessage="* Las claves no coinciden" style="color: #FF0000" Display="Dynamic"></asp:CompareValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                    BackColor="White" ControlToValidate="txt_confirmaNuevaClave" 
                    ErrorMessage="* Este campo está vacío" Display="Dynamic"
                    style="font-size: small; color: #FF0000;">
                </asp:RequiredFieldValidator>
              </p>
            
                <asp:Button ID="Btn_cambiarClave" runat="server" Text="Cambiar Clave" 
                    onclick="Btn_cambiarClave_Click" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="Btn_Cancelar" runat="server" Text="Cancelar" 
                    onclick="Btn_Cancelar_Click" CausesValidation="False" />
            

     </contenttemplate>
    </asp:UpdatePanel>
</asp:Content>
