<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Contrato.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Ejecucion.Contrato.Contrato" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
     <link href="~/Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <link href="../../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />
    <script src="../../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>   
    <script src="../../../Scripts/jquery-ui-1.8.21.custom.min.js" type="text/javascript"></script>
    <script src="../../../Scripts/common.js" type="text/javascript"></script>    
    <style type="text/css">
        html, body {
            background-image: none !important;
        }

        table {
            width: 100%;
        }

        .title {
            font-size: 16px;
        }

        input.boton_Link_Grid_2 {
            width: 0px;
            height: 0px;
            border: 0px;
            background-image: none;
            background: none;
            text-decoration: underline;
            cursor: pointer;
            padding: 0px;
        }

            input.boton_Link_Grid_2:hover {
                width: 0px;
                height: 0px;
                border: 0px;
                text-decoration: none;
                background-image: none;
                background: none;
                padding: 0px;
                background: none !important;
            }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <%Page.DataBind(); %>
        <div class="ContentInfo" style="width: 995px; height: 820px;">
            <br>
            <div>
                <asp:Panel ID="panexosagre" runat="server">
                    <h1>
                        <label>
                            INFORMACIÓN DEL CONTRATO</label>
                    </h1>
                    <br />
                    <table class="Grilla">
                        <tr>
                            <td style="width: 25%;">No Contrato de Colaboración Empresarial:
                            </td>
                            <td style="width: 25%;">
                                <asp:Label ID="lblNumContrato" runat="server"></asp:Label>
                            </td>
                            <td style="width: 25%;">Plazo en meses del ctto:
                            </td>
                            <td style="width: 25%;">
                                <asp:Label ID="lblplazoMeses" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Fecha de Acta de Inicio:
                            </td>
                            <td>
                                <asp:Label ID="lblFechaActa" runat="server"></asp:Label>
                            </td>
                            <td>Numero del ap presupuestal:
                            </td>
                            <td>
                                <asp:Label ID="lblNumAppresupuestal" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Objeto:
                            </td>
                            <td colspan="3">
                                <asp:Label ID="lblObjeto" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Fecha del ap:
                            </td>
                            <td>
                                <asp:Label ID="lblFechaAp" runat="server"></asp:Label>
                            </td>
                            <td>Fecha Firma Del Contrato:
                            </td>
                            <td>
                                <asp:Label ID="lblFechaFirmaContrato" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>No Póliza de Seguro de Vida:
                            </td>
                            <td>
                                <asp:Label ID="lblPolizaSeguro" runat="server"></asp:Label>
                            </td>
                            <td>Compañía Seguro de Vida:
                            </td>
                            <td>
                                <asp:Label ID="lblCompaniaSeguroVida" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Valor inicial en pesos:
                            </td>
                            <td>
                                <asp:Label ID="lblValorInicial" runat="server"></asp:Label>
                            </td>
                            <td colspan="2"></td>
                        </tr>
                    </table>
                    <p>
                        Cargue de archivos adjuntos :
                    </p>
                    <p>
                        <asp:Button ID="lnkSuberArchivo" runat="server" Text="Cargar un archivo" OnClick="lnkSuberArchivo_Click1" CssClass="boton_Link_Grid"></asp:Button>
                    </p>
                    <p>
                        Listado de archivos Adjuntos:
                    </p>
                    <p>
                    </p>
                    <p>
                        <div style="width:800px; height:350px; overflow-y: scroll">
                        <asp:DataList ID="DataList1" runat="server" OnSelectedIndexChanged="btn_Click" Width="70%" Height="50%">
                            <ItemTemplate>
                                <asp:Button ID="Button2" runat="server" CssClass="boton_Link_Grid" OnClick="btn_Click" Text="Eliminar " OnClientClick="var mnb = confirm('¿ Eliminar archivo adjunto  ?'); return mnb;" CommandArgument='<%# Eval("ruta") %>' Visible='<%# Convert.ToBoolean(DataBinder.GetPropertyValue(this, "perfilVisibilidad")) %>' />
                                <asp:Button ID="Button1" runat="server" CssClass="boton_Link_Grid" OnClick="btn_Click" Text='<%# Eval("NombreArchivo") %>' Enabled="false" />
                                <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/Images/buscarrr.png" Width="20px" OnClientClick='<%# string.Format("window.open(|{0}|)", Eval("filePath","http://www.fondoemprender.com:8080/{0}").ToString()).Replace(char.ConvertFromUtf32(124), char.ConvertFromUtf32(39))%>' ImageAlign="Right" />
                            </ItemTemplate>
                        </asp:DataList>
                            </div>
                    </p>
                    <br />
                </asp:Panel>
                <asp:Panel ID="Adjunto" runat="server">
                    <h3 class="title">Carga Archivo</h3>
                    <table>
                        <tr>
                            <td>Seleccione el archivo:
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblErrorDocumento" runat="server" CssClass="failureNotification"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:FileUpload ID="fuArchivo" runat="server" Width="240px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnSubirDocumento" runat="server" Text="Cargar Archivo" OnClick="btnSubirDocumento_Click" />
                                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>Por favor use click en examinar para escoger los archivos Adjuntos
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
        </div>
    </form>
</body>
</html>
