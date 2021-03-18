<%@ Page Language="C#" MasterPageFile="~/Master.master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="CarguesContratos.aspx.cs" Inherits="Fonade.FONADE.Administracion.CarguesContratos" %>

<asp:Content ID="head1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        table {
            width: 100%;
        }

        td {
            vertical-align: top;
        }

        .formatoTabla {
            text-align: center;
        }
    </style>
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="bodyContentPlace">
    <h1>
        <label>CARGUE MASIVO DE CONTRATOS</label>
    </h1>
    <br />
    <br />
    <asp:Panel ID="pnlCargar" runat="server">
        <table>
            <tr>
                <td>Seleccione el archivo:</td>
            </tr>
            <tr>
                <td>
                    <asp:FileUpload ID="fld_cargar" runat="server" Width="400px" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btncargar" runat="server" Text="Subir Archivo" OnClick="btncargar_Click" />
                </td>
            </tr>
            <tr>
                <td>
                    <br />
                    <p>
                        Solo se debe subir el archivo de Cargo Masivo de contratos. Con el siguiente formato
                        (colocar solo las filas de valores, incluir una sola fila de titulos, el nombre de la hoja electrónica debe ser
                        cargue, El formato de la fecha debe ser mm/dd/aaaa):
                    </p>
                </td>
            </tr>
        </table>

        <br />
        <br />

        <table class="Grilla formatoTabla">
            <thead>
                <tr>
                    <th>Numero Campo</th>
                    <th>Nombre Campo</th>
                    <th>Ejemplo</th>
                </tr>
            </thead>

            <tbody>

                <tr>
                    <td>1</td>
                    <td>Id_Proyecto</td>
                    <td>1</td>
                </tr>
                <tr>
                    <td>2</td>
                    <td>Proyecto</td>
                    <td>TOMILLO Y ESPECIAS</td>
                </tr>
                <tr>
                    <td>3</td>
                    <td>No CONTRATO de COLABORACIÓN EMPRESARIAL</td>
                    <td>20611222</td>
                </tr>
                <tr>
                    <td>4</td>
                    <td>OBJETO</td>
                    <td>FINANCIAR LA INICIATIVA EMPRESARIAL CONTENIDA EN EL PLAN DE NEGOCIOS TOMILLO Y ESPECIAS PRESENTADO POR EL BENEFICIARIO Y APROBADO POR EL CONSEJO DIRECTIVO DEL SENA</td>
                </tr>
                <tr>
                    <td>5</td>
                    <td>Valor Inicial en $</td>
                    <td>42.788.889</td>
                </tr>
                <tr>
                    <td>6</td>
                    <td>PLAZO EN MESES DEL CTTO</td>
                    <td>12</td>
                </tr>
                <tr>
                    <td>7</td>
                    <td>NUMERO DEL AP PRESUPUESTAL</td>
                    <td>8175</td>
                </tr>
                <tr>
                    <td>8</td>
                    <td>FECHA DEL AP</td>
                    <td>04/11/2005</td>
                </tr>
                <tr>
                    <td>9</td>
                    <td>Fecha Firma del Contrato</td>
                    <td>04/10/2005</td>
                </tr>
                <tr>
                    <td>10</td>
                    <td>No Póliza de Seguro de Vida</td>
                    <td>012588850</td>
                </tr>
                <tr>
                    <td>11</td>
                    <td>Compañía Seguro de Vida</td>
                    <td>2228200AP</td>
                </tr>
                <tr>
                    <td>12</td>
                    <td>Fecha de Acta de Inicio</td>
                    <td>05/11/2006</td>
                </tr>
            </tbody>
        </table>

    </asp:Panel>
</asp:Content>
