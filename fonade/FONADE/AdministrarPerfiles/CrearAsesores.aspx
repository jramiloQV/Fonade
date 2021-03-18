<%@ Page Language="C#" MasterPageFile="~/Master.master" AutoEventWireup="true" CodeBehind="CrearAsesores.aspx.cs"
    Inherits="Fonade.FONADE.AdministrarPerfiles.CrearAsesores" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
   <script language="javascript" type="text/javascript">

       function enviaCodigoAsesor(cod_asesor)
       {
           location.href = "CrearAsesores.aspx?_cod_contacto=" + cod_asesor;

        //   document.getElementById("<%= tb_NumeroIdentificacion.ClientID %>").value = cod_asesor;
          // __doPostBack("<%= tb_NumeroIdentificacion.ClientID %>", "tb_NumeroIdentificacion_TextChanged");
       }

       function PanelClick()
       {
           __doPostBack('Panel1', 'Click');
       }
   </script>
   
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="bodyContentPlace">
<table width="98%" border="0">
          <tr>
            <td class="style50"><h1><asp:Label runat="server" Text="Nuevo Asesor" ID="lbl_Titulo" style="font-weight: 700"></asp:Label></h1>
	    </td> </tr>
        </table>
    <asp:Panel ID="Panel1" onclick="PanelClick();"  DefaultButton="CrearPerfil"  CssClass="PanelPerfiles"   runat="server">
        <asp:Table runat="server">
            <asp:TableRow>
                <asp:TableCell>
                    <asp:Label ID="Label1" runat="server" Text="Tipo de Identificación:"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox ID="tb_TipodeIdentificacion" runat="server" Enabled="false"></asp:TextBox>
                    <%--<asp:RequiredFieldValidator  CssClass="ErrorValidacion" ForeColor="Red" ID="RequiredFieldValidator2" runat="server" ControlToValidate="tb_TipodeIdentificacion" Display="Dynamic" ErrorMessage="* Este campo no puede estar en blanco"></asp:RequiredFieldValidator>--%>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    <asp:Label ID="Label2" runat="server" Text="Número de Identificación:"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox OnTextChanged="tb_NumeroIdentificacion_TextChanged" ID="tb_NumeroIdentificacion" runat="server"  Enabled="false"></asp:TextBox>
                    <%--<asp:RequiredFieldValidator  CssClass="ErrorValidacion" ForeColor="Red" ID="RequiredFieldValidator1" runat="server" ControlToValidate="tb_NumeroIdentificacion" Display="Dynamic" ErrorMessage="* Este campo no puede estar en blanco"></asp:RequiredFieldValidator>--%>
                    <%-- <asp:CompareValidator CssClass="ErrorValidacion" ForeColor="Red" ID="RequiredFieldValidator5" runat="server" ControlToValidate="tb_NumeroIdentificacion" Display="Dynamic" ErrorMessage="* debe ser numérico"></asp:CompareValidator>--%>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    <asp:Label ID="Label3" runat="server" Text="Nombre Asesor:	"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox ID="tb_NombreAsesor" runat="server"  Enabled="false"></asp:TextBox>
                    <%--<asp:RequiredFieldValidator  CssClass="ErrorValidacion" ForeColor="Red" ID="RequiredFieldValidator3" runat="server" ControlToValidate="tb_NombreAsesor" Display="Dynamic" ErrorMessage="* Este campo no puede estar en blanco"></asp:RequiredFieldValidator>--%>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    <asp:Label ID="Label7" runat="server" Text="Apellido Asesor:	"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox ID="tb_ApellidoAsesor" runat="server"  Enabled="false"></asp:TextBox>
                    <%--<asp:RequiredFieldValidator  CssClass="ErrorValidacion" ForeColor="Red" ID="RequiredFieldValidator3" runat="server" ControlToValidate="tb_NombreAsesor" Display="Dynamic" ErrorMessage="* Este campo no puede estar en blanco"></asp:RequiredFieldValidator>--%>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    <asp:Label ID="Label4" runat="server" Text="Email Asesor:	"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox ID="tb_Email" runat="server"  Enabled="false"></asp:TextBox>
                    <%--<asp:RegularExpressionValidator ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="tb_Email" runat="server"  ForeColor="Red"  Display="Dynamic" ErrorMessage="*Ingrese una dirección de correo valida"></asp:RegularExpressionValidator>
            <asp:RequiredFieldValidator  CssClass="ErrorValidacion" ForeColor="Red" ID="RequiredFieldValidator4" runat="server" ControlToValidate="tb_Email" Display="Dynamic" ErrorMessage="* Este campo no puede estar en blanco"></asp:RequiredFieldValidator>--%>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    <asp:Button ID="CrearPerfil" OnClick="CrearPerfil_onclick" runat="server" Text="Crear Asesor" />
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </asp:Panel>
     
</asp:Content>
