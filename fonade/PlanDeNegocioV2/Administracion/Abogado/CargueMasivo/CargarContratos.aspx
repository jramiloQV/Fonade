<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CargarContratos.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Administracion.Abogado.CargueMasivo.CargarContratos" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style type="text/css">
        table {
            width: 100%;
        }
    </style>
    <link href="../../../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <link href="../../../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />
    <script src="../../../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../../../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../../../../Scripts/common.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div style="overflow-y: scroll; width: 100%; height: 100%;">
            <p>
                <h1>
                    <label>Carga Archivo</label>
                </h1>
            </p>
            <table>
                <tr>
                    <td>
                        <asp:FileUpload ID="archivoPagos" runat="server" Width="500px" />
                    </td>
                    <td>
                        <asp:Button ID="btnsubir" runat="server" OnClick="btnsubir_Click" ValidationGroup="subirarc" Text="Subir Archivo" />
                    </td>
                </tr>
            </table>
            <asp:Panel ID="pnlEjemplo" runat="server" Visible="true">
                <p style="color: red;" >Solo se debe subir el archivo de Información de contratos con el siguiente formato (colocar solo las filas de valores, incluir una sola fila de titulos):</p>
                <br />
                <table class="Grilla" id="tblEjemplo">
                <thead>
                    <tr>
                        <th>Numero Campo</th>
                        <th>Nombre Camp</th>
                        <th>Ejemplo</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>1</td>
                        <td>CodigoProyecto</td>
                        <td>12342</td>
                    </tr>
                    <tr>
                        <td>2</td>
                        <td>NumeroContrato</td>
                        <td>1001383</td>
                    </tr>
                    <tr>
                        <td>3</td>
                        <td>ObjetoContrato</td>
                        <td>El contrato xxx...información del contrato.</td>
                    </tr>
                    <tr>
                        <td>4</td>
                        <td>FechaAP</td>
                        <td>01/02/2018</td>
                    </tr>
                    <tr>
                        <td>5</td>
                        <td>ValorInicialPesos</td>
                        <td>10000</td>
                    </tr>
                    <tr>
                        <td>6</td>
                        <td>PlazoContratoMeses</td>
                        <td>14</td>
                    </tr>
                    <tr>
                        <td>7</td>
                        <td>NumeroApPresupuestal</td>
                        <td>12</td>
                    </tr>
                    <tr>
                        <td>8</td>
                        <td>NumeroActaConcejoDirectivo</td>
                        <td>21313</td>
                    </tr>
                    <tr>
                        <td>9</td>
                        <td>FechaConcejoDirectivo</td>
                        <td>01/02/2018</td>
                    </tr>
                    <tr>
                        <td>10</td>
                        <td>ValorEnte</td>
                        <td>1100000</td>
                    </tr>
                    <tr>
                        <td>11</td>
                        <td>ValorSena</td>
                        <td>1100000</td>
                    </tr>
                    <tr>
                        <td>12</td>
                        <td>CertificadoDisponibilidad</td>
                        <td>124232</td>
                    </tr>
                    <tr>
                        <td>13</td>
                        <td>FechaCertificadoDisponibilidad</td>
                        <td>01/02/2018</td>
                    </tr>
                    <tr>
                        <td>14</td>
                        <td>EstadoContrato</td>
                        <td>Condonado</td>
                    </tr> 
                     <tr>
                        <td>15</td>
                        <td>TipoContrato</td>
                        <td>Prestación de Servicios</td>
                    </tr> 
                </tbody>
            </table>
            </asp:Panel>
            <asp:Panel ID="pnlResultados" runat="server" Visible="false">
                <br />
                <br />
                <table border="0" cellpadding='0' cellspacing='0'>
                    <tr>
                        <td colspan='2' align='left' class='Menu'>Resultado de la carga:</td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <br />
                            <br />
                            <asp:Label ID="successUpload" Text="Cargue existoso." runat="server"  Visible="false"/>
                            <asp:Label ID="partialUpload" Text="Cargue parcial." runat="server"  Visible="false"/>
                            <asp:Label ID="errorUpload" Text="Cargue con errores." runat="server" Visible="false"/>
                            <br />
                            <br />
                        </td>
                    </tr>                    
                    <tr>
                        <td bgcolor='#99FFFF'>&nbsp;&nbsp;</td>
                        <td>&nbsp;&nbsp;Registros modificados (cargados)</td>
                    </tr>
                    <tr>
                        <td bgcolor='#FFCCCC'>&nbsp;&nbsp;</td>
                        <td>&nbsp;&nbsp;Registros con error (No Cargados)</td>
                    </tr>
                    <tr>
                        <td bgcolor='#FFFF66'>&nbsp;&nbsp;</td>
                        <td>&nbsp;&nbsp;Registros con advertencia (No cargados)</td>
                    </tr>
                </table>
                <br />
                <asp:GridView ID="gvMain" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataText="No exixten datos"
                    DataKeyNames="Color" OnRowDataBound="grvResumen_RowDataBound" CssClass="Grilla">
                    <Columns>
                          <asp:BoundField HeaderText="No." DataField="Indice"  />
                          <asp:BoundField HeaderText="Proyecto No." DataField="CodigoProyecto" />
                          <asp:BoundField HeaderText="Número de contrato" DataField="NumeroContrato" />
                          <asp:BoundField HeaderText="Estado del contrato" DataField="EstadoContrato" />      
                          <asp:BoundField HeaderText="Observacion del sistem" DataField="MensajeSistema" />         
                    </Columns>
                </asp:GridView>
            </asp:Panel>
        </div>
    </form>
</body>
</html>
