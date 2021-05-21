using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Configuration;
using ServicioDebito.EN.Tablas;
using ServicioDebito.EN;
using ServicioDebito.LN.Consulta;
using ServicioDebito.LN.Utilidades;

using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using ServicioDebito.LN.Consultas;

namespace WcfServicioDebito
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "Service1" en el código, en svc y en el archivo de configuración.
    // NOTE: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione Service1.svc o Service1.svc.cs en el Explorador de soluciones e inicie la depuración.
    public class ServiceDebito : IServicioDebito
    {
        SqlDataAdapter DataAdapterSql = new SqlDataAdapter();
        Usuario objUsuario = new Usuario();
        Encriptador objEncriptador = new Encriptador();
        WcfUtilidades wfu = new WcfUtilidades();
        DatosDebito objDatosD = new DatosDebito();
        DatosDebitoInconsistente objDatosDI = new DatosDebitoInconsistente();
        DataSet ClienteD = new DataSet();
        DataTable DtClienteD = new DataTable();
        DataSet ClienteDIn = new DataSet();
        DataTable DtClienteDIn = new DataTable();

        String campos = String.Empty;

        /// <summary>
        /// Cambiar primera letra a mayuscula y el resto en Minuscula
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        private String getNombrePropio(String nombre)
        {
            if (nombre != String.Empty)
            {
                String inicial;
                nombre = nombre.ToLower();
                inicial = nombre.Substring(0, 1);
                inicial = inicial.ToUpper();
                nombre = inicial + nombre.Remove(0, 1);
            }
            return nombre;
        }

        /// <summary>
        /// Consultar los correos parametrizandos del banco
        /// </summary>
        /// <param name="CodigoBanco"></param>
        /// <returns></returns>
        private DataSet ConsultarCorreosBancos()
        {
            Banco ObjBan = new Banco();
            DataSet DsBancoCorreo = new DataSet();
            ObjBan.pActivo = true;
            ObjBan.pId = 2;
            DsBancoCorreo = new BancoLN().consultarCorreo(ObjBan);
            return DsBancoCorreo;
        }

        /// <summary>
        /// Metodo interno que consulta en SICO el número de contrato
        /// </summary>
        /// <param name="NroContrato"></param>
        /// <param name="Usuario"></param>
        /// <returns></returns>
        private DataSet ExisteCliente(int NroContrato, string Usuario, Int32 IdTitularCuenta)
        {
            DataTable DtXml = new DataTable();
            DataSet objCliente = new DataSet();

            //Consulta información en SICO
            ClienteSico objS = new ClienteSico();
            objS.pContrato = NroContrato;
            objCliente = new ClienteSICOLN().consultarClienteSICO(objS, "pa_DEB_Consulta_Clientes");

            //Consulta información de un cliente inconsistente en débito
            objDatosDI.pContrato = Convert.ToString(NroContrato);
            ClienteDIn = new DatosDebitoInconsistenteLN().consultarDatos(objDatosDI);

            if (IdTitularCuenta > 0)
            {
                objDatosD.pIdTitularCuenta = IdTitularCuenta;
            }
            //Consulta información si cuenta con débito
            objDatosD.pContrato = Convert.ToString(NroContrato);
            ClienteD = new DatosDebitoLN().consultarDatos(objDatosD);

            if (ClienteD.Tables["ClienteDebito"].Rows.Count > 0)
            {
                DtClienteD = ClienteD.Tables[0].Copy();
                objCliente.Tables.Add(DtClienteD);
            }
            else if (ClienteDIn.Tables["ClienteInconsistente"].Rows.Count > 0)
            {
                DtClienteDIn = ClienteDIn.Tables[0].Copy();
                objCliente.Tables.Add(DtClienteDIn);
            }
            return objCliente;
        }

        /// <summary>
        /// Metodo interno que consulta en la base CHEVYPLAN Para contrato Digital la existencia de un cliente
        /// </summary>
        /// <param name="NroContrato"></param>
        /// <param name="Usuario"></param>
        /// <returns></returns>
        private DataSet ExisteClienteContratoDigital(int NroContrato)
        {
            DataTable DtXml = new DataTable();
            DataSet objCliente = new DataSet();

            //Consulta información en ContratoDigital
            ClienteContratoDigitalEN objS = new ClienteContratoDigitalEN();
            objS.pContrato = NroContrato;
            objCliente = new ClienteContratoDigitalLN().consultarClienteContratoDigitalLN(objS, "pa_DEB_Consulta_Cliente_Contrato_Digital");



            if (objCliente.Tables["ClienteContratoDigital"].Rows.Count > 0)
            {
                DtXml = wfu.AgregarTabla(Recursos.ContratoPuedeCrearse, "1");
                objCliente.Tables.Add(DtXml);
            }
            else 
            {
                DtXml = wfu.AgregarTabla(Recursos.ContratoDigitalNoexistente, "0");
                objCliente.Tables.Add(DtXml);
            }
            return objCliente;
        }

       

        /// <summary>
        /// Metodo interno que homologa los codigos de banco
        /// </summary>
        /// <param name="CodigoBanco"></param>
        /// <returns></returns>
        private DataSet HomologarBanco(int CodigoBanco, bool activo)
        {
            Banco ObjBan = new Banco();
            DataSet DsCodBanco = new DataSet();
            ObjBan.pActivo = activo;
            ObjBan.pId = CodigoBanco;
            DsCodBanco = new BancoLN().consultarBanco(ObjBan);
            return DsCodBanco;
        }

        /// <summary>
        /// Metodo interno que homologa los tipos de cuenta
        /// </summary>
        /// <param name="TipoCuenta"></param>
        /// <returns></returns>
        private DataSet HomologarTipoCuenta(int TipoCuenta)
        {
            DataSet dsTipoC = new DataSet();
            TipoCuenta objTipoC = new TipoCuenta();
            objTipoC.pId = TipoCuenta;
            dsTipoC = new TipoCuentaLN().consultarTipoC(objTipoC);
            return dsTipoC;
        }

        /// <summary>
        /// Metodo interno que homologa los canales de ingreso
        /// </summary>
        /// <param name="CanalIngreso"></param>
        /// <returns></returns>
        private DataSet HomologarCanalIngreso(int CanalIngreso)
        {
            DataSet dsTipoF = new DataSet();
            TipoFormato objTipoF = new TipoFormato();
            objTipoF.pId = CanalIngreso;
            dsTipoF = new TipoFormatoLN().consultarTipoF(objTipoF);
            return dsTipoF;
        }

        /// <summary>
        /// Metodo interno que homologa las fechas de débito
        /// </summary>
        /// <param name="CanalIngreso"></param>
        /// <returns></returns>
        private DataSet HomologarFechaDebito(int FechaDebito, bool activo)
        {
            DataSet dsFechaDebito = new DataSet();
            Fechas objFechas = new Fechas();
            objFechas.pId = FechaDebito;
            objFechas.pHabilita = activo;
            dsFechaDebito = new FechasLN().consultarFechas(objFechas);
            return dsFechaDebito;
        }

        /// <summary>
        /// Metodo interno que guarda el log de la informacion de cliente que se guardo
        /// </summary>
        /// <param name="opcion"></param>
        /// <param name="ContratoCliente"></param>
        /// <param name="Usuario"></param>
        /// <returns></returns>
        private int Log(int opcion, string ContratoCliente, string Usuario, string Mensaje)
        {
            int log = 0;
            LogsUsuario objL = new LogsUsuario();
            switch (opcion)
            {
                case 1:
                    objL.pFecha = String.Empty;
                    objL.pUsuario = Usuario;
                    objL.pDetalle = Mensaje + campos;
                    objL.pContrato = ContratoCliente;
                    objL.pMovimiento = "CREACIÓN";
                    log = new LogsUsuarioLN().insertar(objL);
                    break;
                case 2:
                    objL.pFecha = String.Empty;
                    objL.pUsuario = Usuario;
                    objL.pDetalle = Mensaje + campos;
                    objL.pContrato = ContratoCliente;
                    objL.pMovimiento = "ACTUALIZACIÓN";
                    log = new LogsUsuarioLN().insertar(objL);
                    break;
                case 3:
                    objL.pFecha = String.Empty;
                    objL.pUsuario = Usuario;
                    objL.pDetalle = Mensaje + campos;
                    objL.pContrato = ContratoCliente;
                    objL.pMovimiento = "ELIMINACIÓN";
                    new LogsUsuarioLN().insertar(objL);
                    break;
            }
            return log;
        }

        /// <summary>
        /// Consulta los codigos y nombres de los bancos activos
        /// </summary>
        /// <param name="Usuario"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public string IdBanco(string Usuario, string Password)
        {
            DataSet DsBanco = new DataSet();

            DataTable DtXml = new DataTable();

            try
            {
                objUsuario.pUsuario = Usuario;
                objUsuario.pPassword = objEncriptador.encriptar(Password);

                List<ServicioDebito.EN.Tablas.Usuario> listaU = new UsuarioLN().consultar(objUsuario);

                if (listaU.Count > 0)
                {
                    DsBanco = HomologarBanco(0, true);

                    if (DsBanco.Tables[0].Rows.Count > 0)
                    {
                        DtXml = wfu.AgregarTabla(Recursos.InformacionRetornada, "1");
                        DsBanco.Tables.Add(DtXml);
                        return DsBanco.GetXml();
                    }
                    else
                    {
                        DtXml = wfu.AgregarTabla(Recursos.ErrorProceso, "0");
                        DsBanco.Tables.Add(DtXml);
                        return DsBanco.GetXml();
                    }
                }
                else
                {
                    DtXml = wfu.AgregarTabla(Recursos.AutenticacionErrada, "0");
                    DsBanco.Tables.Add(DtXml);
                    return DsBanco.GetXml();
                }
            }
            catch (Exception)
            {
                DtXml = wfu.AgregarTabla(Recursos.ErrorProceso, "0");
                DsBanco.Tables.Add(DtXml);
                return DsBanco.GetXml();
            }
        }

        /// <summary>
        /// Consulta el id y el nombre de los tipos de cuentas activos
        /// </summary>
        /// <param name="Usuario"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public string TipoCuenta(string Usuario, string Password)
        {
            DataTable DtXml = new DataTable();
            DataSet dsTipoCuenta = new DataSet();
            try
            {
                objUsuario.pUsuario = Usuario;
                objUsuario.pPassword = objEncriptador.encriptar(Password);

                List<ServicioDebito.EN.Tablas.Usuario> listaU = new UsuarioLN().consultar(objUsuario);

                if (listaU.Count > 0)
                {
                    dsTipoCuenta = HomologarTipoCuenta(0);

                    if (dsTipoCuenta.Tables[0].Rows.Count > 0)
                    {
                        DtXml = wfu.AgregarTabla(Recursos.InformacionRetornada, "1");
                        dsTipoCuenta.Tables.Add(DtXml);
                        return dsTipoCuenta.GetXml();
                    }
                    else
                    {
                        DtXml = wfu.AgregarTabla(Recursos.ErrorProceso, "0");
                        dsTipoCuenta.Tables.Add(DtXml);
                        return dsTipoCuenta.GetXml();
                    }

                }
                else
                {
                    DtXml = wfu.AgregarTabla(Recursos.AutenticacionErrada, "0");
                    dsTipoCuenta.Tables.Add(DtXml);
                    return dsTipoCuenta.GetXml();
                }
            }
            catch (Exception)
            {
                DtXml = wfu.AgregarTabla(Recursos.ErrorProceso, "0");
                dsTipoCuenta.Tables.Add(DtXml);
                return dsTipoCuenta.GetXml();
            }
        }

        /// <summary>
        /// Consulta el id y el nombre de los periodos de débito
        /// </summary>
        /// <param name="Usuario"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public string FechaDebito(string Usuario, string Password)
        {
            DataTable DtXml = new DataTable();
            DataSet dsFechaDebito = new DataSet();

            try
            {
                objUsuario.pUsuario = Usuario;
                objUsuario.pPassword = objEncriptador.encriptar(Password);

                List<ServicioDebito.EN.Tablas.Usuario> listaU = new UsuarioLN().consultar(objUsuario);

                if (listaU.Count > 0)
                {
                    dsFechaDebito = HomologarFechaDebito(0, true);

                    if (dsFechaDebito.Tables[0].Rows.Count > 0)
                    {
                        DtXml = wfu.AgregarTabla(Recursos.InformacionRetornada, "1");
                        dsFechaDebito.Tables.Add(DtXml);
                        return dsFechaDebito.GetXml();
                    }
                    else
                    {
                        DtXml = wfu.AgregarTabla(Recursos.ErrorProceso, "0");
                        dsFechaDebito.Tables.Add(DtXml);
                        return dsFechaDebito.GetXml();
                    }
                }
                else
                {
                    DtXml = wfu.AgregarTabla(Recursos.AutenticacionErrada, "0");
                    dsFechaDebito.Tables.Add(DtXml);
                    return dsFechaDebito.GetXml();
                }
            }
            catch (Exception)
            {
                DtXml = wfu.AgregarTabla(Recursos.ErrorProceso, "0");
                dsFechaDebito.Tables.Add(DtXml);
                return dsFechaDebito.GetXml();
            }
        }

        /// <summary>
        /// Consulta el id y nombre de los canales de ingreso activos
        /// </summary>
        /// <param name="Usuario"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public string CanalIngreso(string Usuario, string Password)
        {
            DataTable DtXml = new DataTable();
            DataSet dsCanalIng = new DataSet();
            try
            {
                objUsuario.pUsuario = Usuario;
                objUsuario.pPassword = objEncriptador.encriptar(Password);

                List<ServicioDebito.EN.Tablas.Usuario> listaU = new UsuarioLN().consultar(objUsuario);

                if (listaU.Count > 0)
                {
                    dsCanalIng = HomologarCanalIngreso(0);

                    if (dsCanalIng.Tables[0].Rows.Count > 0)
                    {
                        DtXml = wfu.AgregarTabla(Recursos.InformacionRetornada, "1");
                        dsCanalIng.Tables.Add(DtXml);
                        return dsCanalIng.GetXml();
                    }
                    else
                    {
                        DtXml = wfu.AgregarTabla(Recursos.ErrorProceso, "0");
                        dsCanalIng.Tables.Add(DtXml);
                        return dsCanalIng.GetXml();
                    }
                }
                else
                {
                    DtXml = wfu.AgregarTabla(Recursos.AutenticacionErrada, "0");
                    dsCanalIng.Tables.Add(DtXml);
                    return dsCanalIng.GetXml();
                }
            }
            catch (Exception)
            {
                DtXml = wfu.AgregarTabla(Recursos.ErrorProceso, "0");
                dsCanalIng.Tables.Add(DtXml);
                return dsCanalIng.GetXml();
            }
        }

        /// <summary>
        /// Consulta la información del cliente
        /// </summary>
        /// <param name="Contrato"></param>
        /// <param name="Usuario"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public string ConsultaCliente(int Contrato, int IdTitularCuenta, string Usuario, string Password)
        {
            DataSet ConsultClient = new DataSet();
            String NroContrato = String.Empty;
            String Estado = String.Empty;
            String TipoCliente = String.Empty;
            DataTable DtXml = new DataTable();

            try
            {

                objUsuario.pUsuario = Usuario;
                objUsuario.pPassword = objEncriptador.encriptar(Password);

                List<ServicioDebito.EN.Tablas.Usuario> listaU = new UsuarioLN().consultar(objUsuario);

                if (listaU.Count > 0)
                {
                    ConsultClient = ExisteCliente(Contrato, Usuario, IdTitularCuenta);

                    if (ConsultClient.Tables["ClienteSICO"].Rows.Count > 0)
                    {
                        if (ConsultClient.Tables["ClienteSICO"].Rows[0]["AFINIDAD"].ToString() == "98" ||
                            ConsultClient.Tables["ClienteSICO"].Rows[0]["AFINIDAD"].ToString() == "99") //TERMINACIÓN DE CONTRATO
                        {
                            DtXml = wfu.AgregarTabla(Recursos.TerminacionContrato, "0");
                            ConsultClient.Tables.Add(DtXml);
                            return ConsultClient.GetXml();
                        }

                        NroContrato = ConsultClient.Tables["ClienteSICO"].Rows[0]["CONTRATO"].ToString().TrimStart('0');
                        Estado = ConsultClient.Tables["ClienteSICO"].Rows[0]["ESTADO_PAGO_PLAN"].ToString();
                        TipoCliente = ConsultClient.Tables["ClienteSICO"].Rows[0]["TIPO_CUPO"].ToString();

                        if (ConsultClient.Tables.Contains("ClienteDebito"))
                        {
                            if (Convert.ToInt32(ConsultClient.Tables["ClienteDebito"].Rows[0]["ESTADO"].ToString()) == 1 ||
                                Convert.ToInt32(ConsultClient.Tables["ClienteDebito"].Rows[0]["ESTADO"].ToString()) == 2)
                            {
                                DtXml = wfu.AgregarTabla(Recursos.UsuarioActualizadoPendiente, "1");
                                ConsultClient.Tables.Add(DtXml);
                            }
                            else
                            {
                                DtXml = wfu.AgregarTabla(Recursos.Contrato + " " + Contrato + " " + Recursos.ContratoIngresado, "1");
                                ConsultClient.Tables.Add(DtXml);
                            }

                            DataSet DsBanco = new DataSet();
                            DsBanco = HomologarBanco(Convert.ToInt32(ConsultClient.Tables["ClienteDebito"].Rows[0]["ID_BANCO"].ToString()), true);

                            if (DsBanco.Tables[0].Rows.Count == 0)
                            {
                                DataSet DsBancoTodos = new DataSet();
                                DsBancoTodos = HomologarBanco(Convert.ToInt32(ConsultClient.Tables["ClienteDebito"].Rows[0]["ID_BANCO"].ToString()), false);
                                ConsultClient = wfu.AgregarCampo(ConsultClient, Recursos.BancoInactivo + DsBancoTodos.Tables[0].Rows[0]["NOMBRE"].ToString(), "0");
                            }

                            DataSet DsFechasDeb = new DataSet();
                            DsFechasDeb = HomologarFechaDebito(Convert.ToInt32(ConsultClient.Tables["ClienteDebito"].Rows[0]["FECHA_DEBITO"].ToString()), true);

                            if (DsFechasDeb.Tables[0].Rows.Count == 0)
                            {
                                DataSet DsFechasTodos = new DataSet();
                                DsFechasTodos = HomologarFechaDebito(Convert.ToInt32(ConsultClient.Tables["ClienteDebito"].Rows[0]["FECHA_DEBITO"].ToString()), false);
                                ConsultClient = wfu.AgregarCampo(ConsultClient, Recursos.FechaInactiva + DsFechasTodos.Tables[0].Rows[0]["VALOR"].ToString(), "0");
                            }

                            TitularCuenta objTerc = new TitularCuenta();
                            DataSet ConTerceros = new DataSet();
                            DataTable DtTerceros = new DataTable();

                            if (IdTitularCuenta > 0)
                                objTerc.pId = IdTitularCuenta;
                            else
                                objTerc.pId = Convert.ToInt32(ConsultClient.Tables["ClienteDebito"].Rows[0]["ID_TITULAR_CUENTA"].ToString());

                            ConTerceros = new TitularCuentaLN().consultarTerceros(objTerc);

                            if (ConTerceros.Tables["Titular"].Rows.Count > 0)
                            {
                                DtTerceros = ConTerceros.Tables[0].Copy();
                                ConsultClient.Tables.Add(DtTerceros);
                            }
                            else
                            {
                                return ConsultClient.GetXml();
                            }

                            #region Cesion

                            DataSet DataCesion = new DataSet();
                            ClienteSico ObjCesion = new ClienteSico();
                            DataTable DtSicoCesion = new DataTable();

                            ObjCesion.pTipoDocumento = UtilidadesWeb.homologarDocumentoSico(Convert.ToInt32(ConsultClient.Tables["Titular"].Rows[0]["TIPO_IDENTIFICACION"].ToString()));
                            ObjCesion.pNroDocumento = Convert.ToInt64(ConsultClient.Tables["Titular"].Rows[0]["NUMERO_IDENTIFICACION"].ToString());
                            ObjCesion.pContrato = Contrato;

                            DataCesion = new ClienteSICOLN().consultarClienteCesion(ObjCesion, "pa_DEB_Consulta_Cesion");

                            if (DataCesion.Tables["InfoCesion"].Rows.Count > 0)
                            {
                                DtSicoCesion = DataCesion.Tables[0].Copy();
                                ConsultClient.Tables.Add(DtSicoCesion);
                                return ConsultClient.GetXml();
                            }
                            else
                            {
                                return ConsultClient.GetXml();
                            }

                            #endregion
                        }
                        else if (ConsultClient.Tables.Contains("ClienteInconsistente"))
                        {
                            DtXml = wfu.AgregarTabla(Recursos.Contrato + " " + Contrato + " " + Recursos.ContratoInconsistente, "0");
                            ConsultClient.Tables.Add(DtXml);

                            DataSet DsBancoIn = new DataSet();
                            DsBancoIn = HomologarBanco(Convert.ToInt32(ConsultClient.Tables["ClienteInconsistente"].Rows[0]["ID_BANCO"].ToString()), true);

                            if (DsBancoIn.Tables[0].Rows.Count == 0)
                            {
                                DataSet DsBancoTodos = new DataSet();
                                DsBancoTodos = HomologarBanco(Convert.ToInt32(ConsultClient.Tables["ClienteInconsistente"].Rows[0]["ID_BANCO"].ToString()), false);
                                ConsultClient = wfu.AgregarCampo(ConsultClient, Recursos.BancoInactivo + DsBancoTodos.Tables[0].Rows[0]["NOMBRE"].ToString(), "0");
                            }

                            TitularCuentaInconsistente objTitularIncon = new TitularCuentaInconsistente();
                            DataSet ConTerceroIncon = new DataSet();
                            DataTable DtTerceroIncon = new DataTable();

                            objTitularIncon.pId = Convert.ToInt32(ConsultClient.Tables["ClienteInconsistente"].Rows[0]["ID_TITULAR_CUENTA"].ToString());
                            ConTerceroIncon = new TitularCuentaInconsistenteLN().consultarTerceros(objTitularIncon);

                            if (ConTerceroIncon.Tables["Titular Inconsistente"].Rows.Count > 0)
                            {
                                DtTerceroIncon = ConTerceroIncon.Tables[0].Copy();
                                ConsultClient.Tables.Add(DtTerceroIncon);
                                return ConsultClient.GetXml();
                            }
                            else
                            {
                                return ConsultClient.GetXml();
                            }
                        }
                        else
                        {
                            if (TipoCliente.Equals("Suscriptor") || TipoCliente.Equals("Adjudicado") || TipoCliente.Equals("Ganador") || TipoCliente.Equals("CtasXDevolver"))
                            {
                                if (Estado.Equals("Vigente") || Estado.Equals("Mora") || Estado.Equals("Reemplazado") || Estado.Equals("Prejurídica"))
                                {
                                    DtXml = wfu.AgregarTabla(Recursos.ContratoPuedeCrearse, "1");
                                    ConsultClient.Tables.Add(DtXml);
                                    return ConsultClient.GetXml();
                                }
                                else
                                {
                                    DtXml = wfu.AgregarTabla(Recursos.ContratoEstados, "0");
                                    ConsultClient.Tables.Add(DtXml);
                                    return ConsultClient.GetXml();
                                }
                            }
                            else
                            {
                                DtXml = wfu.AgregarTabla(Recursos.TiposClientes, "0");
                                ConsultClient.Tables.Add(DtXml);
                                return ConsultClient.GetXml();
                            }
                        }
                    }
                    else if (ConsultClient.Tables.Contains("ClienteInconsistente"))
                    {
                        DtXml = wfu.AgregarTabla(Recursos.Contrato + " " + Contrato + " " + Recursos.ContratoInconsistente, "0");
                        ConsultClient.Tables.Add(DtXml);

                        TitularCuentaInconsistente objTitularIncon = new TitularCuentaInconsistente();
                        DataSet ConTerceroIncon = new DataSet();
                        DataTable DtTerceroIncon = new DataTable();

                        objTitularIncon.pId = Convert.ToInt32(ConsultClient.Tables["ClienteInconsistente"].Rows[0]["ID_TITULAR_CUENTA"].ToString());
                        ConTerceroIncon = new TitularCuentaInconsistenteLN().consultarTerceros(objTitularIncon);

                        if (ConTerceroIncon.Tables["Titular Inconsistente"].Rows.Count > 0)
                        {
                            DtTerceroIncon = ConTerceroIncon.Tables[0].Copy();
                            ConsultClient.Tables.Add(DtTerceroIncon);
                            return ConsultClient.GetXml();
                        }
                        else
                        {
                            return ConsultClient.GetXml();
                        }
                    }
                    else
                    {
                        DtXml = wfu.AgregarTabla(Recursos.UsuarioNoExiste, "0");
                        ConsultClient.Tables.Add(DtXml);
                        return ConsultClient.GetXml();
                    }
                }
                else
                {
                    DtXml = wfu.AgregarTabla(Recursos.AutenticacionErrada, "0");
                    ConsultClient.Tables.Add(DtXml);
                    return ConsultClient.GetXml();
                }
            }
            catch (Exception ex)
            {
                DtXml = wfu.AgregarTabla(ex.Message, "0");
                ConsultClient.Tables.Add(DtXml);
                return ConsultClient.GetXml();
            }
        }

        /// <summary>
        /// Guarda la informacion del cliente
        /// </summary>
        /// <param name="Contrato"></param>
        /// <param name="IdBanco"></param>
        /// <param name="TipoCuenta"></param>
        /// <param name="NumeroCuenta"></param>
        /// <param name="CanalIngreso"></param>
        /// <param name="Tercero"></param>
        /// <param name="NombreTercero"></param>
        /// <param name="IdentificacionTercero"></param>
        /// <param name="TipoIdTercero"></param>
        /// <param name="DireccionIp"></param>
        /// <param name="Usuario"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public string GuardarCliente(int Contrato, int IdBanco, int TipoCuenta, string NumeroCuenta, int CanalIngreso, bool Tercero, string NombreTercero, long IdentificacionTercero, int TipoIdTercero, string DireccionIp, int FechaDebito, string Usuario, string Password)
        {
            DataSet ConsultClient = new DataSet();
            DataTable DtXml = new DataTable();
            String PasswordCall = String.Empty;
            int ClienteCesion = 0;
            try
            {
                objUsuario.pUsuario = Usuario;
                objUsuario.pPassword = objEncriptador.encriptar(Password);

                List<ServicioDebito.EN.Tablas.Usuario> listaU = new UsuarioLN().consultar(objUsuario);

                if (listaU.Count > 0)
                {
                    objUsuario = listaU[0];
                    PasswordCall = objEncriptador.desencriptar(objUsuario.pPassword);
                    TitularCuenta objT = new TitularCuenta();

                    ConsultClient = ExisteCliente(Contrato, Usuario, 0);

                    if (ConsultClient.Tables[0].Rows.Count > 0)
                    {
                        if (ConsultClient.Tables.Contains("ClienteDebito"))
                        {
                            if (Convert.ToInt32(ConsultClient.Tables["ClienteDebito"].Rows[0]["ESTADO"].ToString()) == 15)
                            {
                                objT.pId = Convert.ToInt32(ConsultClient.Tables["ClienteDebito"].Rows[0]["ID_TITULAR_CUENTA"].ToString());
                                ClienteCesion++;
                                goto GuardarCesion;
                            }
                            else
                            {
                                DtXml = wfu.AgregarTabla(Recursos.Contrato + " " + Contrato + " " + Recursos.ContratoIngresado, "0");
                                ConsultClient.Tables.Add(DtXml);
                                return ConsultClient.GetXml();
                            }
                        }

                        if (ConsultClient.Tables.Contains("ClienteInconsistente"))
                        {
                            if (Usuario == "APP" || Usuario == "WEB")
                            {
                                TitularCuentaInconsistente objTitularIncon = new TitularCuentaInconsistente();
                                DataSet ConTerceroIncon = new DataSet();
                                int valorTIn = 0;
                                int valorDIn = 0;
                                objTitularIncon.pId = Convert.ToInt32(ConsultClient.Tables["ClienteInconsistente"].Rows[0]["ID_TITULAR_CUENTA"].ToString());
                                ConTerceroIncon = new TitularCuentaInconsistenteLN().consultarTerceros(objTitularIncon);

                                if (ConTerceroIncon.Tables["Titular Inconsistente"].Rows.Count > 0)
                                {
                                    TitularCuentaInconsistente objTIn = new TitularCuentaInconsistente();
                                    objTIn.pId = Convert.ToInt32(ConTerceroIncon.Tables["Titular Inconsistente"].Rows[0]["ID"].ToString());
                                    valorTIn = new TitularCuentaInconsistenteLN().borrar(objTIn);
                                }
                                else
                                {
                                    DtXml = wfu.AgregarTabla(Recursos.ErrorProceso, "0");
                                    ConsultClient.Tables.Add(DtXml);
                                    return ConsultClient.GetXml();
                                }

                                if (valorTIn == 0)
                                {
                                    DatosDebitoInconsistente objDIn = new DatosDebitoInconsistente();
                                    objDIn.pId = Convert.ToInt32(ConsultClient.Tables["ClienteInconsistente"].Rows[0]["ID"].ToString());
                                    valorDIn = new DatosDebitoInconsistenteLN().borrar(objDIn);
                                }
                                else
                                {
                                    DtXml = wfu.AgregarTabla(Recursos.ErrorProceso, "0");
                                    ConsultClient.Tables.Add(DtXml);
                                    return ConsultClient.GetXml();
                                }

                                if (valorTIn == 0 && valorDIn == 0)
                                {
                                    #region (INFORMACION PARA LOG)
                                    campos = string.Concat(Contrato);
                                    #endregion
                                    Log(3, Convert.ToString(Contrato), Usuario, "Se eliminó el contrato Inconsistente N°: ");
                                    ClienteCesion = 0;
                                    goto GuardarCesion;
                                }
                                else
                                {
                                    DtXml = wfu.AgregarTabla(Recursos.ErrorProceso, "0");
                                    ConsultClient.Tables.Add(DtXml);
                                    return ConsultClient.GetXml();
                                }
                            }
                            else
                            {
                                DtXml = wfu.AgregarTabla(Recursos.Contrato + " " + Contrato + " " + Recursos.ContratoInconsistente, "0");
                                ConsultClient.Tables.Add(DtXml);
                                return ConsultClient.GetXml();
                            }
                        }

                    GuardarCesion:

                        if (Tercero == true)
                        {
                            objT.pNombre = NombreTercero;
                            objT.pNumeroIdentificacion = Convert.ToString(IdentificacionTercero);
                            objT.pTipoIdentificacion = TipoIdTercero;
                        }
                        else
                        {
                            objT.pNombre = ConsultClient.Tables["ClienteSICO"].Rows[0]["NOMBRE_CLIENTE"].ToString().TrimStart('0');
                            objT.pNumeroIdentificacion = ConsultClient.Tables["ClienteSICO"].Rows[0]["NUMERO_DOCUMENTO_CLIENTE"].ToString().TrimStart('0');
                            objT.pTipoIdentificacion = UtilidadesWeb.homologarDocumento(ConsultClient.Tables["ClienteSICO"].Rows[0]["TIPO_DOCUMENTO"].ToString());
                        }


                        int valorT = 0;
                        if (ClienteCesion > 0)
                            valorT = new TitularCuentaLN().actualizar(objT);
                        else
                            valorT = new TitularCuentaLN().insertar(objT);

                        if (valorT <= 0)
                        {
                            DtXml = wfu.AgregarTabla(Recursos.ErrorProceso, "0");
                            ConsultClient.Tables.Add(DtXml);
                            return ConsultClient.GetXml();
                        }
                        else
                        {
                            DatosDebito objD = new DatosDebito();
                            objD.pContrato = Convert.ToString(Contrato);
                            objD.pDigito = Convert.ToInt32(UtilidadesWeb.calculoDigito(objD.pContrato));
                            objD.pIdBanco = IdBanco;
                            objD.pEstado = 1; //CLIENTE INICIA EL ESTADO PRENOTA (ESTADO 1)
                            objD.pIntentos = 0;
                            objD.pTipoCuenta = TipoCuenta;
                            objD.pNumeroCuenta = NumeroCuenta;
                            objD.pIdFormatoDebito = CanalIngreso;
                            objD.pIdFormatoCancelacion = 0;
                            objD.pDireccionIp = DireccionIp;
                            objD.pFechaDebito = FechaDebito;
                            objD.pTercero = Tercero;
                            objD.pSuspendido = false;
                            objD.pFechaInicioSus = String.Empty;
                            objD.pFechaFinSus = String.Empty;

                            int valorD = 0;
                            if (ClienteCesion > 0)
                            {
                                objD.pId = Convert.ToInt32(ConsultClient.Tables["ClienteDebito"].Rows[0]["ID"].ToString());
                                objD.pIdTitularCuenta = objT.pId;
                                valorD = new DatosDebitoLN().actualizar(objD);
                            }
                            else
                            {
                                objD.pIdTitularCuenta = valorT;
                                valorD = new DatosDebitoLN().insertar(objD);
                            }

                            if (valorD > 0)
                            {
                                DataSet dsBanco = new DataSet();
                                DataSet dsTipoC = new DataSet();
                                DataSet dsTipoF = new DataSet();
                                DataSet dsFecha = new DataSet();
                                dsBanco = HomologarBanco(IdBanco, true);
                                dsTipoC = HomologarTipoCuenta(TipoCuenta);
                                dsTipoF = HomologarCanalIngreso(CanalIngreso);
                                dsFecha = HomologarFechaDebito(FechaDebito, true);

                                int logueo = 0;
                                #region (INFORMACION PARA LOG)
                                campos = string.Concat(objD.pContrato,
                                    " con BANCO:", Convert.ToString(dsBanco.Tables[0].Rows[0]["NOMBRE"].ToString()).ToUpper(),
                                    ", TIPO DE CUENTA:", Convert.ToString(dsTipoC.Tables[0].Rows[0]["VALOR"].ToString()),
                                    ", NÚMERO DE CUENTA:", Convert.ToString(objD.pNumeroCuenta),
                                    ", FORMATO DEBITO:", Convert.ToString(dsTipoF.Tables[0].Rows[0]["VALOR"].ToString()),
                                    ", CUENTA TERCERO:", Convert.ToString(objD.pTercero).ToUpper(),
                                    ", NOMBRE:", Convert.ToString(objT.pNombre),
                                    ", TIPO DE IDENTIFICACIÓN:", UtilidadesWeb.homologarDocumentoAbrebiatura(Convert.ToInt32(objT.pTipoIdentificacion)),
                                    ", IDENTIFICACIÓN:", Convert.ToString(objT.pNumeroIdentificacion),
                                    ", DÉBITO A PARTIR DEL:", Convert.ToString(dsFecha.Tables[0].Rows[0]["VALOR"].ToString()),
                                    ", DIRECCION_IP:", Convert.ToString(objD.pDireccionIp));
                                #endregion
                                logueo = Log(1, objD.pContrato, Usuario, "Se creó el contrato N°: ");

                                if (logueo > 0)
                                {
                                    String Correo = String.Empty;
                                    String Mensaje = String.Empty;
                                    int LongitudNroCuenta = 0;

                                    if (IdBanco == 27) //Banco BBVA
                                    {
                                        DtXml = wfu.AgregarTabla("!Hola " + getNombrePropio(ConsultClient.Tables["ClienteSICO"].Rows[0]["PRIMERNOMBRE"].ToString()) + " " +
                                        ConsultClient.Tables["ClienteSICO"].Rows[0]["PRONOMBRE"].ToString() + " " + Recursos.InscripcionClienteNuevoBBVA, "1");
                                        ConsultClient.Tables.Add(DtXml);
                                    }
                                    else
                                    {
                                        DtXml = wfu.AgregarTabla("!Hola " + getNombrePropio(ConsultClient.Tables["ClienteSICO"].Rows[0]["PRIMERNOMBRE"].ToString()) + " " +
                                        ConsultClient.Tables["ClienteSICO"].Rows[0]["PRONOMBRE"].ToString() + " " + Recursos.InscripcionClienteNuevo, "1");
                                        ConsultClient.Tables.Add(DtXml);
                                    }

                                    Mensajes objM = new Mensajes();
                                    objM.pTipoContrato = ConsultClient.Tables["ClienteSICO"].Rows[0]["TIPO_CUPO"].ToString().Substring(0, 1);
                                    objM.pEstadoDebito = 1;
                                    objM.pMotivo = 1;
                                    List<ServicioDebito.EN.Tablas.Mensajes> listaM = new MensajesLN().consultar(objM);

                                    if (ConsultClient.Tables["ClienteSICO"].Rows[0]["EMAIL_CLIENTE"].ToString() != String.Empty)
                                    {
                                        if (listaM.Count > 0)
                                        {
                                            if (ConsultClient.Tables["ClienteSICO"].Rows[0]["LeydPcorreo"].ToString() == "SI")
                                            {
                                                DataSet DsCorreoBanco = new DataSet();
                                                DsCorreoBanco = ConsultarCorreosBancos();
                                                String Remitente = String.Empty;
                                                String ConCopia = String.Empty;
                                                String CorreoCliente = String.Empty;

                                                if (DsCorreoBanco.Tables.Contains("tabla"))
                                                {
                                                    Remitente = DsCorreoBanco.Tables["tabla"].Rows[0]["REMITENTE"].ToString();
                                                    ConCopia = DsCorreoBanco.Tables["tabla"].Rows[0]["CORREOS_CONTROL"].ToString();
                                                }

                                                objM = listaM[0];
                                                LongitudNroCuenta = objD.pNumeroCuenta.Length;
                                                Mensaje = objM.pMensaje
                                                .Replace("@FechaActual_", Convert.ToString(DateTime.Now))
                                                .Replace("@Nombrecliente_", objT.pNombre)
                                                .Replace("@NroContrato_", objD.pContrato)
                                                .Replace("@Ciudad_", ConsultClient.Tables["ClienteSICO"].Rows[0]["CIUDAD"].ToString())
                                                .Replace("@Banco_", dsBanco.Tables[0].Rows[0]["NOMBRE"].ToString().ToUpper())
                                                .Replace("@TipoCuenta_", dsTipoC.Tables[0].Rows[0]["VALOR"].ToString())
                                                .Replace("@NroCuenta_", "".PadLeft(5, '*') + objD.pNumeroCuenta.Substring((LongitudNroCuenta - 4), 4))
                                                .Replace("@Debito_", dsFecha.Tables[0].Rows[0]["VALOR"].ToString());

                                                CorreoCliente = ConsultClient.Tables["ClienteSICO"].Rows[0]["EMAIL_CLIENTE"].ToString();

                                                if (ConfigurationManager.AppSettings["CorreoCliente"].ToString() == "Si")
                                                {
                                                    Correo = wfu.EnvioMail("", objM.pAsunto, Mensaje, CorreoCliente, Remitente, ConCopia);
                                                }
                                                else
                                                {
                                                    Correo = wfu.EnvioMail("", objM.pAsunto, Mensaje, ConCopia, Remitente, "");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            DtXml = wfu.AgregarTabla(Recursos.ErrorProceso, "0");
                                            ConsultClient.Tables.Add(DtXml);
                                            return ConsultClient.GetXml();
                                        }
                                    }

                                    return ConsultClient.GetXml();
                                }
                                else
                                {
                                    DtXml = wfu.AgregarTabla(Recursos.ErrorProceso, "0");
                                    ConsultClient.Tables.Add(DtXml);
                                    return ConsultClient.GetXml();
                                }
                            }
                            else
                            {
                                DtXml = wfu.AgregarTabla(Recursos.ErrorProceso, "0");
                                ConsultClient.Tables.Add(DtXml);
                                return ConsultClient.GetXml();
                            }
                        }

                    }
                    else
                    {
                        DtXml = wfu.AgregarTabla(Recursos.UsuarioNoExiste, "0");
                        ConsultClient.Tables.Add(DtXml);
                        return ConsultClient.GetXml();
                    }
                }
                else
                {
                    DtXml = wfu.AgregarTabla(Recursos.AutenticacionErrada, "0");
                    ConsultClient.Tables.Add(DtXml);
                    return ConsultClient.GetXml();
                }
            }
            catch (Exception)
            {
                DtXml = wfu.AgregarTabla(Recursos.ErrorProceso, "0");
                ConsultClient.Tables.Add(DtXml);
                return ConsultClient.GetXml();
            }
        }

        /// <summary>
        /// Modificacion de datos del cliente que realiza desde el APP o la Pagina web
        /// </summary>
        /// <param name="Contrato"></param>
        /// <param name="NumeroCuenta"></param>
        /// <param name="TipoCuenta"></param>
        /// <param name="IdBanco"></param>
        /// <param name="DireccionIp"></param>
        /// <param name="Usuario"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public string ModificarDatos(int Contrato, string NumeroCuenta, int TipoCuenta, int IdBanco, string DireccionIp, int FechaDebito, string Usuario, string Password)
        {
            DataSet ConsultClient = new DataSet();
            DataTable DtXml = new DataTable();
            try
            {
                objUsuario.pUsuario = Usuario;
                objUsuario.pPassword = objEncriptador.encriptar(Password);

                List<ServicioDebito.EN.Tablas.Usuario> listaU = new UsuarioLN().consultar(objUsuario);

                if (listaU.Count > 0)
                {
                    ConsultClient = ExisteCliente(Contrato, Usuario, 0);

                    if (ConsultClient.Tables.Contains("ClienteDebito"))
                    {
                        TitularCuenta objTerc = new TitularCuenta();
                        DataSet ConTerceros = new DataSet();
                        DataTable DtTerceros = new DataTable();

                        objTerc.pId = Convert.ToInt32(ConsultClient.Tables["ClienteDebito"].Rows[0]["ID_TITULAR_CUENTA"].ToString());
                        ConTerceros = new TitularCuentaLN().consultarTerceros(objTerc);

                        if (ConTerceros.Tables["Titular"].Rows.Count > 0)
                        {
                            int actuaclient = 0;
                            if (//Si esta en prenota en proceso ó debito en proceso
                                (Convert.ToInt32(ConsultClient.Tables["ClienteDebito"].Rows[0]["ESTADO"].ToString()) == 2 ||
                                Convert.ToInt32(ConsultClient.Tables["ClienteDebito"].Rows[0]["ESTADO"].ToString()) == 5)
                                && //Y si el banco ó el tipo de cuenta ó el número de cuenta son diferentes
                                (Convert.ToInt32(ConsultClient.Tables["ClienteDebito"].Rows[0]["ID_BANCO"].ToString()) != IdBanco ||
                                Convert.ToInt32(ConsultClient.Tables["ClienteDebito"].Rows[0]["TIPO_CUENTA"].ToString()) != TipoCuenta ||
                                ConsultClient.Tables["ClienteDebito"].Rows[0]["NUMERO_CUENTA"].ToString() != NumeroCuenta)
                                )
                            {

                                ActualizaCliente objAc = new ActualizaCliente();
                                objAc.pContrato = ConsultClient.Tables["ClienteDebito"].Rows[0]["CONTRATO"].ToString();
                                objAc.pIdTitularCuenta = Convert.ToInt32(ConsultClient.Tables["ClienteDebito"].Rows[0]["ID_TITULAR_CUENTA"].ToString());
                                objAc.pIdBanco = IdBanco;
                                objAc.pIdTipoCuenta = TipoCuenta;
                                objAc.pNumeroCuenta = NumeroCuenta;
                                objAc.pDireccionIp = DireccionIp;
                                objAc.pUsuario = Usuario;
                                List<ServicioDebito.EN.Tablas.ActualizaCliente> listaB = new ActualizaClienteLN().consultarDatos(objAc);

                                int logvalor = 0;
                                string MensajeLog = String.Empty;
                                if (listaB.Count > 0)
                                {
                                    actuaclient = new ActualizaClienteLN().actualizar(objAc);
                                    logvalor = 2;
                                    MensajeLog = "Se actualizó temporalmente el contrato N°: ";
                                }
                                else
                                {
                                    actuaclient = new ActualizaClienteLN().insertar(objAc);
                                    logvalor = 1;
                                    MensajeLog = "Se creo temporalmente el contrato N°: ";
                                }

                                int actua = 0;
                                DatosDebito ObjDat = new DatosDebito();
                                ObjDat.pId = Convert.ToInt32(ConsultClient.Tables["ClienteDebito"].Rows[0]["ID"].ToString());
                                ObjDat.pContrato = ConsultClient.Tables["ClienteDebito"].Rows[0]["CONTRATO"].ToString();
                                ObjDat.pFechaDebito = FechaDebito;

                                actua = new DatosDebitoLN().actualizarFecha(ObjDat);

                                if (actuaclient > 0 && actua > 0)
                                {
                                    DataSet dsBanco = new DataSet();
                                    DataSet dsTipoC = new DataSet();
                                    DataSet dsFecha = new DataSet();
                                    dsBanco = HomologarBanco(IdBanco, true);
                                    dsTipoC = HomologarTipoCuenta(TipoCuenta);
                                    dsFecha = HomologarFechaDebito(FechaDebito, true);

                                    int logueo = 0;
                                    #region (INFORMACION PARA LOG)
                                    campos = string.Concat(objAc.pContrato,
                                        " con BANCO:", Convert.ToString(dsBanco.Tables[0].Rows[0]["NOMBRE"].ToString()).ToUpper(),
                                        ", TIPO DE CUENTA:", Convert.ToString(dsTipoC.Tables[0].Rows[0]["VALOR"].ToString()),
                                        ", NUMERO DE CUENTA:", Convert.ToString(objAc.pNumeroCuenta),
                                        ", DÉBITO A PARTIR DEL:", Convert.ToString(dsFecha.Tables[0].Rows[0]["VALOR"].ToString()),
                                        ", DIRECCION_IP:", Convert.ToString(objAc.pDireccionIp));
                                    #endregion
                                    logueo = Log(logvalor, objAc.pContrato, Usuario, MensajeLog);

                                    if (logueo > 0)
                                    {
                                        String Correo = String.Empty;
                                        int LongitudNroCuenta = 0;
                                        String Mensaje = String.Empty;
                                        Mensajes objM = new Mensajes();
                                        objM.pTipoContrato = ConsultClient.Tables["ClienteSICO"].Rows[0]["TIPO_CUPO"].ToString().Substring(0, 1);
                                        objM.pEstadoDebito = 1;
                                        objM.pMotivo = 2;
                                        List<ServicioDebito.EN.Tablas.Mensajes> listaM = new MensajesLN().consultar(objM);

                                        if (listaM.Count > 0)
                                        {
                                            if (ConsultClient.Tables["ClienteSICO"].Rows[0]["EMAIL_CLIENTE"].ToString() != String.Empty)
                                            {
                                                if (ConsultClient.Tables["ClienteSICO"].Rows[0]["LeydPcorreo"].ToString() == "SI")
                                                {
                                                    DataSet DsCorreoBanco = new DataSet();
                                                    DsCorreoBanco = ConsultarCorreosBancos();
                                                    String Remitente = String.Empty;
                                                    String ConCopia = String.Empty;
                                                    String CorreoCliente = String.Empty;

                                                    if (DsCorreoBanco.Tables.Contains("tabla"))
                                                    {
                                                        Remitente = DsCorreoBanco.Tables["tabla"].Rows[0]["REMITENTE"].ToString();
                                                        ConCopia = DsCorreoBanco.Tables["tabla"].Rows[0]["CORREOS_CONTROL"].ToString();
                                                    }

                                                    LongitudNroCuenta = objAc.pNumeroCuenta.Length;
                                                    Mensajes objMen = new Mensajes();
                                                    objMen = listaM[0];
                                                    Mensaje = objMen.pMensaje
                                                    .Replace("@FechaActual_", Convert.ToString(DateTime.Now))
                                                    .Replace("@Nombrecliente_", ConsultClient.Tables["ClienteSICO"].Rows[0]["NOMBRE_CLIENTE"].ToString())
                                                    .Replace("@NroContrato_", ConsultClient.Tables["ClienteDebito"].Rows[0]["CONTRATO"].ToString())
                                                    .Replace("@Ciudad_", ConsultClient.Tables["ClienteSICO"].Rows[0]["CIUDAD"].ToString())
                                                    .Replace("@Banco_", dsBanco.Tables[0].Rows[0]["NOMBRE"].ToString().ToUpper())
                                                    .Replace("@TipoCuenta_", dsTipoC.Tables[0].Rows[0]["VALOR"].ToString())
                                                    .Replace("@NroCuenta_", "".PadLeft(5, '*') + objAc.pNumeroCuenta.Substring((LongitudNroCuenta - 4), 4))
                                                    .Replace("@Debito_", dsFecha.Tables[0].Rows[0]["VALOR"].ToString());

                                                    CorreoCliente = ConsultClient.Tables["ClienteSICO"].Rows[0]["EMAIL_CLIENTE"].ToString();

                                                    if (ConfigurationManager.AppSettings["CorreoCliente"].ToString() == "Si")
                                                    {
                                                        Correo = wfu.EnvioMail("", objMen.pAsunto, Mensaje, CorreoCliente, Remitente, ConCopia);
                                                    }
                                                    else
                                                    {
                                                        Correo = wfu.EnvioMail("", objMen.pAsunto, Mensaje, ConCopia, Remitente, "");
                                                    }

                                                }
                                            }

                                            if (IdBanco == 27) //Banco BBVA
                                            {
                                                DtXml = wfu.AgregarTabla("!Hola " + getNombrePropio(ConsultClient.Tables["ClienteSICO"].Rows[0]["PRIMERNOMBRE"].ToString()) + " " +
                                                Recursos.UsuarioActualizadoBBVA, "1");
                                                ConsultClient.Tables.Add(DtXml);
                                                return ConsultClient.GetXml();
                                            }
                                            else
                                            {
                                                DtXml = wfu.AgregarTabla("!Hola " + getNombrePropio(ConsultClient.Tables["ClienteSICO"].Rows[0]["PRIMERNOMBRE"].ToString()) + " " +
                                                Recursos.UsuarioActualizado, "1");
                                                ConsultClient.Tables.Add(DtXml);
                                                return ConsultClient.GetXml();
                                            }
                                        }
                                        else
                                        {
                                            DtXml = wfu.AgregarTabla(Recursos.ErrorProceso, "0");
                                            ConsultClient.Tables.Add(DtXml);
                                            return ConsultClient.GetXml();
                                        }
                                    }
                                    else
                                    {
                                        DtXml = wfu.AgregarTabla(Recursos.ErrorProceso, "0");
                                        ConsultClient.Tables.Add(DtXml);
                                        return ConsultClient.GetXml();
                                    }
                                }
                                else
                                {
                                    DtXml = wfu.AgregarTabla(Recursos.ErrorProceso, "0");
                                    ConsultClient.Tables.Add(DtXml);
                                    return ConsultClient.GetXml();
                                }
                            }
                            else
                            {
                                DatosDebito objD = new DatosDebito();
                                TitularCuenta objT = new TitularCuenta();
                                int valorT = 0;
                                objD.pId = Convert.ToInt32(ConsultClient.Tables["ClienteDebito"].Rows[0]["ID"].ToString());
                                objD.pContrato = ConsultClient.Tables["ClienteDebito"].Rows[0]["CONTRATO"].ToString();
                                objD.pDigito = Convert.ToInt32(ConsultClient.Tables["ClienteDebito"].Rows[0]["DIGITO"].ToString());
                                objD.pIdBanco = IdBanco;
                                objD.pTipoCuenta = TipoCuenta;
                                objD.pNumeroCuenta = NumeroCuenta;
                                objD.pIdFormatoDebito = Convert.ToInt32(ConsultClient.Tables["ClienteDebito"].Rows[0]["ID_FORMATO_DEBITO"].ToString());
                                objD.pIdFormatoCancelacion = Convert.ToInt32(ConsultClient.Tables["ClienteDebito"].Rows[0]["ID_FORMATO_CANCELACION"].ToString());

                                if (Convert.ToBoolean(ConsultClient.Tables["ClienteDebito"].Rows[0]["TERCERO"].ToString()) == true)
                                {
                                    objD.pTercero = false;
                                    objT.pId = Convert.ToInt32(ConsultClient.Tables["ClienteDebito"].Rows[0]["ID_TITULAR_CUENTA"].ToString());
                                    objT.pNombre = ConsultClient.Tables["ClienteSICO"].Rows[0]["NOMBRE_CLIENTE"].ToString().TrimStart('0');
                                    objT.pNumeroIdentificacion = ConsultClient.Tables["ClienteSICO"].Rows[0]["NUMERO_DOCUMENTO_CLIENTE"].ToString().TrimStart('0');
                                    objT.pTipoIdentificacion = UtilidadesWeb.homologarDocumento(ConsultClient.Tables["ClienteSICO"].Rows[0]["TIPO_DOCUMENTO"].ToString());
                                    valorT = new TitularCuentaLN().actualizar(objT);
                                }
                                else
                                {
                                    objD.pTercero = Convert.ToBoolean(ConsultClient.Tables["ClienteDebito"].Rows[0]["TERCERO"].ToString());
                                    valorT++;
                                }

                                objD.pIdTitularCuenta = Convert.ToInt32(ConsultClient.Tables["ClienteDebito"].Rows[0]["ID_TITULAR_CUENTA"].ToString());
                                objD.pDireccionIp = DireccionIp;
                                objD.pSuspendido = false;
                                objD.pFechaInicioSus = String.Empty;
                                objD.pFechaFinSus = String.Empty;
                                objD.pFechaDebito = FechaDebito;

                                if (Convert.ToInt32(ConsultClient.Tables["ClienteDebito"].Rows[0]["ID_BANCO"].ToString()) != IdBanco ||
                                    Convert.ToInt32(ConsultClient.Tables["ClienteDebito"].Rows[0]["TIPO_CUENTA"].ToString()) != TipoCuenta ||
                                    ConsultClient.Tables["ClienteDebito"].Rows[0]["NUMERO_CUENTA"].ToString() != NumeroCuenta)
                                {
                                    objD.pEstado = 1;
                                }
                                else
                                {
                                    objD.pEstado = Convert.ToInt32(ConsultClient.Tables["ClienteDebito"].Rows[0]["ESTADO"].ToString());
                                }

                                int valorD = 0;

                                valorD = new DatosDebitoLN().actualizar(objD);

                                if (valorD > 0 && valorT > 0)
                                {
                                    DataSet dsBanco = new DataSet();
                                    DataSet dsTipoC = new DataSet();
                                    DataSet dsFecha = new DataSet();
                                    dsBanco = HomologarBanco(IdBanco, true);
                                    dsTipoC = HomologarTipoCuenta(TipoCuenta);
                                    dsFecha = HomologarFechaDebito(FechaDebito, true);



                                    int logueo = 0;
                                    #region (INFORMACION PARA LOG)
                                    campos = string.Concat(objD.pContrato,
                                        " con BANCO:", Convert.ToString(dsBanco.Tables[0].Rows[0]["NOMBRE"].ToString()).ToUpper(),
                                        ", TIPO DE CUENTA:", Convert.ToString(dsTipoC.Tables[0].Rows[0]["VALOR"].ToString()),
                                        ", NÚMERO DE CUENTA:", Convert.ToString(objD.pNumeroCuenta),
                                        ", DÉBITO A PARTIR DEL: ", Convert.ToString(dsFecha.Tables[0].Rows[0]["VALOR"].ToString()),
                                        ", DIRECCION_IP:", Convert.ToString(objD.pDireccionIp));
                                    #endregion
                                    logueo = Log(2, objD.pContrato, Usuario, "Se actualizó el contrato N°: ");

                                    if (logueo > 0)
                                    {
                                        String Correo = String.Empty;
                                        int LongitudNroCuenta = 0;
                                        String Mensaje = String.Empty;
                                        Mensajes objM = new Mensajes();
                                        objM.pTipoContrato = ConsultClient.Tables["ClienteSICO"].Rows[0]["TIPO_CUPO"].ToString().Substring(0, 1);
                                        objM.pEstadoDebito = 1;
                                        objM.pMotivo = 2;
                                        List<ServicioDebito.EN.Tablas.Mensajes> listaM = new MensajesLN().consultar(objM);


                                        if (listaM.Count > 0)
                                        {
                                            if (ConsultClient.Tables["ClienteSICO"].Rows[0]["EMAIL_CLIENTE"].ToString() != String.Empty)
                                            {
                                                if (ConsultClient.Tables["ClienteSICO"].Rows[0]["LeydPcorreo"].ToString() == "SI")
                                                {
                                                    DataSet DsCorreoBanco = new DataSet();
                                                    DsCorreoBanco = ConsultarCorreosBancos();
                                                    String Remitente = String.Empty;
                                                    String ConCopia = String.Empty;
                                                    String CorreoCliente = String.Empty;

                                                    if (DsCorreoBanco.Tables.Contains("tabla"))
                                                    {
                                                        Remitente = DsCorreoBanco.Tables["tabla"].Rows[0]["REMITENTE"].ToString();
                                                        ConCopia = DsCorreoBanco.Tables["tabla"].Rows[0]["CORREOS_CONTROL"].ToString();
                                                    }

                                                    LongitudNroCuenta = objD.pNumeroCuenta.Length;
                                                    Mensajes objMen = new Mensajes();
                                                    objMen = listaM[0];
                                                    Mensaje = objMen.pMensaje
                                                    .Replace("@FechaActual_", Convert.ToString(DateTime.Now))
                                                    .Replace("@Nombrecliente_", ConsultClient.Tables["ClienteSICO"].Rows[0]["NOMBRE_CLIENTE"].ToString())
                                                    .Replace("@NroContrato_", ConsultClient.Tables["ClienteDebito"].Rows[0]["CONTRATO"].ToString())
                                                    .Replace("@Ciudad_", ConsultClient.Tables["ClienteSICO"].Rows[0]["CIUDAD"].ToString())
                                                    .Replace("@Banco_", dsBanco.Tables[0].Rows[0]["NOMBRE"].ToString().ToUpper())
                                                    .Replace("@TipoCuenta_", dsTipoC.Tables[0].Rows[0]["VALOR"].ToString())
                                                    .Replace("@NroCuenta_", "".PadLeft(5, '*') + objD.pNumeroCuenta.Substring((LongitudNroCuenta - 4), 4))
                                                    .Replace("@Debito_", dsFecha.Tables[0].Rows[0]["VALOR"].ToString());

                                                    CorreoCliente = ConsultClient.Tables["ClienteSICO"].Rows[0]["EMAIL_CLIENTE"].ToString();

                                                    if (ConfigurationManager.AppSettings["CorreoCliente"].ToString() == "Si")
                                                    {
                                                        Correo = wfu.EnvioMail("", objMen.pAsunto, Mensaje, CorreoCliente, Remitente, ConCopia);
                                                    }
                                                    else
                                                    {
                                                        Correo = wfu.EnvioMail("", objMen.pAsunto, Mensaje, ConCopia, Remitente, "");
                                                    }

                                                }
                                            }

                                            if (IdBanco == 27)
                                            {
                                                DtXml = wfu.AgregarTabla("!Hola " + getNombrePropio(ConsultClient.Tables["ClienteSICO"].Rows[0]["PRIMERNOMBRE"].ToString()) + " " +
                                                Recursos.UsuarioActualizadoBBVA, "1");
                                                ConsultClient.Tables.Add(DtXml);
                                                return ConsultClient.GetXml();
                                            }
                                            else
                                            {
                                                DtXml = wfu.AgregarTabla("!Hola " + getNombrePropio(ConsultClient.Tables["ClienteSICO"].Rows[0]["PRIMERNOMBRE"].ToString()) + " " +
                                                Recursos.UsuarioActualizado, "1");
                                                ConsultClient.Tables.Add(DtXml);
                                                return ConsultClient.GetXml();
                                            }
                                        }
                                        else
                                        {
                                            DtXml = wfu.AgregarTabla(Recursos.ErrorProceso, "0");
                                            ConsultClient.Tables.Add(DtXml);
                                            return ConsultClient.GetXml();
                                        }
                                    }
                                    else
                                    {
                                        DtXml = wfu.AgregarTabla(Recursos.ErrorProceso, "0");
                                        ConsultClient.Tables.Add(DtXml);
                                        return ConsultClient.GetXml();
                                    }
                                }
                                else
                                {
                                    DtXml = wfu.AgregarTabla(Recursos.ErrorProceso, "0");
                                    ConsultClient.Tables.Add(DtXml);
                                    return ConsultClient.GetXml();
                                }
                            }
                        }
                        else
                        {
                            DtXml = wfu.AgregarTabla(Recursos.UsuarioNoExiste, "0");
                            ConsultClient.Tables.Add(DtXml);
                            return ConsultClient.GetXml();
                        }
                    }
                    else
                    {
                        DtXml = wfu.AgregarTabla(Recursos.UsuarioNoExiste, "0");
                        ConsultClient.Tables.Add(DtXml);
                        return ConsultClient.GetXml();
                    }
                }
                else
                {
                    DtXml = wfu.AgregarTabla(Recursos.AutenticacionErrada, "0");
                    ConsultClient.Tables.Add(DtXml);
                    return ConsultClient.GetXml();
                }
            }
            catch (Exception)
            {
                DtXml = wfu.AgregarTabla(Recursos.ErrorProceso, "0");
                ConsultClient.Tables.Add(DtXml);
                return ConsultClient.GetXml();
            }
        }

        /// <summary>
        /// Consulta el estado y tipo de cliente
        /// </summary>
        /// <param name="Contrato"></param>
        /// <returns></returns>
        public string ConsultaClientePrenota(int Contrato)
        {
            DataTable DtXml = new DataTable();
            DataSet objCliente = new DataSet();
            ClienteSico objS = new ClienteSico();

            try
            {
                objS.pContrato = Contrato;
                objS.pNroDocumento = 0;
                objCliente = new ClienteSICOLN().consultarClienteSICO(objS, "pa_DEB_Consulta_ClientesPrenotas");

                if (objCliente.Tables[0].Rows.Count > 0)
                {
                    return objCliente.GetXml();
                }
                else
                {
                    DtXml = wfu.AgregarTabla(Recursos.UsuarioNoExiste, "0");
                    objCliente.Tables.Add(DtXml);
                    return objCliente.GetXml();
                }
            }
            catch (Exception)
            {
                DtXml = wfu.AgregarTabla(Recursos.ErrorProceso, "0");
                objCliente.Tables.Add(DtXml);
                return objCliente.GetXml();
            }

        }

        #region CONTRATO DIGITAL
        /// <summary>
        /// Consulta la información del cliente en contrato digital
        /// </summary>
        /// <param name="Contrato"></param>
        /// <param name="Usuario"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public string ConsultaClienteContratoDigital(int Contrato, string Usuario, string Password)
        {
            DataSet ConsultaClienteCD = new DataSet();
            DataTable DtXml = new DataTable();
            
            ClienteSico objS = new ClienteSico();

            try
            {
                objUsuario.pUsuario = Usuario;
                objUsuario.pPassword = objEncriptador.encriptar(Password);

                List<ServicioDebito.EN.Tablas.Usuario> listaU = new UsuarioLN().consultar(objUsuario);
                if (listaU.Count > 0)
                {
                    ConsultaClienteCD = ExisteClienteContratoDigital(Contrato);
                    return ConsultaClienteCD.GetXml();
                }
                else
                {
                    DtXml = wfu.AgregarTabla(Recursos.AutenticacionErrada, "0");
                    ConsultaClienteCD.Tables.Add(DtXml);
                    return ConsultaClienteCD.GetXml();
                }
            }
            catch (Exception)
            {
                DtXml = wfu.AgregarTabla(Recursos.ErrorProceso, "0");
                ConsultaClienteCD.Tables.Add(DtXml);
                return ConsultaClienteCD.GetXml();
            }
        }
        /// <summary>
        /// Guarda la informacion del cliente
        /// </summary>
        /// <param name="Contrato"></param>
        /// <param name="IdBanco"></param>
        /// <param name="TipoCuenta"></param>
        /// <param name="NumeroCuenta"></param>
        /// <param name="CanalIngreso"></param>
        /// <param name="DireccionIp"></param>
        /// <param name="Usuario"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public string GuardarClienteContratoDigital(int Contrato, int IdBanco, int TipoCuenta, string NumeroCuenta, int CanalIngreso, string DireccionIp, int FechaDebito, string Usuario, string Password)
        {
            DataSet ConsultClient = new DataSet();
            DataSet ConsultClientDigital= new DataSet();
            DataSet ConsultTitular = new DataSet();
            DataSet ConsultDebito = new DataSet();
            DataSet ConsultError = new DataSet();
            DataTable DtXml = new DataTable();
            String PasswordCall = String.Empty;
         
            try
            {
                objUsuario.pUsuario = Usuario;
                objUsuario.pPassword = objEncriptador.encriptar(Password);

                List<ServicioDebito.EN.Tablas.Usuario> listaU = new UsuarioLN().consultar(objUsuario);

                if (listaU.Count > 0)
                {
                    objUsuario = listaU[0];
                    PasswordCall = objEncriptador.desencriptar(objUsuario.pPassword);
                    TitularCuenta objT = new TitularCuenta();

                    ConsultClientDigital = ExisteClienteContratoDigital(Contrato);

                    if (ConsultClientDigital.Tables[0].Rows.Count > 0)
                    {                     

                            objT.pNombre = ConsultClientDigital.Tables["ClienteContratoDigital"].Rows[0]["NOMBRE_CLIENTE"].ToString().TrimStart('0');
                            objT.pNumeroIdentificacion = ConsultClientDigital.Tables["ClienteContratoDigital"].Rows[0]["NUMERO_DOCUMENTO_CLIENTE"].ToString().TrimStart('0');
                            objT.pTipoIdentificacion = Convert.ToInt32(ConsultClientDigital.Tables["ClienteContratoDigital"].Rows[0]["TIPO_DOCUMENTO"].ToString());


                        //Consulta información en Titular

                        TitularCuentaLN tcl = new TitularCuentaLN();
                        ConsultTitular = tcl.consultarTerceros(objT);
                        int valorT = 0;
                        if (ConsultTitular.Tables["Titular"].Rows.Count == 0)
                        { valorT = new TitularCuentaLN().insertar(objT); }
                        else { valorT = ConsultTitular.Tables["Titular"].Rows.Count; }

                            

                        if (valorT <= 0)
                        {
                            DtXml = wfu.AgregarTabla(Recursos.ErrorProceso, "0");
                            ConsultClient.Tables.Add(DtXml);
                            return ConsultClient.GetXml();
                        }
                        else
                        {
                            DatosDebito objD = new DatosDebito();
                            DatosDebito objD2 = new DatosDebito();
                            objD.pContrato = Convert.ToString(Contrato);
                            objD2.pContrato= Convert.ToString(Contrato);
                            objD.pDigito = Convert.ToInt32(UtilidadesWeb.calculoDigito(objD.pContrato));
                            objD.pIdBanco = IdBanco;
                            objD.pEstado = 16; //CLIENTE INICIA EL ESTADO CONTRATO DIGITAL (ESTADO 16)
                            objD.pIntentos = 0;
                            objD.pTipoCuenta = TipoCuenta;
                            objD.pNumeroCuenta = NumeroCuenta;
                            objD.pIdFormatoDebito = CanalIngreso;//CANAL DE INGRESO 24 PARA CONTRATO DIGITAL
                            objD.pIdFormatoCancelacion = 0;
                            objD.pDireccionIp = DireccionIp;
                            objD.pFechaDebito = FechaDebito;

                            objD.pSuspendido = false;
                            objD.pFechaInicioSus = String.Empty;
                            objD.pFechaFinSus = String.Empty;

                            int valorD = 0;
                            ConsultDebito = new DatosDebitoLN().consultarDatos(objD2);

                            if (ConsultDebito.Tables["ClienteDebito"].Rows.Count > 0)
                            {
                                DtXml = wfu.AgregarTabla(Recursos.ContratoIngresado, "0");
                                ConsultClient.Tables.Add(DtXml);
                                return ConsultClient.GetXml();
                            }
                            objD.pIdTitularCuenta = valorT;
                            valorD = new DatosDebitoLN().insertar(objD);
                            

                            if (valorD > 0)
                            {
                                DataSet dsBanco = new DataSet();
                                DataSet dsTipoC = new DataSet();
                                DataSet dsTipoF = new DataSet();
                                DataSet dsFecha = new DataSet();
                                dsBanco = HomologarBanco(IdBanco, true);
                                dsTipoC = HomologarTipoCuenta(TipoCuenta);
                                dsTipoF = HomologarCanalIngreso(CanalIngreso);
                                dsFecha = HomologarFechaDebito(FechaDebito, true);

                                int logueo = 0;
                                #region (INFORMACION PARA LOG)
                                campos = string.Concat(objD.pContrato,
                                    " con BANCO:", Convert.ToString(dsBanco.Tables[0].Rows[0]["NOMBRE"].ToString()).ToUpper(),
                                    ", TIPO DE CUENTA:", Convert.ToString(dsTipoC.Tables[0].Rows[0]["VALOR"].ToString()),
                                    ", NÚMERO DE CUENTA:", Convert.ToString(objD.pNumeroCuenta),
                                    ", FORMATO DEBITO:", Convert.ToString(dsTipoF.Tables[0].Rows[0]["VALOR"].ToString()),
                                    ", CUENTA TERCERO:", Convert.ToString(objD.pTercero).ToUpper(),
                                    ", NOMBRE:", Convert.ToString(objT.pNombre),
                                    ", TIPO DE IDENTIFICACIÓN:", UtilidadesWeb.homologarDocumentoAbrebiatura(Convert.ToInt32(objT.pTipoIdentificacion)),
                                    ", IDENTIFICACIÓN:", Convert.ToString(objT.pNumeroIdentificacion),
                                    ", DÉBITO A PARTIR DEL:", Convert.ToString(dsFecha.Tables[0].Rows[0]["VALOR"].ToString()),
                                    ", DIRECCION_IP:", Convert.ToString(objD.pDireccionIp));
                                #endregion
                                logueo = Log(1, objD.pContrato, Usuario, "Se creó el contrato N°: ");

                                if (logueo > 0)
                                {
                                    String Correo = String.Empty;
                                    String Mensaje = String.Empty;
                                    int LongitudNroCuenta = 0;

                                    if (IdBanco == 27) //Banco BBVA
                                    {
                                        DtXml = wfu.AgregarTabla("!Hola " + getNombrePropio(ConsultClientDigital.Tables["ClienteContratoDigital"].Rows[0]["PRIMERNOMBRE"].ToString()) + " " +
                                        ConsultClientDigital.Tables["ClienteContratoDigital"].Rows[0]["PRONOMBRE"].ToString() + " " + Recursos.InscripcionClienteNuevoBBVA, "1");
                                        ConsultClient.Tables.Add(DtXml);
                                    }
                                    else
                                    {
                                        DtXml = wfu.AgregarTabla("!Hola " + getNombrePropio(ConsultClientDigital.Tables["ClienteContratoDigital"].Rows[0]["PRIMERNOMBRE"].ToString()) + " " +
                                        ConsultClientDigital.Tables["ClienteContratoDigital"].Rows[0]["PRONOMBRE"].ToString() + " " + Recursos.InscripcionClienteNuevo, "1");
                                        ConsultClient.Tables.Add(DtXml);
                                    }

                                    Mensajes objM = new Mensajes();
                                    objM.pTipoContrato = ConsultClientDigital.Tables["ClienteContratoDigital"].Rows[0]["TIPO_CUPO"].ToString().Substring(0, 1);
                                    objM.pEstadoDebito = 1;
                                    objM.pMotivo = 1;
                                    List<ServicioDebito.EN.Tablas.Mensajes> listaM = new MensajesLN().consultar(objM);

                                    if (ConsultClientDigital.Tables["ClienteContratoDigital"].Rows[0]["EMAIL_CLIENTE"].ToString() != String.Empty)
                                    {
                                        if (listaM.Count > 0)
                                        {
                                            if (ConsultClientDigital.Tables["ClienteContratoDigital"].Rows[0]["LeydPcorreo"].ToString() == "SI")
                                            {
                                                DataSet DsCorreoBanco = new DataSet();
                                                DsCorreoBanco = ConsultarCorreosBancos();
                                                String Remitente = String.Empty;
                                                String ConCopia = String.Empty;
                                                String CorreoCliente = String.Empty;

                                                if (DsCorreoBanco.Tables.Contains("tabla"))
                                                {
                                                    Remitente = DsCorreoBanco.Tables["tabla"].Rows[0]["REMITENTE"].ToString();
                                                    ConCopia = DsCorreoBanco.Tables["tabla"].Rows[0]["CORREOS_CONTROL"].ToString();
                                                }

                                                objM = listaM[0];
                                                LongitudNroCuenta = objD.pNumeroCuenta.Length;
                                                Mensaje = objM.pMensaje
                                                .Replace("@FechaActual_", Convert.ToString(DateTime.Now))
                                                .Replace("@Nombrecliente_", objT.pNombre)
                                                .Replace("@NroContrato_", objD.pContrato)
                                                .Replace("@Ciudad_", ConsultClientDigital.Tables["ClienteContratoDigital"].Rows[0]["CIUDAD"].ToString())
                                                .Replace("@Banco_", dsBanco.Tables[0].Rows[0]["NOMBRE"].ToString().ToUpper())
                                                .Replace("@TipoCuenta_", dsTipoC.Tables[0].Rows[0]["VALOR"].ToString())
                                                .Replace("@NroCuenta_", "".PadLeft(5, '*') + objD.pNumeroCuenta.Substring((LongitudNroCuenta - 4), 4))
                                                .Replace("@Debito_", dsFecha.Tables[0].Rows[0]["VALOR"].ToString());

                                                CorreoCliente = ConsultClientDigital.Tables["ClienteContratoDigital"].Rows[0]["EMAIL_CLIENTE"].ToString();

                                                if (ConfigurationManager.AppSettings["CorreoCliente"].ToString() == "Si")
                                                {
                                                    Correo = wfu.EnvioMail("", objM.pAsunto, Mensaje, CorreoCliente, Remitente, ConCopia);
                                                }
                                                else
                                                {
                                                    Correo = wfu.EnvioMail("", objM.pAsunto, Mensaje, ConCopia, Remitente, "");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            DtXml = wfu.AgregarTabla(Recursos.ErrorProceso, "0");
                                            ConsultClient.Tables.Add(DtXml);
                                            return ConsultClient.GetXml();
                                        }
                                    }

                                    return ConsultClient.GetXml();
                                }
                                else
                                {
                                    DtXml = wfu.AgregarTabla(Recursos.ErrorProceso, "0");
                                    ConsultClient.Tables.Add(DtXml);
                                    return ConsultClient.GetXml();
                                }
                            }
                            else
                            {
                                DtXml = wfu.AgregarTabla(Recursos.ErrorProceso, "0");
                                ConsultClient.Tables.Add(DtXml);
                                return ConsultClient.GetXml();
                            }
                        }

                    }
                    else
                    {
                        DtXml = wfu.AgregarTabla(Recursos.UsuarioNoExiste, "0");
                        ConsultClient.Tables.Add(DtXml);
                        return ConsultClient.GetXml();
                    }
                }
                else
                {
                    DtXml = wfu.AgregarTabla(Recursos.AutenticacionErrada, "0");
                    ConsultClient.Tables.Add(DtXml);
                    return ConsultClient.GetXml();
                }
            }
            catch (Exception)
            {  
                DtXml = wfu.AgregarTabla(Recursos.ErrorProceso, "0");
                ConsultError.Tables.Add(DtXml);
                return ConsultClient.GetXml();
            }
        }

        /// <summary>
        /// Modificacion de datos del cliente que realiza desde el APP o la Pagina web
        /// </summary>
        /// <param name="Contrato"></param>
        /// <param name="NumeroCuenta"></param>
        /// <param name="TipoCuenta"></param>
        /// <param name="IdBanco"></param>
        /// <param name="DireccionIp"></param>
        /// <param name="Usuario"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public string ModificarDatosContratoDigital(int Contrato, string NumeroCuenta, int TipoCuenta, int IdBanco, string DireccionIp, int FechaDebito, string Usuario, string Password)
        {
            DataSet ConsultClient = new DataSet();
            DataSet ConsultClientContratoDigital = new DataSet();
            DataTable DtXml = new DataTable();
            try
            {
                objUsuario.pUsuario = Usuario;
                objUsuario.pPassword = objEncriptador.encriptar(Password);

                List<ServicioDebito.EN.Tablas.Usuario> listaU = new UsuarioLN().consultar(objUsuario);

                if (listaU.Count > 0)
                {
                    DatosDebito objD2 = new DatosDebito();
                    objD2.pContrato = Convert.ToString(Contrato);
                    ConsultClient = new DatosDebitoLN().consultarDatos(objD2);
                    ConsultClientContratoDigital = ExisteClienteContratoDigital(Contrato);
                    if (ConsultClient.Tables.Contains("ClienteDebito"))
                    {
                        TitularCuenta objTerc = new TitularCuenta();
                        DataSet ConTerceros = new DataSet();
                        DataTable DtTerceros = new DataTable();

                        objTerc.pId = Convert.ToInt32(ConsultClient.Tables["ClienteDebito"].Rows[0]["ID_TITULAR_CUENTA"].ToString());
                        ConTerceros = new TitularCuentaLN().consultarTerceros(objTerc);

                        if (ConTerceros.Tables["Titular"].Rows.Count > 0)
                        {
                            int actuaclient = 0;
                            if (//Si esta en prenota en proceso ó debito en proceso
                                (Convert.ToInt32(ConsultClient.Tables["ClienteDebito"].Rows[0]["ESTADO"].ToString()) == 2 ||
                                Convert.ToInt32(ConsultClient.Tables["ClienteDebito"].Rows[0]["ESTADO"].ToString()) == 5)
                                && //Y si el banco ó el tipo de cuenta ó el número de cuenta son diferentes
                                (Convert.ToInt32(ConsultClient.Tables["ClienteDebito"].Rows[0]["ID_BANCO"].ToString()) != IdBanco ||
                                Convert.ToInt32(ConsultClient.Tables["ClienteDebito"].Rows[0]["TIPO_CUENTA"].ToString()) != TipoCuenta ||
                                ConsultClient.Tables["ClienteDebito"].Rows[0]["NUMERO_CUENTA"].ToString() != NumeroCuenta)
                                )
                            {

                                ActualizaCliente objAc = new ActualizaCliente();
                                objAc.pContrato = ConsultClient.Tables["ClienteDebito"].Rows[0]["CONTRATO"].ToString();
                                objAc.pIdTitularCuenta = Convert.ToInt32(ConsultClient.Tables["ClienteDebito"].Rows[0]["ID_TITULAR_CUENTA"].ToString());
                                objAc.pIdBanco = IdBanco;
                                objAc.pIdTipoCuenta = TipoCuenta;
                                objAc.pNumeroCuenta = NumeroCuenta;
                                objAc.pDireccionIp = DireccionIp;
                                objAc.pUsuario = Usuario;
                                List<ServicioDebito.EN.Tablas.ActualizaCliente> listaB = new ActualizaClienteLN().consultarDatos(objAc);

                                int logvalor = 0;
                                string MensajeLog = String.Empty;
                                if (listaB.Count > 0)
                                {
                                    actuaclient = new ActualizaClienteLN().actualizar(objAc);
                                    logvalor = 2;
                                    MensajeLog = "Se actualizó temporalmente el contrato N°: ";
                                }
                                else
                                {
                                    actuaclient = new ActualizaClienteLN().insertar(objAc);
                                    logvalor = 1;
                                    MensajeLog = "Se creo temporalmente el contrato N°: ";
                                }

                                int actua = 0;
                                DatosDebito ObjDat = new DatosDebito();
                                ObjDat.pId = Convert.ToInt32(ConsultClient.Tables["ClienteDebito"].Rows[0]["ID"].ToString());
                                ObjDat.pContrato = ConsultClient.Tables["ClienteDebito"].Rows[0]["CONTRATO"].ToString();
                                ObjDat.pFechaDebito = FechaDebito;

                                actua = new DatosDebitoLN().actualizarFecha(ObjDat);

                                if (actuaclient > 0 && actua > 0)
                                {
                                    DataSet dsBanco = new DataSet();
                                    DataSet dsTipoC = new DataSet();
                                    DataSet dsFecha = new DataSet();
                                    dsBanco = HomologarBanco(IdBanco, true);
                                    dsTipoC = HomologarTipoCuenta(TipoCuenta);
                                    dsFecha = HomologarFechaDebito(FechaDebito, true);

                                    int logueo = 0;
                                    #region (INFORMACION PARA LOG)
                                    campos = string.Concat(objAc.pContrato,
                                        " con BANCO:", Convert.ToString(dsBanco.Tables[0].Rows[0]["NOMBRE"].ToString()).ToUpper(),
                                        ", TIPO DE CUENTA:", Convert.ToString(dsTipoC.Tables[0].Rows[0]["VALOR"].ToString()),
                                        ", NUMERO DE CUENTA:", Convert.ToString(objAc.pNumeroCuenta),
                                        ", DÉBITO A PARTIR DEL:", Convert.ToString(dsFecha.Tables[0].Rows[0]["VALOR"].ToString()),
                                        ", DIRECCION_IP:", Convert.ToString(objAc.pDireccionIp));
                                    #endregion
                                    logueo = Log(logvalor, objAc.pContrato, Usuario, MensajeLog);

                                    if (logueo > 0)
                                    {
                                        String Correo = String.Empty;
                                        int LongitudNroCuenta = 0;
                                        String Mensaje = String.Empty;
                                        Mensajes objM = new Mensajes();
                                        objM.pTipoContrato = ConsultClientContratoDigital.Tables["ClienteContratoDigital"].Rows[0]["TIPO_CUPO"].ToString().Substring(0, 1);
                                        objM.pEstadoDebito = 1;
                                        objM.pMotivo = 2;
                                        List<ServicioDebito.EN.Tablas.Mensajes> listaM = new MensajesLN().consultar(objM);

                                        if (listaM.Count > 0)
                                        {
                                            if (ConsultClientContratoDigital.Tables["ClienteContratoDigital"].Rows[0]["EMAIL_CLIENTE"].ToString() != String.Empty)
                                            {
                                                if (ConsultClientContratoDigital.Tables["ClienteContratoDigital"].Rows[0]["LeydPcorreo"].ToString() == "SI")
                                                {
                                                    DataSet DsCorreoBanco = new DataSet();
                                                    DsCorreoBanco = ConsultarCorreosBancos();
                                                    String Remitente = String.Empty;
                                                    String ConCopia = String.Empty;
                                                    String CorreoCliente = String.Empty;

                                                    if (DsCorreoBanco.Tables.Contains("tabla"))
                                                    {
                                                        Remitente = DsCorreoBanco.Tables["tabla"].Rows[0]["REMITENTE"].ToString();
                                                        ConCopia = DsCorreoBanco.Tables["tabla"].Rows[0]["CORREOS_CONTROL"].ToString();
                                                    }

                                                    LongitudNroCuenta = objAc.pNumeroCuenta.Length;
                                                    Mensajes objMen = new Mensajes();
                                                    objMen = listaM[0];
                                                    Mensaje = objMen.pMensaje
                                                    .Replace("@FechaActual_", Convert.ToString(DateTime.Now))
                                                    .Replace("@Nombrecliente_", ConsultClientContratoDigital.Tables["ClienteContratoDigital"].Rows[0]["NOMBRE_CLIENTE"].ToString())
                                                    .Replace("@NroContrato_", ConsultClientContratoDigital.Tables["ClienteContratoDigital"].Rows[0]["CONTRATO"].ToString())
                                                    .Replace("@Ciudad_", ConsultClientContratoDigital.Tables["ClienteContratoDigital"].Rows[0]["CIUDAD"].ToString())
                                                    .Replace("@Banco_", dsBanco.Tables[0].Rows[0]["NOMBRE"].ToString().ToUpper())
                                                    .Replace("@TipoCuenta_", dsTipoC.Tables[0].Rows[0]["VALOR"].ToString())
                                                    .Replace("@NroCuenta_", "".PadLeft(5, '*') + objAc.pNumeroCuenta.Substring((LongitudNroCuenta - 4), 4))
                                                    .Replace("@Debito_", dsFecha.Tables[0].Rows[0]["VALOR"].ToString());

                                                    CorreoCliente = ConsultClientContratoDigital.Tables["ClienteContratoDigital"].Rows[0]["EMAIL_CLIENTE"].ToString();

                                                    if (ConfigurationManager.AppSettings["CorreoCliente"].ToString() == "Si")
                                                    {
                                                        Correo = wfu.EnvioMail("", objMen.pAsunto, Mensaje, CorreoCliente, Remitente, ConCopia);
                                                    }
                                                    else
                                                    {
                                                        Correo = wfu.EnvioMail("", objMen.pAsunto, Mensaje, ConCopia, Remitente, "");
                                                    }

                                                }
                                            }

                                            if (IdBanco == 27) //Banco BBVA
                                            {
                                                DtXml = wfu.AgregarTabla("!Hola " + getNombrePropio(ConsultClientContratoDigital.Tables["ClienteContratoDigital"].Rows[0]["PRIMERNOMBRE"].ToString()) + " " +
                                                Recursos.UsuarioActualizadoBBVA, "1");
                                                ConsultClient.Tables.Add(DtXml);
                                                return ConsultClient.GetXml();
                                            }
                                            else
                                            {
                                                DtXml = wfu.AgregarTabla("!Hola " + getNombrePropio(ConsultClientContratoDigital.Tables["ClienteContratoDigital"].Rows[0]["PRIMERNOMBRE"].ToString()) + " " +
                                                Recursos.UsuarioActualizado, "1");
                                                ConsultClient.Tables.Add(DtXml);
                                                return ConsultClient.GetXml();
                                            }
                                        }
                                        else
                                        {
                                            DtXml = wfu.AgregarTabla(Recursos.ErrorProceso, "0");
                                            ConsultClient.Tables.Add(DtXml);
                                            return ConsultClient.GetXml();
                                        }
                                    }
                                    else
                                    {
                                        DtXml = wfu.AgregarTabla(Recursos.ErrorProceso, "0");
                                        ConsultClient.Tables.Add(DtXml);
                                        return ConsultClient.GetXml();
                                    }
                                }
                                else
                                {
                                    DtXml = wfu.AgregarTabla(Recursos.ErrorProceso, "0");
                                    ConsultClient.Tables.Add(DtXml);
                                    return ConsultClient.GetXml();
                                }
                            }
                            else
                            {
                                DatosDebito objD = new DatosDebito();
                                TitularCuenta objT = new TitularCuenta();
                                int valorT = 0;
                                objD.pId = Convert.ToInt32(ConsultClient.Tables["ClienteDebito"].Rows[0]["ID"].ToString());
                                objD.pContrato = ConsultClient.Tables["ClienteDebito"].Rows[0]["CONTRATO"].ToString();
                                objD.pDigito = Convert.ToInt32(ConsultClient.Tables["ClienteDebito"].Rows[0]["DIGITO"].ToString());
                                objD.pIdBanco = IdBanco;
                                objD.pTipoCuenta = TipoCuenta;
                                objD.pNumeroCuenta = NumeroCuenta;
                                objD.pIdFormatoDebito = Convert.ToInt32(ConsultClient.Tables["ClienteDebito"].Rows[0]["ID_FORMATO_DEBITO"].ToString());
                                objD.pIdFormatoCancelacion = Convert.ToInt32(ConsultClient.Tables["ClienteDebito"].Rows[0]["ID_FORMATO_CANCELACION"].ToString());

                                if (Convert.ToBoolean(ConsultClient.Tables["ClienteDebito"].Rows[0]["TERCERO"].ToString()) == true)
                                {
                                    objD.pTercero = false;
                                    objT.pId = Convert.ToInt32(ConsultClient.Tables["ClienteDebito"].Rows[0]["ID_TITULAR_CUENTA"].ToString());
                                    objT.pNombre = ConsultClient.Tables["ClienteSICO"].Rows[0]["NOMBRE_CLIENTE"].ToString().TrimStart('0');
                                    objT.pNumeroIdentificacion = ConsultClientContratoDigital.Tables["ClienteContratoDigital"].Rows[0]["NUMERO_DOCUMENTO_CLIENTE"].ToString().TrimStart('0');
                                    objT.pTipoIdentificacion = UtilidadesWeb.homologarDocumento(ConsultClientContratoDigital.Tables["ClienteContratoDigital"].Rows[0]["TIPO_DOCUMENTO"].ToString());
                                    valorT = new TitularCuentaLN().actualizar(objT);
                                }
                                else
                                {
                                    objD.pTercero = Convert.ToBoolean(ConsultClient.Tables["ClienteDebito"].Rows[0]["TERCERO"].ToString());
                                    valorT++;
                                }

                                objD.pIdTitularCuenta = Convert.ToInt32(ConsultClient.Tables["ClienteDebito"].Rows[0]["ID_TITULAR_CUENTA"].ToString());
                                objD.pDireccionIp = DireccionIp;
                                objD.pSuspendido = false;
                                objD.pFechaInicioSus = String.Empty;
                                objD.pFechaFinSus = String.Empty;
                                objD.pFechaDebito = FechaDebito;

                                if (Convert.ToInt32(ConsultClient.Tables["ClienteDebito"].Rows[0]["ID_BANCO"].ToString()) != IdBanco ||
                                    Convert.ToInt32(ConsultClient.Tables["ClienteDebito"].Rows[0]["TIPO_CUENTA"].ToString()) != TipoCuenta ||
                                    ConsultClient.Tables["ClienteDebito"].Rows[0]["NUMERO_CUENTA"].ToString() != NumeroCuenta)
                                {
                                    objD.pEstado = 1;
                                }
                                else
                                {
                                    objD.pEstado = Convert.ToInt32(ConsultClient.Tables["ClienteDebito"].Rows[0]["ESTADO"].ToString());
                                }

                                int valorD = 0;

                                valorD = new DatosDebitoLN().actualizar(objD);

                                if (valorD > 0 && valorT > 0)
                                {
                                    DataSet dsBanco = new DataSet();
                                    DataSet dsTipoC = new DataSet();
                                    DataSet dsFecha = new DataSet();
                                    dsBanco = HomologarBanco(IdBanco, true);
                                    dsTipoC = HomologarTipoCuenta(TipoCuenta);
                                    dsFecha = HomologarFechaDebito(FechaDebito, true);



                                    int logueo = 0;
                                    #region (INFORMACION PARA LOG)
                                    campos = string.Concat(objD.pContrato,
                                        " con BANCO:", Convert.ToString(dsBanco.Tables[0].Rows[0]["NOMBRE"].ToString()).ToUpper(),
                                        ", TIPO DE CUENTA:", Convert.ToString(dsTipoC.Tables[0].Rows[0]["VALOR"].ToString()),
                                        ", NÚMERO DE CUENTA:", Convert.ToString(objD.pNumeroCuenta),
                                        ", DÉBITO A PARTIR DEL: ", Convert.ToString(dsFecha.Tables[0].Rows[0]["VALOR"].ToString()),
                                        ", DIRECCION_IP:", Convert.ToString(objD.pDireccionIp));
                                    #endregion
                                    logueo = Log(2, objD.pContrato, Usuario, "Se actualizó el contrato N°: ");

                                    if (logueo > 0)
                                    {
                                        String Correo = String.Empty;
                                        int LongitudNroCuenta = 0;
                                        String Mensaje = String.Empty;
                                        Mensajes objM = new Mensajes();
                                        objM.pTipoContrato = ConsultClientContratoDigital.Tables["ClienteContratoDigital"].Rows[0]["TIPO_CUPO"].ToString().Substring(0, 1);
                                        objM.pEstadoDebito = 1;
                                        objM.pMotivo = 2;
                                        List<ServicioDebito.EN.Tablas.Mensajes> listaM = new MensajesLN().consultar(objM);


                                        if (listaM.Count > 0)
                                        {
                                            if (ConsultClientContratoDigital.Tables["ClienteContratoDigital"].Rows[0]["EMAIL_CLIENTE"].ToString() != String.Empty)
                                            {
                                                if (ConsultClientContratoDigital.Tables["ClienteContratoDigital"].Rows[0]["LeydPcorreo"].ToString() == "SI")
                                                {
                                                    DataSet DsCorreoBanco = new DataSet();
                                                    DsCorreoBanco = ConsultarCorreosBancos();
                                                    String Remitente = String.Empty;
                                                    String ConCopia = String.Empty;
                                                    String CorreoCliente = String.Empty;

                                                    if (DsCorreoBanco.Tables.Contains("tabla"))
                                                    {
                                                        Remitente = DsCorreoBanco.Tables["tabla"].Rows[0]["REMITENTE"].ToString();
                                                        ConCopia = DsCorreoBanco.Tables["tabla"].Rows[0]["CORREOS_CONTROL"].ToString();
                                                    }

                                                    LongitudNroCuenta = objD.pNumeroCuenta.Length;
                                                    Mensajes objMen = new Mensajes();
                                                    objMen = listaM[0];
                                                    Mensaje = objMen.pMensaje
                                                    .Replace("@FechaActual_", Convert.ToString(DateTime.Now))
                                                    .Replace("@Nombrecliente_", ConsultClientContratoDigital.Tables["ClienteContratoDigital"].Rows[0]["NOMBRE_CLIENTE"].ToString())
                                                    .Replace("@NroContrato_", ConsultClient.Tables["ClienteDebito"].Rows[0]["CONTRATO"].ToString())
                                                    .Replace("@Ciudad_", ConsultClientContratoDigital.Tables["ClienteContratoDigital"].Rows[0]["CIUDAD"].ToString())
                                                    .Replace("@Banco_", dsBanco.Tables[0].Rows[0]["NOMBRE"].ToString().ToUpper())
                                                    .Replace("@TipoCuenta_", dsTipoC.Tables[0].Rows[0]["VALOR"].ToString())
                                                    .Replace("@NroCuenta_", "".PadLeft(5, '*') + objD.pNumeroCuenta.Substring((LongitudNroCuenta - 4), 4))
                                                    .Replace("@Debito_", dsFecha.Tables[0].Rows[0]["VALOR"].ToString());

                                                    CorreoCliente = ConsultClientContratoDigital.Tables["ClienteContratoDigital"].Rows[0]["EMAIL_CLIENTE"].ToString();

                                                    if (ConfigurationManager.AppSettings["CorreoCliente"].ToString() == "Si")
                                                    {
                                                        Correo = wfu.EnvioMail("", objMen.pAsunto, Mensaje, CorreoCliente, Remitente, ConCopia);
                                                    }
                                                    else
                                                    {
                                                        Correo = wfu.EnvioMail("", objMen.pAsunto, Mensaje, ConCopia, Remitente, "");
                                                    }

                                                }
                                            }

                                            if (IdBanco == 27)
                                            {
                                                DtXml = wfu.AgregarTabla("!Hola " + getNombrePropio(ConsultClientContratoDigital.Tables["ClienteContratoDigital"].Rows[0]["PRIMERNOMBRE"].ToString()) + " " +
                                                Recursos.UsuarioActualizadoBBVA, "1");
                                                ConsultClient.Tables.Add(DtXml);
                                                return ConsultClient.GetXml();
                                            }
                                            else
                                            {
                                                DtXml = wfu.AgregarTabla("!Hola " + getNombrePropio(ConsultClientContratoDigital.Tables["ClienteContratoDigital"].Rows[0]["PRIMERNOMBRE"].ToString()) + " " +
                                                Recursos.UsuarioActualizado, "1");
                                                ConsultClient.Tables.Add(DtXml);
                                                return ConsultClient.GetXml();
                                            }
                                        }
                                        else
                                        {
                                            DtXml = wfu.AgregarTabla(Recursos.ErrorProceso, "0");
                                            ConsultClient.Tables.Add(DtXml);
                                            return ConsultClient.GetXml();
                                        }
                                    }
                                    else
                                    {
                                        DtXml = wfu.AgregarTabla(Recursos.ErrorProceso, "0");
                                        ConsultClient.Tables.Add(DtXml);
                                        return ConsultClient.GetXml();
                                    }
                                }
                                else
                                {
                                    DtXml = wfu.AgregarTabla(Recursos.ErrorProceso, "0");
                                    ConsultClient.Tables.Add(DtXml);
                                    return ConsultClient.GetXml();
                                }
                            }
                        }
                        else
                        {
                            DtXml = wfu.AgregarTabla(Recursos.UsuarioNoExiste, "0");
                            ConsultClient.Tables.Add(DtXml);
                            return ConsultClient.GetXml();
                        }
                    }
                    else
                    {
                        DtXml = wfu.AgregarTabla(Recursos.UsuarioNoExiste, "0");
                        ConsultClient.Tables.Add(DtXml);
                        return ConsultClient.GetXml();
                    }
                }
                else
                {
                    DtXml = wfu.AgregarTabla(Recursos.AutenticacionErrada, "0");
                    ConsultClient.Tables.Add(DtXml);
                    return ConsultClient.GetXml();
                }
            }
            catch (Exception)
            {
                DtXml = wfu.AgregarTabla(Recursos.ErrorProceso, "0");
                ConsultClient.Tables.Add(DtXml);
                return ConsultClient.GetXml();
            }
        }
        #endregion
    }
}
