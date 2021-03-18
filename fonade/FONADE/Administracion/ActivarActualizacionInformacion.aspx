<%@ Page Title="FONDO EMPRENDER" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true"
    CodeBehind="ActivarActualizacionInformacion.aspx.cs" Inherits="Fonade.FONADE.Administracion.ActivarActualizacionInformacion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <%--Información General--%>
    <h1>
        <asp:Label ID="lbl_enunciado" runat="server" Text="ACTIVACIÓN DE ACTUALIZACIÓN DE LA INFORMACIÓN" />
    </h1>
    <asp:Panel ID="pnlPrincipal" runat="server">
        <table width="95%" border="1" cellpadding="0" cellspacing="0" style="border-color: #4E77AF;">
            <tbody>
                <tr>
                    <td align="center" valign="top" width="98%">
                        <table width="95%" border="0" align="center" cellspacing="0" cellpadding="3">
                            <tbody>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr valign="top">
                                    <td class="TitDestacado" align="left">
                                        <b>Activar Actualizacion de informacion de los usuarios:</b>
                                    </td>
                                </tr>
                                <tr valign="top">
                                    <td class="TitDestacado" align="left">
                                        <p>
                                            <asp:CheckBox ID="chk_actualizarInfo" runat="server" Text="Activar:" TextAlign="Left" />
                                            &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;
                                            &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; Pedir actualización
                                            cada
                                            <asp:TextBox ID="txt_diasActualizacion" runat="server" Width="20px" MaxLength="2" />
                                            &nbsp;dias.
                                        </p>
                                    </td>
                                </tr>
                                <tr valign="top">
                                    <td align="right" class="TitDestacado">
                                        <asp:Button ID="btn_Actualizar" Text="Actualizar" runat="server" ToolTip="Actualizar"
                                            OnClick="btn_Actualizar_Click" />
                                    </td>
                                </tr>
                                <tr valign="top" id="alert_time" runat="server">
                                    <td style="color:red;text-align:left" >
                                        <b>Este proceso puede tardar varias horas. Por Favor, espere</b>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>
    </asp:Panel>
    <br />
    <div id="div_tabla" runat="server" style="margin-left: 15px;" visible="false">
        <table width='95%' border='0' cellspacing='0' cellpadding='2'>
            <tr>
                <td width='175' align='center' valign='baseline' bgcolor='#3D5A87' class='Blanca'>
                    <span style="color: White;">Se envió correo a los siguientes contactos</span>
                </td>
            </tr>
            <tr>
                <td>
                   

                
                <%--BackColor="#3D5A87" ForeColor="White"--%>
                <%-- <asp:UpdateProgress></asp:UpdateProgress> 
                <asp:GridView ID="gv_MensajesEnviados" runat="server" AutoGenerateColumns="true"
                    Width="400px" CssClass="Grilla" AllowPaging="True">
                    <PagerStyle CssClass="Paginador" />
                    <RowStyle HorizontalAlign="Left" />
                    <Columns>
                        <asp:TemplateField HeaderText="Grupo">
                            <ItemTemplate>
                                <asp:Label ID="lbl_nombreCompletoContacto" Text='<%#Eval("Grupo")%>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Nombre del Contacto">
                            <ItemTemplate>
                                <asp:Label ID="lbl_nombreCompletoContacto" Text='<%# Eval("Nombres")+ " " + Eval("Apellidos") %>'
                                    runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lbl_nombre" Text='<%#Eval("Nombres")%>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lbl_apellidos" Text='<%#Eval("Apellidos")%>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                --%>
                    </td>
            </tr>
        </table>
    </div>
    <%--<table width='95%' border='0' cellspacing='0' cellpadding='2'>
            <tr>
                <td width='175' colspan='2' align='center' valign='baseline' bgcolor='#3D5A87' class='Blanca'>
                    <span style="color: White;">Se envió correo a los siguientes contactos</span>
                </td>
            </tr>
            <tr bgcolor="#3d5a87">
                <td class="tituloTabla">
                    <span style="color: White;"><b>Grupo</b></span>
                </td>
                <td class="tituloTabla">
                    <span style="color: White;"><b>Nombre del Contacto</b></span>
                </td>
            </tr>
            <tr>
                <td>
                    NOMBRE DEL CONTACTO DE LA CONSULTA = [TRENVIOCORREO]
                </td>
                <td>
                    NOMBRE Y APELLIDO CONCATENADO
                </td>
            </tr>
        </table>--%>
</asp:Content>
