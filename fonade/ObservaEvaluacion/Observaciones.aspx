<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Observaciones.aspx.cs" 
    Inherits="Fonade.ObservaEvaluacion.Observaciones" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="Styles/ObservacionesStyle.css" rel="stylesheet" />
     <script>
      (function(i,s,o,g,r,a,m){i['GoogleAnalyticsObject']=r;i[r]=i[r]||function(){
      (i[r].q=i[r].q||[]).push(arguments)},i[r].l=1*new Date();a=s.createElement(o),
      m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m)
      })(window,document,'script','https://www.google-analytics.com/analytics.js','ga');

      ga('create', 'UA-85402368-1', 'auto');
      ga('send', 'pageview');
    </script>
    <!-- Facebook Pixel Code -->
    <script>
    !function(f,b,e,v,n,t,s){if(f.fbq)return;n=f.fbq=function(){n.callMethod?
    n.callMethod.apply(n,arguments):n.queue.push(arguments)};if(!f._fbq)f._fbq=n;
    n.push=n;n.loaded=!0;n.version='2.0';n.queue=[];t=b.createElement(e);t.async=!0;
    t.src=v;s=b.getElementsByTagName(e)[0];s.parentNode.insertBefore(t,s)}(window,
    document,'script','https://connect.facebook.net/en_US/fbevents.js');
    fbq('init', '541324449399250', {
    em: 'insert_email_variable,'
    });
    fbq('track', 'PageView');
    </script>
    <noscript><img height="1" width="1" style="display:none"
    src="https://www.facebook.com/tr?id=541324449399250&ev=PageView&noscript=1"
    /></noscript>
    <!-- DO NOT MODIFY -->
    <!-- End Facebook Pixel Code -->
</head>
<body>
    <form id="form1" runat="server">
    <div >
     <table style="font-family:Arial; font-size:smaller">
         <tr>
             <td>...</td>
             <td style="width:3px"></td>
             <td>
                 <table>
                     <tr>
                         <td><img src="icoInscripcion.jpg" /></td>
                         <td style="float:left; line-height: 90%"><span id="titleForm" style="clear:both; " class="tituloGrande">REGISTRAR <br />OBSERVACIONES<br />EVALUACIÓN</span></td>
                     </tr>
                 </table>
             </td>
             <td></td>
             <td></td>
         </tr>
         <tr>
             <td></td>
             <td></td>
             <td>
                 <span>Convocatoria:</span>
             </td>
             <td></td>
             <td>
                 <asp:DropDownList ID="ddlConvocatorias" runat="server" Width="300px" />
             </td>
         </tr>
         <tr>
             <td></td>
             <td></td>
             <td><span>No Plan de Negocio</span></td>
             <td></td>
             <td><asp:TextBox ID="txtNoPlanNegocio" runat="server" type="number" Width="300px" required /></td>
         </tr>
         <tr>
             <td></td>
             <td></td>
             <td><span>Nombre y Apellidos</span></td>
             <td></td>
             <td>
                 <asp:TextBox ID="txtNombres" runat="server" Width="300px" required />
             </td>
         </tr>
         <tr>
             <td></td><td></td>
             <td><span>Email Contacto</span></td>
             <td></td>
             <td>
                 <asp:TextBox ID="txtEmail" runat="server" type="email" Width="300px" required />
             </td>
         </tr>
         <tr>
             <td></td><td></td>
             <td><span>Perfil</span></td>
             <td></td>
             <td>
                 <asp:DropDownList ID="ddlPerfil" runat="server" Width="150px" />
             </td>
         </tr>
         <tr>
             <td></td><td></td>
             <td><span>Comentarios <br /> (Máximo 5000 Caracteres):</span></td>
             <td></td>
             <td><asp:TextBox ID="txtComentarios" runat="server" TextMode="MultiLine" Width="400px" Height="100px" MaxLength="5000" required /></td>
         </tr>
         <tr>
             <td></td>
             <td></td>
             <td></td>
             <td></td>
             <td>
                 <table style="text-align:right">
                     <tr>
                         <td><asp:Button ID="btnEnviar" Text="Enviar" CssClass="Boton" runat="server" OnClick="btnEnviar_Click" /></td>
                         <td><asp:Button ID="btnCancelar" Text="Cancelar" CssClass="Boton" runat="server" /></td>
                     </tr>
                 </table>
             </td>
         </tr>
     </table>
    </div>
    </form>
</body>
</html>
