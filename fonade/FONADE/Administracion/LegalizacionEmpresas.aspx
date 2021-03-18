<%@ Page Language="C#" MasterPageFile="~/Master.master" EnableEventValidation="false"
    AutoEventWireup="true" CodeBehind="LegalizacionEmpresas.aspx.cs" Inherits="Fonade.FONADE.Administracion.LegalizacionEmpresas" %>

<asp:Content ID="head1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        td {
            vertical-align: top;
        }

        #bodyContentPlace_gvplanesnegocio tr:nth-child(n+2) td:nth-child(2) input[type="submit"] {
            text-align: left !important;
        }
    </style>

    <style>
        .overlay {
            position: fixed;
            z-index: 98;
            top: 0px;
            left: 0px;
            right: 0px;
            bottom: 0px;
            background-color: #fff;
            filter: alpha(opacity=80);
            opacity: 0.8;
        }

        .overlayContent {
            z-index: 99;
            margin: 250px auto;
            width: 80px;
            height: 80px;
        }

            .overlayContent h2 {
                font-size: 18px;
                font-weight: bold;
                color: #000;
            }

            .overlayContent img {
                width: 80px;
                height: 80px;
            }
    </style>

</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="bodyContentPlace">

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div>
        <asp:Label ID="lblOperador" runat="server" Text="Operador: "></asp:Label>
        <asp:DropDownList ID="ddlOperador" 
            runat="server"  AutoPostBack="true"
            OnSelectedIndexChanged="ddlOperador_SelectedIndexChanged"></asp:DropDownList>
    </div>
    <h1>
        <label>
            PLANES DE NEGOCIO - CONTRATOS</label>
    </h1>
    <br />
    <br />
    <table id="GrillaPlanes" runat="server">
        <tr>
            <td>
                <div style="overflow-y: scroll;">
                    <asp:GridView ID="gvplanesnegocio" runat="server" CssClass="Grilla" AutoGenerateColumns="false"
                        Width="100%" OnRowCommand="gvplanesnegocio_RowCommand" DataKeyNames="id_proyecto">
                        <Columns>
                            <asp:BoundField HeaderText="ID" DataField="ID_Proyecto" />
                            <asp:TemplateField HeaderText="Nombre">
                                <ItemTemplate>
                                    <asp:Button ID="btnproyectoframesset" runat="server" Text='<%# Eval("NomProyecto") %>'
                                        CommandArgument='<%# Eval("id_proyecto") %>' CssClass="boton_Link_Grid" CommandName="proyectoframeset"
                                        Width="200px" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Documentación">
                                <ItemTemplate>
                                    <table style="text-align: center">
                                        <tr>
                                            <td>Garantía
                                            </td>
                                            <td>Pagaré
                                            </td>
                                            <td>Contrato
                                            </td>
                                            <td>Plan Operativo
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="cbxgarantia" runat="server" Checked='<%# Eval("Garantia") %>' />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="cbxpagare" runat="server" Checked='<%# Eval("Pagare") %>' />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="cbxcontrato" runat="server" Checked='<%# Eval("Contrato") %>' />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="cbxplanoperativo" runat="server" Checked='<%# Eval("Contrato") %>' />
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Empresa" DataField="NomProyecto" />
                            <asp:TemplateField HeaderText="Legalizado">
                                <ItemTemplate>
                                    <table style="text-align: center">
                                        <tr>
                                            <td>SI
                                            </td>
                                            <td>NO
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rbtnlegalizadosi" runat="server" Checked='<%# Eval("Legalizado") %>' GroupName="lealizado" />
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rbtnlegalizadono" runat="server" GroupName="lealizado" />
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Operador" DataField="NombreOperador" />
                        </Columns>
                    </asp:GridView>
                </div>
            </td>
        </tr>
    </table>
    <table id="GrillaActaLegalizacion" runat="server">
        <tr>
            <td colspan="2">
                <h2>Detalles del Memorando:</h2>
            </td>
        </tr>
        <tr>
            <td>Número Memorando:
            </td>
            <td>
                <asp:TextBox ID="txtnummemorando" runat="server" Text="" Width="300px"></asp:TextBox>
                <br />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtnummemorando"
                    ErrorMessage="RequiredFieldValidator" ForeColor="Red" ValidationGroup="actulaizar">Debe Ingresar un valor de Numero Memorando</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>Nombre Memorando:
            </td>
            <td>
                <asp:TextBox ID="txtnommemorando" runat="server" Text="" Width="300px" />
                <br />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtnommemorando"
                    ErrorMessage="RequiredFieldValidator" ForeColor="Red" ValidationGroup="actulaizar">Debe Ingresar un valor de Nombre Memorando</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>Fecha Memorando:
            </td>
            <td>
                <%--<asp:Calendar ID="cldmemorando" runat="server" CssClass="Grilla" Height="16px" Width="16px"></asp:Calendar>--%>
                <asp:DropDownList ID="dd_fecha_dias_Memorando" runat="server">
                    <asp:ListItem Text="1" Value="1" />
                    <asp:ListItem Text="2" Value="2" />
                    <asp:ListItem Text="3" Value="3" />
                    <asp:ListItem Text="4" Value="4" />
                    <asp:ListItem Text="5" Value="5" />
                    <asp:ListItem Text="6" Value="6" />
                    <asp:ListItem Text="7" Value="7" />
                    <asp:ListItem Text="8" Value="8" />
                    <asp:ListItem Text="9" Value="9" />
                    <asp:ListItem Text="10" Value="10" />
                    <asp:ListItem Text="11" Value="11" />
                    <asp:ListItem Text="12" Value="12" />
                    <asp:ListItem Text="13" Value="13" />
                    <asp:ListItem Text="14" Value="14" />
                    <asp:ListItem Text="15" Value="15" />
                    <asp:ListItem Text="16" Value="16" />
                    <asp:ListItem Text="17" Value="17" />
                    <asp:ListItem Text="18" Value="18" />
                    <asp:ListItem Text="19" Value="19" />
                    <asp:ListItem Text="20" Value="20" />
                    <asp:ListItem Text="20" Value="20" />
                    <asp:ListItem Text="21" Value="21" />
                    <asp:ListItem Text="22" Value="22" />
                    <asp:ListItem Text="23" Value="23" />
                    <asp:ListItem Text="24" Value="24" />
                    <asp:ListItem Text="25" Value="25" />
                    <asp:ListItem Text="26" Value="26" />
                    <asp:ListItem Text="27" Value="27" />
                    <asp:ListItem Text="28" Value="28" />
                    <asp:ListItem Text="29" Value="29" />
                    <asp:ListItem Text="30" Value="30" />
                    <asp:ListItem Text="31" Value="31" />
                </asp:DropDownList>
                &nbsp;
                <asp:DropDownList ID="dd_fecha_mes_Memorando" runat="server">
                    <asp:ListItem Text="Ene" Value="1" />
                    <asp:ListItem Text="Feb" Value="2" />
                    <asp:ListItem Text="Mar" Value="3" />
                    <asp:ListItem Text="Abr" Value="4" />
                    <asp:ListItem Text="May" Value="5" />
                    <asp:ListItem Text="Jun" Value="6" />
                    <asp:ListItem Text="Jul" Value="7" />
                    <asp:ListItem Text="Ago" Value="8" />
                    <asp:ListItem Text="Sep" Value="9" />
                    <asp:ListItem Text="Oct" Value="10" />
                    <asp:ListItem Text="Nov" Value="11" />
                    <asp:ListItem Text="Dic" Value="12" />
                </asp:DropDownList>
                &nbsp;
                <asp:DropDownList ID="dd_fecha_year_Memorando" runat="server" />
            </td>
        </tr>
        <tr>
            <td>Convocatoria:
            </td>
            <td>
                <asp:DropDownList ID="ddlconvocatoria" runat="server" Width="300px" />
                <br />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlconvocatoria"
                    ErrorMessage="RequiredFieldValidator" ForeColor="Red" ValidationGroup="actulaizar">Debe Selecionar una convocatoria</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>Observaciones:
            </td>
            <td>
                <asp:TextBox ID="txtobservaciones" runat="server" Text="" TextMode="MultiLine" Width="300px"
                    Height="100px"></asp:TextBox>
                <br />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtobservaciones"
                    ErrorMessage="RequiredFieldValidator" ForeColor="Red" ValidationGroup="actulaizar">Debe Ingresar comentarios</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="text-align: center;">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:Button ID="btnactualizar" runat="server" Text="Crear" OnClick="btnactualizar_Click"
                            ValidationGroup="actulaizar" />
                    </ContentTemplate>
                </asp:UpdatePanel>

            </td>
        </tr>
    </table>

    <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Button ID="Button1" runat="server" Text="Click Me" onclick="Button1_Click" />
        </ContentTemplate>
    </asp:UpdatePanel>--%>

    <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="UpdatePanel1">
        <ProgressTemplate>
            <div class="overlay" />
            <div class="overlayContent">
                <%--<h2>Cargando...</h2>                --%>
                <img src="../../Images/LoadSpin-1.1s-129px.gif" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

</asp:Content>
