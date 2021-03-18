<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="ParametricasInterventoria.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Administracion.Interventoria.ParametricasInterventoria" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
     <h1>
        <asp:Label runat="server" ID="lbl_Titulo" Text="Parametricas de Interventoria" /></h1>
    <hr />
    <table style="width:100%;">
        <tr>
            <td>
                 <img src="../../../Images/ImgFlechaTit.gif" />
                <asp:HyperLink ID="hl1"
                    NavigateUrl="/Fonade/Interventoria/CatalogoTipoAmbito.aspx"
                    Text="Tipo de ámbito"
                    runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                 <img src="../../../Images/ImgFlechaTit.gif" />
                <asp:HyperLink ID="HyperLink1"
                    NavigateUrl="/Fonade/Interventoria/CatalogoAmbito.aspx"
                    Text="Ambito"
                    runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                 <img src="../../../Images/ImgFlechaTit.gif" />
                <asp:HyperLink ID="HyperLink2"
                    NavigateUrl="/Fonade/Interventoria/CatalogoTipoVariable.aspx"
                    Text="Factor evaluación"
                    runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                 <img src="../../../Images/ImgFlechaTit.gif" />
                <asp:HyperLink ID="HyperLink3"
                    NavigateUrl="/Fonade/Interventoria/CatalogoVariable.aspx"
                    Text="Variable"
                    runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                 <img src="../../../Images/ImgFlechaTit.gif" />
                <asp:HyperLink ID="HyperLink4"
                    NavigateUrl="/Fonade/Interventoria/CatalogoTipoCriterio.aspx"
                    Text="Criterio"
                    runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                 <img src="../../../Images/ImgFlechaTit.gif" />
                <asp:HyperLink ID="HyperLink5"
                    NavigateUrl="/Fonade/Interventoria/CatalogoCriterioDetalle.aspx"
                    Text="Cumplimiento a Verificar"
                    runat="server" />
            </td>
        </tr>
    </table>

</asp:Content>
