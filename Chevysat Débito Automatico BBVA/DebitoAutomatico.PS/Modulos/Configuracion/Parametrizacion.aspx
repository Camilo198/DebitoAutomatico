<%@ Page Title="" Language="C#" MasterPageFile="~/PaginaMaestra/Sitio.Master" AutoEventWireup="true" CodeBehind="Parametrizacion.aspx.cs" Inherits="DebitoAutomatico.PS.Modulos.Configuracion.Parametrizacion" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphCabecera" runat="server">
    <style type="text/css">
        .auto-style1 {
            margin-left: 40px;
            width: 175px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBarraHerramientas" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphContenido" runat="server">
    <script type="text/javascript" language="javascript">
        function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";

            var Validacion = document.getElementById("<%= HidValParametro.ClientID %>");

            if (confirm("¿Desea eliminar este registro?")) {
                Validacion.value = "1";
            }
            else {
                Validacion.value = "0";
            }
            document.forms[0].appendChild(confirm_value);
        }

        </script>
        
           
    
    
    <asp:UpdatePanel ID="upContenido" runat="server">
        <ContentTemplate>
            <table style="width: 100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="BarraSubTitulo">Parametrización Listas
                    </td>
                </tr>
                <tr>
                    <td class="SeparadorSubTitulo"></td>
                </tr>
            </table>
            <table style="width: 100%" class="ColorContenedorDatos" cellpadding="0" cellspacing="0">
                <tr>
                    <td style="height: 10px" colspan="2"></td>
                </tr>
                <tr>
                    <td style="width: 10px"></td>
                    <td>
                        <asp:Panel ID="pnlCliente" runat="server" ScrollBars="Auto" Height="100%" Width="900px">

                            <asp:Panel ID="pnlDatos" CssClass="PanelBordesRedondos" runat="server" Width="99%">
                                <table style="width: 100%" cellpadding="0" cellspacing="2">
                                    <tr>
                                        <td class="LetraLeyendaColor" colspan="9">Parametrización
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 10px" colspan="9"></td>
                                    </tr>
                                          <tr>
                                        <td class="EspaciadoInicial">
                                        </td>
                                        <td class="EstiloEtiquetas80">
                                            <asp:Label ID="lbParametro" runat="server" Text="Parametro:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio">
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlListas" runat="server" CssClass="BordeListas" ValidationGroup="1"
                                                Width="150px" AutoPostBack="True" OnSelectedIndexChanged="ddlListas_SelectedIndexChanged">
                                                <asp:ListItem Value="0">[Seleccione]</asp:ListItem>
                                                <asp:ListItem Value="2">Formatos</asp:ListItem>
                                                <asp:ListItem Value="4">Tipo Causal</asp:ListItem>
                                                <asp:ListItem Value="5">Tipo Inconsistencia</asp:ListItem>
                                                <asp:ListItem Value="6">Fechas débito</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td colspan="9">
                                        </td>
                                    </tr>
                                     <tr>
                                        <td style="height: 10px" colspan="5">
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:RoundedCornersExtender ID="rceEsquinasRedondas" Radius="3" BorderColor="181, 198, 214"
                                TargetControlID="pnlDatos" runat="server" Enabled="True">
                            </asp:RoundedCornersExtender>
                            <br />

                             <asp:Panel ID="pnlMovimientos" CssClass="PanelBordesRedondos" runat="server" Width="99%">
                                <table style="width: 100%" cellpadding="0" cellspacing="2">
                                    <%-- <tr>
                                        <td class="LetraLeyendaColor" colspan="4">
                                            Movimientos
                                        </td>
                                    </tr>--%>
                                    <tr>
                                        <td class="LetraLeyendaColor" colspan="3">
                                            Valores a editar
                                     
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 10px" colspan="5">
                                        </td>
                                    </tr>
                                 
                                    <tr>
                                        <td class="EspaciadoInicial">
                                        </td>
                                        <td>
                                           <asp:GridView ID="gvCausales" runat="server" AutoGenerateColumns="false" CssClass="EstiloEtiquetas81" Enabled="true" HeaderStyle-BackColor="#61A6F8" HeaderStyle-Font-Bold="true" HeaderStyle-ForeColor="White" OnRowCancelingEdit="gvCausales_RowCancelingEdit" OnRowCommand="gvCausales_RowCommand" OnRowDeleting="gvCausales_RowDeleting" OnRowEditing="gvCausales_RowEditing" OnRowUpdating="gvCausales_RowUpdating" PageSize="4" RowStyle-HorizontalAlign="Justify" RowStyle-VerticalAlign="Middle" ShowFooter="true" Width="409px">
                            <Columns>
                                <asp:TemplateField HeaderText="Editar">
                                    <HeaderStyle Width="50px" />
                                    <EditItemTemplate>
                                        <asp:ImageButton ID="imgbtnUpdate" runat="server" CommandName="Update" Height="20px" ImageUrl="~/MarcaVisual/iconos/aceptar.png" ToolTip="Actualizar" Width="20px" />
                                        &nbsp;
                                        <asp:ImageButton ID="imgbtnCancel" runat="server" CommandName="Cancel" Height="20px" ImageUrl="~/MarcaVisual/iconos/deshacer.png" ToolTip="Cancelar" Width="20px" />
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgbtnEdit" runat="server" CommandName="Edit" Height="18px" ImageUrl="~/MarcaVisual/iconos/editar.png" ToolTip="Editar" Width="18px" />
                                        <asp:ImageButton ID="imgbtnDelete" runat="server" CommandName="Delete" Height="18px" ImageUrl="~/MarcaVisual/iconos/borrar.png" ToolTip="Eliminar" Width="18px" OnClientClick = "Confirm()" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:ImageButton ID="imgbtnAdd" runat="server" CommandName="AddNew" Height="18px" ImageUrl="~/MarcaVisual/iconos/agregar.png" ToolTip="Nuevo" ValidationGroup="validation" Width="18px" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Nombre" >
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txbCausal" runat="server" Text='<%#Eval("pValor") %>' />
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblCausal" runat="server" Text='<%#Eval("pValor") %>'/>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtCausal" runat="server" Height="24px" Width="163px" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle BackColor="#88AADA" />
                            <PagerStyle BackColor="#C5C5C6" Font-Strikeout="False" Font-Underline="False" />
                            <RowStyle BackColor="AliceBlue" BorderColor="#D0DEF0" />
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

                             <asp:Panel ID="pnlFechas" Visible="false" CssClass="PanelBordesRedondos" runat="server" Width="99%">
                                <table style="width: 100%" cellpadding="0" cellspacing="2">
                                    <%-- <tr>
                                        <td class="LetraLeyendaColor" colspan="4">
                                            Movimientos
                                        </td>
                                    </tr>--%>
                                    <tr>
                                        <td class="LetraLeyendaColor" colspan="3">
                                            Fecha para débito
                                     
                                        </td>
                                    </tr>
                                    <tr>
                                        <td >
                        <asp:ImageButton ID="imgBtnNuevo" runat="server" ImageUrl="~/MarcaVisual/iconos/nuevo.png"
                            Width="16px" ToolTip="Nuevo..." OnClick="imgBtnNuevo_Click" />
                    </td>
                    <td>
                        <asp:ImageButton ID="imgBtnGuardar" runat="server" ImageUrl="~/MarcaVisual/iconos/guardar.png"
                            Width="16px" ToolTip="Guardar" OnClick="imgBtnGuardar_Click" ValidationGroup="1" />
                    </td>
                                    </tr>
                             
                                 

               <tr>
                                        <td style="height: 10px" colspan="9">
                                        </td>
                                    </tr>
