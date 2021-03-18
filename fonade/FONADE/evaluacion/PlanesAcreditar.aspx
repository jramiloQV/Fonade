<%@ Page Title="FONDO EMPRENDER" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true"
    CodeBehind="PlanesAcreditar.aspx.cs" Inherits="Fonade.FONADE.evaluacion.PlanesaAcreditar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <asp:Panel ID="PnlProyectos" runat="server">
        <h1>
            PLANES DE NEGOCIO A ACREDITAR
        </h1>
        <asp:GridView ID="GrvPlanesAcreditar" runat="server" Width="100%" AutoGenerateColumns="False"
            CssClass="Grilla" EmptyDataText="No se encontraron Datos" AllowPaging="True"
            OnPageIndexChanging="GrvPlanesAcreditarPageIndexChanging" 
            OnRowCommand="GrvPlanesAcreditarRowcommad" 
            onsorting="GrvPlanesAcreditarSorting">
            <Columns>
                <asp:TemplateField HeaderText="Nombre">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkconvocatoria" runat="server" CausesValidation="False" CommandArgument='<%# Eval("ID_PROYECTO") %>'
                            CommandName="frameset" Text='<%# Eval("ID_PROYECTO") + "-" +  Eval("NOMPROYECTO") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="FechaAsignacion" HeaderText="Fecha de Asignación" />
                <asp:BoundField DataField="Dias" HeaderText="Días transcurridos asignación" />
                <asp:BoundField DataField="Estado" HeaderText="Estado" />
                <asp:TemplateField HeaderText="Acreditar">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkproyectoAcreditacion" runat="server" CausesValidation="False"
                            CommandArgument='<%# Eval("ID_PROYECTO")  + "-" +  Eval("CODCONVOCATORIA")   %>'
                            CommandName="acreditar" Text='Acreditar' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <PagerStyle CssClass="Paginador" />
        </asp:GridView>
    </asp:Panel>
</asp:Content>
