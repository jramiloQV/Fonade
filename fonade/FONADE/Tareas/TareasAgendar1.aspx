<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TareasAgendar1.aspx.cs" Inherits="Fonade.FONADE.Tareas.TareasAgendar1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <h1>
        <asp:Label runat="server" ID="lbl_Titulo"></asp:Label></h1>
    <label id="estadoTarea" runat='server'>
    </label>
    <asp:Panel ID="Panel1" runat="server">
        <asp:Table ID="tbl1" runat="server">
            <asp:TableRow>
                <asp:TableCell Width="200px">
                    <asp:ListBox AutoPostBack="false" Enabled="true" SelectionMode="Multiple" Height="300px"
                        Width="270px" ID="ListBox1" runat="server"></asp:ListBox>
                    <asp:Label ID="Label1" Text="Para seleccionar varios nombres mantenga presionada la tecla 'Ctrl' mientras hace clic"
                        runat="server"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Table ID="tbl2" runat="server">
                        <asp:TableRow>
                            <asp:TableCell>Actividad:</asp:TableCell>
                            <asp:TableCell>
                                <asp:DropDownList Width="180px" ID="ddl_usuarios" runat="server" ValidationGroup="crearTarea">
                                </asp:DropDownList>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>Tarea:</asp:TableCell>
                            <asp:TableCell>
                                <asp:TextBox Width="180px" ID="tb_tarea" runat="server" ValidationGroup="crearTarea"></asp:TextBox>
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
                                <asp:RequiredFieldValidator CssClass="ErrorValidacion" ForeColor="Red" ID="RequiredFieldValidator1"
                                    runat="server" ControlToValidate="tb_descripcion" Display="Dynamic" ErrorMessage="* Este campo no puede estar en blanco"
                                    ValidationGroup="crearTarea"></asp:RequiredFieldValidator>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>Plan de Negocio:</asp:TableCell>
                            <asp:TableCell>
                                <asp:DropDownList ID="DropDownList1" runat="server" ValidationGroup="crearTarea"
                                    Width="200px" DataTextField="NomProyecto" DataValueField="Id_Proyecto">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                </asp:DropDownList>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>Fecha</asp:TableCell>
                            <asp:TableCell>
                               
                                <asp:TextBox runat="server" ID="txtDate2" Text="" ValidationGroup="crearTarea" />
                                <asp:Image runat="server" ID="btnDate2" AlternateText="cal2" ImageUrl="~/images/icomodificar.gif" />
                               
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>Avisar por e-mail:</asp:TableCell>
                            <asp:TableCell>
                                <asp:DropDownList Width="79px" ID="ddl_avisar" runat="server" ValidationGroup="crearTarea">
                                    <asp:ListItem>No</asp:ListItem>
                                    <asp:ListItem>Sí</asp:ListItem>
                                </asp:DropDownList>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>Urgencia:</asp:TableCell>
                            <asp:TableCell>
                                <asp:DropDownList Width="79px" ID="ddl_urgencia" runat="server" ValidationGroup="crearTarea">
                                    <asp:ListItem Value='0'>Muy alta</asp:ListItem>
                                    <asp:ListItem Value='1'>Alta</asp:ListItem>
                                    <asp:ListItem Value='2' Selected> Normal</asp:ListItem>
                                    <asp:ListItem Value='3'>Baja</asp:ListItem>
                                    <asp:ListItem Value='4'>Muy baja</asp:ListItem>
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
                            <asp:TableCell>
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
                <td>
                    De:
                </td>
                <td>
                    <asp:TextBox ID="TextBox1" runat="server" Enabled="false" Width="500px" Height="16px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Para:
                </td>
                <td>
                    <asp:TextBox ID="TextBox2" runat="server" Enabled="false" Width="500px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Actividad:
                </td>
                <td>
                    <asp:TextBox ID="TextBox3" runat="server" Enabled="false" Width="500px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Plan de Negocio:
                </td>
                <td>
                    <asp:TextBox ID="TextBox4" runat="server" Enabled="false" Width="500px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Tarea:
                </td>
                <td>
                    <asp:TextBox ID="TextBox5" runat="server" Enabled="false" Width="500px" Height="50px" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Descripción:
                </td>
                <td>
                    <asp:TextBox ID="TextBox6" runat="server" Enabled="false" Width="500px" Height="50px" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Fecha:
                </td>
                <td>
                    <asp:TextBox ID="TextBox7" runat="server" Enabled="false" Width="500px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Urgencia:
                </td>
                <td>
                    <asp:TextBox ID="TextBox8" runat="server" Enabled="false" Width="500px" Height="16px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Observaciones:
                </td>
                <td>
                    <asp:TextBox ID="TextBox9" TextMode="MultiLine" runat="server" Width="500px" Height="100px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Marcar como finalizada:
                </td>
                <td>
                    <asp:CheckBox ID="CheckBox1" runat="server" Enabled="false" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btn_Grabar" runat="server" Text="Grabar" OnClick="btn_Grabar_Click" />
                    ´+</td>
            </tr>
        </table>
    </asp:Panel>
    </div>
    </form>
</body>
</html>
