<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImpresionMetasSociales.ascx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.Controles.ImpresionPlan.ImpresionMetasSociales" %>
<asp:Panel ID="pnlPrincipal" Visible="true" runat="server">
    <br />
    <% Page.DataBind(); %>
    <table width="780" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td width="19">&nbsp;
            </td>
            <td width="761">
                <table width='95%' border='0' cellspacing='0' cellpadding='0'>
                    <tr>
                        <td colspan="2">
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 50%">

                                        <label>
                                            Metas Sociales del Plan de Negocio
                                        </label>
                                    </td>

                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>

                        <td colspan="2">
                            <table border="0">
                                <tr>
                                    <td>
                                        <label>Plan Nacional de Desarrollo</label>
                                    </td>
                                </tr>
                            </table>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="txtPlanNacional" runat="server" Width="810px" Height="100px" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table border="0">
                                <tr>
                                    <td><label> Plan Regional de Desarrollo</label>
                                    </td>
                                </tr>
                            </table>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:label ID="txtPlanRegional" runat="server" Width="810px" Height="100px"/>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table border="0">
                                <tr>
                                    <td><label>Cluster o Cadena Productiva</label>
                                    </td>
                                </tr>
                            </table>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="txtCluster" runat="server" Width="810px" Height="100px"/>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table border="0">
                                <tr>
                                    <td><label>Empleo</label>
                                    </td>
                                </tr>
                            </table>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table id="tabla_1" runat="server" width='850px' border='0' cellspacing='1' cellpadding='4'>
                                <tr bgcolor='#3D5A87'>
                                    <td style="color: White;" align="center">Empleos Directos
                                    </td>
                                    <td colspan="2">&nbsp;
                                    </td>
                                    <td align="center" style="color: White;">Jovenes
                                    </td>
                                    <td colspan="7" align="center" style="color: White;">Población Vulnerable
                                    </td>
                                </tr>
                                <tr bgcolor='#3D5A87'>
                                    <td style="color: White; width: 140px; font-size: 10px">Cargo
                                    </td>
                                    <td style="color: White; width: 170px; font-size: 10px">Sueldo Mes
                                    </td>
                                    <td style="color: White; width: 100px; font-size: 10px">Generado en el Primer Año
                                    </td>
                                    <td style="color: White; width: 60px; font-size: 10px">Edad entre 18 y 24 años
                                    </td>
                                    <td style="color: White; width: 60px; font-size: 10px">Desplazado por la violencia
                                    </td>
                                    <td style="color: White; width: 60px; font-size: 10px">Madre Cabeza de Familia
                                    </td>
                                    <td style="color: White; width: 60px; font-size: 10px">Minoría Etnica (Indigena o Negritud)
                                    </td>
                                    <td style="color: White; width: 60px; font-size: 10px">Recluido Carceles INPEC
                                    </td>
                                    <td style="color: White; width: 60px; font-size: 10px">Desmovilizado o Reinsertado
                                    </td>
                                    <td style="color: White; width: 60px; font-size: 10px">Discapacitado
                                    </td>
                                    <td style="color: White; width: 60px; font-size: 10px">Desvinculado de Entidades del Estado
                                    </td>
                                </tr>
                            </table>
                            <asp:Label ID="Label1" runat="server" Text="Personal Calificado" CssClass="TitulosRegistrosGrilla"></asp:Label>
                            <asp:GridView ID="gw_Empleos" runat="server" Width="850px" AutoGenerateColumns="False" DataKeyNames="IdCargo"
                                CssClass="Grilla" ShowHeader="False">
                                <Columns>
                                    <asp:BoundField DataField="Cargo" HeaderText="Cargo" ItemStyle-Width="140px" />
                                    <asp:BoundField DataField="ValorMensual" HeaderText="Sueldo Mes" ItemStyle-Width="80px" DataFormatString="{0:c}" />
                                    <asp:TemplateField HeaderText="Generado en el Primer Año" ItemStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlGeneradoMes" runat="server" AppendDataBoundItems="True" SelectedValue='<%# !Convert.IsDBNull(Eval("GeneradoPrimerAnio"))? ((string)Eval("GeneradoPrimerAnio")=="0" ? ((bool)Eval( "EmprendedorFormulacion") == false ? String.Empty : (string)Eval("GeneradoPrimerAnio")) : (string)Eval("GeneradoPrimerAnio")) : String.Empty %>' Enabled="false">
                                                <asp:ListItem Text="Seleccione" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="" Value=""></asp:ListItem>
                                                <asp:ListItem Value="1" Text="Mes 1"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="Mes 2"></asp:ListItem>
                                                <asp:ListItem Value="3" Text="Mes 3"></asp:ListItem>
                                                <asp:ListItem Value="4" Text="Mes 4"></asp:ListItem>
                                                <asp:ListItem Value="5" Text="Mes 5"></asp:ListItem>
                                                <asp:ListItem Value="6" Text="Mes 6"></asp:ListItem>
                                                <asp:ListItem Value="7" Text="Mes 7"></asp:ListItem>
                                                <asp:ListItem Value="8" Text="Mes 8"></asp:ListItem>
                                                <asp:ListItem Value="9" Text="Mes 9"></asp:ListItem>
                                                <asp:ListItem Value="10" Text="Mes 10"></asp:ListItem>
                                                <asp:ListItem Value="11" Text="Mes 11"></asp:ListItem>
                                                <asp:ListItem Value="12" Text="Mes 12"></asp:ListItem>
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Edad entre 18 y 24 años">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chkEdad18_24" Checked='<%# Convert.IsDBNull(Eval("EsJoven"))?false:(bool?) Eval("EsJoven") %>' onclick="return false;"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Desplazado por la violencia">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chkDesplazado" Checked='<%# Convert.IsDBNull(Eval("EsDesplazado"))?false:(bool?) Eval("EsDesplazado") %>' onclick="return false;" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Madre Cabeza de Familia">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chkMadreCabeza" Checked='<%# Convert.IsDBNull(Eval("EsMadre"))?false:(bool?) Eval("EsMadre") %>' onclick="return false;" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Minoría Etnica (Indigena o Negritud)	">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chkMinoriaEtnica" Checked='<%# Convert.IsDBNull(Eval("EsMinoria"))?false:(bool?) Eval("EsMinoria") %>' onclick="return false;" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Recluido Carceles INPEC">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chkRecluidoCarceles" Checked='<%# Convert.IsDBNull(Eval("EsRecluido"))?false:(bool?) Eval("EsRecluido") %>' onclick="return false;" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Desmovilizado o Reinsertado">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chkDesmovilizado" Checked='<%# Convert.IsDBNull(Eval("EsDesmovilizado"))?false:(bool?) Eval("EsDesmovilizado") %>' onclick="return false;" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Discapacitado">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chkDiscapacitado" Checked='<%# Convert.IsDBNull(Eval("EsDiscapacitado"))?false:(bool?) Eval("EsDiscapacitado") %>' onclick="return false;" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Desvinculado de Entidades del Estado">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chkDesvinculado" Checked='<%# Convert.IsDBNull(Eval("EsDesvinculado"))?false:(bool?) Eval("EsDesvinculado") %>' Enabled="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:Label ID="Label_ManoObra" runat="server" Text="Mano de Obra Directa" CssClass="TitulosRegistrosGrilla" Visible="false"></asp:Label>
                            <asp:GridView ID="gw_ManoObra" runat="server" Width="850px" AutoGenerateColumns="false" DataKeyNames="IdCargo"
                                CssClass="Grilla" ShowHeader="false">
                                <Columns>
                                    <asp:BoundField DataField="Cargo" HeaderText="Cargo" ItemStyle-Width="140px">
                                        <ItemStyle Width="140px" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Sueldo" ItemStyle-Width="80px">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblSueldo" Width="80px" Text='<%# !Convert.IsDBNull(Eval( "ValorMensual"))?Eval( "ValorMensual"): "0" %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="80px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Generado en el Primer Año" ItemStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlGeneradoMes" runat="server" AppendDataBoundItems="True" SelectedValue='<%# !Convert.IsDBNull(Eval("GeneradoPrimerAnio"))? ((string)Eval("GeneradoPrimerAnio")=="0" ? ((bool)Eval( "EmprendedorFormulacion") == false ? String.Empty : (string)Eval("GeneradoPrimerAnio")) : (string)Eval("GeneradoPrimerAnio")) : String.Empty %>' Enabled="false" TabIndex="0">
                                                <asp:ListItem Text="Seleccione" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="" Value=""></asp:ListItem>
                                                <asp:ListItem Value="1" Text="Mes 1"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="Mes 2"></asp:ListItem>
                                                <asp:ListItem Value="3" Text="Mes 3"></asp:ListItem>
                                                <asp:ListItem Value="4" Text="Mes 4"></asp:ListItem>
                                                <asp:ListItem Value="5" Text="Mes 5"></asp:ListItem>
                                                <asp:ListItem Value="6" Text="Mes 6"></asp:ListItem>
                                                <asp:ListItem Value="7" Text="Mes 7"></asp:ListItem>
                                                <asp:ListItem Value="8" Text="Mes 8"></asp:ListItem>
                                                <asp:ListItem Value="9" Text="Mes 9"></asp:ListItem>
                                                <asp:ListItem Value="10" Text="Mes 10"></asp:ListItem>
                                                <asp:ListItem Value="11" Text="Mes 11"></asp:ListItem>
                                                <asp:ListItem Value="12" Text="Mes 12"></asp:ListItem>
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                        <ItemStyle Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Edad entre 18 y 24 años">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chkEdad18_24" Checked='<%# Convert.IsDBNull(Eval("EsJoven"))?false:(bool?) Eval("EsJoven") %>' onclick="return false;" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Desplazado por la violencia">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chkDesplazado" Checked='<%# Convert.IsDBNull(Eval("EsDesplazado"))?false:(bool?) Eval("EsDesplazado") %>' onclick="return false;" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Madre Cabeza de Familia">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chkMadreCabeza" Checked='<%# Convert.IsDBNull(Eval("EsMadre"))?false:(bool?) Eval("EsMadre") %>' onclick="return false;" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Minoría Etnica (Indigena o Negritud)	">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chkMinoriaEtnica" Checked='<%# Convert.IsDBNull(Eval("EsMinoria"))?false:(bool?) Eval("EsMinoria") %>' onclick="return false;" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Recluido Carceles INPEC">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chkRecluidoCarceles" Checked='<%# Convert.IsDBNull(Eval("EsRecluido"))?false:(bool?) Eval("EsRecluido") %>' onclick="return false;" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Desmovilizado o Reinsertado">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chkDesmovilizado" Checked='<%# Convert.IsDBNull(Eval("EsDesmovilizado"))?false:(bool?) Eval("EsDesmovilizado") %>' onclick="return false;" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Discapacitado">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chkDiscapacitado" Checked='<%# Convert.IsDBNull(Eval("EsDiscapacitado"))?false:(bool?) Eval("EsDiscapacitado") %>' onclick="return false;" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Desvinculado de Entidades del Estado">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chkDesvinculado" Checked='<%# Convert.IsDBNull(Eval("EsDesvinculado"))?false:(bool?) Eval("EsDesvinculado") %>' onclick="return false;" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <br />
                            <br />
                            <span class="TitulosRegistrosGrilla">EMPLEOS A GENERAR EN EL PRIMER AÑO:
                                        <asp:Label ID="primer_ano" runat="server"></asp:Label></span>
                            <br />
                            <span class="TitulosRegistrosGrilla">EMPLEOS A GENERAR EN LA TOTALIDAD DEL PROYECTO:
                                        <asp:Label ID="Total_empleos" runat="server"></asp:Label></span>
                            <br />
                            <br />
                            Empleos Indirectos:
                                <asp:label runat="server" ID="txtEmpleosIndirectos" Width="30px" />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table border="0">
                                <tr>
                                    <td><label>Emprendedores</label> 
                                    </td>
                                </tr>
                            </table>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:GridView ID="gw_emprendedores" runat="server" Width="600px" AutoGenerateColumns="false" CssClass="Grilla"
                                RowStyle-Height="35px" DataKeyNames="Id_Contacto">
                                <Columns>
                                    <asp:BoundField DataField="nombres" HeaderText="Nombre" />
                                    <asp:TemplateField HeaderText="Beneficiario del Fondo emprender" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="70" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chkBeneficiario" Checked='<%# Eval("Beneficiario") %>' onclick="return false;" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Participación Accionaria (%)" HeaderStyle-Width="70" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblParticipacion" runat="server" Text='<%# Eval("Participacion") %>' ></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br />
    <br />
    <br />
</asp:Panel>
