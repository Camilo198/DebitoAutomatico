<%@ Page Title="" Language="C#" MasterPageFile="~/PaginaMaestra/Sitio.Master" AutoEventWireup="true" CodeBehind="Reporte_Pagos.aspx.cs" Inherits="DebitoAutomatico.PS.Modulos.Servicio.Reportes_Pagos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphCabecera" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBarraHerramientas" runat="server">
    <asp:UpdatePanel ID="UpdatePanelHerramientas" runat="server">
        <ContentTemplate>
            <table align="left" cellpadding="0" cellspacing="0">
            </table>
            <asp:UpdateProgress ID="UpdateProgressPanelHerramientas" runat="server" AssociatedUpdatePanelID="UpdatePanelHerramientas">
                <ProgressTemplate>
                    <div class="contenedor">
                        <div class="centrado">
                            <div class="contenido">
                                <asp:Image ID="ImagePanelHerramientas" runat="server" ImageUrl="~/MarcaVisual/iconos/loading.gif"
                                    Height="20px" Width="100px" ImageAlign="Middle" />
                            </div>
                        </div>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphContenido" runat="server">
    <asp:UpdatePanel ID="upContenido" runat="server">
        <ContentTemplate>
            <table style="width: 100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="BarraSubTitulo">Reportes Pagos
                    </td>
                </tr>
                <tr>
                    <td class="SeparadorSubTitulo"></td>
                </tr>
            </table>
            <table style="width: 100%" class="ColorContenedorDatos" cellpadding="0" cellspacing="0">
                <tr>
                    <td style="height: 10px" colspan="3">&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 10px"></td>
                    <td>
                        <asp:Panel ID="pnlRespuestasTransacciones" runat="server" ScrollBars="Auto" Height="100%" Width="900px">

                            <asp:Panel ID="pnlFiltros" CssClass="PanelBordesRedondos" runat="server" Width="99%">
                                <table style="width: 100%" cellpadding="0" cellspacing="2">

                                    <tr>
                                        <td style="height: 10px" colspan="13" align ="center">
                                            <asp:Button ID="btnRptPagos" runat="server" Text="Reportes de Pagos" OnClick="btnReportePagos_Click" Font-Size="Small" Height="44px" Width="171px" />
                                        </td>
                                    </tr>

                                </table>
                            </asp:Panel>
                            <asp:RoundedCornersExtender ID="rceRutas" Radius="3" BorderColor="181, 198, 214"
                                TargetControlID="pnlFiltros" runat="server" Enabled="True">
                            </asp:RoundedCornersExtender>
                            <br />

                        </asp:Panel>
                    </td>
                </tr>
            </table>

            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="upContenido">
                <ProgressTemplate>
                    <div class="contenedor">
                        <div class="centrado">
                            <div class="contenido" style="width: 100px; height: 20px">
                                <asp:Image ID="Image1" runat="server" ImageUrl="~/MarcaVisual/iconos/loading.gif"
                                    Height="20px" Width="100px" ImageAlign="Middle" />
                            </div>
                        </div>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBarraEstado" runat="server">
    <asp:Label ID="lbEstado" runat="server"></asp:Label>
</asp:Content>
