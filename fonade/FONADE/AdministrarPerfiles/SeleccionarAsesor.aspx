<%@ Page Language="C#" MasterPageFile="~/Emergente.master"  AutoEventWireup="true" CodeBehind="SeleccionarAsesor.aspx.cs" Inherits="Fonade.FONADE.AdministrarPerfiles.SeleccionarAsesor" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
 <asp:Content  ContentPlaceHolderID="head" runat="server" ID="HeadContent"  >
   
 </asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="bodyContentPlace">
<asp:Panel ID="Panel2" CssClass="PanelBuscador"  ClientIDMode="Static" DefaultButton="Buscar" runat="server">
	<script type="text/javascript" >
		$(document).ready(function () {
		    $("#tb_NumeroDocumento").keydown(function (e) {
				// Allow: backspace, delete, tab, escape, enter and .
				if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
					// Allow: Ctrl+A
					(e.keyCode == 65 && e.ctrlKey === true) ||
					// Allow: Ctrl+C
					(e.keyCode == 67 && e.ctrlKey === true) ||
					// Allow: Ctrl+X
					(e.keyCode == 88 && e.ctrlKey === true) ||
					// Allow: home, end, left, right
					(e.keyCode >= 35 && e.keyCode <= 39)) {
					// let it happen, don't do anything
					return;
				}
				// Ensure that it is a number and stop the keypress
				if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
					e.preventDefault();
				}
			});
		});
	</script>

		 <table width="98%" border="0">
		  <tr>
			<td class="style50"><h1><asp:Label runat="server" ID="lbl_Titulo" Text="Buscar Nuevo Asesor" style="font-weight: 700"></asp:Label></h1>
		</td></tr>
		</table>
		<table>
			<tr>
				<td>
					<asp:Label runat="server" ID="label6"></asp:Label>
				</td>
				<td>
					<asp:DropDownList ID="ddl_tipoDocumento" runat="server">
					</asp:DropDownList>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Label runat="server" ID="label5" />
				</td>
				<td>
					
					<asp:TextBox ID="tb_NumeroDocumento"  runat="server" ClientIDMode="Static" MaxLength="10" placeholder="Ingrese solo numeros" pattern="\d*" />
				</td>
			</tr>
		   
			 <tr>
				<td colspan="3">
					<asp:RequiredFieldValidator CssClass="ErrorValidacion" ForeColor="Red" ID="RequiredFieldValidator4"
						runat="server" ControlToValidate="tb_NumeroDocumento" Display="Dynamic" ErrorMessage="* El Número de Identificaión es requerido"
						SetFocusOnError="True"></asp:RequiredFieldValidator><br />
						<asp:RegularExpressionValidator ID="RegularExpressionValidator1" ForeColor="Red" runat="server" ControlToValidate="tb_NumeroDocumento"
						ErrorMessage="* El número de identificación debe ser un dato numérico"  ValidationExpression="^\d+$"  ></asp:RegularExpressionValidator>
				</td>
			</tr>

			<tr>
				<td colspan="3" align="center">
					<asp:Button ID="Buscar" OnClick="Buscar_onclick" runat="server" Text="Buscar" />
				</td>
			</tr>
		</table>
	</asp:Panel>

<asp:Panel ID="Panel1" CssClass="PanelSeleccionarAsesor"  ClientIDMode="Static"  DefaultButton="Buscar" runat="server">
 <table width="98%" border="0">
		  <tr>
			<td class="style50"><h1><asp:Label runat="server" ID="Label2" Text="Seleccionar Asesor" style="font-weight: 700"></asp:Label></h1>
		</td></tr>
		</table>
		<table>
			<tr>
				<td>
					<asp:Label runat="server" ID="label1" Text="SELECCIONAR ASESOR"></asp:Label>
				</td>
				
			</tr>
			<tr>
				<td>
				<asp:Label runat="server" ID="pruebas" Visible="false"></asp:Label>
				 <asp:Button  runat="server"  id="btn_SeleccionarAsesor"  class="boton_Link_Grid" Text=""  OnClick="btn_Asesor_click"/>  
				</td>
				<td>
				  Rol:  <asp:Label runat="server" ID="lbl_rol" />
				</td>
			</tr>
			<tr>
				<td>
					<asp:Label runat="server" ID="Label3"></asp:Label>
				</td>
				
			</tr>
			 <tr>
				 
				 <td align="center">
					<asp:Button  runat="server"  id="btn_nuevabusqueda"  class="boton_Link_Grid" Text="Realizar otra Búsqueda"  OnClick="btn_nuevabusqueda_click"/>
				 </td>
				 
			</tr>

		</table>
	</asp:Panel>

	<asp:Panel ID="Panel3" onclick="PanelClick();"   CssClass="PanelPerfiles" DefaultButton="CrearPerfil"  runat="server">
	 <table width="98%" border="0">
		  <tr>
			<td class="style50"><h1><asp:Label runat="server" ID="Label4" Text="Adicionar Asesor" style="font-weight: 700"></asp:Label></h1>
		</td></tr>
		</table>
		<asp:Table ID="Table1" runat="server">
		   
			<asp:TableRow>
				<asp:TableCell>
					<asp:Label ID="Label7" runat="server" Text="Nombre Asesor:	"></asp:Label>
				</asp:TableCell>
				<asp:TableCell>
					<asp:TextBox ID="tb_NombreAsesor" runat="server"  Enabled="false"></asp:TextBox>
					<%--<asp:RequiredFieldValidator  CssClass="ErrorValidacion" ForeColor="Red" ID="RequiredFieldValidator3" runat="server" ControlToValidate="tb_NombreAsesor" Display="Dynamic" ErrorMessage="* Este campo no puede estar en blanco"></asp:RequiredFieldValidator>--%>
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow>
				<asp:TableCell>
					<asp:Label ID="Label8" runat="server" Text="Apellido Asesor:	"></asp:Label>
				</asp:TableCell>
				<asp:TableCell>
					<asp:TextBox ID="tb_ApellidoAsesor" runat="server"  Enabled="false"></asp:TextBox>
					<%--<asp:RequiredFieldValidator  CssClass="ErrorValidacion" ForeColor="Red" ID="RequiredFieldValidator3" runat="server" ControlToValidate="tb_NombreAsesor" Display="Dynamic" ErrorMessage="* Este campo no puede estar en blanco"></asp:RequiredFieldValidator>--%>
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow>
				<asp:TableCell>
					<asp:Label ID="Label9" runat="server" Text="Email Asesor:	"></asp:Label>
				</asp:TableCell>
				<asp:TableCell>
					<asp:TextBox ID="tb_Email" runat="server"  Enabled="false"></asp:TextBox>
					<%--<asp:RegularExpressionValidator ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="tb_Email" runat="server"  ForeColor="Red"  Display="Dynamic" ErrorMessage="*Ingrese una dirección de correo valida"></asp:RegularExpressionValidator>
			<asp:RequiredFieldValidator  CssClass="ErrorValidacion" ForeColor="Red" ID="RequiredFieldValidator4" runat="server" ControlToValidate="tb_Email" Display="Dynamic" ErrorMessage="* Este campo no puede estar en blanco"></asp:RequiredFieldValidator>--%>
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow>
				<asp:TableCell>
					<asp:Button ID="CrearPerfil" OnClick="CrearPerfil_onclick" runat="server" Text="Crear ASesor" />
				</asp:TableCell>
			</asp:TableRow>
		</asp:Table>
	</asp:Panel>


	</asp:Content>
