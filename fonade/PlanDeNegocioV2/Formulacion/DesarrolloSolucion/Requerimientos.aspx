<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Requerimientos.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.DesarrolloSolucion.Requerimientos" EnableEventValidation="false" %>

<%@ Register Src="~/PlanDeNegocioV2/Formulacion/Controles/Encabezado.ascx" TagPrefix="uc1" TagName="Encabezado" %>
<%@ Register Src="../../../Controles/Post_It.ascx" TagName="Post_It" TagPrefix="uc2" %>
<%@ Register Src="~/PlanDeNegocioV2/Formulacion/Controles/Help.ascx" TagPrefix="uc3" TagName="Help" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../../Styles/siteProyecto.css" rel="stylesheet" />
    <script src="../../../Scripts/jquery-1.11.1.min.js"></script>
    <script src="../../../Scripts/jquery-ui-1.10.3.min.js"></script>
    <script src="../../../Scripts/common.js"></script>
    <link href="../../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" />
    <script type="text/javascript" src="../../../Scripts/jquery.number.min.js"></script>
    <script>
        $(function () {
            $('.money').number(true, 2);
        });
    </script>

</head>
<body>
    
    <form id="form1" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager" runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <div class="parentContainer">
            <div class="childContainer">
                <asp:UpdatePanel ID="UpdatePanel" runat="server" Width="100%">
                    <ContentTemplate>
                        <% Page.DataBind(); %>
                        <uc1:Encabezado runat="server" ID="Encabezado" />
                        <br />
                        <div style="position: relative; left: 705px; width: 160px;">
                            <uc2:Post_It ID="Post_It" runat="server" Visible='<%# PostitVisible %>' _txtCampo="Requerimientos" />
                            <br />
                        </div>
                        <div style="text-align: center">
                            <h1>IV. ¿Cómo desarrollo mi solución?</h1>
                            <br />
                            <br />
                        </div>
                        <uc3:Help runat="server" ID="HelpPregunta14" Mensaje="RequerimientosInfraestrutura" Titulo="14. Defina los requerimientos en: Infraestructura - adecuaciones, maquinaria y equipos, muebles y enseres, y demás activos" />
                        <br />
                        <br />
                        <uc3:Help runat="server" ID="HelpPregunta141" Mensaje="FuncionamientoNegocio" Titulo="14.1. ¿Para el funcionamiento del negocio, es necesario un lugar físico de operación? (SI / NO, justificación)" />
                        &nbsp;<asp:RequiredFieldValidator ID="rvPregunta141ddl" runat="server" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),Fonade.Negocio.Mensajes.Mensajes.GetMensaje(105)) %>' Text="" Display="None" Font-Bold="True"
                            Font-Size="X-Large" ForeColor="Red" ControlToValidate="ddlPregunta141" SetFocusOnError="True" ToolTip="Requerido" ValidationGroup="grupo1"></asp:RequiredFieldValidator>

                        <asp:DropDownList ID="ddlPregunta141" runat="server" AutoPostBack="True" CausesValidation="True" ClientIDMode="Static" ValidationGroup="grupo1" Enabled='<%#((bool)DataBinder.GetPropertyValue(this, "AllowUpdate"))%>'>
                            <asp:ListItem Value=""></asp:ListItem>
                            <asp:ListItem Value="1">SI</asp:ListItem>
                            <asp:ListItem Value="0">NO</asp:ListItem>
                        </asp:DropDownList>

                        <br />
                        &nbsp;<asp:RequiredFieldValidator ID="rvPregunta141cke" runat="server" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),Fonade.Negocio.Mensajes.Mensajes.GetMensaje(106)) %>' Text="" Display="None" Font-Bold="True"
                            Font-Size="X-Large" ForeColor="Red" ControlToValidate="cke_Pregunta141" SetFocusOnError="True" ToolTip="Requerido" ValidationGroup="grupo1"></asp:RequiredFieldValidator>

                        <CKEditor:CKEditorControl ID="cke_Pregunta141" runat="server" EnableTabKeyTools="true" ForcePasteAsPlainText="false" BasePath="~/ckeditor"
                            Enabled='<%#((bool)DataBinder.GetPropertyValue(this, "AllowUpdate"))%>' ValidationGroup="grupo1"></CKEditor:CKEditorControl>
                        <br />

                        <uc3:Help runat="server" ID="HelpPregunta142" Mensaje="IdentifiqueRequerimientos" Titulo="14.2. Identifique los requerimientos de inversión" />
                        <br />
                        <blockquote style="font-style: italic">Nota: Se debe listar la totalidad de requerimientos en inversión, independientemente de si se financiarán con recursos del Fondo Emprender, propios o de otras fuentes.</blockquote>
                        <br />

                        <div>
                            <h1>1. Infraestructura - Adecuaciones</h1>
                            <br />
                        </div>
                        <div runat="server" visible='<%# AllowUpdate %>'>
                            <asp:ImageButton ID="imgagregar1421" runat="server" ImageUrl="~/Images/icoAdicionarUsuario.gif" OnClick="btnAddRequerimiento_Click" CommandArgument="gwPregunta1421" />
                            &nbsp;
                        <asp:LinkButton ID="btnAddRequerimiento1421" runat="server" Text="Adicionar Descripción" OnClick="btnAddRequerimiento_Click" CommandArgument="gwPregunta1421"></asp:LinkButton>

                        </div>
                        <br />
                        <br />
                        <asp:GridView ID="gwPregunta1421" runat="server" Width="100%" AutoGenerateColumns="False" OnPageIndexChanging="gwGrillas_PageIndexChanging"
                            CssClass="Grilla" AllowPaging="true" PageSize="5"
                            ShowHeaderWhenEmpty="True" EmptyDataText='<%# Fonade.Negocio.Mensajes.Mensajes.GetMensaje(101)%>' ShowFooter="True">
                            <RowStyle HorizontalAlign="Left" />
                            <Columns>
                                <asp:TemplateField HeaderText="Eliminar">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnEliminar1421" runat="server" ImageUrl="~/Images/icoBorrar.gif" CommandArgument='<%# Bind("IdProyectoInfraestructura") %>' OnClick="btnEliminar_Click"
                                            OnClientClick="return confirm('¿Está seguro de eliminar el registro seleccionado?')" Enabled='<%#((bool)DataBinder.GetPropertyValue(this, "AllowUpdate"))%>' />

                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" Width="40px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Descripción">
                                    <ItemTemplate>
                                        <div style="word-wrap: break-word;">
                                        <asp:LinkButton ID="btnEditarRequerimiento1421" runat="server" OnClick="btnEditarRequerimiento_Click" Width="100px"
                                            CommandArgument='<%# Bind("IdProyectoInfraestructura") %>' Enabled='<%#((bool)DataBinder.GetPropertyValue(this, "AllowUpdate"))%>'>
                                                                <%# Eval("NomInfraestructura") %></asp:LinkButton>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Requisitos Técnicos">
                                    <ItemTemplate>
                                        <div style="word-wrap: break-word;">
                                            <asp:Label ID="lblReqTecnico" runat="server" Text='<%# Bind("RequisitosTecnicos") %>' Width="220"></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Fuente de Financiación">
                                    <ItemTemplate>
                                        <div style="word-wrap: break-word;">
                                            <asp:Label ID="lblFuenteFin" runat="server" Text='<%# Bind("FuenteFinanciacion") %>' Width="150"></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cantidad">
                                    <ItemTemplate>
                                        <div style="word-wrap: break-word">
                                            <asp:Label ID="lblCantidad" runat="server" Text='<%# Bind("Cantidad") %>' Width="50"></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Valor Unitario" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblvlr1421" runat="server" Text='<%# Eval("ValorUnidadCadena")%>' />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <div style="text-align: right;">
                                            <asp:Label ID="lblTotalVlrG1421" runat="server" Font-Bold="true" Text='<%#(DataBinder.GetPropertyValue(this, "TotalG1421"))%>' />
                                        </div>
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>

                        </asp:GridView>


                        <div>
                            <h1>2. Maquinaria y Equipo</h1>
                            <br />
                        </div>
                        <div runat="server" visible='<%# AllowUpdate %>'>
                            <asp:ImageButton ID="imgagregar1422" runat="server" ImageUrl="~/Images/icoAdicionarUsuario.gif" OnClick="btnAddRequerimiento_Click" CommandArgument="gwPregunta1422" />
                            &nbsp;
                        <asp:LinkButton ID="btnAddRequerimiento1422" runat="server" Text="Adicionar Descripción" OnClick="btnAddRequerimiento_Click" CommandArgument="gwPregunta1422"></asp:LinkButton>

                        </div>
                        <br />
                        <br />
                        <asp:GridView ID="gwPregunta1422" runat="server" Width="100%" AutoGenerateColumns="False" OnPageIndexChanging="gwGrillas_PageIndexChanging"
                            CssClass="Grilla" AllowPaging="true" PageSize="5"
                            ShowHeaderWhenEmpty="True" EmptyDataText='<%# Fonade.Negocio.Mensajes.Mensajes.GetMensaje(101)%>' ShowFooter="True">
                            <RowStyle HorizontalAlign="Left" />
                            <Columns>
                                <asp:TemplateField HeaderText="Eliminar">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnEliminar1422" runat="server" ImageUrl="~/Images/icoBorrar.gif" CommandArgument='<%# Bind("IdProyectoInfraestructura") %>' OnClick="btnEliminar_Click"
                                            OnClientClick="return confirm('¿Está seguro de eliminar el registro seleccionado?')" Enabled='<%#((bool)DataBinder.GetPropertyValue(this, "AllowUpdate"))%>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" Width="40px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Descripción">
                                    <ItemTemplate>
                                        <div style="word-wrap: break-word;">
                                        <asp:LinkButton ID="btnEditarRequerimiento1422" runat="server" OnClick="btnEditarRequerimiento_Click" Width="100px"
                                            CommandArgument='<%# Bind("IdProyectoInfraestructura") %>' Enabled='<%#((bool)DataBinder.GetPropertyValue(this, "AllowUpdate"))%>'>
                                                                <%# Eval("NomInfraestructura") %></asp:LinkButton>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Requisitos Técnicos">
                                    <ItemTemplate>
                                        <div style="word-wrap: break-word;">
                                            <asp:Label ID="lblReqTecnico1" runat="server" Text='<%# Bind("RequisitosTecnicos") %>' Width="220"></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Fuente de Financiación">
                                    <ItemTemplate>
                                        <div style="word-wrap: break-word;">
                                            <asp:Label ID="lblFuenteFin1" runat="server" Text='<%# Bind("FuenteFinanciacion") %>' Width="150"></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cantidad">
                                    <ItemTemplate>
                                        <div style="word-wrap: break-word">
                                            <asp:Label ID="lblCantidad1" runat="server" Text='<%# Bind("Cantidad") %>' Width="50"></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Valor Unitario" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblvlr1421" runat="server" Text='<%# Eval("ValorUnidadCadena")%>' />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <div style="text-align: right;">
                                            <asp:Label ID="lblTotalVlrG1422" runat="server" Font-Bold="true" Text='<%#(DataBinder.GetPropertyValue(this, "TotalG1422"))%>' />
                                        </div>
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>

                        </asp:GridView>


                        <div>
                            <h1>3. Equipo de Comunicación y Computación</h1>
                            <br />
                        </div>
                        <div runat="server" visible='<%# AllowUpdate %>'>
                            <asp:ImageButton ID="imgagregar1423" runat="server" ImageUrl="~/Images/icoAdicionarUsuario.gif" OnClick="btnAddRequerimiento_Click" CommandArgument="gwPregunta1423" />
                            &nbsp;
                        <asp:LinkButton ID="btnAddRequerimiento1423" runat="server" Text="Adicionar Descripción" OnClick="btnAddRequerimiento_Click" CommandArgument="gwPregunta1423"></asp:LinkButton>

                        </div>
                        <br />
                        <br />
                        <asp:GridView ID="gwPregunta1423" runat="server" Width="100%" AutoGenerateColumns="False" OnPageIndexChanging="gwGrillas_PageIndexChanging"
                            CssClass="Grilla" AllowPaging="true" PageSize="5"
                            ShowHeaderWhenEmpty="True" EmptyDataText='<%# Fonade.Negocio.Mensajes.Mensajes.GetMensaje(101)%>' ShowFooter="True">
                            <RowStyle HorizontalAlign="Left" />
                            <Columns>
                                <asp:TemplateField HeaderText="Eliminar">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnEliminar1423" runat="server" ImageUrl="~/Images/icoBorrar.gif" CommandArgument='<%# Bind("IdProyectoInfraestructura") %>' OnClick="btnEliminar_Click"
                                            OnClientClick="return confirm('¿Está seguro de eliminar el registro seleccionado?')" Enabled='<%#((bool)DataBinder.GetPropertyValue(this, "AllowUpdate"))%>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" Width="40px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Descripción">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnEditarRequerimiento1423" runat="server" OnClick="btnEditarRequerimiento_Click" Width="100px"
                                            CommandArgument='<%# Bind("IdProyectoInfraestructura") %>' Enabled='<%#((bool)DataBinder.GetPropertyValue(this, "AllowUpdate"))%>'>
                                                                <%# Eval("NomInfraestructura") %></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Requisitos Técnicos">
                                    <ItemTemplate>
                                        <div style="word-wrap: break-word;">
                                            <asp:Label ID="lblReqTecnico2" runat="server" Text='<%# Bind("RequisitosTecnicos") %>' Width="220"></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Fuente de Financiación">
                                    <ItemTemplate>
                                        <div style="word-wrap: break-word;">
                                            <asp:Label ID="lblFuenteFin2" runat="server" Text='<%# Bind("FuenteFinanciacion") %>' Width="150"></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cantidad">
                                    <ItemTemplate>
                                        <div style="word-wrap: break-word">
                                            <asp:Label ID="lblCantidad2" runat="server" Text='<%# Bind("Cantidad") %>' Width="50"></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Valor Unitario" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblvlr1423" runat="server" Text='<%# Eval("ValorUnidadCadena")%>' />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <div style="text-align: right;">
                                            <asp:Label ID="lblTotalVlrG1423" runat="server" Font-Bold="true" Text='<%#(DataBinder.GetPropertyValue(this, "TotalG1423"))%>' />
                                        </div>
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>

                        </asp:GridView>


                        <div>
                            <h1>4. Muebles y Enseres y Otros</h1>
                            <br />
                        </div>
                        <div runat="server" visible='<%# AllowUpdate %>'>
                            <asp:ImageButton ID="imgagregar1424" runat="server" ImageUrl="~/Images/icoAdicionarUsuario.gif" OnClick="btnAddRequerimiento_Click" CommandArgument="gwPregunta1424" />
                            &nbsp;
                        <asp:LinkButton ID="btnAddRequerimiento1424" runat="server" Text="Adicionar Descripción" OnClick="btnAddRequerimiento_Click" CommandArgument="gwPregunta1424"></asp:LinkButton>

                        </div>
                        <br />
                        <br />
                        <asp:GridView ID="gwPregunta1424" runat="server" Width="100%" AutoGenerateColumns="False" OnPageIndexChanging="gwGrillas_PageIndexChanging"
                            CssClass="Grilla" AllowPaging="true" PageSize="5"
                            ShowHeaderWhenEmpty="True" EmptyDataText='<%# Fonade.Negocio.Mensajes.Mensajes.GetMensaje(101)%>' ShowFooter="True">
                            <RowStyle HorizontalAlign="Left" />
                            <Columns>
                                <asp:TemplateField HeaderText="Eliminar">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnEliminar1424" runat="server" ImageUrl="~/Images/icoBorrar.gif" CommandArgument='<%# Bind("IdProyectoInfraestructura") %>' OnClick="btnEliminar_Click"
                                            OnClientClick="return confirm('¿Está seguro de eliminar el registro seleccionado?')" Enabled='<%#((bool)DataBinder.GetPropertyValue(this, "AllowUpdate"))%>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" Width="40px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Descripción">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnEditarRequerimiento1424" runat="server" OnClick="btnEditarRequerimiento_Click" Width="100px"
                                            CommandArgument='<%# Bind("IdProyectoInfraestructura") %>' Enabled='<%#((bool)DataBinder.GetPropertyValue(this, "AllowUpdate"))%>'>
                                                                <%# Eval("NomInfraestructura") %></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Requisitos Técnicos">
                                    <ItemTemplate>
                                        <div style="word-wrap: break-word;">
                                            <asp:Label ID="lblReqTecnico3" runat="server" Text='<%# Bind("RequisitosTecnicos") %>' Width="220"></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Fuente de Financiación">
                                    <ItemTemplate>
                                        <div style="word-wrap: break-word;">
                                            <asp:Label ID="lblFuenteFin3" runat="server" Text='<%# Bind("FuenteFinanciacion") %>' Width="150"></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cantidad">
                                    <ItemTemplate>
                                        <div style="word-wrap: break-word">
                                            <asp:Label ID="lblCantidad3" runat="server" Text='<%# Bind("Cantidad") %>' Width="50"></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Valor Unitario" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblvlr1424" runat="server" Text='<%# Eval("ValorUnidadCadena")%>' />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <div style="text-align: right;">
                                            <asp:Label ID="lblTotalVlrG1424" runat="server" Font-Bold="true" Text='<%#(DataBinder.GetPropertyValue(this, "TotalG1424"))%>' />
                                        </div>
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>

                        </asp:GridView>


                        <div>
                            <h1>5. Otros (incluído herrramientas)</h1>
                            <br />
                        </div>
                        <div runat="server" visible='<%# AllowUpdate %>'>
                            <asp:ImageButton ID="imgagregar1425" runat="server" ImageUrl="~/Images/icoAdicionarUsuario.gif" OnClick="btnAddRequerimiento_Click" CommandArgument="gwPregunta1425" />
                            &nbsp;
                        <asp:LinkButton ID="btnAddRequerimiento1425" runat="server" Text="Adicionar Descripción" OnClick="btnAddRequerimiento_Click" CommandArgument="gwPregunta1425"></asp:LinkButton>

                        </div>
                        <br />
                        <br />
                        <asp:GridView ID="gwPregunta1425" runat="server" Width="100%" AutoGenerateColumns="False" OnPageIndexChanging="gwGrillas_PageIndexChanging"
                            CssClass="Grilla" AllowPaging="true" PageSize="5"
                            ShowHeaderWhenEmpty="True" EmptyDataText='<%# Fonade.Negocio.Mensajes.Mensajes.GetMensaje(101)%>' ShowFooter="True">
                            <RowStyle HorizontalAlign="Left" />
                            <Columns>
                                <asp:TemplateField HeaderText="Eliminar">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnEliminar1425" runat="server" ImageUrl="~/Images/icoBorrar.gif" CommandArgument='<%# Bind("IdProyectoInfraestructura") %>' OnClick="btnEliminar_Click"
                                            OnClientClick="return confirm('¿Está seguro de eliminar el registro seleccionado?')" Enabled='<%#((bool)DataBinder.GetPropertyValue(this, "AllowUpdate"))%>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" Width="40px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Descripción">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnEditarRequerimiento1425" runat="server" OnClick="btnEditarRequerimiento_Click" Width="100px"
                                            CommandArgument='<%# Bind("IdProyectoInfraestructura") %>' Enabled='<%#((bool)DataBinder.GetPropertyValue(this, "AllowUpdate"))%>'>
                                                                <%# Eval("NomInfraestructura") %></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Requisitos Técnicos">
                                    <ItemTemplate>
                                        <div style="word-wrap: break-word;">
                                            <asp:Label ID="lblReqTecnico4" runat="server" Text='<%# Bind("RequisitosTecnicos") %>' Width="220"></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Fuente de Financiación">
                                    <ItemTemplate>
                                        <div style="word-wrap: break-word;">
                                            <asp:Label ID="lblFuenteFin4" runat="server" Text='<%# Bind("FuenteFinanciacion") %>' Width="150"></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cantidad">
                                    <ItemTemplate>
                                        <div style="word-wrap: break-word">
                                            <asp:Label ID="lblCantidad4" runat="server" Text='<%# Bind("Cantidad") %>' Width="50"></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Valor Unitario" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblvlr1425" runat="server" Text='<%# Eval("ValorUnidadCadena")%>' />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <div style="text-align: right;">
                                            <asp:Label ID="lblTotalVlrG1425" runat="server" Font-Bold="true" Text='<%#(DataBinder.GetPropertyValue(this, "TotalG1425"))%>' />
                                        </div>
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>

                        </asp:GridView>


                        <div>
                            <h1>6. Gastos Preoperativos</h1>
                            <br />
                        </div>
                        <div runat="server" visible='<%# AllowUpdate %>'>
                            <asp:ImageButton ID="imgagregar1426" runat="server" ImageUrl="~/Images/icoAdicionarUsuario.gif" OnClick="btnAddRequerimiento_Click" CommandArgument="gwPregunta1426" />
                            &nbsp;
                        <asp:LinkButton ID="btnAddRequerimiento1426" runat="server" Text="Adicionar Descripción" OnClick="btnAddRequerimiento_Click" CommandArgument="gwPregunta1426"></asp:LinkButton>

                        </div>
                        <br />
                        <br />
                        <asp:GridView ID="gwPregunta1426" runat="server" Width="100%" AutoGenerateColumns="False" OnPageIndexChanging="gwGrillas_PageIndexChanging"
                            CssClass="Grilla" AllowPaging="true" PageSize="5"
                            ShowHeaderWhenEmpty="True" EmptyDataText='<%# Fonade.Negocio.Mensajes.Mensajes.GetMensaje(101)%>' ShowFooter="True">
                            <RowStyle HorizontalAlign="Left" />
                            <Columns>
                                <asp:TemplateField HeaderText="Eliminar">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnEliminar1426" runat="server" ImageUrl="~/Images/icoBorrar.gif" CommandArgument='<%# Bind("IdProyectoInfraestructura") %>' OnClick="btnEliminar_Click"
                                            OnClientClick="return confirm('¿Está seguro de eliminar el registro seleccionado?')" Enabled='<%#((bool)DataBinder.GetPropertyValue(this, "AllowUpdate"))%>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" Width="40px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Descripción">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnEditarRequerimiento1426" runat="server" OnClick="btnEditarRequerimiento_Click" Width="100px"
                                            CommandArgument='<%# Bind("IdProyectoInfraestructura") %>' Enabled='<%#((bool)DataBinder.GetPropertyValue(this, "AllowUpdate"))%>'>
                                                                <%# Eval("NomInfraestructura") %></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Requisitos Técnicos">
                                    <ItemTemplate>
                                        <div style="word-wrap: break-word;">
                                            <asp:Label ID="lblReqTecnico5" runat="server" Text='<%# Bind("RequisitosTecnicos") %>' Width="220"></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Fuente de Financiación">
                                    <ItemTemplate>
                                        <div style="word-wrap: break-word;">
                                            <asp:Label ID="lblFuenteFin5" runat="server" Text='<%# Bind("FuenteFinanciacion") %>' Width="150"></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cantidad">
                                    <ItemTemplate>
                                        <div style="word-wrap: break-word">
                                            <asp:Label ID="lblCantidad5" runat="server" Text='<%# Bind("Cantidad") %>' Width="50"></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Valor Unitario" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblvlr1426" runat="server" Text='<%# Eval("ValorUnidadCadena")%>' />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <div style="text-align: right;">
                                            <asp:Label ID="lblTotalVlrG1426" runat="server" Font-Bold="true" Text='<%#(DataBinder.GetPropertyValue(this, "TotalG1426"))%>' />
                                        </div>
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>

                        </asp:GridView>

                        <br />

                       <asp:ValidationSummary ID="vsErrores"
                            runat="server"
                            HeaderText="Advertencia: "
                            ForeColor="Red"
                            Font-Italic="true"
                            ValidationGroup="grupo1" />
                        <br />
                        <div style="text-align: center">
                            <asp:Button ID="btnLimpiarCampos" runat="server" Text="Limpiar Campos" OnClick="btnLimpiarCampos_Click" Visible='<%#((bool)DataBinder.GetPropertyValue(this, "AllowUpdate"))%>' />&nbsp;
                        <asp:Button ID="btnGuardar" runat="server" Text="Guardar" OnClick="btnGuardar_Click" ValidationGroup="grupo1" Visible='<%#((bool)DataBinder.GetPropertyValue(this, "AllowUpdate"))%>' />
                        </div>
                        <asp:UpdateProgress ID="UpdateProgress" runat="server" AssociatedUpdatePanelID="UpdatePanel">
                            <ProgressTemplate>
                                <div style="text-align: right"><b>Procesando información</b>&nbsp;&nbsp;<img src="../../../Images/fbloader.gif" />&nbsp;&nbsp;</div>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br />
                <br />
                <br />
                <br />
            </div>
            <asp:HiddenField ID="HiddenWidth" runat="server" />
            <script>
                $(document).ready(function () { $('input[name="HiddenWidth"]').val(screen.width); });
            </script>

        </div>
    </form>
</body>
</html>
