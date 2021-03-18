<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrameInterventorEmpresas.aspx.cs"
    Inherits="Fonade.FONADE.interventoria.FrameInterventorEmpresas" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>FONDO EMPRENDER - Asignar Interventor</title>
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function RadioCheck(rb) {
            var gv = document.getElementById("<%=gv_SubDetalles_interventores.ClientID%>");
            var rbs = gv.getElementsByTagName("input");

            var row = rb.parentNode.parentNode;
            for (var i = 0; i < rbs.length; i++) {
                if (rbs[i].type == "radio") {
                    if (rbs[i].checked && rbs[i] != rb) {
                        rbs[i].checked = false;
                        break;
                    }
                }
            }
        }
    </script>
    <style type="text/css">
        html, body {
            background-color: #fff !important;
            background-image: none !important;
        }

        #gv_sectores_encontrados {
            overflow: auto;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table border="0" cellpadding="0" cellspacing="5">
                <tr>
                    <%--ListaProyectosIntervenidos--%>
                    <td valign="top" style="width: 50%;">
                        <div style="text-align: center; font-weight: bold;">
                            EMPRESAS
                        </div>
                        <p>
                            <span>Sector:</span>
                        </p>
                        <asp:DropDownList ID="dd_Sectores" runat="server" AutoPostBack="true" Width="100%"
                            OnSelectedIndexChanged="dd_Sectores_SelectedIndexChanged" />
                        <asp:Panel ID="pnl_seleccione_sector" runat="server" Visible="false">
                            <asp:Label ID="lbl_seleccione_sector" Text="Seleccione un sector de la lista" runat="server" />
                        </asp:Panel>
                        <asp:GridView ID="gv_sectores_encontrados" runat="server" AutoGenerateColumns="false" Width="100%"
                            GridLines="None"  OnRowCommand="gv_sectores_encontrados_RowCommand"
                            CssClass="Grilla">
                            <EmptyDataTemplate>
                                No hay Empresas en ninguna convocatoria para el sector seleccionado
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:BoundField HeaderText="" DataField="Id_Sector" Visible="false" />
                                <asp:BoundField HeaderText="" DataField="codproyecto" Visible="false" />
                                <asp:BoundField HeaderText="" DataField="razonsocial" Visible="false" />
                                <asp:BoundField HeaderText="" DataField="CodSubSector" Visible="false" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Image ID="img_SinEmpresa" ImageUrl="/Images/admiracion.gif" runat="server" Visible="false"
                                            AlternateText="Plan de negocio sin interventor." />
                                        <asp:LinkButton ID="lnk_btn_sector_seleccionar" Text='<%# Eval("razonsocial") %>' runat="server" CausesValidation="false"
                                            CommandName="mostrar" CommandArgument='<%# Eval("Id_Sector")+ ";" + Eval("codproyecto")+ ";" +Eval("razonsocial")  %>'
                                            Style="text-decoration: none;" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                    <%--ListaInterventor--%>
                    <td valign="top" style="width: 50%; margin-top: 25px;">
                        <asp:Panel ID="pnl_info" runat="server">
                            <asp:Label ID="lbl_info" Text="Para ver el interventor de una Empresa, seleccione uno a la <br/>izquierda."
                                runat="server" Font-Bold="true" />
                        </asp:Panel>
                        <asp:Label ID="lbl_info_detalles_sector" Text="" runat="server" Visible="false" />
                        <asp:GridView ID="gv_detalles_interventor" runat="server" AutoGenerateColumns="false"
                            CssClass="Grilla" Visible="false" Width="360px">
                            <Columns>
                                <asp:BoundField HeaderText="" DataField="Id_Contacto" Visible="false" />
                                <asp:BoundField HeaderText="interventor Líder" DataField="Nombres" HeaderStyle-Width="150px" />
                                <asp:TemplateField HeaderText="Email interventor" HeaderStyle-Width="200px">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hlnk_mailto" runat="server" NavigateUrl='<%#"mailto:"+ Eval("Email") %>'
                                            Text='<%# Eval("Email") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:GridView ID="gv_interventoresCreados" runat="server" AutoGenerateColumns="false"
                            Visible="false" CssClass="Grilla" Width="360px">
                            <Columns>
                                <asp:BoundField HeaderText="" DataField="Id_Contacto" Visible="false" />
                                <asp:TemplateField HeaderText="interventor" HeaderStyle-Width="150px">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_interv" Text='<%# Eval("DT_FULLNAME")%>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Email interventor" HeaderStyle-Width="200px">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hlnk_mailto_interv" runat="server" NavigateUrl='<%#"mailto:"+ Eval("Email") %>'
                                            Text='<%# Eval("Email") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <br />
                        <center>
                            <asp:LinkButton ID="lnkbtn_asignarinterventor" Text=">> ASIGNACIÓN DE INTERVENTOR <<"
                                Visible="false" runat="server" ForeColor="Red" Style="text-decoration: none;"
                                OnClick="lnkbtn_asignarinterventor_Click" />
                        </center>
                        <asp:GridView ID="gv_SubDetalles_interventores" runat="server" AutoGenerateColumns="false"
                            CssClass="Grilla" DataKeyNames="Id_Contacto" OnRowCommand="gv_SubDetalles_interventores_RowCommand"
                            OnRowDataBound="gv_SubDetalles_interventores_RowDataBound" Width="360px">
                            <Columns>
                                <asp:BoundField HeaderText="" DataField="Id_Contacto" Visible="false" />
                                <asp:TemplateField HeaderText="Interventor">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdf_contacto" runat="server" Value='<%# Eval("Id_Contacto") %>' />
                                        <asp:HiddenField ID="hdf_inactivo_inter" runat="server" Value='<%# Eval("Inactivo") %>' />
                                        <asp:CheckBox ID="chk_objeto" runat="server" ForeColor="Black" />
                                        <asp:LinkButton ID="lnk_acreditador" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Id_Contacto")+ ";" + Eval("Nombres")+ " " + Eval("Apellidos")  %>'
                                            CommandName="mostrar_acreditador" Text='<%# Eval("Nombres")+ " " + Eval("Apellidos")  %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Interventor Líder" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:RadioButton ID="rb_interv_lider" runat="server" onclick="RadioCheck(this);" />
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#Eval("Id_Contacto")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:Panel ID="pnl_btn_actualizar" runat="server" HorizontalAlign="Right" Visible="false">
                            <asp:HiddenField ID="hdf_IdEmpresa" runat="server" Visible="false" />
                            <asp:HiddenField ID="hdf_RazonSocial" runat="server" Visible="false" />
                            <asp:HiddenField ID="hdf_Nit" runat="server" Visible="false" />
                            <asp:HiddenField ID="hdf_ObjSocial" runat="server" Visible="false" />
                            <asp:Button ID="btn_Actualizar_Interventores" Text="Actualizar" runat="server" Visible="false"
                                OnClick="btn_Actualizar_Interventores_Click" />
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
