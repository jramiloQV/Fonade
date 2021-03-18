<%@ Page Language="C#" MasterPageFile="~/Master.master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="TrasladoPlanes.aspx.cs" Inherits="Fonade.FONADE.Administracion.TrasladoPlanes" %>

<asp:Content ID="head1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        table {
            width: 60%;
        }

        td {
            vertical-align: top;
        }
        .auto-style2 {
            height: 22px;
        }
        #bodyContentPlace_gv_listarpannegocio{
            width:100% !important;
        }
    </style>
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="bodyContentPlace">

    <asp:Panel ID="pnlprincipal" runat="server" Width="100%">
        <h1>
            <label>PLANES DE NEGOCIO</label>
        </h1>
        <br />
        <br />

        <table style="width:100% !important;">
            <tr>
                <td>Nombre Plan Negocio:
                </td>
                <td>
                    <asp:TextBox ID="txtnombrepalnnegico" runat="server"></asp:TextBox>
                </td>
                <td>Número Plan Negocio:
                </td>
                <td>
                    <asp:TextBox ID="txtnumeropannegocio" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="4" style="text-align: right;">
                    <asp:Button ID="btnbuscar" runat="server" Text="Buscar" OnClick="btnbuscar_Click" />
                </td>
            </tr>
        </table>

        <table style="width:100% !important;">
            <tr>
                <td>
                    <asp:GridView ID="gv_listarpannegocio" runat="server" 
                        CssClass="Grilla" AutoGenerateColumns="false" 
                        OnRowCommand="gv_listarpannegocio_RowCommand">
                        <Columns>
                            <asp:BoundField HeaderText="Número Plan de Negocio" DataField="Id_Proyecto" />
                            <asp:BoundField DataField="" />
                            <asp:TemplateField HeaderText="Nombre Plan de Negocio" >
                                <ItemStyle Width="400" />
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnnombreplan" runat="server" Text='<%# Eval("NomProyecto") + " (" + Eval("NomProyecto") + ")" %>' CommandArgument='<%# Eval("Id_ciudad") + ";" + Eval("id_Proyecto") %>' CssClass="boton_Link_Grid" />
                                    <%--<asp:Button Width="400" ID="btnnombreplan" runat="server" Text='<%# Eval("NomProyecto") + " (" + Eval("NomProyecto") + ")" %>' CommandArgument='<%# Eval("Id_ciudad") + ";" + Eval("id_Proyecto") %>' CssClass="boton_Link_Grid" />--%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Unidad de Emprendimiento Actual" DataField="NomUnidad" />
                            <asp:BoundField HeaderText="Operador" DataField="nomOperador" />
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </asp:Panel>

    <asp:Panel ID="pnltransaldar" runat="server" Width="100%">

        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <h1>
            <label>TRASLADO PLAN DE NEGOCIO</label><br>
            <asp:Label ID="lblnombreproyecto" runat="server" Text=""></asp:Label>
        </h1>
        <br />
        <table>
            <tr>
                <td colspan="2" style="text-align: center;" class="auto-style2">Datos Actuales</td>
            </tr>
            <tr>
                <td style="text-align: left; width: 40%;">Nombre Unidad:</td>
                <td style="text-align: center; width: 60%;">
                    <asp:TextBox ID="txtNombreUnidad" runat="server" Text="" Width="300px" ReadOnly="true"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: left; width: 40%;">Departamento:</td>
                <td style="text-align: center; width: 60%;">
                    <asp:TextBox ID="txtNombreDepto" runat="server" Text="" Width="300px" ReadOnly="true"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: left; width: 40%;">Ciudad:</td>
                <td style="text-align: center; width: 60%;">
                    <asp:TextBox ID="txtNombreCiudad" runat="server" Text="" Width="300px" ReadOnly="true"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: left; width: 40%;">Sector:</td>
                <td style="text-align: center; width: 60%;">
                    <asp:TextBox ID="txtNombreSector" runat="server" Text="" Width="300px" ReadOnly="true"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: left; width: 40%;">Subsector:</td>
                <td style="text-align: center; width: 60%;">
                    <asp:TextBox ID="txtNombreSubsector" runat="server" Text="" Width="300px" ReadOnly="true"></asp:TextBox>
                    <asp:TextBox ID="txtIdSubsector" runat="server" Text="" Width="300px" Visible="false" Enabled="false"></asp:TextBox>
                </td>
            </tr>
        </table>
        <br />
        <br />
        <table>
            <tr>
                <td colspan="2" style="text-align: center;">Datos Nuevos</td>
            </tr>
            <tr>
                <td style="text-align: left; width: 40%;">Nombre Unidad:</td>
                <td style="text-align: center; width: 60%;">
                    <asp:DropDownList ID="ddlnombreunidad" runat="server" Width="300px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="text-align: left; width: 40%;">Departamento:</td>
                <td style="text-align: center; width: 60%;">
                    <asp:DropDownList ID="ddldepartament" runat="server" Width="300px" OnChange="javascript:WebForm_DoPostBackWithOptions(new WebForm_PostBackOptions(this.name, '', true, '', '', false, true))" OnSelectedIndexChanged="ddldepartament_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="text-align: left; width: 40%;">Ciudad:</td>
                <td style="text-align: center; width: 60%;">

                    <asp:UpdatePanel ID="panelDropDowList" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlciudad" runat="server" Width="300px">
                            </asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddldepartament" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td style="text-align: left; width: 40%;">Sector:</td>
                <td style="text-align: center; width: 60%;">
                    <asp:DropDownList ID="ddlsector" runat="server" Width="300px" OnChange="javascript:WebForm_DoPostBackWithOptions(new WebForm_PostBackOptions(this.name, '', true, '', '', false, true))" OnSelectedIndexChanged="ddlsector_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="text-align: left; width: 40%;">Subsector:</td>
                <td style="text-align: center; width: 60%;">


                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlsubsector" runat="server" Width="300px">
                            </asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlsector" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align: right;">
                    <asp:Button ID="btntrasladar" runat="server" Text="Trasladar Plan de Negocio" OnClick="btntrasladar_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
