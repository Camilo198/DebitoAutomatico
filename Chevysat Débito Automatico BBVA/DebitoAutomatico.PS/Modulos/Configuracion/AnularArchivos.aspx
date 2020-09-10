<%@ Page Title="" Language="C#" MasterPageFile="~/PaginaMaestra/Sitio.Master" AutoEventWireup="true" CodeBehind="AnularArchivos.aspx.cs" Inherits="DebitoAutomatico.PS.Modulos.Configuracion.AnularArchivos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphCabecera" runat="server">
    <style type="text/css">
        .auto-style1 {
            width: 150px;
        }
        .auto-style2 {
            font-size: 8pt;
            text-align: right;
            vertical-align: middle;
            width: 81px;
        }
        .auto-style3 {
            font-size: 8pt;
            text-align: right;
            vertical-align: middle;
            width: 90px;
        }
        .auto-style4 {
            font-size: 9pt;
            text-align: left;
            vertical-align: middle;
            width: 199px;
        }
        .auto-style5 {
            font-size: 9pt;
            text-align: left;
            vertical-align: middle;
            width: 97px;
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
                    <td class="BarraSubTitulo">Anular Archivos
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
                        <asp:Panel ID="pnlRespuestasTransacciones" runat="server" ScrollBars="Auto" Height="100%" Width="980px">
                            <asp:Panel ID="pnlBanco" CssClass="PanelBordesRedondos" runat="server" Width="99%">
                                <table style="width: 100%" cellpadding="0" cellspacing="2">
                                    <tr>
                                        <td class="LetraLeyendaColor" colspan="9">Filtros
                                    </tr>
                                    <tr>
                                        <td style="height: 10px" colspan="15"></td>
                                    </tr>
                 

                                          <tr>
                                        <td class="EspaciadoInicial">
                                        </td>
                                        <td class="auto-style2">
                                            <asp:Label ID="lbFechaProceso" runat="server" Width="100px" Text="Fecha Generación:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td class="auto-style1">
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
                                        <td>
                                        </td>
                                         <td class="auto-style3">
                                             
                                     <%--<asp:Label ID="lbBancoDebito" runat="server" Text="Entidad Bancaria:"></asp:Label>--%>
                                             <asp:Label ID="lblTipoMovimiento" runat="server" Text="Tipo Movimiento:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td class="auto-style4">
                                         <asp:DropDownList ID="ddlTipoMovimiento" runat="server" CssClass="BordeListas"  Width="180px" Height="16px">
                                           </asp:DropDownList>
                                                 <asp:RequiredFieldValidator ID="rfvBanco" runat="server" ErrorMessage="Favor seleccionar Tipo Movimiento!"
                                                ForeColor="Red" ControlToValidate="ddlTipoMovimiento" InitialValue="0" ValidationGroup="1"
                                                Text="*" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceBanco" runat="server" Enabled="True" TargetControlID="rfvBanco"
                                                WarningIconImageUrl="~/MarcaVisual/iconos/advertencia.png" PopupPosition="BottomLeft"
                                                HighlightCssClass="Resaltar">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                                 <td class="auto-style3">
                                             
                                     <asp:Label ID="lblManual" runat="server"  Width="100px"  Text="¿Archivo Manual?"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td class="auto-style4">
                                            <asp:CheckBox ID="chbManual" runat="server" />
                                        </td>


                                          </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                   
                                         <td class="EstiloEtiquetas80">
                                            
                                             <asp:Button ID="btnConsultar" runat="server" OnClick="btnConsultar_Click" Text="Consultar" ValidationGroup="1" />
                                            
                                        </td>
                                        
                                    </tr>
                            
                                    <tr>
                                       
                                        <td style="height: 10px" colspan="13"></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:RoundedCornersExtender ID="rceBanco" Radius="3" BorderColor="181, 198, 214"
                                TargetControlID="pnlBanco" runat="server" Enabled="True">
                            </asp:RoundedCornersExtender>
                            <br />
                            <asp:Panel ID="pnlPrenotas" Visible="false" CssClass="PanelBordesRedondos" runat="server" Width="99%">
                                <table style="width: 100%" cellpadding="0" cellspacing="2">
                                    <tr>
                                        <td class="LetraLeyendaColor" colspan="9">Archivos Prenota
                                            
                                        </td>
                                    </tr>
                                    <tr>
                                         <td colspan="9" style="height: 10px"></td>
                                    </tr>
                                    <tr>

                                        <td>
                                 <asp:GridView ID="gvHistorial" runat="server" AutoGenerateColumns="False" Width="95%"
                                                AllowSorting="True" BorderColor="#D0DEF0" BorderStyle="Solid" BorderWidth="1px"
                                                GridLines="None" CssClass="EstiloEtiquetas80">
                                                   <Columns>
                                                    <asp:BoundField HeaderText="Fecha" DataField="FECHA">
                                                        <ControlStyle Width="200px" />
                                                        <HeaderStyle Width="60px" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="Nombre del banco" DataField="NOMBRE_BANCO">
                                                 <ControlStyle Width="200px" />
                                                        <HeaderStyle Width="60px" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="Tipo de movimiento" DataField="MOVIMIENTO">
                                                        <ControlStyle Width="200px" />
                                                        <HeaderStyle Width="60px" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                        <asp:BoundField HeaderText="Contratos" DataField="CONTRATO">
                                                        <ControlStyle Width="200px" />
                                                        <HeaderStyle Width="60px" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="Valor" DataField="VALOR">
                                                        <ControlStyle Width="100px" />
                                                        <HeaderStyle Width="60px" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="Nombre del archivo" DataField="NOMBRE_ARCHIVO">
                                                        <ControlStyle Width="150px" />
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
                                    </tr>
           <tr>
                                        <td style="height: 10px" colspan="9"></td>
                                    </tr>
                                   <tr>
                                         <td colspan="6" style="text-align: center">
                                            <asp:Button ID="btnAnular" runat="server" Text="Anular" Width="150px"
                                                 Height="31px" ValidationGroup="1" OnClick="btnAnular_Click" />
                                        </td
                                    </tr>
                                   <tr>
                                        <td style="height: 10px" colspan="9"></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:RoundedCornersExtender ID="rceRutas" Radius="3" BorderColor="181, 198, 214"
                                TargetControlID="pnlPrenotas" runat="server" Enabled="True">
                            </asp:RoundedCornersExtender>
                            <br />

             <asp:Panel ID="pnlDebito" CssClass="PanelBordesRedondos" Visible="false" runat="server" Width="99%">
                                <table style="width: 100%" cellpadding="0" cellspacing="2">
                                    <tr>
                                        <td class="LetraLeyendaColor" colspan="9">Archivos Debito
                                            
                                        </td>
                                    </tr>
                                    <tr>
                                         <td colspan="9" style="height: 10px"></td>
                                    </tr>
                                    <tr>

                                        <td>
                      <asp:GridView ID="gvRespuestas"  runat="server" AutoGenerateColumns="False" Width="95%"
                                                AllowSorting="True" BorderColor="#D0DEF0" BorderStyle="Solid" BorderWidth="1px"
                                                GridLines="None"
                                OnRowCommand="gvRespuestas_RowCommand" CssClass="EstiloEtiquetas80">
                                <Columns>
               
                                    <asp:ButtonField ButtonType="Image" CommandName="Eliminar" ImageUrl="~/MarcaVisual/iconos/borrar.png"
                                        Text="Eliminar">
                                        <ItemStyle Width="30px" />
                                        <ControlStyle Width="16px" />
                                    </asp:ButtonField>
                                                    <asp:BoundField HeaderText="Fecha" DataField="FECHA">
                                                        <ControlStyle Width="200px" />
                                                        <HeaderStyle Width="60px" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                <asp:BoundField HeaderText="ID" DataField="IDBANCO" ItemStyle-CssClass="OcultarControles"
                                                        HeaderStyle-CssClass="OcultarControles" >
                                                        <HeaderStyle Width="40px" CssClass="OcultarControles" />
                                                        <ItemStyle Width="60px" CssClass="OcultarControles" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="Nombre del banco" DataField="NOMBRE_BANCO">
                                                 <ControlStyle Width="200px" />
                                                        <HeaderStyle Width="60px" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="Tipo de movimiento" DataField="MOVIMIENTO">
                                                        <ControlStyle Width="200px" />
                                                        <HeaderStyle Width="60px" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                        <asp:BoundField HeaderText="Contratos" DataField="CONTRATO">
                                                        <ControlStyle Width="200px" />
                                                        <HeaderStyle Width="60px" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="Valor" DataField="VALOR">
                                                        <ControlStyle Width="100px" />
                                                        <HeaderStyle Width="60px" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="Nombre del archivo" DataField="NOMBRE_ARCHIVO">
                                                        <ControlStyle Width="150px" />
                                                        <HeaderStyle Width="60px" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
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
                        
                                   <tr>
                                        <td style="height: 10px" colspan="9"></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:RoundedCornersExtender ID="rceDebito" Radius="3" BorderColor="181, 198, 214"
                                TargetControlID="pnlDebito" runat="server" Enabled="True">
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
