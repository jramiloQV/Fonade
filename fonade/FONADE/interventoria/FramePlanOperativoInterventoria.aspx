<%@ Page Title="FONDO EMPRENDER" Language="C#" AutoEventWireup="true" CodeBehind="FramePlanOperativoInterventoria.aspx.cs"
    Inherits="Fonade.FONADE.interventoria.FramePlanOperativoInterventoria" %>

<%@ Register Src="~/Controles/Post_It.ascx" TagPrefix="uc1" TagName="Post_It" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.9.1.js" type="text/javascript"></script>
    <link href="../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../../Scripts/common.js" type="text/javascript"></script>
    <script type="text/javascript">
        function alerta() {
            return confirm('¿Está seguro que desea eliminar la actividad seleccionada?');
        }
        function borrar() {
            return confirm('¿Esta seguro que desea borrar el avance seleccionado?');
        }
    </script>
    <style type="text/css">
        
        /*table.Grilla tr:nth-child(3)
        {
            display: none !important;
        }*/
        
        #t_anexos tbody tr 
        {
            white-space:nowrap;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:Panel ID="pnlPrincipal" Visible="true" runat="server">
        <table>
            <tr>
                <td align="left">
                    <div class="help_container">
                        <div>
                            <a onclick="textoAyuda({titulo: 'Plan Operativo', texto: 'PlanOperativo'});">
                                <img src="../../Images/imgAyuda.gif" border="0" alt="help_infraestructura"></img>
                                Plan Operativo: </a>
                        </div>
                    </div>
                </td>
                <td align="left" colspan="3">
                </td>
                <%--Mauricio Arias Olave. "21/04/2014": En el clásico NO está, por ello, se comenta este código.--%>
                <td align="right">
                    <uc1:Post_It ID="Post_It2" runat="server" _txtCampo="PlanOperativo" _txtTab="1" _mostrarPost="true"/>
                </td>
            </tr>
        </table>
        <br />
        <div id="divadicionar" runat="server">
            <table style="width: 100%">
                <tr>
                    <td style="width: 250px">
                        <asp:Label ID="lblvalidador" runat="server" Style="display: none" />
                        <asp:ImageButton ID="Adicionar" runat="server" ImageUrl="../../Images/icoAdicionarUsuario.gif"
                            Style="cursor: pointer;" OnClick="Adicionar_Click" />
                        &nbsp;
                        <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click" Visible="False"> Adicionar Actividad al Plan Operativo</asp:LinkButton>
                    </td>
                    <td>
                        <asp:LinkButton ID="Pagos_actividad" runat="server" OnClick="Pagos_actividad_Click">Pagos</asp:LinkButton>
                    </td>
                    <td>
                        <asp:LinkButton ID="lnkImprimir" runat="server" OnClick="lnkImprimir_Click" >Imprimir</asp:LinkButton>
                    </td>
                    <td>
                       <asp:Label runat="server" ID="lblpuestosPendientesConteo" Text="Actividades Pendientes de Aprobar: 2" />
                    </td>   
                </tr>
            </table>
        </div>
        <br />
        <table width='100%' align="Center" border='0' cellpadding='0' cellspacing='0'>
            <tr>
                <td align='left' valign='top' width='98%'>
                    <table width='100%' border='0' cellspacing='1' cellpadding='4'>
                    </table>
                    <table cellpadding="0px" cellspacing="0px">
                        <tr>
                            <td valign="Top" style="height: 590px !important;">
                                <div style="height: auto !important; width: 420px; overflow: auto; overflow-x: hidden;
                                    border-right: silver 1px solid">
                                    <asp:GridView ID="gw_Anexos" runat="server" Width="400px" AutoGenerateColumns="false"
                                        CssClass="Grilla" OnPageIndexChanging="GwAnexosPageIndexChanging" OnRowCommand="GwAnexosRowCommand"
                                        OnRowDataBound="GwAnexosRowDataBound">
                                        <Columns>
                                            <asp:TemplateField>

                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkeliminar" runat="server" OnClientClick="return alerta()" CommandArgument='<%# Eval("id_Actividad")+ ";" + Eval("Nomactividad")+ ";" + Eval("Item") + ";" + Eval("Metas") %>'
                                                        CommandName="eliminar" CausesValidation="false">
                                                        <asp:Label runat="server" ID="lblactividaPOI" Visible="False" Text='<%# Eval("ActividadPo") %>' />
                                                        <asp:Image ID="imgeditar" ImageUrl="../../Images/icoBorrar.gif" runat="server" Style="cursor: pointer;"
                                                            actividad='<%# Eval("id_Actividad") %>' proyecto='<%= CodProyecto %>'></asp:Image>
                                                    </asp:LinkButton></ItemTemplate></asp:TemplateField><asp:BoundField DataField="Item" HeaderText="Item" />

                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkmostrar" runat="server" CausesValidation="False" CommandArgument='<%# Eval("id_Actividad")  + ";" + Eval("Nomactividad") %>'
                                                        CommandName="mostrar" Text="Mostrar" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Actividad">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkeditar" runat="server" CausesValidation="False" CommandArgument='<%# Eval("id_Actividad") %>'
                                                        CommandName="editar" Text='<%# Eval("Nomactividad") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerStyle CssClass="Paginador" />
                                    </asp:GridView>
                                    <br />
                                    <asp:Panel runat="server" ID="pnlActividades">
                                        <asp:GridView ID="GrvActividadesNoAprovadas" runat="server" Width="400px" AutoGenerateColumns="False"
                                            OnRowCommand="GrvActividadesNoAprovadasRowCommand" BackColor="White" BorderColor="#CC9966"
                                            BorderStyle="None" BorderWidth="1px" CellPadding="4" OnPageIndexChanging="GrvActividadesNoAprovadas_PageIndexChanging"><Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Item" HeaderText="Item" />
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Actividades en Aprobación">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkactividad" runat="server" CausesValidation="False" CommandArgument='<%# Eval("id_Actividad") %>'
                                                            CommandName="editar" Text='<%# Eval("Nomactividad") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <FooterStyle BackColor="#FFFFCC" ForeColor="#330099" />
                                            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="#FFFFCC" />
                                            <RowStyle BackColor="White" ForeColor="#330099" />
                                            <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="#663399" />
                                            <SortedAscendingCellStyle BackColor="#FEFCEB" />
                                            <SortedAscendingHeaderStyle BackColor="#AF0101" />
                                            <SortedDescendingCellStyle BackColor="#F6F0C0" />
                                            <SortedDescendingHeaderStyle BackColor="#7E0000" />
                                            <PagerStyle CssClass="Paginador" />
                                        </asp:GridView>
                                    </asp:Panel>
                                </div>
                            </td>
                            <td valign="Top">

                                <div style="width: 438px; height: 5px !important;">
                                    <table id="tabla_meses" class="Grilla" cellpadding="0" cellspacing="0" width="500px"
                                        style="content:box; align-content:initial; border: solid  white 1px;">
                                        <tr><th style="width: 260px; text-align: center"><asp:Label ID="lblnomactividad" 
                                                    runat="server" /></th></tr></table>
                                     
                                    <asp:GridView ID="gw_AnexosActividad" runat="server" Width="3380px" AutoGenerateColumns="False"
                                        CssClass="Grilla" RowStyle-Height="35px" ShowFooter="true" OnRowDataBound="gw_AnexosActividad_RowDataBound"
                                        OnRowCommand="gw_AnexosActividad_RowCommand" ><Columns>
                                            <asp:TemplateField HeaderText="Fondo" ItemStyle-Width="130px">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="fondo1" />
                                                    <asp:Label runat="server" ID="fondo1F" Visible="False" />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:ImageButton ID="imgAvance1" runat="server" ImageUrl="~/Images/icoAdicionarUsuario.gif" />
                                                    <asp:LinkButton ID="lnkactividad1" runat="server" CausesValidation="False" CommandName="editar"
                                                        Text='Ver Avance' />
                                                </FooterTemplate>
                                                <ItemStyle Width="130px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Emprendedor">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label1" runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle Width="130px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Fondo">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label2" runat="server" />
                                                    <asp:Label ID="fondo2F" runat="server" Visible="False" />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:ImageButton ID="imgAvance2" runat="server" ImageUrl="~/Images/icoAdicionarUsuario.gif" />
                                                    <asp:LinkButton ID="lnkactividad2" runat="server" CausesValidation="False" CommandName="editar"
                                                        Text='Ver Avance' />
                                                </FooterTemplate>
                                                <ItemStyle Width="130px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Emprendedor">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label3" runat="server"></asp:Label></ItemTemplate><ItemStyle Width="130px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Fondo">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label4" runat="server"></asp:Label><asp:Label ID="fondo3F" runat="server"
                                                        Visible="False" />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:ImageButton ID="imgAvance3" runat="server" ImageUrl="~/Images/icoAdicionarUsuario.gif" />
                                                    <asp:LinkButton ID="lnkactividad3" runat="server" CausesValidation="False" CommandName="editar"
                                                        Text='Ver Avance' />
                                                </FooterTemplate>
                                                <ItemStyle Width="130px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Emprendedor">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label5" runat="server"></asp:Label></ItemTemplate><ItemStyle Width="130px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Fondo">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label6" runat="server"></asp:Label><asp:Label ID="fondo4F" runat="server"
                                                        Visible="False" />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:ImageButton ID="imgAvance4" runat="server" ImageUrl="~/Images/icoAdicionarUsuario.gif" />
                                                    <asp:LinkButton ID="lnkactividad4" runat="server" CausesValidation="False" CommandName="editar"
                                                        Text='Ver Avance' />
                                                </FooterTemplate>
                                                <ItemStyle Width="130px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Emprendedor">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label7" runat="server"></asp:Label></ItemTemplate><ItemStyle Width="130px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Fondo">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label8" runat="server"></asp:Label><asp:Label ID="fondo5F" runat="server"
                                                        Visible="False" />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:ImageButton ID="imgAvance5" runat="server" ImageUrl="~/Images/icoAdicionarUsuario.gif" />
                                                    <asp:LinkButton ID="lnkactividad5" runat="server" CausesValidation="False" CommandName="editar"
                                                        Text='Ver Avance' />
                                                </FooterTemplate>
                                                <ItemStyle Width="130px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Emprendedor">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label9" runat="server"></asp:Label></ItemTemplate><ItemStyle Width="130px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Fondo">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label10" runat="server" />
                                                    <asp:Label ID="fondo6F" runat="server" Visible="False" />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:ImageButton ID="imgAvance6" runat="server" ImageUrl="~/Images/icoAdicionarUsuario.gif" />
                                                    <asp:LinkButton ID="lnkactividad6" runat="server" CausesValidation="False" CommandName="editar"
                                                        Text='Ver Avance' />
                                                </FooterTemplate>
                                                <ItemStyle Width="130px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Emprendedor">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox11" runat="server"></asp:TextBox></EditItemTemplate><ItemTemplate>
                                                    <asp:Label ID="Label11" runat="server"></asp:Label></ItemTemplate><ItemStyle Width="130px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Fondo">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label12" runat="server"></asp:Label><asp:Label ID="fondo7F" runat="server"
                                                        Visible="False" />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:ImageButton ID="imgAvance7" runat="server" ImageUrl="~/Images/icoAdicionarUsuario.gif" />
                                                    <asp:LinkButton ID="lnkactividad7" runat="server" CausesValidation="False" CommandName="editar"
                                                        Text='Ver Avance' />
                                                </FooterTemplate>
                                                <ItemStyle Width="130px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Emprendedor">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label13" runat="server"></asp:Label></ItemTemplate><ItemStyle Width="130px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Fondo">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label14" runat="server"></asp:Label><asp:Label ID="fondo8F" runat="server"
                                                        Visible="False" />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:ImageButton ID="imgAvance8" runat="server" ImageUrl="~/Images/icoAdicionarUsuario.gif" />
                                                    <asp:LinkButton ID="lnkactividad8" runat="server" CausesValidation="False" CommandName="editar"
                                                        Text='Ver Avance' />
                                                </FooterTemplate>
                                                <ItemStyle Width="130px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Emprendedor">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label15" runat="server"></asp:Label></ItemTemplate><ItemStyle Width="130px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Fondo">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label16" runat="server"></asp:Label><asp:Label ID="fondo9F" runat="server"
                                                        Visible="False" />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:ImageButton ID="imgAvance9" runat="server" ImageUrl="~/Images/icoAdicionarUsuario.gif" />
                                                    <asp:LinkButton ID="lnkactividad9" runat="server" CausesValidation="False" CommandName="editar"
                                                        Text='Ver Avance' />
                                                </FooterTemplate>
                                                <ItemStyle Width="130px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Emprendedor">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label17" runat="server"></asp:Label></ItemTemplate><ItemStyle Width="130px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Fondo">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label18" runat="server"></asp:Label><asp:Label ID="fondo10F" runat="server"
                                                        Visible="False" />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:ImageButton ID="imgAvance10" runat="server" ImageUrl="~/Images/icoAdicionarUsuario.gif" />
                                                    <asp:LinkButton ID="lnkactividad10" runat="server" CausesValidation="False" CommandName="editar"
                                                        Text='Ver Avance' />
                                                </FooterTemplate>
                                                <ItemStyle Width="130px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Emprendedor">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label19" runat="server"></asp:Label></ItemTemplate><ItemStyle Width="130px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Fondo">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label20" runat="server"></asp:Label><asp:Label ID="fondo11F" runat="server"
                                                        Visible="False" />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:ImageButton ID="imgAvance11" runat="server" ImageUrl="~/Images/icoAdicionarUsuario.gif" />
                                                    <asp:LinkButton ID="lnkactividad11" runat="server" CausesValidation="False" CommandName="editar"
                                                        Text='Ver Avance' />
                                                </FooterTemplate>
                                                <ItemStyle Width="130px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Emprendedor">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label21" runat="server"></asp:Label></ItemTemplate><ItemStyle Width="130px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Fondo">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label22" runat="server"></asp:Label><asp:Label ID="fondo12F" runat="server"
                                                        Visible="False" />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:ImageButton ID="imgAvance12" runat="server" ImageUrl="~/Images/icoAdicionarUsuario.gif" />
                                                    <asp:LinkButton ID="lnkactividad12" runat="server" CausesValidation="False" CommandName="editar"
                                                        Text='Ver Avance' />
                                                </FooterTemplate>
                                                <ItemStyle Width="130px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Emprendedor">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox23" runat="server"></asp:TextBox></EditItemTemplate><ItemTemplate>
                                                    <asp:Label ID="Label23" runat="server"></asp:Label></ItemTemplate><ItemStyle Width="130px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Fondo">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label24" runat="server"></asp:Label><asp:Label ID="fondo13F" runat="server"
                                                        Visible="False" />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:ImageButton ID="imgAvance13" runat="server" ImageUrl="~/Images/icoAdicionarUsuario.gif" />
                                                    <asp:LinkButton ID="lnkactividad13" runat="server" CausesValidation="False" CommandName="editar"
                                                        Text='Ver Avance' />
                                                </FooterTemplate>
                                                <ItemStyle Width="130px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Emprendedor">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox24" runat="server"></asp:TextBox></EditItemTemplate><ItemTemplate>
                                                    <asp:Label ID="Label25" runat="server"></asp:Label></ItemTemplate><ItemStyle Width="130px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Fondo">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label26" runat="server"></asp:Label><asp:Label ID="fondo14F" runat="server"
                                                        Visible="False" />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:ImageButton ID="imgAvance14" runat="server" ImageUrl="~/Images/icoAdicionarUsuario.gif" />
                                                    <asp:LinkButton ID="lnkactividad14" runat="server" CausesValidation="False" CommandName="editar"
                                                        Text='Ver Avance' />
                                                </FooterTemplate>
                                                <ItemStyle Width="130px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Emprendedor">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox25" runat="server"></asp:TextBox></EditItemTemplate><ItemTemplate>
                                                    <asp:Label ID="Label27" runat="server"></asp:Label></ItemTemplate><ItemStyle Width="130px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Fondo">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox28" runat="server"></asp:TextBox></EditItemTemplate><ItemTemplate>
                                                    <asp:Label ID="Label28" runat="server"></asp:Label></ItemTemplate><ItemStyle Width="130px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Emprendedor">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox29" runat="server"></asp:TextBox></EditItemTemplate><ItemTemplate>
                                                    <asp:Label ID="Label29" runat="server"></asp:Label></ItemTemplate><ItemStyle Width="130px" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <RowStyle Height="35px" />
                                    </asp:GridView>

                                    <br />
                                    <asp:Table ID="t_anexos" runat="server" Width="100%" Visible="true" BorderWidth="0" CellPadding="4"
                                        CellSpacing="1" CssClass="Grilla">
                                    </asp:Table>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </asp:Panel>
    </form>
</body>
</html>
