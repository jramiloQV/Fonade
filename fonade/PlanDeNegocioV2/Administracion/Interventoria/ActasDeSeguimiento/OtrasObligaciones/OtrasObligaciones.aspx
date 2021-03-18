<%@ Page Language="C#"
    AutoEventWireup="true"
    CodeBehind="OtrasObligaciones.aspx.cs"
    Inherits="Fonade.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimiento.OtrasObligaciones.OtrasObligaciones" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Otras Obligaciones</title>
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .auto-style1 {
            width: 31px;
        }

        .auto-style2 {
            width: 84px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="titulo">
            <h1>
                <asp:Label ID="Label1" runat="server"
                    Text="6. OTRAS OBLIGACIONES CONTRACTUALES"></asp:Label>
            </h1>
        </div>
        <!--6.1 Reporte de Información en Plataforma-->
        <div>
            <h3>6.1 Reporte de Información en Plataforma</h3>
        </div>
        <div style="text-align: center">
            <h2 style="padding-left: 0px;">Descripción de Información en Plataforma</h2>
        </div>
        <asp:Panel ID="pnlInfoInformacionPlataforma" runat="server">
            <div style="text-align: center;">
                <asp:TextBox ID="txtInformacionPlataforma" runat="server" TextMode="MultiLine"
                    Style="height: 70px; width: 500px;" required>
                </asp:TextBox>
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlGridInformacionPlataforma" runat="server">
            <asp:GridView ID="gvInformacionPlataforma" runat="server"
                AutoGenerateColumns="False"
                CssClass="Grilla"
                DataKeyNames="id" 
                OnPageIndexChanging="gvInformacionPlataforma_PageIndexChanging"
                EmptyDataText="No se ha registrado indicador."
                AllowPaging="True" ForeColor="#666666" Width="100%">
                <Columns>
                    <asp:BoundField DataField="visita" HeaderText="Visita" />
                    <asp:BoundField DataField="valoracion" HeaderText="Si/No/Parcial" />
                    <asp:BoundField DataField="observacion" HeaderText="Observaciones" />
                </Columns>
            </asp:GridView>
            <div>
                <table style="width: 100%;">

                    <tr style="background-color: #00468f; color: white;">
                        <td class="auto-style1">Visita</td>
                        <td class="auto-style2">Si/No/Parcial</td>

                        <td>Observaciones</td>
                    </tr>
                    <tr>
                        <td class="auto-style1" style="text-align: center">
                            <asp:Label ID="lblnumV" runat="server" Text=""></asp:Label>
                        </td>
                        <td class="auto-style2">
                            <asp:DropDownList ID="ddlValoracionInfoPlataforma" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox ID="txtObserInfoPlataforma" runat="server" TextMode="MultiLine"
                                MaxLength="10000" Width="97%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center" colspan="3">
                            <asp:Button ID="btnSaveInfoPlataforma" runat="server" Height="30px"
                                Text="Guardar" OnClick="btnSaveInfoPlataforma_Click" />
                        </td>
                    </tr>
                </table>
            </div>
        </asp:Panel>
        <hr />
        <!--6.2 Tiempo de Dedicación del Emprendedor-->
        <div>
            <h3>6.2 Tiempo de Dedicación del Emprendedor</h3>
        </div>
        <div style="text-align: center">
            <h2 style="padding-left: 0px;">Descripción de Tiempo de Dedicación del Emprendedor</h2>
        </div>
        <asp:Panel ID="pnlInfoDedicaEmprendedor" runat="server">
            <div style="text-align: center;">
                <asp:TextBox ID="txtDedicacionEmprendedor" runat="server" TextMode="MultiLine"
                    Style="height: 70px; width: 500px;" required>
                </asp:TextBox>
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlGridDedicaEmprendedor" runat="server">
            <asp:GridView ID="gvDedicaEmprendedor" runat="server"
                AutoGenerateColumns="False"
                CssClass="Grilla"
                DataKeyNames="id"
                OnPageIndexChanging="gvDedicaEmprendedor_PageIndexChanging"
                EmptyDataText="No se ha registrado indicador."
                AllowPaging="True" ForeColor="#666666" Width="100%">
                <Columns>
                    <asp:BoundField DataField="visita" HeaderText="Visita" />
                    <asp:BoundField DataField="valoracion" HeaderText="Si/No/Parcial" />
                    <asp:BoundField DataField="observacion" HeaderText="Observaciones" />
                </Columns>
            </asp:GridView>
            <div>
                <table style="width: 100%;">

                    <tr style="background-color: #00468f; color: white;">
                        <td class="auto-style1">Visita</td>
                        <td class="auto-style2">Si/No/Parcial</td>

                        <td>Observaciones</td>
                    </tr>
                    <tr>
                        <td class="auto-style1" style="text-align: center">
                            <asp:Label ID="lblNumVD" runat="server" Text=""></asp:Label>
                        </td>
                        <td class="auto-style2">
                            <asp:DropDownList ID="ddlValDedicaEmprendedor" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox ID="txtObservDedicaEmprendedor" runat="server" TextMode="MultiLine"
                                MaxLength="10000" Width="97%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center" colspan="3">
                            <asp:Button ID="btnSaveDedicaEmprendedor" runat="server" Height="30px"
                                Text="Guardar" OnClick="btnSaveDedicaEmprendedor_Click" />
                        </td>
                    </tr>
                </table>
            </div>
        </asp:Panel>
        <hr />
        <!--6.3 Acompañamiento y Asesoría de la Unidad de Emprendimiento-->
        <div>
            <h3>6.3 Acompañamiento y Asesoría de la Unidad de Emprendimiento</h3>
        </div>
        <div style="text-align: center">
            <h2 style="padding-left: 0px;">Descripción de Acompañamiento y Asesoría de la Unidad de Emprendimiento</h2>
        </div>
        <asp:Panel ID="pnlInfoAcompAsesoria" runat="server">
            <div style="text-align: center;">
                <asp:TextBox ID="txtAcompAsesoria" runat="server" TextMode="MultiLine"
                    Style="height: 70px; width: 500px;" required>
                </asp:TextBox>
            </div>
        </asp:Panel>
           <asp:Panel ID="pnlGridAcomAsesoria" runat="server">
            <asp:GridView ID="gvAcomAsesoria" runat="server"
                AutoGenerateColumns="False"
                CssClass="Grilla"
                DataKeyNames="id"
                OnPageIndexChanging="gvAcomAsesoria_PageIndexChanging"
                EmptyDataText="No se ha registrado indicador."
                AllowPaging="True" ForeColor="#666666" Width="100%">
                <Columns>
                    <asp:BoundField DataField="visita" HeaderText="Visita" />
                    <asp:BoundField DataField="valoracion" HeaderText="Si/No/Parcial" />
                    <asp:BoundField DataField="observacion" HeaderText="Observaciones" />
                </Columns>
            </asp:GridView>
            <div>
                <table style="width: 100%;">

                    <tr style="background-color: #00468f; color: white;">
                        <td class="auto-style1">Visita</td>
                        <td class="auto-style2">Si/No/Parcial</td>

                        <td>Observaciones</td>
                    </tr>
                    <tr>
                        <td class="auto-style1" style="text-align: center">
                            <asp:Label ID="lblNumVA" runat="server" Text=""></asp:Label>
                        </td>
                        <td class="auto-style2">
                            <asp:DropDownList ID="ddlValAcomAsesoria" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox ID="txtObservAcomAsesoria" runat="server" TextMode="MultiLine"
                                MaxLength="10000" Width="97%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center" colspan="3">
                            <asp:Button ID="btnSaveAcomAsesoria" runat="server" Height="30px"
                                Text="Guardar" OnClick="btnSaveAcomAsesoria_Click" />
                        </td>
                    </tr>
                </table>
            </div>
        </asp:Panel>
        <hr />
        <div style="text-align: center">
            <asp:Button ID="btnGuardarDescripciones" runat="server"
                Text="Guardar" Height="29px" OnClick="btnGuardarDescripciones_Click" />
        </div>

    </form>
</body>
</html>
