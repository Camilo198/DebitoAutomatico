<%@ Page Title="" Language="C#" MasterPageFile="~/PaginaMaestra/Sitio.Master" AutoEventWireup="true" CodeBehind="ReanudarDebito.aspx.cs" Inherits="DebitoAutomatico.PS.Modulos.Configuracion.ReanudarDebito" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphCabecera" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBarraHerramientas" runat="server">
    <table align="left" cellpadding="0" cellspacing="0">
        </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphContenido" runat="server">
    <asp:UpdatePanel ID="upContenido" runat="server">
        <ContentTemplate>
            <table style="width: 100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="BarraSubTitulo">Reanudación de debito
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
                        <asp:Panel ID="pnlRespuestasTransacciones" runat="server" ScrollBars="Auto" Height="100%" Width="900px">
                            <asp:Panel ID="pnlBanco" CssClass="PanelBordesRedondos" runat="server" Width="99%">
                                <table style="width: 100%" cellpadding="0" cellspacing="2">
                                    <tr>
                                        <td class="LetraLeyendaColor" colspan="9">Reanudar
                                    </tr>
                                    <tr>
                                        <td style="height: 10px" colspan="9"></td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial"></td>
                                        <td class="EstiloEtiquetas80">
                                            <asp:Button ID="btnReanudarDebito" runat="server" Text="Reanudar Debito:" OnClick="btnReanudarDebito_Click" />
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="EspaciadoCeldaControl">
   
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td style="width: 20%" class="EstiloEtiquetas80">
                                            
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="EspaciadoCeldaControl">
                                            
                                        </td>
                                        <td class="EspaciadoFinal"></td>
                                    </tr>
                                    <tr>
                                        <td style="height: 10px" colspan="9"></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:RoundedCornersExtender ID="rceBanco" Radius="3" BorderColor="181, 198, 214"
                                TargetControlID="pnlBanco" runat="server" Enabled="True">
                            </asp:RoundedCornersExtender>
                            <br />
                            <asp:Panel ID="pnlRespuestas" CssClass="PanelBordesRedondos" runat="server" Width="99%">
                                <table style="width: 100%" cellpadding="0" cellspacing="2">
                                    <tr>
                                        <td class="LetraLeyendaColor" colspan="9">Historico de Movimientos
                                        </td>
                                    </tr>
                                    <tr>
                                         <td colspan="9" style="height: 10px"></td>
                                    </tr>
                                    <tr>
                                    </tr>
                                    <tr>
                                        <td style="height: 10px" colspan="9">
                                            <asp:GridView ID="gvReanudacion" runat="server" AllowPaging="True" AllowSorting="True"
                                AutoGenerateColumns="False" BorderColor="#D0DEF0" BorderStyle="Solid" BorderWidth="1px"
                                GridLines="Horizontal" PageSize="9" Width="100%" CssClass="EstiloEtiquetas80"
                                OnPageIndexChanging="gvReanudacion_PageIndexChanging">
                                <Columns>
                                    <asp:BoundField HeaderText="ID" DataField="ID" ItemStyle-CssClass="OcultarControles"
                                        HeaderStyle-CssClass="OcultarControles">
                                        <HeaderStyle CssClass="OcultarControles" />
                                        <ItemStyle CssClass="OcultarControles" />
                                    </asp:BoundField>
                                     <asp:BoundField DataField="MES_REANUDADO" HeaderText="Mes Reanudado" >
                                         <HeaderStyle Width="150px" HorizontalAlign="Center" />
                                         <ItemStyle HorizontalAlign="Center" />
                                     </asp:BoundField>

                                      <asp:BoundField DataField="AÑO_REANUDADO" HeaderText="Año Reanudado" >
                                         <HeaderStyle Width="150px" HorizontalAlign="Center" />
                                         <ItemStyle HorizontalAlign="Center" />
                                     </asp:BoundField>

                                <asp:BoundField DataField="USUARIO" HeaderText="Usuario Proceso" >
                                         <HeaderStyle Width="100px" HorizontalAlign="Center" />
                                         <ItemStyle HorizontalAlign="Center" />
                                     </asp:BoundField>
                                          <asp:BoundField DataField="FECHA_PROCESO" HeaderText="Fecha Proceso" >
                                         <HeaderStyle Width="50px" HorizontalAlign="Center" />
                                         <ItemStyle HorizontalAlign="Center" />
                                     </asp:BoundField>
                                    <asp:BoundField DataField="OBSERVACIONES" HeaderText="Observaciones" >
                                         <HeaderStyle Width="50px" HorizontalAlign="Center" />
                                         <ItemStyle HorizontalAlign="Center" />
                                     </asp:BoundField>
                                           <asp:BoundField HeaderText="ESTADO" DataField="ESTADO" ItemStyle-CssClass="OcultarControles"
                                        HeaderStyle-CssClass="OcultarControles">
                                        <HeaderStyle CssClass="OcultarControles" />
                                        <ItemStyle CssClass="OcultarControles" />
                                    </asp:BoundField>
                                </Columns>
                                <HeaderStyle BackColor="#C5C5C6" />
                                <PagerStyle BackColor="#C5C5C6" Font-Strikeout="False" Font-Underline="False" />
                                <RowStyle BackColor="#F0F0F0" BorderColor="#D0DEF0" HorizontalAlign="Center" />
                            </asp:GridView>

                                        </td>
                                    </tr>
          
                                    <tr>
                                        <td style="height: 10px" colspan="9"></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:RoundedCornersExtender ID="rceRutas" Radius="3" BorderColor="181, 198, 214"
                                TargetControlID="pnlRespuestas" runat="server" Enabled="True">
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
