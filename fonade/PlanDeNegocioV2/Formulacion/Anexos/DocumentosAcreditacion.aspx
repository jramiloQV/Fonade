<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DocumentosAcreditacion.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.Anexos.DocumentosAcreditacion" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../../Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="../../../Scripts/ScriptsGenerales.js" type="text/javascript"></script>
    <link href="../../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <link href="../../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />
    <script src="../../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../../../Scripts/common.js" type="text/javascript"></script>
    <style type="text/css">
        html, body {
            background-color: #fff !important;
            background-image: none !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>Documentos de acreditación</h1>

            <asp:GridView ID="gw_DocumentosAcreditacion" runat="server" Width="100%" AutoGenerateColumns="false"
                CssClass="Grilla" OnRowCommand="gw_DocumentosAcreditacion_RowCommand" DataKeyNames="Id_Documento"
                ShowHeaderWhenEmpty="True" EmptyDataText='<%# Fonade.Negocio.Mensajes.Mensajes.GetMensaje(101)%>'>
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="3%" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:ImageButton ID="btn_Borrar" CommandName="Borrar" runat="server" ImageUrl="/Images/icoBorrar.gif"
                                Visible="false" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Archivo" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:LinkButton ID="hlnk_url" runat="server" CausesValidation="false" CommandName="VerDocumento" 
                                CommandArgument='<%# ConfigurationManager.AppSettings.Get("DirVirtual") + Eval("Ruta") %>'>
                                <asp:Image ID="img_Url" runat="server" ImageUrl="~/Images/IcoDocNormal.gif" />
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="TipoArchivo" HeaderText="Tipo" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" ItemStyle-HorizontalAlign="Left" />                   
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>
