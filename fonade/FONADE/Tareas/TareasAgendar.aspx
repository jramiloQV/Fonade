<%@ Page Language="C#" Title="Fondo Emprender - Agendar Tarea" MasterPageFile="~/Master.master"
    EnableViewState="true" AutoEventWireup="true" CodeBehind="TareasAgendar.aspx.cs"
    Inherits="Fonade.FONADE.Tareas.TareasAgendar" EnableSessionState="True" Culture="es-CO" UICulture="es-CO" %>

<%@ OutputCache Duration="3600" VaryByParam="none" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
    <%--<asp:TextBox ID="txtDate2" runat="server" Type="Date" ValidationGroup="crearTarea" ></asp:TextBox>--%>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="bodyContentPlace">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false" EnableScriptGlobalization="true" EnableScriptLocalization="true">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <h1>
                <asp:Label runat="server" ID="lbl_Titulo" />
            </h1>
            <asp:Label ID="estadoTarea" runat="server" />
            <asp:Panel ID="Panel1" runat="server">
                <asp:Table ID="tbl1" runat="server">
                    <asp:TableRow>
                        <asp:TableCell Width="200px">
                            <asp:LinqDataSource ID="ldslistbox" runat="server" ContextTypeName="Datos.FonadeDBDataContext"
                                OnSelecting="ldslistbox_Selecting">
                            </asp:LinqDataSource>
                            <asp:RequiredFieldValidator ID="RangeValidator2" runat="server" CssClass="ErrorValidacion" ControlToValidate="ListBox1" ErrorMessage="Seleccione usuario(s) para agendar tarea" SetFocusOnError="True" Display="Dynamic" ValidationGroup="crearTarea" Text="Seleccione usuario(s) para agendar tarea" ForeColor="OrangeRed"></asp:RequiredFieldValidator>
                            <asp:ListBox AutoPostBack="false" SelectionMode="Multiple" Height="300px" Width="350px"
                                DataTextField="Nombre" DataValueField="Id_Contacto" DataSourceID="ldslistbox"
                                ID="ListBox1" runat="server" CausesValidation="True" ValidationGroup="crearTarea" />
                            <asp:Label ID="Label1" Text="Para seleccionar varios nombres mantenga presionada la tecla 'Ctrl' mientras hace clic"
                                runat="server"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Table ID="tbl2" runat="server">
                                <asp:TableRow>
                                    <asp:TableCell>Actividad:</asp:TableCell>
                                    <asp:TableCell>
                                        <asp:LinqDataSource ID="ldscontacto" runat="server" ContextTypeName="Datos.FonadeDBDataContext"
                                            OnSelecting="ldscontacto_Selecting">
                                        </asp:LinqDataSource>
                                        <asp:DropDownList Width="180px" ID="ddl_usuarios" runat="server" ValidationGroup="crearTarea" />

                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell>Tarea:</asp:TableCell>
                                    <asp:TableCell>
                                        <asp:TextBox Width="180px" ID="tb_tarea" runat="server" ValidationGroup="crearTarea"></asp:TextBox>
                                        <br />
                                        <asp:RequiredFieldValidator CssClass="ErrorValidacion" ForeColor="Red" ID="RequiredFieldValidator2"
                                            runat="server" ControlToValidate="tb_tarea" Display="Dynamic" ErrorMessage="* Este campo no puede estar en blanco"
                                            ValidationGroup="crearTarea"></asp:RequiredFieldValidator>
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell>Descripción</asp:TableCell>
                                    <asp:TableCell>
                                        <asp:TextBox Width="200px" TextMode="MultiLine" ID="tb_descripcion" runat="server"
                                            Height="120px" ValidationGroup="crearTarea"></asp:TextBox>
                                        <br />
                                        <asp:RequiredFieldValidator CssClass="ErrorValidacion" ForeColor="Red" ID="RequiredFieldValidator1"
                                            runat="server" ControlToValidate="tb_descripcion" Display="Dynamic" ErrorMessage="* Este campo no puede estar en blanco"
                                            ValidationGroup="crearTarea"></asp:RequiredFieldValidator>
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow runat="server" ID="labelPlan">
                                    <asp:TableCell>Plan de Negocio:</asp:TableCell>
                                    <asp:TableCell>
                                        <asp:DropDownList ID="DropDownList1" runat="server" Width="200px" AutoPostBack="true"
                                            OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged" />
                                        <asp:HiddenField ID="plan_seleccionado" runat="server" />
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell>Fecha</asp:TableCell>
                                    <asp:TableCell>
                                        <asp:TextBox runat="server" ID="txtDate2" Text="" ValidationGroup="crearTarea" Enabled="false" />
                                        <asp:Image runat="server" ID="btnDate2" AlternateText="cal2" ImageUrl="~/images/icomodificar.gif" />
                                        <ajaxToolkit:CalendarExtender runat="server" ID="calExtender2" PopupButtonID="btnDate2"
                                            TargetControlID="txtDate2" Format="dd/MM/yyyy" />                                        
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell>Avisar por e-mail:</asp:TableCell>
                                    <asp:TableCell>
                                        <asp:DropDownList Width="79px" ID="ddl_avisar" runat="server" ValidationGroup="crearTarea">
                                            <asp:ListItem Value="False">No</asp:ListItem>
                                            <asp:ListItem Value="True">Sí</asp:ListItem>
                                        </asp:DropDownList>
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell>Urgencia:</asp:TableCell>
                                    <asp:TableCell>
                                        <asp:DropDownList Width="79px" ID="ddl_urgencia" runat="server" ValidationGroup="crearTarea">
                                            <asp:ListItem Value="1">Muy alta</asp:ListItem>
                                            <asp:ListItem Value="2">Alta</asp:ListItem>
                                            <asp:ListItem Value="3" Selected="True"> Normal</asp:ListItem>
                                            <asp:ListItem Value="4">Baja</asp:ListItem>
                                            <asp:ListItem Value="5">Muy baja</asp:ListItem>
                                        </asp:DropDownList>
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell>Requiere Respuesta:</asp:TableCell>
                                    <asp:TableCell>
                                        <asp:DropDownList Width="79px" ID="ddl_respuesta" runat="server" ValidationGroup="crearTarea">
                                            <asp:ListItem>No</asp:ListItem>
                                            <asp:ListItem>Sí</asp:ListItem>
                                        </asp:DropDownList>
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell HorizontalAlign="Right" ColumnSpan="3">
                                        <asp:Button ID="Button1" OnClick="Button1_click" runat="server" Text="Grabar" ValidationGroup="crearTarea" />
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:Panel>
            <asp:Panel ID="Panel2" runat="server" Visible="False">
                <table>
                    <tr>
                        <td>De:
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox1" runat="server" Enabled="false" Width="500px" Height="16px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Para:
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox2" runat="server" Enabled="false" Width="500px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Actividad:
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox3" runat="server" Enabled="false" Width="500px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="tr_planNegocio" runat="server">
                        <td>Plan de Negocio:
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox4" runat="server" Enabled="false" Width="500px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Tarea:
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox5" runat="server" Enabled="false" Width="500px" Height="50px"
                                TextMode="MultiLine"></asp:TextBox>
                            <asp:LinkButton ID="lnk_SolicitudPago" runat="server" Visible="false" Font-Bold="true"
                                CausesValidation="false"
                                CommandArgument='<%# Eval("Ejecutable") +";"+ Eval("Parametros") %>'
                                OnClick="lnk_SolicitudPago_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>Descripción:
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox6" runat="server" Enabled="false" Width="500px" Height="50px"
                                TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Fecha:
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox7" runat="server" Enabled="false" Width="500px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Urgencia:
                        </td>
                        <td>
                            <asp:Image ID="img_urgencia" ImageUrl="../../Images/Tareas/Urgencia1.gif" runat="server" />
                            &nbsp;<asp:Label ID="lblUrgencia_Text" Text="Muy Alta" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>Observaciones:
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox9" TextMode="MultiLine" runat="server" Width="500px" Height="100px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Marcar como finalizada:
                        </td>
                        <td>
                            <asp:CheckBox ID="CheckBox1" runat="server" Enabled="false" />
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>
                            <asp:Label ID="lbl_Titulo0" runat="server" Font-Bold="True" ForeColor="#CC0000" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btn_Grabar" runat="server" Text="Grabar" OnClick="btn_Grabar_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <div onloadeddata="document.execCommand('stop')"></div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
