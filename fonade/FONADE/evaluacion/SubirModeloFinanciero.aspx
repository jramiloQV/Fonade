<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SubirModeloFinanciero.aspx.cs" Inherits="Fonade.FONADE.evaluacion.SubirModeloFinanciero"   MasterPageFile="~/Emergente.Master"  %>
<%@ Register Src="../../Controles/Alert.ascx" TagName="Alert" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="BodyContent"  runat="server" ContentPlaceHolderID="bodyContentPlace">


        <asp:Panel ID="PanelsubirArchivo" runat="server">

             <uc2:Alert ID="Alert1" runat="server" />

        <table width="570" border="0">
          <tr>
            <td colspan="2">
                <h1><asp:Label runat="server" ID="lbl_Titulo" style="font-weight: 700"></asp:Label></h1>
              </td>
            <td colspan="2" align="right">
                <asp:Label ID="l_fechaActual" runat="server" style="font-weight: 700"></asp:Label>
              </td>
          </tr>
        </table>

        <table width="570">
          <tr>
            <td class="auto-style1"><strong>Seleccione un archivo:</strong></td>
            <td>

                <asp:FileUpload ID="fu_archivo" runat="server" Width="380px" />

            </td>
          </tr>
          <tr>
            <td colspan="2" align="center" class="auto-style2">
                <asp:Button ID="btn_Cargar" runat="server" OnClick="btn_Cargar_Click" Text="Cargar Archivo" />
                &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btn_cerrar" runat="server" Text="Cerrar" OnClick="btn_cerrar_Click" />
              </td> 
          </tr>
          <tr>
            <td colspan="2" align="center">(Solo se debe subir el archivo &quot;modeloFinanciero.xls&quot; descargado desde esta página).</td>
          </tr>
        </table>
        </asp:Panel>
 
</asp:Content>
<asp:Content ID="Content1" runat="server" contentplaceholderid="head">
    <style type="text/css">
        .auto-style1
        {
            width: 143px;
        }
        .auto-style2
        {
            height: 55px;
        }
    </style>
</asp:Content>
