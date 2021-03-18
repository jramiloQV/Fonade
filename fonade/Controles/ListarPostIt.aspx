<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListarPostIt.aspx.cs" Inherits="Fonade.Controles.ListarPostIt1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <link href="../Scripts/jquery-ui-1.10.3.min.js" rel="stylesheet" type="text/css" />

    <script src="../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../Scripts/common.js" type="text/javascript"></script>

    <style type="text/css">
        .auto-style1 {
            width: 730px;
            height: auto;
        }

        .para {
            height: 200px;
            width: 300px;
            overflow: scroll;
        }

        .Grilla {
        }
    </style>
    <script type="text/javascript">
        function closeWindow() {
            window.parent.opener.focus();
            window.close();
        }
    </script>
</head>
<body class="auto-style1">
    <form id="form1" runat="server">
        <div class="auto-style1">
            <asp:Panel ID="P_PostIt" runat="server" CssClass="auto-style1">
                <br />
                <br />
                <table class="auto-style1">
                    <thead>
                        <tr style="width: 100%;">
                            <th style="background-color: #00468f; text-align: left; padding-left: 50px">
                                <asp:Label ID="L_PostIt" runat="server" ForeColor="White" Text="LISTA POST IT PENDIENTES" Width="260px"></asp:Label>
                            </th>
                        </tr>
                    </thead>
                    <tr>
                        <td style="width: 100%; text-align: left;">
                            <asp:Label ID="L_Nombreusuario" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%; vertical-align: top;">
                            <asp:GridView ID="gw_Tareas" runat="server" AutoGenerateColumns="False" AllowPaging="true" PageSize="5" AllowSorting="True" CssClass="Grilla"
                                DataKeyNames="Id_tareaRepeticion" OnRowCommand="gw_Tareas_RowCommand" Width="100%" OnPageIndexChanging="gw_Tareas_PageIndexChanging" >
                                <Columns>
                                    <asp:BoundField DataField="Id_tareaRepeticion" HeaderText="Id_tareaRepeticion" InsertVisible="False" ReadOnly="True" SortExpression="Id_tareaRepeticion" Visible="False" />
                                    <asp:BoundField DataField="codcontacto" HeaderText="codcontacto" SortExpression="codcontacto" Visible="False" />
                                    <asp:BoundField DataField="CodContactoAgendo" HeaderText="CodContactoAgendo" SortExpression="CodContactoAgendo" Visible="False" />
                                    <asp:BoundField DataField="Fecha" HeaderText="Fecha" SortExpression="Fecha" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="19%" DataFormatString="{0:dd-MM-yyyy}">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle Width="19%"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:TemplateField SortExpression="NomTareaUsuario" HeaderText="Tarea" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="id_tarea" runat="server" Text='<%# Eval("NomTareaUsuario") %>' Style="text-decoration: none;"
                                                CausesValidation="False" CommandArgument='<%# Bind("Id_tareaRepeticion") %>'
                                                CommandName="mostrar_tarea" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Agendo" HeaderText="Agendado a" SortExpression="Agendadoa"
                                        HeaderStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Agendadoa" HeaderText="Agendó" SortExpression="Agendo"
                                        HeaderStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    </asp:BoundField>
                                </Columns>
                            </asp:GridView>
                            <br />
                            <br />
                            <div style="background-color: #00468f; text-align: left; padding-left: 50px">
                                <b><asp:Label ID="Label1" runat="server" ForeColor="White" Text="HISTORICO POST IT CERRADOS" Width="260px" /></b>
                            </div>
                            
                            <asp:GridView ID="grvHistoPost" runat="server" AutoGenerateColumns="False" AllowPaging="true" PageSize="5" AllowSorting="True" CssClass="Grilla"
                                DataKeyNames="Id_tareaRepeticion" OnRowCommand="gw_Tareas_RowCommand" Width="100%" OnPageIndexChanging="grvHistoPost_PageIndexChanging">
                                <Columns>
                                    <asp:BoundField DataField="Id_tareaRepeticion" HeaderText="Id_tareaRepeticion" InsertVisible="False" ReadOnly="True" SortExpression="Id_tareaRepeticion" Visible="False" />
                                    <asp:BoundField DataField="codcontacto" HeaderText="codcontacto" SortExpression="codcontacto" Visible="False" />
                                    <asp:BoundField DataField="CodContactoAgendo" HeaderText="CodContactoAgendo" SortExpression="CodContactoAgendo" Visible="False" />
                                    <asp:BoundField DataField="Fecha" HeaderText="Fecha" SortExpression="Fecha" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="19%" DataFormatString="{0:dd-MM-yyyy}">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle Width="19%"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:TemplateField SortExpression="NomTareaUsuario" HeaderText="Tarea" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="id_tarea" runat="server" Text='<%# Eval("NomTareaUsuario") %>' Style="text-decoration: none;"
                                                CausesValidation="False" CommandArgument='<%# Bind("Id_tareaRepeticion") %>'
                                                CommandName="mostrar_tarea" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Agendo" HeaderText="Agendado a" SortExpression="Agendadoa"
                                        HeaderStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Agendadoa" HeaderText="Agendó" SortExpression="Agendo"
                                        HeaderStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Fecha cierre" DataField="fechacierre" SortExpression="fechacierre" DataFormatString="{0:dd-MM-yyyy}" />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%;">
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%; text-align: center;">
                            <asp:Button ID="B_Cerrar" runat="server" OnClientClick="closeWindow();" Text="Cerrar" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </div>
    </form>
</body>
</html>
