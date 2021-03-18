<%@ Page Title="FONDO EMPRENDER" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="ActasReporteFinalAcreditacion.aspx.cs" Inherits="Fonade.FONADE.evaluacion.ActasReporteFinalAcreditacion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }

        .sinlinea {
            border: none;
            background-color: none;
            border-collapse: collapse;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <asp:ObjectDataSource ID="ODS_Convocatoria" runat="server" SelectMethod="Convocatoria" TypeName="Fonade.FONADE.evaluacion.ActasReporteFinalAcreditacion"></asp:ObjectDataSource>

    <asp:ObjectDataSource ID="ODS_Acta" runat="server" SelectMethod="resultadoData" TypeName="Fonade.FONADE.evaluacion.ActasReporteFinalAcreditacion"></asp:ObjectDataSource>

    <table style="width: 100%;">
        <thead>
            <tr>
                <th style="text-align: left; padding-left: 50px">
                    <h1>
                        <asp:Label ID="L_ReportesEvaluacion" runat="server" Text="ACTAS REPORTE FINAL DE ACREDITACIÓN"></asp:Label>
                    </h1>
                </th>
            </tr>
        </thead>
        <tr>
            <td></td>
        </tr>
        <tr>
            <td>

                <asp:GridView ID="GV_Actas" runat="server" CssClass="Grilla" AutoGenerateColumns="False" Width="100%" DataSourceID="ODS_Acta" DataKeyNames="ID_CONVOCATORIA">
                    <Columns>
                        <asp:TemplateField HeaderText="Id">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("CODIGO") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="RPF_ID" runat="server" OnClick="RPF_ID_Click" Tex="">
                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("CODIGO") %>'></asp:Label>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Nombre">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("NOMACTA") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="RPF_NOMACTA" runat="server" OnClick="RPF_ID_Click" Tex="">
                                    <asp:Label ID="Label2" runat="server" Text='<%# Bind("NOMACTA") %>'></asp:Label>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Convocatoria">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("NOMCONVOCATORIA") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="RPF_NOMCONVOCATORIA" runat="server" OnClick="RPF_ID_Click" Tex="">
                                    <asp:Label ID="Label3" runat="server" Text='<%# Bind("NOMCONVOCATORIA") %>'></asp:Label>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Fecha de Creación">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("FECHACREACION") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="RPF_FECHACREACION" runat="server" OnClick="RPF_ID_Click" Tex="">
                                    <asp:Label ID="Label4" runat="server" Text='<%# Bind("FECHACREACION") %>'></asp:Label>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Fecha de Transmisión">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox5" runat="server" Text='<%# Bind("FECHATRANSMISION") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="RPF_FECHATRANSMISION" runat="server" OnClick="RPF_ID_Click" Tex="">
                                    <asp:Label ID="Label5" runat="server" Text='<%# Bind("FECHATRANSMISION") %>'></asp:Label>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="False">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox6" runat="server" Text='<%# Bind("ID_CONVOCATORIA") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="RPF_ID_CONVOCATORIA" runat="server" OnClick="RPF_ID_Click" Tex="">
                                    <asp:Label ID="Label6" runat="server" Text='<%# Bind("ID_CONVOCATORIA") %>'></asp:Label>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Exportar a Excel">
                            <ItemTemplate>
                                <asp:LinkButton ID="LB_Exportar" runat="server" Text="" OnClick="LB_Exportar_Click" CssClass="sinlinea">
                                    <asp:Image ID="I_Exportar" runat="server" ImageUrl="~/Images/IcoDocExel.gif" CssClass="sinlinea" />
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

            </td>
        </tr>
    </table>

    <div id="CrearApta" style="text-align: center;">
        <table style="margin: 0px auto; text-align: center; width: 100%">
            <tr>
                <td style="text-align: center; width: 100%;">
                    <asp:Button ID="B_CrearApta" runat="server" Text="CREAR ACTA" OnClick="B_CrearApta_Click" />
                </td>
            </tr>
        </table>
    </div>


    <asp:Panel ID="divCrearActa" runat="server" Visible="false" DefaultButton="B_Crear">
        <table class="auto-style1">
            <tr>
                <td style="text-align: center; width: 50%;">
                    <asp:Label ID="Label1" runat="server" Text="Nombre del Acta:"></asp:Label>
                </td>
                <td style="text-align: center; width: 50%;">

                    <asp:TextBox ID="TB_NombreActa" runat="server" Width="300px" ValidationGroup="CrearApta"></asp:TextBox>
                    <br />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TB_NombreActa" ErrorMessage="RequiredFieldValidator" ForeColor="Red" ValidationGroup="CrearApta">Campo Requerido</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; width: 50%;">
                    <asp:Label ID="Label2" runat="server" Text="Convocatoria:"></asp:Label>
                </td>
                <td style="text-align: center; width: 50%;">
                    <asp:DropDownList ID="DDL_Convocatoria" runat="server" Width="300px" DataSourceID="ODS_Convocatoria" DataTextField="NomConvocatoria" DataValueField="Id_Convocatoria" ValidationGroup="CrearApta">
                    </asp:DropDownList>
                    <br />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="DDL_Convocatoria" ErrorMessage="RequiredFieldValidator" ForeColor="Red" ValidationGroup="CrearApta">Campo Requerido</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; width: 50%;">
                    <asp:Button ID="B_Crear" runat="server" Text="Crear" OnClick="B_Crear_Click" ValidationGroup="CrearApta" />
                </td>
                <td style="text-align: center; width: 50%;">
                    <asp:Button ID="B_Ocultar" runat="server" Text="Ocultar" OnClick="B_Ocultar_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
