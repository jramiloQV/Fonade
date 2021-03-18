<%@ Page Title="FONDO EMPRENDER" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="ReporteFinalAcreditacion.aspx.cs" Inherits="Fonade.FONADE.evaluacion.ReporteFinalAcreditacion" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
    .auto-style1 {
        width: 100%;
    }
</style>
    <script type="text/javascript">
        function ocultarMostar(Opt) {
            var obj = document.getElementById("");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">

    <table class="auto-style1">
        <thead>
            <tr>
                <th colspan="2" style=" text-align:left; padding-left:50px">
                    <h1>
                    <asp:Label ID="L_ReportesEvaluacion" runat="server" Text="ACREDITAR PLAN DE NEGOCIO"></asp:Label>
                        </h1>
                </th>
            </tr>
        </thead>
        
        <tr>
            <td>
                <br />
                <asp:Panel ID="P_puedeTransmitirse" runat="server" Visible="false" Width="70%">
                    <table class="auto-style1">
                        <tr style="vertical-align:top;">
                            <td>
                                <asp:Label ID="L_FechaTransmision" runat="server" Text="Fecha De Transmisión:"></asp:Label>
                            </td>
                            <td>
                                <asp:LinkButton ID="LB_Calendar" runat="server" Text="Calendario" OnClick="LB_Calendar_Click"></asp:LinkButton>
                                <asp:Calendar ID="C_calendar" runat="server" OnSelectionChanged="C_calendar_SelectionChanged" Visible="False" BackColor="White" BorderColor="Black" BorderStyle="Solid" CellSpacing="1" Font-Names="Verdana" Font-Size="9pt" ForeColor="Black" Height="16px" NextPrevFormat="ShortMonth" ShowGridLines="True" ShowNextPrevMonth="False" Width="16px">
                                    <DayHeaderStyle Font-Bold="True" Font-Size="8pt" ForeColor="#333333" Height="8pt" />
                                    <DayStyle BackColor="#CCCCCC" />
                                    <NextPrevStyle Font-Bold="True" Font-Size="8pt" ForeColor="White" />
                                    <OtherMonthDayStyle ForeColor="#999999" />
                                    <SelectedDayStyle BackColor="#333399" ForeColor="White" />
                                    <TitleStyle BackColor="#333399" BorderStyle="Solid" Font-Bold="True" Font-Size="12pt" ForeColor="White" Height="12pt" />
                                    <TodayDayStyle BackColor="#999999" ForeColor="White" />
                                </asp:Calendar>
                                <asp:LinkButton ID="LB_ocultar" runat="server" Text="Ocultar Calendario" OnClick="LB_ocultar_Click" Visible="false"></asp:LinkButton>
                            </td>
                            <td>
                                <asp:Label ID="L_Hora" runat="server" Text="Hora"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="DDL_Hora" runat="server">
                                    <asp:ListItem Value="01" Text="01"></asp:ListItem>
                                    <asp:ListItem Value="02" Text="02"></asp:ListItem>
                                    <asp:ListItem Value="03" Text="03"></asp:ListItem>
                                    <asp:ListItem Value="04" Text="04"></asp:ListItem>
                                    <asp:ListItem Value="05" Text="05"></asp:ListItem>
                                    <asp:ListItem Value="06" Text="06"></asp:ListItem>
                                    <asp:ListItem Value="07" Text="07"></asp:ListItem>
                                    <asp:ListItem Value="08" Text="08"></asp:ListItem>
                                    <asp:ListItem Value="09" Text="09"></asp:ListItem>
                                    <asp:ListItem Value="10" Text="10"></asp:ListItem>
                                    <asp:ListItem Value="11" Text="11"></asp:ListItem>
                                    <asp:ListItem Value="12" Text="12"></asp:ListItem>
                                    <asp:ListItem Value="13" Text="13"></asp:ListItem>
                                    <asp:ListItem Value="14" Text="14"></asp:ListItem>
                                    <asp:ListItem Value="15" Text="15"></asp:ListItem>
                                    <asp:ListItem Value="16" Text="16"></asp:ListItem>
                                    <asp:ListItem Value="17" Text="17"></asp:ListItem>
                                    <asp:ListItem Value="18" Text="18"></asp:ListItem>
                                    <asp:ListItem Value="19" Text="19"></asp:ListItem>
                                    <asp:ListItem Value="20" Text="20"></asp:ListItem>
                                    <asp:ListItem Value="21" Text="21"></asp:ListItem>
                                    <asp:ListItem Value="22" Text="22"></asp:ListItem>
                                    <asp:ListItem Value="23" Text="23"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="L_Minuto" runat="server" Text="Minuto"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="DDL_Minuto" runat="server">
                                    <asp:ListItem Value="01" Text="01"></asp:ListItem>
                                    <asp:ListItem Value="02" Text="02"></asp:ListItem>
                                    <asp:ListItem Value="03" Text="03"></asp:ListItem>
                                    <asp:ListItem Value="04" Text="04"></asp:ListItem>
                                    <asp:ListItem Value="05" Text="05"></asp:ListItem>
                                    <asp:ListItem Value="06" Text="06"></asp:ListItem>
                                    <asp:ListItem Value="07" Text="07"></asp:ListItem>
                                    <asp:ListItem Value="08" Text="08"></asp:ListItem>
                                    <asp:ListItem Value="09" Text="09"></asp:ListItem>
                                    <asp:ListItem Value="10" Text="10"></asp:ListItem>
                                    <asp:ListItem Value="11" Text="11"></asp:ListItem>
                                    <asp:ListItem Value="12" Text="12"></asp:ListItem>
                                    <asp:ListItem Value="13" Text="13"></asp:ListItem>
                                    <asp:ListItem Value="14" Text="14"></asp:ListItem>
                                    <asp:ListItem Value="15" Text="15"></asp:ListItem>
                                    <asp:ListItem Value="16" Text="16"></asp:ListItem>
                                    <asp:ListItem Value="17" Text="17"></asp:ListItem>
                                    <asp:ListItem Value="18" Text="18"></asp:ListItem>
                                    <asp:ListItem Value="19" Text="19"></asp:ListItem>
                                    <asp:ListItem Value="20" Text="20"></asp:ListItem>
                                    <asp:ListItem Value="21" Text="21"></asp:ListItem>
                                    <asp:ListItem Value="22" Text="22"></asp:ListItem>
                                    <asp:ListItem Value="23" Text="23"></asp:ListItem>
                                    <asp:ListItem Value="24" Text="24"></asp:ListItem>
                                    <asp:ListItem Value="25" Text="25"></asp:ListItem>
                                    <asp:ListItem Value="26" Text="26"></asp:ListItem>
                                    <asp:ListItem Value="27" Text="27"></asp:ListItem>
                                    <asp:ListItem Value="28" Text="28"></asp:ListItem>
                                    <asp:ListItem Value="29" Text="29"></asp:ListItem>
                                    <asp:ListItem Value="30" Text="30"></asp:ListItem>
                                    <asp:ListItem Value="31" Text="31"></asp:ListItem>
                                    <asp:ListItem Value="32" Text="32"></asp:ListItem>
                                    <asp:ListItem Value="33" Text="33"></asp:ListItem>
                                    <asp:ListItem Value="34" Text="34"></asp:ListItem>
                                    <asp:ListItem Value="35" Text="35"></asp:ListItem>
                                    <asp:ListItem Value="36" Text="36"></asp:ListItem>
                                    <asp:ListItem Value="37" Text="37"></asp:ListItem>
                                    <asp:ListItem Value="38" Text="38"></asp:ListItem>
                                    <asp:ListItem Value="39" Text="39"></asp:ListItem>
                                    <asp:ListItem Value="40" Text="40"></asp:ListItem>
                                    <asp:ListItem Value="41" Text="41"></asp:ListItem>
                                    <asp:ListItem Value="42" Text="42"></asp:ListItem>
                                    <asp:ListItem Value="43" Text="43"></asp:ListItem>
                                    <asp:ListItem Value="44" Text="44"></asp:ListItem>
                                    <asp:ListItem Value="45" Text="45"></asp:ListItem>
                                    <asp:ListItem Value="46" Text="46"></asp:ListItem>
                                    <asp:ListItem Value="47" Text="47"></asp:ListItem>
                                    <asp:ListItem Value="48" Text="48"></asp:ListItem>
                                    <asp:ListItem Value="49" Text="49"></asp:ListItem>
                                    <asp:ListItem Value="50" Text="50"></asp:ListItem>
                                    <asp:ListItem Value="51" Text="51"></asp:ListItem>
                                    <asp:ListItem Value="52" Text="52"></asp:ListItem>
                                    <asp:ListItem Value="53" Text="53"></asp:ListItem>
                                    <asp:ListItem Value="54" Text="54"></asp:ListItem>
                                    <asp:ListItem Value="55" Text="55"></asp:ListItem>
                                    <asp:ListItem Value="56" Text="56"></asp:ListItem>
                                    <asp:ListItem Value="57" Text="57"></asp:ListItem>
                                    <asp:ListItem Value="58" Text="58"></asp:ListItem>
                                    <asp:ListItem Value="59" Text="59"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="B_Enviar" runat="server" Text="Enviar" OnClick="B_Enviar_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td style="width:100%; text-align:center;">
                <br />
                <asp:LinkButton ID="LB_Exportar" runat="server" Text="Exportar a Excel" OnClick="LB_Exportar_Click"></asp:LinkButton>
                <br />
                <br />
            </td>
        </tr>
        <tr>
            <td>
                <br />
                <div style="width:735px; overflow:scroll;height:auto;">
                    <asp:Panel ID="P_tabla" runat="server" Width="100%">

                    </asp:Panel>
                </div>
            </td>
        </tr>
    </table>

</asp:Content>