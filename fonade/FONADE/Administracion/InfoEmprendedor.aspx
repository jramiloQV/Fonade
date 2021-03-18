<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InfoEmprendedor.aspx.cs" Inherits="Fonade.FONADE.Administracion.InfoEmprendedor" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../../Scripts/common.js" type="text/javascript"></script>
    <style type="text/css">
        table {
            width:100%;
        }
    </style>
</head>
<body style="width:450px; height:670px;">
    <form id="form1" runat="server">
    <div style="width:450px; height:670px;">
        <h1>
            <label>INFORMACIÓN DE EMPRENDEDOR</label>
        </h1>
        <br />
        <br />
        <table>
            <tr>
                <td>Nombre:</td>
                <td>
                    <asp:Label ID="lblnombre" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Apellidos:</td>
                <td>
                    <asp:Label ID="lblapellidos" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Tipo de identificación:</td>
                <td>
                    <asp:Label ID="lbltipoidentificacion" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Identificación:</td>
                <td>
                    <asp:Label ID="lblidentificacion" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Ciudad de expedición:</td>
                <td>
                    <asp:Label ID="lblciudadexpedicion" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Correo Electrónico:</td>
                <td>
                    <asp:Label ID="lblcorreo" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Género:</td>
                <td>
                    <asp:Label ID="lblgenero" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Fecha de nacimiento (mm/dd/aaaa):</td>
                <td>
                    <asp:Label ID="lblfecha" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Ciudad de nacimiento:</td>
                <td>
                    <asp:Label ID="lblciudadnacimiento" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Número telefónico:</td>
                <td>
                    <asp:Label ID="lblnumero" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Tipo de aprendíz:</td>
                <td>
                    <asp:Label ID="lbltipoaprendiz" runat="server"></asp:Label>
                </td>
            </tr>
        </table>

        <br />
        <hr />
        <br />

        <table>
            <tr>
                <td>Nivel de estudio:</td>
                <td>
                    <asp:Label ID="lblnivelestudio" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Programa realizado:</td>
                <td>
                    <asp:Label ID="lblprogramarealizado" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Institución:</td>
                <td>
                    <asp:Label ID="lblinstitucion" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Ciudad de institución:</td>
                <td>
                    <asp:Label ID="lblciudadinstitucion" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Estado:</td>
                <td>
                    <asp:Label ID="lblestado" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Fecha de inicio (mm/dd/aaaa):</td>
                <td>
                    <asp:Label ID="lblfechainicion" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Fecha de grado (mm/dd/aaaa):</td>
                <td>
                    <asp:Label ID="lblfechagrado" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Fecha de finalización de materias (mm/dd/aaaa):</td>
                <td>
                    <asp:Label ID="lblfechafinalizacionmaterias" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Fecha de finalización de etapa o curso (mm/dd/aaaa):</td>
                <td>
                    <asp:Label ID="lblfechafinalizacionetapa" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Horas o semestres cursados:</td>
                <td>
                    <asp:Label ID="lbltotalsemestres" runat="server"></asp:Label>
                </td>
            </tr>
        </table>

        <br />
        <table>
            <tr>
                <td colspan="2" style="text-align:center;">
                    <asp:Button ID="btncerrar" runat="server" Text="Cerrar" OnClientClick="window.close();" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
