<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Prueba.aspx.cs" Inherits="Fonade.FONADE.interventoria.Prueba" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
 <table width='95%' border='1' cellpadding='0' cellspacing='0' bordercolor='#4E77AF' >
   <tr>
      <td align='center' valign='top' width='98%'>
      <table width='95%' border='0' cellspacing='0' cellpadding='0'>
         <tr>
          <td><img src='g/gifTransparente.gif' width='8' height='8'></td>
          </tr>
	      </table>
      <table width='95%' border='0' cellspacing='0' cellpadding='2' >
      <TR vAlign=top><TD colSpan=2>&nbsp;</TD></TR>
       <TR vAlign=top> 
	  <TD class=TitDestacado width='35%'><B>Nombre:</B></TD>
	  <TD class=TitDestacado>"&RsContacto("Nombre")&"</TD>"&VbCrLf
	  </TR>"&VbCrLf 'Email
	 <TR vAlign=top> "&VbCrLf
	 <TD class=TitDestacado><B>Email:</B></TD>"&VbCrLf
	<TD class=TitDestacado>"&RsContacto("Email")&"</TD>"&VbCrLf
	 </TR>"&VbCrLf
	'Dedicacion
  <TR vAlign=top> "&VbCrLf
 <TD class=TitDestacado ><B>Dedicación a la Unidad:</B></TD>"&VbCrLf
 <TD class=TitDestacado>
</TD>"&VbCrLf
      </TR>"&VbCrLf
 <TR vAlign=top><TD colSpan=2>&nbsp;</TD></TR>"&VbCrLf	'Experiencia
 <TR vAlign=top> "&VbCrLf
  <TD class=TitDestacado colspan=2><B>Experiencia Docente:</B></TD>"&VbCrLf
 </TR>"&VbCrLf
 <TR vAlign=top> "&VbCrLf
 <TD class=TitDestacado colspan=2>"&RsContacto("Experiencia")&"</TD>"&VbCrLf
 </TR>"&VbCrLf
<TR vAlign=top><TD colSpan=2>&nbsp;</TD></TR>"&VbCrLf

			'Hoja de Vida
<TR vAlign=top> "&VbCrLf
 <TD class=TitDestacado  colspan=2><B>Resumen Hoja de Vida:</B></TD>"&VbCrLf
 </TR>"&VbCrLf
 <TR vAlign=top> "&VbCrLf
 <TD class=TitDestacado  colspan=2>"&RSContacto("HojaVida")&"</TD>"&VbCrLf
 </TR>"&VbCrLf
 <TR vAlign=top><TD colSpan=2>&nbsp;</TD></TR>"&VbCrLf

'Intereses
 <TR vAlign=top> "&VbCrLf
 <TD class=TitDestacado ><B>Experiencia e Intereses:</B></TD>"&VbCrLf
 </TR>"&VbCrLf
<TR vAlign=top> "&VbCrLf
 <TD class=TitDestacado  colspan=2>"&RSContacto("Intereses")&"</TD>"&VbCrLf
</TR>"&VbCrLf
<TR vAlign=top> "&VbCrLf
<TD class=TitDestacado><B>Fecha de Nacimiento:</B></TD>"&VbCrLf
 <TD class=TitDestacado>"&arrMes(Month(RsContacto("FechaNacimiento")))&" "&Day(RsContacto("FechaNacimiento"))&" de "&Year(RsContacto("FechaNacimiento"))&"</TD>"&VbCrLf
 </TR>"&VbCrLf
 <TR vAlign=top> "&VbCrLf
<TD class=TitDestacado><B>Lugar de Nacimiento:</B></TD>"&VbCrLf
 <TD class=TitDestacado>"&RsContacto("NomCiudad")&" ("&RsContacto("NomDepartamento")&")</TD>"&VbCrLf
</TR>"&VbCrLf
          <TR vAlign=top><TD colSpan=2>&nbsp;</TD></TR>"&VbCrLf
<TR vAlign=top><TD colSpan=2>&nbsp;</TD></TR>"&VbCrLf
<TR vAlign=top><TD colSpan=2 class='tituloDestacados'>Sectores a los que aplica</TD></TR>"&VbCrLf
<TR vAlign=top bgcolor='"&color(i mod 2)&"'><TD class='titulo'>Sector Principal:</TD><td>"&rsContacto("nomSector")&"</td></TR>"&VbCrLf
<TR vAlign=top bgcolor='"&color(i mod 2)&"'><TD class='titulo'>Experiencia:</TD><td>"&rsContacto("ExperienciaPrincipal")&"</td></TR>"&VbCrLf
<TR><TD><img src='g/giftransparente.gif' height='8'></td></TR>"&VbCrLf
<TR vAlign=top bgcolor='"&color(i mod 2)&"'><TD class='titulo'>Sector Secundario:</TD><td>"&rsContacto("nomSector")&"</td></TR>"&VbCrLf
<TR vAlign=top bgcolor='"&color(i mod 2)&"'><TD class='titulo'>Experiencia:</TD><td>"&rsContacto("ExperienciaSecundaria")&"</td></TR>"&VbCrLf
<TR><TD><img src='g/giftransparente.gif' height='8'></td></TR>"&VbCrLf
<TR vAlign=top bgcolor='"&color(i mod 2)&"'><TD colspan='2' class='titulo'>Otros Sectores</TD></TR>"&VbCrLf
 <TR vAlign=top bgcolor='"&color(i mod 2)&"'><td colspan='2'>"&rsContacto("nomSector")&"</td></TR>"&VbCrLf
    </div>
    </form>
</body>
</html>
