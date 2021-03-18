<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ErrorPage.aspx.cs" Inherits="Fonade.Account.ErrorPage" MasterPageFile="~/Master.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="BodyContent"  runat="server" ContentPlaceHolderID="bodyContentPlace">
    <span class="style10"><strong>ERROR AL ACCEDER A LA PÁGINA.

    <br />
    COMUNÍQUESE CON SU ADMINISTRADOR DE SERVIDOR.</strong></span>  
</asp:Content>

<asp:Content ID="Content1" runat="server" contentplaceholderid="head">
    <style type="text/css">
        .style10
        {
            font-size: large;
        }
    </style>
</asp:Content>


