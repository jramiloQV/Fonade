<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CatalogoProduccionPOInterventoria.aspx.cs"
	Inherits="Fonade.FONADE.evaluacion.CatalogoProduccionPOInterventoria" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title></title>
	<style type="text/css">
		.auto-style1 {
			width: 80%;
			margin: 0px auto;
			text-align: center;
		}

		.panelmeses {
			margin: 0px auto;
			text-align: center;
		}

		.auto-style2 {
			height: 23px;
		}

		.blanco {
			background-color: white !important;
		}
	</style>
	<link href="../../Styles/Site.css" rel="stylesheet" />
	<script type="text/javascript">
		function ValidNum(e) {
			var tecla = document.all ? tecla = e.keyCode : tecla = e.which;
			return (tecla > 47 && tecla < 58);
		}
	</script>
</head>
<body>
	<form id="form1" runat="server">
		<div class="blanco">
			<table class="auto-style1">
				<tr>
					<td colspan="2">
						<!-- class="auto-style2"-->
						<p style="text-align: left;">
							<asp:Label ID="lbl_enunciado" runat="server" BackColor="#000066" ForeColor="White"
								Text="" Width="40%" />
							<asp:Label ID="lbl_Interventor" runat="server" BackColor="#000066" ForeColor="White"
								Width="40%" />
							<asp:Label ID="lbl_tiempo" runat="server" ForeColor="Red" />
						</p>
					</td>
				</tr>
				<tr id="tr_mes" runat="server">
					<td style="text-align: center">
						<asp:Label ID="Label1" runat="server" Text="Mes:" />
					</td>
					<td>
						<asp:TextBox ID="txt_mes" runat="server" ValidationGroup="accionar" Width="250px" Enabled="false" />
					</td>
				</tr>
				<tr id="tr_producto" runat="server">
					<td style="text-align: center">
						<asp:Label ID="Label2" runat="server" Text="Producto o Servicio:" />
					</td>
					<td style="margin-left: 120px">
						<asp:TextBox ID="txt_cargo" runat="server" ValidationGroup="accionar" Enabled="false"
							Width="250px" />
					</td>
				</tr>
				<tr>
					<td style="text-align: center">
						<asp:Label ID="Label9" runat="server" Text="Fecha Avance:"></asp:Label>
					</td>
					<td>
						<asp:TextBox ID="txtFechaAvance" runat="server" Width="250px" Enabled="False"></asp:TextBox>
					</td>
				</tr>
				<tr>
					<td style="text-align: center">
						<asp:Label ID="Label3" runat="server" Text="Observaciones:" />
					</td>
					<td>
						<asp:TextBox ID="txt_observaciones" runat="server" ValidationGroup="accionar" Width="250px"
							TextMode="MultiLine" Rows="7" />
					</td>
				</tr>
				<tr>
					<td style="text-align: center">
						<asp:Label ID="Label10" runat="server" Text="Fecha Aprobacion:"></asp:Label>
					</td>
					<td>
						<asp:TextBox ID="txtFechaAprobacion" runat="server" Width="250px" Enabled="False"></asp:TextBox>
					</td>
				</tr>
				<tr id="tr_obser_inter" runat="server">
					<td style="text-align: center">
						<asp:Label ID="Label4" runat="server" Text="Observaciones Interventor:" />
					</td>
					<td>
						<asp:TextBox ID="txt_observ_interventor" runat="server" ValidationGroup="accionar"
							Width="250px" TextMode="MultiLine" Rows="7" />
					</td>
				</tr>
				<tr id="tr_act_aprobada" runat="server">
					<td style="text-align: center">
						<asp:Label ID="Label5" runat="server" Text="Actividad Aprobada:" />
					</td>
					<td>
						<asp:DropDownList ID="dd_aprobado" runat="server">
						</asp:DropDownList>
					</td>
				</tr>
				<tr>
					<td colspan="2" class="auto-style2">
						<%--Cambiar el título dependiendo del mes seleccionado--%>
						<asp:Label ID="lbl_tipoReq_Enunciado" runat="server" BackColor="#000066" ForeColor="White"
							Text="REQUERIMIENTOS DE RECURSOS POR MES" Width="100%" />
					</td>
				</tr>
				<tr>
					<td style="text-align: center">
						<asp:Label ID="Label6" runat="server" Text="Cantidad:" />
						<asp:TextBox ID="txt_sueldo_obtenido" runat="server" />
					</td>
					<td style="text-align: center">
						<asp:Label ID="Label7" runat="server" Text="Costo:" />
						<asp:TextBox ID="txt_prestaciones_obtenidas" runat="server" />
					</td>
				</tr>
				<tr>
					<td>
						<asp:Label ID="lbl_total_Enunciado" Text="Total: " runat="server" Font-Bold="true" Visible="false" />
						<asp:Label ID="lbl_Total" Text="" runat="server" />
					</td>
				</tr>
				<tr>
					<td style="text-align: right;">
						<asp:Button ID="B_Acion" runat="server" ValidationGroup="accionar" OnClick="B_Acion_Click" />
					</td>
					<td>
						<asp:Button ID="B_Cancelar" runat="server" Text="Cancelar" OnClick="B_Cancelar_Click" />
					</td>
				</tr>
				<tr>
					<td colspan="2" class="auto-style2" style="text-align: center">
						<asp:Label ID="Label8" runat="server" BackColor="#000066" ForeColor="White" Text="ADJUNTAR ARCHIVOS"
							Width="100%" />
					</td>
				</tr>
				<tr>
					<td style="text-align: center;" colspan="2">
						<asp:ImageButton ID="img_btn_NuevoDocumento" ImageUrl="../../Images/icoClip.gif"
							runat="server" ToolTip="Nuevo Documento" OnClick="img_btn_NuevoDocumento_Click"
							CommandName="NuevoDocumento" Visible="false" />
						&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:ImageButton ID="img_btn_enlazar_grilla_PDF" ImageUrl="../../Images/icoClip2.gif"
						runat="server" ToolTip="Ver Documentos" OnClick="img_btn_enlazar_grilla_PDF_Click" />
					</td>
				</tr>
			</table>
		</div>
		<div style="text-align: center;color: white;background: #00468f;border-bottom: solid;">
            HISTORICO AVANCE
        </div>
        <div id="gridHistorico">
            <asp:GridView ID="gvHistoricoAvances" runat="server"
                AutoGenerateColumns="False"
                CssClass="Grilla"
                DataKeyNames="idHistorico" 
                EmptyDataText="No se tiene historico del avance."
                ForeColor="#666666" Width="100%">
                <Columns>
                    <asp:BoundField DataField="ObservacionEmprendedor" HeaderText="Observacion Emprendedor" />
                    <asp:BoundField DataField="FechaAvanceEmprendedor" HeaderText="Fecha Avance Emprendedor" />                     
                    <asp:BoundField DataField="ObservacionInterventor" HeaderText="Observacion Interventor" />                     
                    <asp:BoundField DataField="FechaAvanceInterventor" HeaderText="Fecha Avance Interventor" />                     
                    <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" /> 
                    <asp:BoundField DataField="Costo" HeaderText="Costo" /> 
                    <asp:BoundField DataField="avanceAprobado" HeaderText="Aprobado" />                    
                    <asp:BoundField DataField="nombres" HeaderText="Registrado Por" />                     
                </Columns>
            </asp:GridView>
        </div>
	</form>
</body>
</html>
