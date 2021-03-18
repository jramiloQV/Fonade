<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EvaluacionConvocatoria.aspx.cs" Inherits="Fonade.FONADE.Convocatoria.EvaluacionConvocatoria" MasterPageFile="~/Master.Master" %>

<asp:Content ID="Content1" runat="server" contentplaceholderid="head">
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }
        .clasemas {
            color:#00468f;
            
        }
        .fondo {
            background-color:#00468f;
            color:white;
        }
        .panelmeses {
            margin:0px auto;
            text-align:left;
            width:100%;
        }
    </style>
    <script type="text/javascript">
        function ValidNum(e) {
            var tecla = document.all ? tecla = e.keyCode : tecla = e.which;
            return (tecla > 47 && tecla < 58);
        }
    </script>
     <script type="text/ecmascript">
         function url() {
             open("../Ayuda/Mensaje.aspx", "Proyección de ventas", "width=500,height=200");
         }
         function OpenPage(strPage) {
             //var ActivarVentana = document.getElementById("hidInsumo")
             //if (ActivarVentana.value == "1") {
             //window.open(strPage, 'Insumo', 'status:false;dialogWidth:900px;dialogHeight:700px')
             //window.open(strPage, 'Insumo', 'status:false;Width:900px;Height:700px')
             window.open(strPage, "_blank", "menubar=1,resizable=1,width=800,height=450,scrollbars=yes")
                 //ActivarVentana.value = 0
             //}
         }
    </script>
</asp:Content>

<asp:Content ID="BodyContent"  runat="server" ContentPlaceHolderID="bodyContentPlace">
    <table style="width:100%;">
            <tr>
                <td>&nbsp;</td>
            </tr>
        <tr>
            <td class="style11">
                <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/add.png" />
                <asp:Label ID="lblLink" runat="server" Text="Label"></asp:Label>
            </td>
        </tr>
            <tr>
                <td>
                    <div style="margin:0px auto; text-align:center;padding-right:2%;">
                    <asp:Panel ID="P_Observaciones" runat="server" Width="100%">

                        <asp:Table ID="T_Observaciones" runat="server" CssClass="panelmeses">
                            
                        </asp:Table>
                    </asp:Panel>
                        </div>
                </td>
            </tr>
        <tr>
            <td>
                <input id="TotalRows" type="hidden" runat="server" />
                <input id="CampoName" type="hidden" runat="server" />
                <input id="CamposMax" type="hidden" runat="server" />
               
                <br />
                <asp:Button ID="btnActualizat" runat="server" Text="Actualizar" OnClick="btnActualizat_Click" />
            </td>
        </tr>
            <tr>
                <td>&nbsp;</td>
            </tr>
        </table>
</asp:Content>