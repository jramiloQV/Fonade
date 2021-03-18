<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccesoDenegado.aspx.cs" 
    Inherits="Fonade.FONADE.evaluacion.AccesoDenegado" MasterPageFile="~/Master.Master" %>

<asp:Content ID="Content1" runat="server" contentplaceholderid="head">
    <style type="text/css">
        .denegado{
            width:100%;
            height:auto;
            text-align:center;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">

    <div class="denegado">
        <img src="../../Images/Advertencia.gif"><br>
        <asp:button id="backButton" runat="server" text="Volver" 
OnClientClick="JavaScript:window.history.back(1);return false;"></asp:button>
    </div>

</asp:Content>

