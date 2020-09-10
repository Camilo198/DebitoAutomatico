<%@ Page Title="" Language="C#" MasterPageFile="~/PaginaMaestra/Sitio.Master" AutoEventWireup="true" CodeBehind="ConvenioBancos.aspx.cs" Inherits="DebitoAutomatico.PS.Modulos.Configuracion.ConvenioBancos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphCabecera" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBarraHerramientas" runat="server">
    <table align="left" cellpadding="0" cellspacing="0">
        <tr>
            <td class="CuadranteBotonImagen">
                <asp:ImageButton ID="imgBtnNuevo" runat="server" ImageUrl="~/MarcaVisual/iconos/nuevo.png" Width="16px"
                    ToolTip="Nuevo..." OnClick="imgBtnNuevo_Click" />
            </td>
            <td class="CuadranteBotonImagen">
                <asp:ImageButton ID="imgBtnGuardar" runat="server"
                    ImageUrl="~/MarcaVisual/iconos/guardar.png" Width="16px"
                    ToolTip="Guardar" OnClick="imgBtnGuardar_Click" ValidationGroup="1" />
            </td>
        </tr>

    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphContenido" runat="server">
    <asp:UpdatePanel ID="upContenido" runat="server">
        <ContentTemplate>
            <table style="width: 100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="BarraSubTitulo">Convenio entre Bancos
                    </td>
                </tr>
                <tr>
                    <td class="SeparadorSubTitulo"></td>
                </tr>
            </table>
            <table style="width: 100%" class="ColorContenedorDatos" cellpadding="0" cellspacing="0">
                <tr>
                    <td style="height: 10px" colspan="3"></td>
                </tr>
                <tr>
                    <td style="width: 10px"></td>
                    <td>
                        <asp:Panel ID="pnlConvenio" runat="server" ScrollBars="Auto" Height="100%" Width="600px">
                            <asp:Panel ID="pnlBancos" CssClass="PanelBordesRedondos" runat="server" Width="99%">
                                <table style="width: 100%" cellpadding="0" cellspacing="2">
                                    <tr>
                                        <td class="LetraLeyendaColor" colspan="5">Bancos con Débito Automático
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 10px" colspan="5"></td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial"></td>
                                        <td class="EstiloEtiquetas80">
                                            <asp:Label ID="lbBancoDebito" runat="server" Text="Entidad Bancaria:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td>
                                            <asp:DropDownList ID="ddlBancoDebito" runat="server"
                                                CssClass="BordeListas" ValidationGroup="1"
                                                OnSelectedIndexChanged="ddlBancoDebito_SelectedIndexChanged" Width="150px"
                                                AutoPostBack="True">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="EspaciadoFinal"></td>
                                    </tr>

                   

                                      <tr>
                                        <td class="EspaciadoInicial"></td>
                                        <td class="EstiloEtiquetas80">
                                            <asp:Label ID="lblTipoProceso" runat="server" Text="Tipo Proceso:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td>
                                            <asp:DropDownList ID="ddlTipoProceso" runat="server" Enabled="false"
                                                CssClass="BordeListas" ValidationGroup="1" Width="150px"
                                                AutoPostBack="True" OnSelectedIndexChanged="ddlTipoProceso_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="EspaciadoFinal"></td>
                                    </tr>

                                    <tr>
                                        <td style="height: 10px" colspan="5"></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:RoundedCornersExtender ID="rceBancos" Radius="3" BorderColor="181, 198, 214"
                                TargetControlID="pnlBancos" runat="server" Enabled="True">
                            </asp:RoundedCornersExtender>
                            <br />
                            <asp:Panel ID="pnlBancosConvenio" CssClass="PanelBordesRedondos" runat="server" Width="99%">
                                <table style="width: 100%" cellpadding="0" cellspacing="2">
                                    <tr>
                                        <td class="LetraLeyendaColor" colspan="4">Bancos
                                        </td>
                                    </tr>
                                       <tr>
                                                <td class="EspaciadoInicial">
                                                </td>
                                                <td class="EstiloEtiquetas125Right">
                                                    <asp:Button ID="btnSeleccion" Visible="false" runat="server" Text="S/N" Height="27px" Width="42px"
                                                        OnClick="btnSeleccion_Click"/>
                                                </td>
                                                <td class="EspaciadoFinal">
                                                </td>
                                            </tr>
                                    <tr>
                                        <td class="EspaciadoInicial"></td>
                                        <td>
                                            <asp:GridView ID="gvConvenios" runat="server" AutoGenerateColumns="False" Width="99%"
                                                AllowSorting="True" BorderColor="#D0DEF0" BorderStyle="Solid" BorderWidth="1px"
                                                GridLines="Horizontal" CssClass="EstiloEtiquetas80">
                                                <Columns>
                                                    <asp:BoundField HeaderText="ID" DataField="pId" ItemStyle-CssClass="OcultarControles"
                                                        HeaderStyle-CssClass="OcultarControles">
                                                        <HeaderStyle CssClass="OcultarControles" />
                                                        <ItemStyle CssClass="OcultarControles" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="CODIGO" DataField="pCodigo">
                                                        <ControlStyle Width="80px" />
                                                        <HeaderStyle Width="30px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="BANCO" DataField="pNombre">
                                                        <ControlStyle Width="200px" />
                                                        <HeaderStyle Width="60px" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="CONVENIO">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chbConvenio" runat="server" />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="20px" />
                                                        <ItemStyle Width="20px" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <HeaderStyle BackColor="#C5C5C6" />
                                                <RowStyle HorizontalAlign="Center" BorderStyle="Solid" BorderWidth="1pt" />
                                                <PagerStyle BackColor="#C5C5C6" Font-Strikeout="False" Font-Underline="False" />
                                                <RowStyle BackColor="#F0F0F0" BorderColor="#D0DEF0" />
                                            </asp:GridView>
                                        </td>
                                        <td class="EspaciadoFinal"></td>
                                    </tr>
                                    <tr>
                                        <td style="height: 10px" colspan="3"></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:RoundedCornersExtender ID="rceBancosConvenio" Radius="3" BorderColor="181, 198, 214"
                                TargetControlID="pnlBancosConvenio" runat="server" Enabled="True">
                            </asp:RoundedCornersExtender>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBarraEstado" runat="server">
    <asp:Label ID="lbEstado" runat="server"></asp:Label>
</asp:Content>
