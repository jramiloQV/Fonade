<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="OpcionesGerenciales.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Administracion.Operador.OpcionesGerenciales" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <h1>
        <asp:Label runat="server" ID="lbl_Titulo" Text="Opciones Gerenciales" /></h1>
    <hr />
    <table style="width: 100%;">
        <tr>
            <td>
                <img src="../../../Images/ImgFlechaTit.gif" />
                <asp:HyperLink ID="hl1"
                    NavigateUrl="/FONADE/AdministrarPerfiles/Convenios/CatalogoTexto.aspx"
                    Text="Administración de mensajes"
                    runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <img src="../../../Images/ImgFlechaTit.gif" />
                <asp:HyperLink ID="hl2"
                    NavigateUrl="/FONADE/AdministrarPerfiles/AdministrarUsuarios.aspx?codGrupo=9"
                    Text="Crear Gerente Evaluador"
                    runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <img src="../../../Images/ImgFlechaTit.gif" />
                <asp:HyperLink ID="hl3"
                    NavigateUrl="/FONADE/AdministrarPerfiles/AdministrarUsuarios.aspx?codGrupo=19"
                    Text="Crear Acreditador"
                    runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <img src="../../../Images/ImgFlechaTit.gif" />
                <asp:HyperLink ID="hl4"
                    NavigateUrl="/FONADE/AdministrarPerfiles/AdministrarUsuarios.aspx?codGrupo=8"
                    Text="Crear Call Center"
                    runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <img src="../../../Images/ImgFlechaTit.gif" />
                <asp:HyperLink ID="hl20"
                    NavigateUrl="/FONADE/AdministrarPerfiles/AdministrarUsuarios.aspx?codGrupo=20"
                    Text="Crear Call Center Operador"
                    runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <img src="../../../Images/ImgFlechaTit.gif" />
                <asp:HyperLink ID="hl5"
                    NavigateUrl="/FONADE/AdministrarPerfiles/AdministrarUsuarios.aspx?codGrupo=12"
                    Text="Crear Gerente Interventor"
                    runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <img src="../../../Images/ImgFlechaTit.gif" />
                <asp:HyperLink ID="hl6"
                    NavigateUrl="/FONADE/AdministrarPerfiles/Convenios/CatalogoCriterioPriorizacion.aspx"
                    Text="Criterios de Priorización"
                    runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <img src="../../../Images/ImgFlechaTit.gif" />
                <asp:HyperLink ID="hl7"
                    NavigateUrl="/FONADE/AdministrarPerfiles/AdministrarUsuarios.aspx?codGrupo=15"
                    Text="Crear Perfil Fiduciaria"
                    runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <img src="../../../Images/ImgFlechaTit.gif" />
                <asp:HyperLink ID="hl8"
                    NavigateUrl="/FONADE/AdministrarPerfiles/Convenios/CatalogoConvenios.aspx"
                    Text="Convenios"
                    runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <img src="../../../Images/ImgFlechaTit.gif" />
                <asp:HyperLink ID="hl9"
                    NavigateUrl="/FONADE/Priorizacion_de_Proyectos/ProyectosPriorizacion.aspx"
                    Text="Priorización de Proyectos"
                    runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <img src="../../../Images/ImgFlechaTit.gif" />
                <asp:HyperLink ID="hl10"
                    NavigateUrl="/FONADE/Priorizacion_de_Proyectos/CatalogoActaAsignacionRecursos.aspx"
                    Text="Acta de Asignación de Recursos"
                    runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <img src="../../../Images/ImgFlechaTit.gif" />
                <asp:HyperLink ID="hl11"
                    NavigateUrl="/FONADE/Convocatoria/CatalogoConvocatoria.aspx"
                    Text="Crear Convocatoria"
                    runat="server" /></td>
        </tr>
    </table>
</asp:Content>