<tr>
                                        <td class="EspaciadoInicial"></td>
                                       <td style="width: 20%" class="EstiloEtiquetas80">
                                            <asp:Label ID="Label3" runat="server" Text="Dias habiles:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="auto-style1">
                                            <asp:DropDownList ID="ddldiaHabil" AutoPostBack="true" runat="server" CssClass="BordeListas" ValidationGroup="1"
                                                Width="150px">
                                               <asp:ListItem Value="0">[Seleccione]</asp:ListItem>
                                               <asp:ListItem Value="1">1er día hábil del mes</asp:ListItem>
                                               <asp:ListItem Value="2">2do día hábil del mes</asp:ListItem>
                                               <asp:ListItem Value="3">3er día hábil del mes</asp:ListItem>
                                               <asp:ListItem Value="4">4to día hábil del mes</asp:ListItem>
                                               <asp:ListItem Value="5">5to día hábil del mes</asp:ListItem>
                                               <asp:ListItem Value="6">6to día hábil del mes</asp:ListItem>
                                               <asp:ListItem Value="7">7to día hábil del mes</asp:ListItem>
                                               <asp:ListItem Value="8">8vo día hábil del mes</asp:ListItem>
                                               <asp:ListItem Value="9">9vo día hábil del mes</asp:ListItem>
                                               <asp:ListItem Value="10">10mo día hábil del mes</asp:ListItem>
                                               <asp:ListItem Value="11">11vo día hábil del mes</asp:ListItem>
                                               <asp:ListItem Value="12">12mo día hábil del mes</asp:ListItem>
                                               <asp:ListItem Value="13">13vo día hábil del mes</asp:ListItem>
                                               <asp:ListItem Value="14">14vo día hábil del mes</asp:ListItem>
                                               <asp:ListItem Value="15">15vo día hábil del mes</asp:ListItem>
                                               <asp:ListItem Value="16">16vo día hábil del mes</asp:ListItem>
                                               <asp:ListItem Value="17">17vo día hábil del mes</asp:ListItem>
                                               <asp:ListItem Value="18">18vo día hábil del mes</asp:ListItem>
                                               <asp:ListItem Value="19">19vo día hábil del mes</asp:ListItem>
                                               <asp:ListItem Value="20">20vo día hábil del mes</asp:ListItem>                
                                            </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Favor seleccionar el mes a debitar!"
                                                ForeColor="Red" ControlToValidate="ddldiaHabil" InitialValue="0" ValidationGroup="1"
                                                Text="*" SetFocusOnError="true"></asp:RequiredFieldValidator>   
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td style="width: 20%" class="EstiloEtiquetas80">
                                            <asp:Label ID="lblHabilita" runat="server" Text="Habilita:"></asp:Label>
                                        </td>
                                        <td class="EspaciadoIntermedio"></td>
                                        <td class="EspaciadoCeldaControl">
                                                   
                                        <asp:CheckBox ID="chbHabilita" runat="server" />
                                        </td>
                                        <td class="EspaciadoFinal"></td>
                                    </tr>
                                        <tr>
                                        <td style="height: 10px" colspan="9">
                                        </td>
                                    </tr>
              <tr>
                                        <td class="EspaciadoInicial"></td>
                                        <td colspan="7" style="height: 110px; width: 500px; overflow: auto;">
                                              <asp:GridView ID="gvFechas" runat="server" AutoGenerateColumns="False" Width="450px"
                                                AllowSorting="True" BorderColor="#D0DEF0" BorderStyle="Solid" BorderWidth="1px"
                                                GridLines="Horizontal" OnRowCommand="gvFechas_RowCommand" CssClass="EstiloEtiquetas81">
                                                <Columns>
                                                    <asp:ButtonField ButtonType="Image" CommandName="Editar" ImageUrl="~/MarcaVisual/iconos/editar.png" 
                                                        Text="Editar">
                                                        <ItemStyle Width="30px" />
                                                        <ControlStyle Width="16px" />
                                                    </asp:ButtonField>
                                                    <asp:ButtonField ButtonType="Image"  CommandName="Eliminar" ImageUrl="~/MarcaVisual/iconos/borrar.png"
                                                        Text="Eliminar">
                                                        <ItemStyle Width="30px" />
                                                        <ControlStyle Width="16px" />
                                                    </asp:ButtonField>
                                                    <asp:BoundField HeaderText="ID" DataField="pId" ItemStyle-CssClass="OcultarControles"
                                                        HeaderStyle-CssClass="OcultarControles">
                                                        <HeaderStyle CssClass="OcultarControles" />
                                                        <ItemStyle CssClass="OcultarControles" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="pDia" HeaderText="DÍA HABIL">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="pValor" HeaderText="DESCRIPCIÓN">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                       <asp:CheckBoxField DataField="pHabilita" HeaderText="HABILITA">
                                                        <HeaderStyle Width="60px" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:CheckBoxField>
                                                </Columns>
                                                <HeaderStyle BackColor="#C5C5C6" />
                                                <PagerStyle BackColor="#C5C5C6" Font-Strikeout="False" Font-Underline="False" />
                                                <RowStyle BackColor="#F0F0F0" BorderColor="#D0DEF0" />
                                            </asp:GridView>
                                        </td>
                                        <td class="EspaciadoFinal"></td>
                                    </tr>

                                    <tr>
                                        <td style="height: 10px" colspan="9">
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:RoundedCornersExtender ID="rceFechas" Radius="3" BorderColor="181, 198, 214"
                                TargetControlID="pnlFechas" runat="server" Enabled="True">
                            </asp:RoundedCornersExtender>

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
            <asp:HiddenField ID="HidValParametro" runat="server" />
        </ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBarraEstado" runat="server">
</asp:Content>
