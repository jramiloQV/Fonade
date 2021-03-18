<%@ Page Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="AsignarProyectosAcreditadores.aspx.cs"
    Inherits="Fonade.FONADE.evaluacion.AsignarProyectosAcreditadores" %>

<%--<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="bodyContentPlace">
    <asp:ScriptManager ID="scr_1" runat="server" />
    <asp:UpdatePanel ID="updtnl_1" runat="server">
        <ContentTemplate>
            <table width="98%" border="0" cellspacing="0" cellpadding="2">
                <tbody>
                    <tr>
                        <td colspan="2">
                            <h1>
                                <asp:Label ID="L_ReportesEvaluacion" runat="server" Text="PLANES DE NEGOCIO PARA ACREDITACIÓN" /></h1>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <table border="0" cellpadding="2">
                                <tbody>
                                    <tr>
                                        <td align="left" width="10%">
                                            <asp:Label ID="L_Convocatoria" runat="server" Text="Convocatoria:" />&nbsp;
                                        </td>
                                        <td width="80%">
                                            &nbsp;<asp:DropDownList ID="DDL_Convocatoria" runat="server" AutoPostBack="true"
                                                Width="400px" OnSelectedIndexChanged="DDL_Convocatoria_SelectedIndexChanged" />
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                    <tr id="tr_1" runat="server" visible="false">
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr id="tr_2" runat="server">
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr id="tr_txt_buscar_plan" runat="server" visible="false">
                        <td>
                            <strong>BUSCAR PLAN DE NEGOCIOS</strong>
                        </td>
                    </tr>
                    <tr id="tr_textbox_buscar_plan" runat="server" visible="false">
                        <td width="10%">
                            <asp:TextBox ID="TB_Buscar" runat="server" Width="170px" ToolTip="Puede buscar el código o el nombre del plan."
                                MaxLength="30" />
                        </td>
                        <td>
                            &nbsp;<asp:Button ID="Buscar" runat="server" Text="Buscar" OnClick="Buscar_Click"
                                ValidationGroup="Buscar" />
                        </td>
                    </tr>
                    <tr id="tr_3" runat="server" visible="false">
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr id="tr_btn_seleccionar" runat="server" visible="false">
                        <td colspan="2">
                            <asp:Button ID="B_SeleccionarAcreditador" runat="server" Text="Seleccionar Acreditador"
                                OnClick="B_SeleccionarAcreditador_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr id="tr_control_abc" runat="server" visible="false">
                        <td colspan="2" align="center">
                            <asp:LinkButton ID="lnkbtn_opcion_A" Text="A" runat="server" OnClick="lnkbtn_opcion_A_Click"
                                ForeColor="Black" Style="text-decoration: none;" />&nbsp;
                            <asp:LinkButton ID="lnkbtn_opcion_B" Text="B" runat="server" OnClick="lnkbtn_opcion_B_Click"
                                ForeColor="Black" Style="text-decoration: none;" />&nbsp;
                            <asp:LinkButton ID="lnkbtn_opcion_C" Text="C" runat="server" OnClick="lnkbtn_opcion_C_Click"
                                ForeColor="Black" Style="text-decoration: none;" />&nbsp;
                            <asp:LinkButton ID="lnkbtn_opcion_D" Text="D" runat="server" OnClick="lnkbtn_opcion_D_Click"
                                ForeColor="Black" Style="text-decoration: none;" />&nbsp;
                            <asp:LinkButton ID="lnkbtn_opcion_E" Text="E" runat="server" OnClick="lnkbtn_opcion_E_Click"
                                ForeColor="Black" Style="text-decoration: none;" />&nbsp;
                            <asp:LinkButton ID="lnkbtn_opcion_F" Text="F" runat="server" OnClick="lnkbtn_opcion_F_Click"
                                ForeColor="Black" Style="text-decoration: none;" />&nbsp;
                            <asp:LinkButton ID="lnkbtn_opcion_G" Text="G" runat="server" OnClick="lnkbtn_opcion_G_Click"
                                ForeColor="Black" Style="text-decoration: none;" />&nbsp;
                            <asp:LinkButton ID="lnkbtn_opcion_H" Text="H" runat="server" OnClick="lnkbtn_opcion_H_Click"
                                ForeColor="Black" Style="text-decoration: none;" />&nbsp;
                            <asp:LinkButton ID="lnkbtn_opcion_I" Text="I" runat="server" OnClick="lnkbtn_opcion_I_Click"
                                ForeColor="Black" Style="text-decoration: none;" />&nbsp;
                            <asp:LinkButton ID="lnkbtn_opcion_J" Text="J" runat="server" OnClick="lnkbtn_opcion_J_Click"
                                ForeColor="Black" Style="text-decoration: none;" />&nbsp;
                            <asp:LinkButton ID="lnkbtn_opcion_K" Text="K" runat="server" OnClick="lnkbtn_opcion_K_Click"
                                ForeColor="Black" Style="text-decoration: none;" />&nbsp;
                            <asp:LinkButton ID="lnkbtn_opcion_L" Text="L" runat="server" OnClick="lnkbtn_opcion_L_Click"
                                ForeColor="Black" Style="text-decoration: none;" />&nbsp;
                            <asp:LinkButton ID="lnkbtn_opcion_M" Text="M" runat="server" OnClick="lnkbtn_opcion_M_Click"
                                ForeColor="Black" Style="text-decoration: none;" />&nbsp;
                            <asp:LinkButton ID="lnkbtn_opcion_N" Text="N" runat="server" OnClick="lnkbtn_opcion_N_Click"
                                ForeColor="Black" Style="text-decoration: none;" />&nbsp;
                            <asp:LinkButton ID="lnkbtn_opcion_O" Text="O" runat="server" OnClick="lnkbtn_opcion_O_Click"
                                ForeColor="Black" Style="text-decoration: none;" />&nbsp;
                            <asp:LinkButton ID="lnkbtn_opcion_P" Text="P" runat="server" OnClick="lnkbtn_opcion_P_Click"
                                ForeColor="Black" Style="text-decoration: none;" />&nbsp;
                            <asp:LinkButton ID="lnkbtn_opcion_Q" Text="Q" runat="server" OnClick="lnkbtn_opcion_Q_Click"
                                ForeColor="Black" Style="text-decoration: none;" />&nbsp;
                            <asp:LinkButton ID="lnkbtn_opcion_R" Text="R" runat="server" OnClick="lnkbtn_opcion_R_Click"
                                ForeColor="Black" Style="text-decoration: none;" />&nbsp;
                            <asp:LinkButton ID="lnkbtn_opcion_S" Text="S" runat="server" OnClick="lnkbtn_opcion_S_Click"
                                ForeColor="Black" Style="text-decoration: none;" />&nbsp;
                            <asp:LinkButton ID="lnkbtn_opcion_T" Text="T" runat="server" OnClick="lnkbtn_opcion_T_Click"
                                ForeColor="Black" Style="text-decoration: none;" />&nbsp;
                            <asp:LinkButton ID="lnkbtn_opcion_U" Text="U" runat="server" OnClick="lnkbtn_opcion_U_Click"
                                ForeColor="Black" Style="text-decoration: none;" />&nbsp;
                            <asp:LinkButton ID="lnkbtn_opcion_V" Text="V" runat="server" OnClick="lnkbtn_opcion_V_Click"
                                ForeColor="Black" Style="text-decoration: none;" />&nbsp;
                            <asp:LinkButton ID="lnkbtn_opcion_W" Text="W" runat="server" OnClick="lnkbtn_opcion_W_Click"
                                ForeColor="Black" Style="text-decoration: none;" />&nbsp;
                            <asp:LinkButton ID="lnkbtn_opcion_X" Text="X" runat="server" OnClick="lnkbtn_opcion_X_Click"
                                ForeColor="Black" Style="text-decoration: none;" />&nbsp;
                            <asp:LinkButton ID="lnkbtn_opcion_Y" Text="Y" runat="server" OnClick="lnkbtn_opcion_Y_Click"
                                ForeColor="Black" Style="text-decoration: none;" />&nbsp;
                            <asp:LinkButton ID="lnkbtn_opcion_Z" Text="Z" runat="server" OnClick="lnkbtn_opcion_Z_Click"
                                ForeColor="Black" Style="text-decoration: none;" />&nbsp; <strong style="color: Black;">
                                    |</strong>
                            <asp:LinkButton ID="lnkbtn_opcion_todos" Text="Todos" runat="server" OnClick="lnkbtn_opcion_todos_Click"
                                ForeColor="Black" Style="text-decoration: none;" />
                        </td>
                    </tr>
                    <tr id="tr_grilla" runat="server" visible="false">
                        <td colspan="2">
                            <asp:GridView ID="GV_COnvocatoria" runat="server" CssClass="Grilla" Width="100%"
                                AutoGenerateColumns="False" AllowPaging="True" EnableTheming="True" DataKeyNames="CODIGO"
                                OnRowDataBound="GV_COnvocatoria_RowDataBound" AllowSorting="True" 
                                OnSorting="GV_COnvocatoria_Sorting" 
                                onpageindexchanging="GV_COnvocatoria_PageIndexChanging">
                                <RowStyle VerticalAlign="Top" HorizontalAlign="Left" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Id" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Left">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("CODIGO") %>' ToolTip="Seleccionar Acreditador." />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LB_Codigo" runat="server" Text='<%# Bind("CODIGO") %>' OnClick="LB_Codigo_Click"
                                                ToolTip="Seleccionar Acreditador." Style="text-decoration: none;" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CB_Seleccionado" runat="server" TextAlign="Left" ToolTip="Seleccionar este plan." />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Nombre" ItemStyle-Width="30%" ItemStyle-HorizontalAlign="Left"
                                        SortExpression="NOMPROYECTO">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("NOMPROYECTO") %>' ToolTip="Seleccionar Acreditador." />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LB_NomProyecto" runat="server" Text='<%# Bind("NOMPROYECTO") %>'
                                                OnClick="LB_NomProyecto_Click" ToolTip="Seleccionar Acreditador." Style="text-decoration: none;" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Acreditador" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Left"
                                        SortExpression="Acreditador">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("Acreditador") %>' ToolTip="Seleccionar Acreditador." />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LB_Acreditador" runat="server" Text='<%# Bind("Acreditador") %>'
                                                OnClick="LB_Acreditador_Click" ToolTip="Seleccionar Acreditador." Style="text-decoration: none;" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Nombre Acreditador" ItemStyle-Width="30%" ItemStyle-HorizontalAlign="Left"
                                        SortExpression="NOMACREDITADOR">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("NOMACREDITADOR") %>' ToolTip="Seleccionar Acreditador." />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LB_NomAcreditador" runat="server" Text='<%# Bind("NOMACREDITADOR") %>'
                                                OnClick="LB_NomAcreditador_Click" ToolTip="Seleccionar Acreditador." Style="text-decoration: none;" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Fecha de Asignación / Reasignación" ItemStyle-Width="25%"
                                        ItemStyle-HorizontalAlign="Left" SortExpression="FechaAsignacion">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox5" runat="server" Text='<%# Bind("FechaAsignacion") %>' ToolTip="Seleccionar Acreditador." />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LB_FechaAsignacion" runat="server" Text='<%# Bind("a_FechaAsignacion") %>'
                                                OnClick="LB_FechaAsignacion_Click" ToolTip="Seleccionar Acreditador." Style="text-decoration: none;" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </tbody>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Buscar" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="B_SeleccionarAcreditador" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
