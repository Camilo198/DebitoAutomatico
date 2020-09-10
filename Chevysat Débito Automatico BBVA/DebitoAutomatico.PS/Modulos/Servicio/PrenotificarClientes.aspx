<%@ Page Title="" Language="C#" MasterPageFile="~/PaginaMaestra/Sitio.Master" AutoEventWireup="true"
    CodeBehind="PrenotificarClientes.aspx.cs" Inherits="DebitoAutomatico.PS.Modulos.Servicio.AutorizacionCuentas" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphCabecera" runat="server">
    <style type="text/css">
        .auto-style1 {
            width: 20px;
            height: 23px;
        }
        .auto-style2 {
            width: 15px;
            height: 23px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBarraHerramientas" runat="server">
    <table align="left" cellpadding="0" cellspacing="0">
        <tr>
            <td class="CuadranteBotonImagen">
                <asp:ImageButton ID="imgBtnNuevo" runat="server" ImageUrl="~/MarcaVisual/iconos/nuevo.png"
                    Width="16px" ToolTip="Nuevo..." OnClick="imgBtnNuevo_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphContenido" runat="server">
    <asp:UpdatePanel ID="upContenido" runat="server">
        <ContentTemplate>
            <table style="width: 100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="BarraSubTitulo">Autorización de Cuentas
                    </td>
                </tr>
                <tr>
                    <td class="SeparadorSubTitulo"></td>
                </tr>
            </table>
            <%-- <asp:UpdatePanel ID="upDebito" runat="server" UpdateMode="Conditional">
                <ContentTemplate>--%>
            <table style="width: 100%" class="ColorContenedorDatos" cellpadding="0" cellspacing="0">
                <tr>
                    <td style="height: 10px" colspan="3"></td>
                </tr>
                <tr>
                    <td style="width: 10px"></td>
                    <td>
                        <asp:Panel ID="pnlBanco" runat="server" ScrollBars="Auto" Height="100%" Width="900px">
                            <asp:Panel ID="pnlPrenotificar" CssClass="PanelBordesRedondos" runat="server" Width="99%">
                                <table style="width: 100%" cellpadding="0" cellspacing="2">
                                    <tr>
                                        <td class="LetraLeyendaColor" colspan="3">Contratos en Prenota:
                                                    <asp:Label ID="lbNPrenotificar" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial"></td>
                                        <td class="EstiloEtiquetas125Right">&nbsp;</td>
                                        <td class="EspaciadoFinal"></td>
                                    </tr>
                                 
                                    <tr>
                                        <td class="EspaciadoInicial"></td>
                                        <td class="EstiloEtiquetas80">
                                            <asp:Label ID="lbBancoDebito" runat="server" Text="Entidad Bancaria:"></asp:Label>&nbsp;
                                            <asp:DropDownList ID="ddlBancoDebito" runat="server"
                                                CssClass="BordeListas" 
                                                Width="150px" ValidationGroup="1" 
                                                AutoPostBack="True">
                                            </asp:DropDownList>
                                     <asp:RequiredFieldValidator ID="rfvBanco" runat="server" ErrorMessage="Favor seleccionar Banco!"
                                                ForeColor="Red" ControlToValidate="ddlBancoDebito" InitialValue="0" ValidationGroup="1"
                                                Text="*" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceBanco" runat="server" Enabled="True" TargetControlID="rfvBanco"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" PopupPosition="BottomLeft"
                                                HighlightCssClass="Resaltar">
                                            </asp:ValidatorCalloutExtender>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Label ID="lbFechaProceso" runat="server" Text="Fecha Proceso:"></asp:Label>
                                            <asp:TextBox ID="txbFechaInicial" runat="server" CssClass="FuenteDDL" TabIndex="2"
                                                ValidationGroup="1" Width="85px"></asp:TextBox>
                                            <asp:ImageButton ID="imgBtnFechaInicial" runat="server" ImageAlign="Middle" ImageUrl="~/MarcaVisual/iconos/calendario.png"
                                                Width="20px" />
                                            <asp:CalendarExtender ID="ceFechaInicial" runat="server" Enabled="True" Format="dd/MM/yyyy"
                                                PopupButtonID="imgBtnFechaInicial" TargetControlID="txbFechaInicial">
                                            </asp:CalendarExtender>
                                            <asp:MaskedEditExtender ID="meeFechaInicial" runat="server" InputDirection="LeftToRight"
                                                Mask="99/99/9999" MaskType="Date" TargetControlID="txbFechaInicial" UserDateFormat="DayMonthYear">
                                            </asp:MaskedEditExtender>
                                            <asp:RequiredFieldValidator ID="rfvFechaInicial" runat="server" ControlToValidate="txbFechaInicial"
                                                ErrorMessage="Favor digitar o seleccionar del calendariola fecha inicial!" ForeColor="Red"
                                                SetFocusOnError="true" Text="*" ValidationGroup="1">
                                            </asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceFechaInicial" runat="server" Enabled="True"
                                                HighlightCssClass="Resaltar" TargetControlID="rfvFechaInicial">
                                            </asp:ValidatorCalloutExtender>
                                       </td>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>


                                    </tr>
                                   
                                          
                                    <tr>
                                        <td class="EspaciadoInicial"></td>

                                        <td class="EstiloEtiquetas125Right">&nbsp;</td>
                                        <td class="EspaciadoFinal"></td>
                                    </tr>

                                    <tr>
                                        <td class="EspaciadoInicial"></td>
                                        <td style="height: 110px; width: 660px; overflow: auto;">
                                            <asp:GridView ID="gvPrenotificar" runat="server" AutoGenerateColumns="False" Width="99%"
                                                AllowSorting="True" BorderColor="#D0DEF0" BorderStyle="Solid" BorderWidth="1px"
                                                GridLines="None" CssClass="EstiloEtiquetas80">
                                                <Columns>
                                                    <asp:BoundField HeaderText="ID" DataField="ID" ItemStyle-CssClass="OcultarControles"
                                                        HeaderStyle-CssClass="OcultarControles">
                                                        <HeaderStyle CssClass="OcultarControles" />
                                                        <ItemStyle CssClass="OcultarControles" />
                                                    </asp:BoundField>
                                                    <%-- <asp:TemplateField HeaderText="CONTRATO">
                                                                <ControlStyle Width="80px" />
                                                                <HeaderStyle Width="30px" />
                                                                <ItemTemplate>
                                                                    <%# Eval("CONTRATO").ToString().Substring(0, (Eval("CONTRATO").ToString().Length )-1)%>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>--%>
                                                    <asp:BoundField HeaderText="CONTRATO" DataField="CONTRATO">
                                                        <ControlStyle Width="100px" />
                                                        <HeaderStyle Width="30px" />
                                                    </asp:BoundField>

                                                    <asp:BoundField HeaderText="IDCLIENTE" DataField="IDCLIENTE" ItemStyle-CssClass="OcultarControles"
                                                        HeaderStyle-CssClass="OcultarControles">
                                                        <HeaderStyle CssClass="OcultarControles" />
                                                        <ItemStyle CssClass="OcultarControles" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="TI" DataField="ABREVIATURA">
                                                        <ControlStyle Width="200px" />
                                                        <HeaderStyle Width="60px" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="IDENTIFICACION" DataField="NUMERO_IDENTIFICACION">
                                                        <ControlStyle Width="200px" />
                                                        <HeaderStyle Width="60px" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="NOMBRE DEL CLIENTE" DataField="CLIENTE">
                                                        <ControlStyle Width="200px" />
                                                        <HeaderStyle Width="60px" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="TC" DataField="VALOR">
                                                        <ControlStyle Width="100px" />
                                                        <HeaderStyle Width="60px" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="CUENTA" DataField="NUMERO_CUENTA">
                                                        <ControlStyle Width="200px" />
                                                        <HeaderStyle Width="60px" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>

                                                    <asp:BoundField HeaderText="BANCO" DataField="BANCO">
                                                        <ControlStyle Width="200px" />
                                                        <HeaderStyle Width="60px" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>

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
                                        <td class="EspaciadoInicial"></td>

                                        <td class="EstiloEtiquetas125Right">&nbsp;</td>
                                        <td class="EspaciadoFinal"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="6" style="text-align: center">
                                            <asp:Button ID="btnEnviar" runat="server" Text="Generar Prenotificación" Width="150px"
                                                OnClick="btnEnviar_Click" Height="31px" ValidationGroup="1" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 10px" colspan="3"></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:RoundedCornersExtender ID="rcePrenotificar" Radius="3" BorderColor="181, 198, 214"
                                TargetControlID="pnlPrenotificar" runat="server" Enabled="True">
                            </asp:RoundedCornersExtender>
                            <br />
                                       <asp:Panel ID="pnlPrenotificacionProceso" CssClass="PanelBordesRedondos" runat="server" Width="99%">
                                <table style="width: 100%" cellpadding="0" cellspacing="2">
                                    <tr>
                                        <td class="LetraLeyendaColor" colspan="3">Contratos Prenota en Proceso:
                                                    <asp:Label ID="lblPrenotificarProceso" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                 

                                    <tr>
                                        <td class="EspaciadoInicial"></td>

                                        <td class="EstiloEtiquetas125Right">&nbsp;</td>
                                        <td class="EspaciadoFinal"></td>
                                    </tr>

                                    <tr>
                                        <td class="EspaciadoInicial"></td>
                                        <td style="height: 110px; width: 660px; overflow: auto;">
                                            <asp:GridView ID="gvPrenotificarProceso" runat="server" AutoGenerateColumns="False" Width="99%"
                                                AllowSorting="True" BorderColor="#D0DEF0" BorderStyle="Solid" BorderWidth="1px"
                                                GridLines="None" CssClass="EstiloEtiquetas80">
                                                <Columns>
                                                    <asp:BoundField HeaderText="ID" DataField="ID" ItemStyle-CssClass="OcultarControles"
                                                        HeaderStyle-CssClass="OcultarControles">
                                                        <HeaderStyle CssClass="OcultarControles" />
                                                        <ItemStyle CssClass="OcultarControles" />
                                                    </asp:BoundField>
                                                    <%-- <asp:TemplateField HeaderText="CONTRATO">
                                                                <ControlStyle Width="80px" />
                                                                <HeaderStyle Width="30px" />
                                                                <ItemTemplate>
                                                                    <%# Eval("CONTRATO").ToString().Substring(0, (Eval("CONTRATO").ToString().Length )-1)%>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>--%>
                                                    <asp:BoundField HeaderText="CONTRATO" DataField="CONTRATO">
                                                        <ControlStyle Width="100px" />
                                                        <HeaderStyle Width="30px" />
                                                    </asp:BoundField>

                                                    <asp:BoundField HeaderText="IDCLIENTE" DataField="IDCLIENTE" ItemStyle-CssClass="OcultarControles"
                                                        HeaderStyle-CssClass="OcultarControles">
                                                        <HeaderStyle CssClass="OcultarControles" />
                                                        <ItemStyle CssClass="OcultarControles" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="TI" DataField="ABREVIATURA">
                                                        <ControlStyle Width="200px" />
                                                        <HeaderStyle Width="60px" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="IDENTIFICACION" DataField="NUMERO_IDENTIFICACION">
                                                        <ControlStyle Width="200px" />
                                                        <HeaderStyle Width="60px" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="NOMBRE DEL CLIENTE" DataField="CLIENTE">
                                                        <ControlStyle Width="200px" />
                                                        <HeaderStyle Width="60px" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="TC" DataField="VALOR">
                                                        <ControlStyle Width="100px" />
                                                        <HeaderStyle Width="60px" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="CUENTA" DataField="NUMERO_CUENTA">
                                                        <ControlStyle Width="200px" />
                                                        <HeaderStyle Width="60px" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>

                                                    <asp:BoundField HeaderText="BANCO" DataField="BANCO">
                                                        <ControlStyle Width="200px" />
                                                        <HeaderStyle Width="60px" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>

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
                            <asp:RoundedCornersExtender ID="rcePrenotificarProceso" Radius="3" BorderColor="181, 198, 214"
                                TargetControlID="pnlPrenotificacionProceso" runat="server" Enabled="True">
                            </asp:RoundedCornersExtender>
                            <br />
                           

                            <%-- <asp:RoundedCornersExtender ID="rceDebito" Radius="3" BorderColor="181, 198, 214"
                                        TargetControlID="pnlDebito" runat="server" Enabled="True">
                                    </asp:RoundedCornersExtender>--%>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <%--      </ContentTemplate>
            </asp:UpdatePanel>--%>
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
