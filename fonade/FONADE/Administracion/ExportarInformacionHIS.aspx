<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="ExportarInformacionHIS.aspx.cs" Inherits="Fonade.FONADE.Administracion.ExportarInformacionHIS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        function alerta() {
            var sltConvocatoria = document.getElementById('bodyContentPlace_ddlconvocatoria');
            var valorConvocatoria = sltConvocatoria.options[sltConvocatoria.selectedIndex].text;

            var sltEtapa = document.getElementById('bodyContentPlace_ddlEtapa');
            var valorEtapa = sltEtapa.options[sltEtapa.selectedIndex].text;

            if (valorConvocatoria != "Seleccione") {
                return confirm('¿Está seguro de exportar la información histórica de ' + valorEtapa +
                    ' de la convocatoria ' + valorConvocatoria + ' ? '+
                    ', este proceso puede tardar varios minutos, por favor no cierre la ventana.');
            }
            else {
                alert('Debe seleccionar una convocatoria');
                return false;
            }


        }

        var ddlText, ddlValue, ddl, lblMesg;
        function CacheItems() {
            ddlText = new Array();
            ddlValue = new Array();
            ddl = document.getElementById("<%=ddlconvocatoria.ClientID %>");
            lblMesg = document.getElementById("<%=lblMessage.ClientID%>");
            for (var i = 0; i < ddl.options.length; i++) {
                ddlText[ddlText.length] = ddl.options[i].text;
                ddlValue[ddlValue.length] = ddl.options[i].value;
            }
        }
        window.onload = CacheItems;

        function FilterItems(value) {
            ddl.options.length = 0;
            for (var i = 0; i < ddlText.length; i++) {
                if (ddlText[i].toLowerCase().indexOf(value) != -1) {
                    AddItem(ddlText[i], ddlValue[i]);
                }
            }
            lblMesg.innerHTML = ddl.options.length + " item(s) encontrado(s).";
            if (ddl.options.length == 0) {
                AddItem("No se encontró el item.", "");
            }
        }

        function AddItem(text, value) {
            var opt = document.createElement("option");
            opt.text = text;
            opt.value = value;
            ddl.options.add(opt);
        }
    </script>

    <script type="text/javascript">


        function ShowProgress() {
            document.getElementById('<% Response.Write(UpdateProgress1.ClientID); %>').style.display = "inline";
        }


    </script>

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
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div>
        <asp:Label ID="lblOperador" runat="server" Text="Operador: "></asp:Label>
        <asp:DropDownList ID="ddlOperador"
            runat="server" AutoPostBack="true"
            OnSelectedIndexChanged="ddlOperador_SelectedIndexChanged">
        </asp:DropDownList>
    </div>
    <h1>
        <label>
            Exportación información Histórica</label>
    </h1>
    <div>
        <div>
            <asp:Label ID="Label13" runat="server" Font-Bold="True" Text="Filtrar: " />
            <asp:TextBox ID="txtSearch" runat="server" onkeyup="FilterItems(this.value)"></asp:TextBox>
            <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
        </div>
        <div>
            <asp:Label ID="lblConvocatoria" runat="server" Text="Convocatoria: "></asp:Label>
            <asp:DropDownList ID="ddlconvocatoria" runat="server" Width="300px" />
        </div>
        <div>
            <asp:Label ID="lblEtapa" runat="server" Text="Etapa: "></asp:Label>
            <asp:DropDownList ID="ddlEtapa" runat="server">
                <asp:ListItem Value="FOR">Formulación</asp:ListItem>
                <asp:ListItem Value="EVA">Evaluación</asp:ListItem>
                <asp:ListItem Value="INT">Interventoria</asp:ListItem>
            </asp:DropDownList>
        </div>
    </div>

    <asp:Button ID="btnMostrarConfirmacion" runat="server" Text="Exportar Informacion Histórica"
        OnClick="btnMostrarConfirmacion_Click" />

    <asp:UpdatePanel ID="upExportar" runat="server" ChildrenAsTriggers="False" UpdateMode="Conditional">
        <ContentTemplate>

            <div>

                <asp:Panel ID="pnlInfo" runat="server" Visible="false">
                    <div style="padding: 10px; border-radius: 10px; margin: 10px; background: #33b5e5">
                        <div>
                            <asp:Label ID="lblPanelInfo" runat="server" Text="Aviso"
                                ForeColor="White"></asp:Label>
                        </div>
                        <div>
                            <asp:Button ID="btnExportar" runat="server" Text="Generar Archivo"
                                OnClientClick="ShowProgress()" OnClick="btnExportar_Click" />
                        </div>
                    </div>

                </asp:Panel>

                <asp:Panel ID="pnlAvisoOK" runat="server" Visible="false">
                    <div style="padding: 10px; border-radius: 10px; margin: 10px; background: #00C851">
                        <asp:Label ID="lblAvisoOK" runat="server" Text="Aviso"
                            ForeColor="White"></asp:Label>
                    </div>
                </asp:Panel>

                <asp:Panel ID="pnlAvisoError" runat="server" Visible="false">
                    <div style="padding: 10px; border-radius: 10px; margin: 10px; background: #ff4444">
                        <asp:Label ID="lblAvisoError" runat="server" Text="Aviso"
                            ForeColor="White"></asp:Label>
                    </div>
                </asp:Panel>

            </div>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExportar" />
        </Triggers>
    </asp:UpdatePanel>

    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="upExportar">
        <ProgressTemplate>
            <div class="overlay" />
            <div class="overlayContent">

                <img src="../../Images/LoadSpin-1.1s-129px.gif" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

</asp:Content>
