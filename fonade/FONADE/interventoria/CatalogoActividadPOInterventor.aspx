<%@ Page Title="FONDO EMPRENDER" Language="C#" AutoEventWireup="true" CodeBehind="CatalogoActividadPOInterventor.aspx.cs"
    Inherits="Fonade.FONADE.interventoria.CatalogoActividadPOInterventor" ValidateRequest="false"  %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
            background-color: #fff !important;
            background-image: none !important;
        }

        .auto-style1 {
            width: 80%;
            margin: 0px auto;
        }

        .panelmeses {
            margin: 0px auto;
            text-align: center;
        }

        .auto-style2 {
            height: 23px;
        }

        .auto-style3 {
            height: 24px;
        }
    </style>
    <link href="../../Styles/Site.css" rel="stylesheet" />
    <script type="text/javascript">
        function ValidNum(e) {
            var tecla = document.all ? tecla = e.keyCode : tecla = e.which;
            return (tecla > 47 && tecla < 58);
        }
    </script>
    <script type="text/javascript">
        function suma() {
            var val1 = document.getElementById("txt_sueldo_obtenido").value;
            var val2 = document.getElementById("txt_prestaciones_obtenidas").value;
            var suma = eval(parseInt(val1) + parseInt(val2)).toString();
            var total = '$ ' + suma.replace(/\B(?=(\d{3})+(?!\d))/g, ".");
            if (total != "$ NaN") {
                document.getElementById('lbl_Total').innerHTML = total;
            }
            console.log(total);
        }

        function ActualizaParent() {
            //window.opener.ActualizaPlanOper();
            window.opener.document.getElementsByName('btnRefresh').serverclick();
        }

        //window.onbeforeunload = function (event) {
        //    var message = 'Important: Please click on \'Save\' button to leave this page.';
        //    if (typeof event == 'undefined') {
        //        event = window.event;
        //    }
        //    if (event) {
        //        event.returnValue = message;
        //    }
        //    return message;
        //};

        <%--window.onbeforeunload = function (event) {
            if (typeof event == 'undefined') {
                event.window.event;
            }
            if (event) {
                '<% Session["Accion"] = null;  %>';
                '<% Session["CodActividad"] = null;  %>';
                '<% Session["pagina"] = null;  %>';
            }
        };--%>

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div style="width: 100%; height: auto; margin-top: 10px;">
            <table class="auto-style1">
                <tr>
                    <td colspan="2">
                        <!-- class="auto-style2"-->
                        <p style="text-align: left;">
                            <b><asp:Label ID="lbl_enunciado" runat="server" BackColor="#000066" ForeColor="White"
                                Text="" Width="40%" /></b>
                            <asp:Label ID="lbl_Interventor" runat="server" BackColor="#000066" ForeColor="White"
                                Width="40%" />
                            <asp:Label ID="lbl_tiempo" runat="server" ForeColor="Red" />
                        </p>
                    </td>
                </tr>
                <tr id="tr_mes" runat="server">
                    <td style="text-align: center" class="auto-style3">
                        <asp:Label ID="Label1" runat="server" Text="Mes:"></asp:Label>
                    </td>
                    <td class="auto-style3">
                        <asp:TextBox ID="txt_mes" runat="server" ValidationGroup="accionar" Width="350px" Enabled="false"></asp:TextBox>
                    </td>
                </tr>
                <tr id="tr_actividad" runat="server">
                    <td style="text-align: center">
                        <asp:Label ID="Label2" runat="server" Text="Actividad:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_cargo" runat="server" ValidationGroup="accionar" Width="350px" Enabled="false"></asp:TextBox>
                    </td>
                </tr>
				 <tr>
                    <td style="text-align: center">
                        <asp:Label ID="Label6" runat="server" Text="Fecha Avance:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtFechaAvance" runat="server" Width="350px" Enabled="False"
							
                            ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center">
                        <asp:Label ID="Label3" runat="server" Text="Observaciones:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_observaciones" runat="server" ValidationGroup="accionar" Width="350px"
                            TextMode="MultiLine" Rows="7"></asp:TextBox>
                    </td>
                </tr>
				 <tr>
                    <td style="text-align: center">
                        <asp:Label ID="Label7" runat="server" Text="Fecha Aprobacion:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtFechaAprobacion" runat="server" Width="350px" Enabled="False"
							
                            ></asp:TextBox>
                    </td>
                </tr>
                <tr id="tr_observ_inter" runat="server">
                    <td style="text-align: center">
                        <asp:Label ID="Label4" runat="server" Text="Observaciones Interventor:"></asp:Label>
                    </td>
                    <td style="margin-left: 20px;">
                        <asp:TextBox ID="txt_observ_interventor" runat="server" ValidationGroup="accionar"
                            Width="350px" TextMode="MultiLine" Rows="7"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center">
                        <asp:Label ID="Label5" runat="server" Text="Actividad Aprobada:"></asp:Label>
                    </td>
                    <td style="margin-left: 20px;">
                        <asp:DropDownList ID="dd_aprobado" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="auto-style2" style="text-align: center">
						<%--Cambiar el título dependiendo del mes seleccionado--%>
                        <asp:Label ID="lbl_tipoReq_Enunciado" runat="server" BackColor="#000066" ForeColor="White"
                            Width="100%"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center">
                        <asp:Label ID="lblFondo" runat="server" Text="Fondo Emprender:"></asp:Label>
                        <asp:TextBox ID="txt_sueldo_obtenido" runat="server" onChange="javascript:suma()" />
                    </td>
                    <td style="text-align: center">
                        <asp:Label ID="labelaporte" runat="server" Text="Aporte Emprendedor"></asp:Label>
                        <asp:TextBox ID="txt_prestaciones_obtenidas" runat="server" onChange="javascript:suma()" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbl_total_Enunciado" Text="Total: " runat="server" Font-Bold="true" />
                        <asp:Label ID="lbl_Total" Text="" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center;" colspan="2">
                        <asp:Button ID="B_Acion" runat="server" ValidationGroup="accionar" OnClientClick="ActualizaParent();" 
                            Width="100px" Text="Actualizar" OnClick="B_Acion_Click" />
                        <asp:Button ID="B_Cancelar" runat="server" Text="Cancelar" OnClick="B_Cancelar_Click"  />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="auto-style2" style="text-align: center">
                        <asp:Label ID="Label8" runat="server" BackColor="#000066" ForeColor="White" Text="ADJUNTAR ARCHIVOS"
                            Width="100%"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center;" colspan="2">
                        <asp:ImageButton ID="img_btn_NuevoDocumento" ImageUrl="../../Images/icoClip.gif"
                            runat="server"  ToolTip="Nuevo Documento" OnClick="img_btn_NuevoDocumento_Click"
                            CommandName="NuevoDocumento" Visible="false" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:ImageButton ID="img_btn_enlazar_grilla_PDF" ImageUrl="../../Images/icoClip2.gif"
                        runat="server"  ToolTip="Ver Documento" CommandName="ListarDocumentos" OnClick="img_btn_enlazar_grilla_PDF_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbl_mensaje" Text="" runat="server" Visible="false" />
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
                    <asp:BoundField DataField="ValorFondoEmprender" HeaderText="Valor Fondo Emprender" /> 
                    <asp:BoundField DataField="ValorAporteEmprendedor" HeaderText="Valor Aporte Emprendedor" /> 
                    <asp:BoundField DataField="avanceAprobado" HeaderText="Aprobado" />                    
                    <asp:BoundField DataField="nombres" HeaderText="Registrado Por" /> 
                    
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>
