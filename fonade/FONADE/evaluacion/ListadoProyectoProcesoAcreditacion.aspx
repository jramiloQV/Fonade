<%@ Page Title="FONDO EMPRENDER" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true"
    CodeBehind="ListadoProyectoProcesoAcreditacion.aspx.cs" Inherits="Fonade.FONADE.evaluacion.ListadoProyectoProcesoAcreditacion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/Site.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <h1>
        Reporte Planes de Acreditación</h1>
    <br />
    <table width="100%">
        <tr>
            <td>
            </td>
            <td>
                <asp:Label ID="Label1" runat="server" Font-Bold="True" ForeColor="Black" Text="Convocatoria"></asp:Label>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                <asp:DropDownList ID="ddlConvocatoria" runat="server" Height="22px" Width="715px">
                </asp:DropDownList>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                <table width="100%">
                    <tr>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="Número del Plan de negocio" ForeColor="Black"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label3" runat="server" Text="Nombre del Plan de Negocio" ForeColor="Black"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label4" runat="server" Text="Estado del Plan de negocio" ForeColor="Black"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtcodigoproyecto" runat="server" Width="180px" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtnombreproyecto" runat="server" Width="180px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlestados" runat="server" Height="22px" Width="200px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label5" runat="server" Text="Nombre del emprendedor" ForeColor="Black"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label6" runat="server" Text="Apellidos del emprendedor" ForeColor="Black"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label7" runat="server" Text="Documento del Emplendedor" ForeColor="Black"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtnombreemprendedor" runat="server" Width="180px" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtapellidosemprendedor" runat="server" Width="180px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtdocemprendedor" runat="server" Width="180px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td align="center">
                            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Buscar" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td align="center">
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
    <table width="100%">
        <tr>
            <td>
                <asp:Panel ID="PnlProyectos" runat="server">
                    <asp:GridView ID="GrvConvocatorias" runat="server" Width="100%" AutoGenerateColumns="False"
                        CssClass="Grilla" EmptyDataText="No se encontraron Datos" AllowPaging="True"
                        OnPageIndexChanging="GrvConvocatoriasPageIndexChanging" OnRowCommand="GrvConvocatoriaRowcommad">
                        <Columns>
                            <asp:BoundField DataField="CodProyecto" HeaderText="No." />
                            <asp:TemplateField HeaderText="Plan de Negocio">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkconvocatoria" runat="server" CausesValidation="False" CommandArgument='<%# Eval("CodProyecto") + ";" + Eval("CodConvocatoria")  %>'
                                        CommandName="proyecto" Text='<%# Eval("NomProyecto") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="NomConvocatoria" HeaderText="Convocatoria" />
                            <asp:BoundField DataField="NombreEmprendedor" HeaderText="Nombre Emprendedor" />
                            <asp:BoundField DataField="ApellidosEmprendedor" HeaderText="Apellidos Emprendedor" />
                            <asp:BoundField DataField="Email" HeaderText="Email" />
                            <asp:BoundField DataField="NomEstado" HeaderText="Estado Plan" />
                        </Columns>
                        <PagerStyle CssClass="Paginador" />
                    </asp:GridView>
                </asp:Panel>
            </td>
        </tr>
    </table>
</asp:Content>
