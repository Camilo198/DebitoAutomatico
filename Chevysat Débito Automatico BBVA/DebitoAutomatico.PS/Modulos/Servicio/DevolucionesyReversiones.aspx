<%@ Page Title="" Language="C#" MasterPageFile="~/PaginaMaestra/Sitio.Master" AutoEventWireup="true" CodeBehind="DevolucionesyReversiones.aspx.cs" Inherits="DebitoAutomatico.PS.Modulos.Servicio.DevolucionesyReversiones" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphCabecera" runat="server">
    <style type="text/css">
        .auto-style1 {
            height: 10px;
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
                    <td class="BarraSubTitulo">
                        Devolución y Reversiones
                    </td>
                </tr>
                <tr>
                    <td class="SeparadorSubTitulo">
                    </td>
                </tr>
            </table>
            <table style="width: 100%" class="ColorContenedorDatos" cellpadding="0" cellspacing="0">
                <tr>
                    <td style="height: 10px" colspan="3">
                    </td>
                </tr>
                <tr>
                    <td style="width: 10px">
                    </td>
                    <td>
                        <asp:Panel ID="pnlReversiones" runat="server" ScrollBars="Auto" Height="100%" Width="1000px">

                                               <asp:Panel ID="pnlProceso"  CssClass="PanelBordesRedondos" runat="server" Width="99%">
                                <table style="width: 100%" cellpadding="0" cellspacing="2">

                                    <tr>
                                        <td style="height: 10px" colspan="13">
                                        </td>
                                    </tr>


                                           <tr>
                                        <td class="EspaciadoInicial">
                                        </td>
                                        <td class="EstiloEtiquetas80">
                                            <asp:Label ID="lblProceso" runat="server" Text="Proceso:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td>
                                             <asp:DropDownList ID="ddlProceso" AutoPostBack="true" runat="server" CssClass="BordeListas" ValidationGroup="1"
                                                Width="150px" OnSelectedIndexChanged="ddlProceso_SelectedIndexChanged"  >
                                                <asp:ListItem Value="0">[Seleccione]</asp:ListItem>
                                                <asp:ListItem Value="1">Devolución</asp:ListItem>
                                                <asp:ListItem Value="2">Reversiones</asp:ListItem>
                                            </asp:DropDownList>
                                               
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td class="EstiloEtiquetas125Right">
                                            
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td>

                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td class="EstiloEtiquetas125Right">
                                  
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                            
                                        </td>
                                        <td>
                                          
                                        </td>
                                        <td class="EspaciadoFinal">
                                        </td>
                                    </tr>

                                             <tr>
                                        <td class="auto-style1" colspan="13">
                                        </td>
                                        
         
                                    </tr>
                                                           
                               
                                </table>
                            </asp:Panel>
                            <asp:RoundedCornersExtender ID="RoundedCornersExtender1" Radius="3" BorderColor="181, 198, 214"
                                TargetControlID="pnlProceso" runat="server" Enabled="True">
                            </asp:RoundedCornersExtender>
                            <br />
                            <asp:Panel ID="pnlFiltros" Visible="false" CssClass="PanelBordesRedondos" runat="server" Width="99%">
                                <table style="width: 100%" cellpadding="0" cellspacing="2">
                                  

                               
                                  
                                    <tr>
                                        <td style="height: 10px" colspan="13">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial">
                                        </td>
                                        <td class="EstiloEtiquetas80">
                                            <asp:Label ID="lblBancoDebito" runat="server" Text="Banco convenio:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td>
                                               <asp:DropDownList ID="ddlBancoDebito" runat="server" CssClass="BordeListas" ValidationGroup="1"
                                                Width="150px">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                         <td class="EstiloEtiquetas80">
                                            <asp:Label ID="lblConvenio" runat="server" Text="Banco del cliente:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlConvenios" runat="server" CssClass="BordeListas" Width="150px">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                         <td class="EstiloEtiquetas80">
                                            <asp:Label ID="lblEstadoDebito" runat="server" Text="Estado Debito:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlEstadoDebito" runat="server" CssClass="BordeListas"
                                                Width="150px">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="EspaciadoFinal">
                                        </td>
                                    </tr>
                                          <tr>
                                        <td style="height: 10px" colspan="13">
                                        </td>
                                    </tr>                    
                                
                                      <tr>
                                        <td class="EspaciadoInicial">
                                        </td>
                                        <td class="EstiloEtiquetas80">
                                            <asp:Label ID="lblContrato" runat="server" Text="Contrato:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td>
                                               <asp:TextBox ID="txbContrato" runat="server" CssClass="FuenteDDL" TabIndex="2"
                                               Width="85px"></asp:TextBox>
                                            
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                         <td class="EstiloEtiquetas80">
                                            <asp:Label ID="lbBancoDebito" runat="server" Text="Fecha del debito:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td>
                                               <asp:TextBox ID="txbFechaInicial" runat="server" CssClass="FuenteDDL" TabIndex="2"
                                               Width="85px"></asp:TextBox>
                                            <asp:ImageButton ID="imgBtnFechaInicial" runat="server" ImageAlign="Middle" ImageUrl="~/MarcaVisual/iconos/calendario.png"
                                                Width="20px" />
                                            <asp:CalendarExtender ID="ceFechaInicial" runat="server" Enabled="True" Format="dd/MM/yyyy"
                                                PopupButtonID="imgBtnFechaInicial" TargetControlID="txbFechaInicial">
                                            </asp:CalendarExtender>
                                            <asp:MaskedEditExtender ID="meeFechaInicial" runat="server" InputDirection="LeftToRight"
                                                Mask="99/99/9999" MaskType="Date" TargetControlID="txbFechaInicial" UserDateFormat="DayMonthYear">
                                            </asp:MaskedEditExtender>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td class="EstiloEtiquetas125Right">
                                            
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td>
                                             <asp:Button ID="btnContrato" runat="server" Text="Busqueda" OnClick="btnContrato_Click" />
                                        </td>
                                        <td class="EspaciadoFinal">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 10px" colspan="13">
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:RoundedCornersExtender ID="rceFiltros" Radius="3" BorderColor="181, 198, 214"
                                TargetControlID="pnlFiltros" runat="server" Enabled="True">
                            </asp:RoundedCornersExtender>
                            <br />
                            <asp:Panel ID="pnlMovimientos" Visible="false" CssClass="PanelBordesRedondos" runat="server" Width="99%">
                                <table style="width: 100%" cellpadding="0" cellspacing="2">
                                    <%-- <tr>
                                        <td class="LetraLeyendaColor" colspan="4">
                                            Movimientos
                                        </td>
                                    </tr>--%>
                                    <tr>
                                        <td class="LetraLeyendaColor" colspan="3">
                                            Cuentas Débito en Proceso:
                                            <asp:Label ID="lbDebProceso" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 10px" colspan="5">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial">
                                        </td>
                                        <%-- <td class="EstiloEtiquetasLeft">
                                            <asp:Label ID="lbSeleccion" runat="server" Text="Seleccione Bancos:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>--%>
                                        <td class="EstiloEtiquetas80">
                                            <asp:Button ID="btnSeleccion" runat="server" Text="S/N" Height="27px" Width="42px"
                                               OnClick="btnSeleccion_Click" />
                                        </td>
                                        <td class="EspaciadoFinal">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial">
                                        </td>
                                        <td>
                                            <asp:GridView ID="gvMovimientos" runat="server" Width="920px" AllowSorting="True" BorderColor="#D0DEF0"
                                                BorderStyle="Solid" BorderWidth="1px" GridLines="Horizontal" AutoGenerateColumns="False"
                                                Font-Size="XX-Small"  AllowPaging="True" OnPageIndexChanging="gvMovimientos_PageIndexChanging" PageSize="15">

                                                <Columns>
                                                    <asp:BoundField HeaderText="Id" DataField="pId" ItemStyle-CssClass="OcultarControles"
                                                     HeaderStyle-CssClass="OcultarControles">
                                                    <HeaderStyle CssClass="OcultarControles" />
                                                    <ItemStyle CssClass="OcultarControles" />
                                                    </asp:BoundField>
                                                       <asp:BoundField HeaderText="Id_Cliente" DataField="pIdCliente" ItemStyle-CssClass="OcultarControles"
                                                     HeaderStyle-CssClass="OcultarControles">
                                                    <HeaderStyle CssClass="OcultarControles" />
                                                    <ItemStyle CssClass="OcultarControles" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="Contrato" DataField="pcontrato"></asp:BoundField>
                                                    <asp:BoundField HeaderText="Identificación" DataField="pNumeroIdentificacion" />
                                                    <asp:BoundField HeaderText="Nombre" DataField="pNombreCliente" />
                                                    <asp:BoundField HeaderText="Banco" DataField="pNombreBanco" />
                                                    <asp:BoundField HeaderText="TC" DataField="pTipoCuenta" />
                                                    <asp:BoundField HeaderText="Numero Cuenta" DataField="pNumeroCuenta" />
                                                    <asp:BoundField HeaderText="Estado Cliente" DataField="pEstadoCliente" />
                                                    <asp:BoundField HeaderText="Banco Debita" DataField="pNombreBancoDebita" />
                                                    <asp:BoundField HeaderText="Movimiento" DataField="pTipoTransferencia" />
                                                    <asp:BoundField HeaderText="Valor" DataField="pValor"/>
                                                    <asp:BoundField HeaderText="Fecha Debito" DataField="pFecha" DataFormatString="{0:dd-MM-yyyy}" />
                                                    <asp:BoundField HeaderText="Estado" DataField="pEstado" />
                                                    <asp:TemplateField HeaderText="MARCAR">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chbReversar" runat="server" Checked="False" />
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
                                        <td class="EspaciadoFinal">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 10px" colspan="5">
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:RoundedCornersExtender ID="rceMovimientos" Radius="3" BorderColor="181, 198, 214"
                                TargetControlID="pnlMovimientos" runat="server" Enabled="True">
                            </asp:RoundedCornersExtender>
                            <br />
                            <asp:Panel ID="pnlCambio" Visible="false" CssClass="PanelBordesRedondos" runat="server" Width="99%">
                                <table style="width: 100%" cellpadding="0" cellspacing="2">

                                    <tr>
                                        <td style="height: 10px" colspan="13">
                                        </td>
                                    </tr>


                                           <tr>
                                        <td class="EspaciadoInicial">
                                        </td>
                                        <td class="EstiloEtiquetas80">
                                            <asp:Label ID="lblCausal" runat="server" Text="Causal:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlTipoCausales" runat="server" CssClass="BordeListas" ValidationGroup="1"
                                                Width="150px">

                                            </asp:DropDownList>
                                               
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td class="EstiloEtiquetas125Right">
                                            <asp:Label ID="lblEstadoActual" runat="server" Text="Estado Actual:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlEstadoTransaccionInicial" AutoPostBack="true" runat="server" CssClass="BordeListas" ValidationGroup="1"
                                                Width="150px" OnSelectedIndexChanged="ddlEstadoTransaccionInicial_SelectedIndexChanged" >
                                                <asp:ListItem Value="0">[Seleccione]</asp:ListItem>
                                                <asp:ListItem Value="1">ACEPTADO</asp:ListItem>
                                                <asp:ListItem Value="2">RECHAZADO</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td class="EstiloEtiquetas125Right">
                                            <asp:Label ID="lblEstadoNuevo" runat="server" Text="Estado Nuevo:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                            <asp:DropDownList ID="ddlEstadoTransaccionFinal" runat="server" CssClass="BordeListas" Width="150px">
                                                 <asp:ListItem Value="0">[Seleccione]</asp:ListItem>
                                                <asp:ListItem Value="1">ACEPTADA</asp:ListItem>
                                                <asp:ListItem Value="2">RECHAZADA</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                          
                                        </td>
                                        <td class="EspaciadoFinal">
                                        </td>
                                    </tr>

                                             <tr>
                                        <td class="auto-style1" colspan="13">
                                        </td>
                                                 <tr>
                                                     <td class="EspaciadoInicial"></td>
                                                     <td class="EstiloEtiquetas80">
                                                         <asp:Label ID="lblFechaGrio" runat="server" Text="Fecha del Giro:"></asp:Label>
                                                     </td>
                                                     <td class="EspaciadoIntermedio"></td>
                                                     <td>
                                                         <asp:TextBox ID="txbFechaGiro" runat="server" CssClass="FuenteDDL" TabIndex="2" ValidationGroup="1" Width="85px"></asp:TextBox>
                                                         <asp:ImageButton ID="imgBtnFechaGiro" runat="server" ImageAlign="Middle" ImageUrl="~/MarcaVisual/iconos/calendario.png" Width="20px" />
                                                         <asp:CalendarExtender ID="ceFechaGiro" runat="server" Enabled="True" Format="dd/MM/yyyy" PopupButtonID="imgBtnFechaGiro" TargetControlID="txbFechaGiro">
                                                         </asp:CalendarExtender>
                                                         <asp:MaskedEditExtender ID="meFechaGiro" runat="server" InputDirection="LeftToRight" Mask="99/99/9999" MaskType="Date" TargetControlID="txbFechaGiro" UserDateFormat="DayMonthYear">
                                                         </asp:MaskedEditExtender>
                                                     </td>
                                                     <td class="EspaciadoIntermedio"></td>
                                                     <td class="EstiloEtiquetas125Right"></td>
                                                     <td class="EspaciadoIntermedio"></td>
                                                     <td></td>
                                                     <td class="EspaciadoIntermedio"></td>
                                                     <td class="EstiloEtiquetas125Right"></td>
                                                     <td class="EspaciadoIntermedio"></td>
                                                     <td></td>
                                                     <td class="EspaciadoFinal"></td>
                                                 </tr>
                                                 <tr>
                                                     <td colspan="13" style="height: 10px"></td>
                                                 </tr>
                                    </tr>
                                                           
                               
                                </table>
                            </asp:Panel>
                            <asp:RoundedCornersExtender ID="rceCambio" Radius="3" BorderColor="181, 198, 214"
                                TargetControlID="pnlCambio" runat="server" Enabled="True">
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
