<%@ Page Title="FONDO EMPRENDER" Language="C#" MasterPageFile="~/Emergente.Master" AutoEventWireup="true"
    CodeBehind="InfoEmprendedor.aspx.cs" Inherits="Fonade.FONADE.evaluacion.InfoEmprendedor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style10
        {
            width: 224px;
        }
        .style11
        {
            width: 224px;
            height: 21px;
        }
        .style12
        {
            height: 21px;
        }
        .style13
        {
            width: 53px;
        }
    </style>
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <title>Información De Asesor</title>
    <br />
    <table width="70%" border="0"style="margin-left: 20px;" >
        <tr>
            <td colspan="2" >
             
                  <h1>
          <asp:Label runat="server" ID="lbl_Titulo" ForeColor="Black">Información De Emprendedor</asp:Label></h1> 
            </td>
            <td colspan="2" align="right">
                <asp:Label ID="l_fechaActual" runat="server" Style="font-weight: 700"></asp:Label>
            </td>
        </tr>
    </table>
    <table width="100%" border="0" cellspacing="0" cellpadding="0" 
        style="margin-left: 20px;">
        <tr>
            <td align="left">
                <table width="80%" cellpadding="0" cellspacing="4">
                    <tr>
                        <td class="style10">
                            Nombre:
                        </td>
                        <td width="30%">
                            <asp:Label ID="nombre" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="style10">
                            Apellidos:
                        </td>
                        <td width="30%">
                            <asp:Label ID="apellidos" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="style11">
                            Tipo de identificaci&oacute;n:
                        </td>
                        <td class="style12" width="30%">
                            <asp:Label ID="tipo" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="style10">
                            Identificaci&oacute;n:
                        </td>
                        <td width="30%">
                            <asp:Label ID="cedula" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="style10">
                            Ciudad de expedici&oacute;n:
                        </td>
                        <td width="30%">
                            <asp:Label ID="expedicion" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="style10">
                            Correo Electr&oacute;nico:
                        </td>
                        <td width="30%">
                            <asp:Label ID="email" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="style10">
                            G&eacute;nero:
                        </td>
                        <td width="30%">
                            <asp:Label ID="genero" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="style10">
                            Fecha de nacimiento<br />
&nbsp;(mm/dd/aaaa):
                        </td>
                        <td width="30%">
                            <asp:Label ID="fechan" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="style10">
                            Ciudad de nacimiento:
                        </td>
                        <td width="30%">
                            <asp:Label ID="ciuddn" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="style10">
                            N&uacute;mero telef&oacute;nico:
                        </td>
                        <td width="30%">
                            <asp:Label ID="numerot" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="style10">
                            Tipo de aprend&iacute;z:
                        </td>
                        <td width="30%">
                            <asp:Label ID="aprendiz" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <hr />
                        </td>
                    </tr>
                    <tr id="tr_nivel" runat="server">
                        <td class="style10">
                            Nivel de estudio:
                        </td>
                        <td width="30%">
                            <asp:Label ID="nivelE" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr id="tr_programa" runat="server">
                        <td class="style10">
                            Programa realizado:
                        </td>
                        <td width="30%">
                            <asp:Label ID="programR" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr id="tr_institucion" runat="server">
                        <td class="style10">
                            Instituci&oacute;n:
                        </td>
                        <td width="30%">
                            <asp:Label ID="institucion" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr id="tr_ciudadi" runat="server">
                        <td class="style10">
                            Ciudad de instituci&oacute;n:
                        </td>
                        <td width="30%">
                            <asp:Label ID="ciudadinst" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr id="tr_estado" runat="server">
                        <td class="style10">
                            Estado:
                        </td>
                        <td width="30%">
                            <asp:Label ID="estado" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr id="tr_fechai" runat="server">
                        <td class="style10">
                            Fecha de inicio<br />
&nbsp;(dd/mm/aaaa):
                        </td>
                        <td width="30%">
                            <asp:Label ID="fechaini" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr id="tr_fechagrado" runat="server">
                        <td class="style10">
                            Fecha de grado<br />
&nbsp;(dd/mm/aaaa):
                        </td>
                        <td width="30%">
                            <asp:Label ID="fechag" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr id="tr_fechafinalizacionM" runat="server">
                        <td class="style10">
                            Fecha de finalizaci&oacute;n de materias 
                            <br />
                            (dd/mm/aaaa):
                        </td>
                        <td width="30%">
                            <asp:Label ID="fechafm" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr id="tr_fecgafubakuzacionc" runat="server">
                        <td class="style10">
                            Fecha de finalizaci&oacute;n de etapa o curso (dd/mm/aaaa):
                        </td>
                        <td width="30%">
                            <asp:Label ID="fechafinalizacionc" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr id="tr_horas" runat="server">
                        <td class="style10">
                            Horas o semestres cursados:
                        </td>
                        <td width="30%">
                            <asp:Label ID="horas" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="style10">
                            &nbsp;
                        </td>
                        <td width="30%">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <asp:Button ID="BtnCerrar" runat="server" Text="Cerrar" OnClick="BtnCerrar_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
