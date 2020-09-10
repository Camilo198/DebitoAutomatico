<%@ Page Title="" Language="C#" MasterPageFile="~/PaginaMaestra/Sitio.Master" AutoEventWireup="true"
    CodeBehind="GenerarDebitoAutomatico.aspx.cs" Inherits="DebitoAutomatico.PS.Modulos.Servicio.GenerarArchivos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphCabecera" runat="server">
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
                    <td class="BarraSubTitulo">
                        Generar Debito
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
                        <asp:Panel ID="pnlGeneraArchivo" runat="server" ScrollBars="Auto" Height="100%" Width="650px">
                            <asp:Panel ID="pnlDatos" CssClass="PanelBordesRedondos" runat="server" Width="98%">
                                <table style="width: 98%" cellpadding="0" cellspacing="2">                                   
                                    <tr>
                                        <td style="height: 10px" colspan="5">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial">
                                        </td>
                                        <td class="EstiloEtiquetas80">
                                            <asp:Label ID="lbTipoClientes" runat="server" Text="Tipo Clientes:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td >
                                            <asp:CheckBox ID="chbSuscriptores" runat="server" Text="Suscriptores" Font-Size="12px"/>
                                            <asp:CheckBox ID="chbGanadores" runat="server" Text="Ganadores"  Font-Size="12px"/>
                                            <asp:CheckBox ID="chbAdjudicados" runat="server" Text="Adjudicados"  Font-Size="12px"/>
                                            <asp:CheckBox ID="chbCuotasxDevolver" runat="server" Text="Cuotas Por Devolver"  Font-Size="12px"/>
                                        </td>
                                        <td class="EspaciadoFinal">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial">
                                        </td>
                                        <td  class="EstiloEtiquetas80">
                                            <asp:Label ID="lbFechaInicio" runat="server" Text="Fecha del débito:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td>
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

                                        <td class="EspaciadoFinal">
                                        </td>
                                    </tr>
   <tr>
                                        <td class="EspaciadoInicial">
                                        </td>
                                        <td  class="EstiloEtiquetas80">
                                            <asp:Label ID="Label2" runat="server" Text="Año a debitar:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td>
                                       <asp:TextBox ID="txbAnoDebito" runat="server" CssClass="BordeControles" MaxLength="4"
                                                Width="60px" ValidationGroup="1" AutoPostBack="true"></asp:TextBox>
                                              <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" 
                                                FilterType="Numbers" TargetControlID="txbAnoDebito">
                                            </asp:FilteredTextBoxExtender>
                                         <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Favor seleccionar el mes a debitar!"
                                                ForeColor="Red" ControlToValidate="txbAnoDebito" InitialValue="0" ValidationGroup="1"
                                                Text="*" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                        </td>

                                        <td class="EspaciadoFinal">
                                        </td>
                                    </tr>
                                        <tr>
                                        <td class="EspaciadoInicial">
                                        </td>
                                        <td  class="EstiloEtiquetas80">
                                            <asp:Label ID="Label1" runat="server" Text="Mes a debitar:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td>
                                       <asp:DropDownList ID="ddlMesadebitar" AutoPostBack="true" runat="server" CssClass="BordeListas" ValidationGroup="1"
                                                Width="150px" >
                                                <asp:ListItem Value="0">[Seleccione]</asp:ListItem>
                                                <asp:ListItem Value="01">Enero</asp:ListItem>
                                                <asp:ListItem Value="02">Febrero</asp:ListItem>
                                                <asp:ListItem Value="03">Marzo</asp:ListItem>
                                                <asp:ListItem Value="04">Abril</asp:ListItem>
                                                <asp:ListItem Value="05">Mayo</asp:ListItem>
                                                <asp:ListItem Value="06">Junio</asp:ListItem>
                                                <asp:ListItem Value="07">Julio</asp:ListItem>
                                                <asp:ListItem Value="08">Agosto</asp:ListItem>
                                                <asp:ListItem Value="09">Septiembre</asp:ListItem>
                                                <asp:ListItem Value="10">Octubre</asp:ListItem>
                                                <asp:ListItem Value="11">Noviembre</asp:ListItem>
                                                <asp:ListItem Value="12">Diciembre</asp:ListItem>
                                            </asp:DropDownList>
                                         <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Favor seleccionar el mes a debitar!"
                                                ForeColor="Red" ControlToValidate="ddlMesadebitar" InitialValue="0" ValidationGroup="1"
                                                Text="*" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                        </td>

                                        <td class="EspaciadoFinal">
                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="EspaciadoInicial">
                                        </td>
                                        <td class="EstiloEtiquetas80">
                                            <asp:Label ID="lbBancoDebito" runat="server" Text="Entidad Bancaria:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlBancoDebito" runat="server" CssClass="BordeListas" ValidationGroup="1"
                                                 Width="250px" AutoPostBack="True">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvBanco" runat="server" ErrorMessage="Favor seleccionar Banco!"
                                                ForeColor="Red" ControlToValidate="ddlBancoDebito" InitialValue="0" ValidationGroup="1"
                                                Text="*" SetFocusOnError="true"></asp:RequiredFieldValidator>
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
                            <asp:RoundedCornersExtender ID="rceDatos" Radius="3" BorderColor="181, 198, 214"
                                TargetControlID="pnlDatos" runat="server" Enabled="True">
                            </asp:RoundedCornersExtender>                         
                            <br />

                                                        <asp:Panel ID="pnlMovimiento" CssClass="PanelBordesRedondos" runat="server" Width="98%">
                                <table style="width: 98%" cellpadding="0" cellspacing="2">                                 
                                    <tr>
                                        <td style="height: 10px" colspan="5">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="EspaciadoInicial">
                                        </td>
                                      
                                   
                                 <td style="text-align: center">
                                                 <asp:Button ID="btnGenerar" runat="server" Text="Generar" Height="30px" Width="122px" OnClick="btnGenerar_Click" ValidationGroup="1"/>
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
                            <asp:RoundedCornersExtender ID="rceMovimiento" Radius="3" BorderColor="181, 198, 214"
                                TargetControlID="pnlMovimiento" runat="server" Enabled="True">
                            </asp:RoundedCornersExtender>
                            <table width="99%">
                                <tr>
                                    <td class="EspaciadoInicial">
                                    </td>
                                    <td class="EstiloInformacion">
                                 
                                    </td>

                                    <td class="EspaciadoFinal">
                                    </td>
                                </tr>
                            </table>
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
