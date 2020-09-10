using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using DebitoAutomatico.AD.Administracion;
using DebitoAutomatico.EN;

namespace DebitoAutomatico.LN.Utilidades
{
    public class LectorXML
    {
        public String RutaXML { get; set; }
        public String Error { get; set; }

        /// <summary>
        /// Lee un campo del archivo xml de configuraciones
        /// </summary>
        /// <param name="objCamposXML">Valores del campo que se necesita consultar, tabla y campo</param>
        /// <returns>Valor del campo solicitado</returns>
        public String leerDatosXML(CamposXML objCamposXML)
        {
            ConsultorXML objConsultor = new ConsultorXML();
            objConsultor.RutaXML = RutaXML;
            Encriptador objEncriptador = new Encriptador();
            String strValor = objEncriptador.desencriptar(objConsultor.leerDatosXML(objCamposXML));
            Error = objConsultor.Error;
            return strValor;
        }

        /// <summary>
        /// Modifica una atributo del xml de configuraciones
        /// </summary>
        /// <param name="objCamposXML">Valores del campo que se necesita modificar, tabla, campo y valor</param>
        public void modificarXML(CamposXML objCamposXML)
        {
            ConsultorXML objConsultor = new ConsultorXML();
            objConsultor.RutaXML = RutaXML;
            Encriptador objEncriptador = new Encriptador();
            objCamposXML.pValor = objEncriptador.encriptar(objCamposXML.pValor);
            objConsultor.modificarXML(objCamposXML);
            Error = objConsultor.Error;
        }

        /// <summary>
        /// Trae el valor de la cadena de conexion a la base de datos.
        /// </summary>
        /// <returns>Valor de la cadena de conexion</returns>
        public String leerCadenaConexion()
        {
            ConsultorXML objConsultor = new ConsultorXML();
            objConsultor.RutaXML = RutaXML;
            CamposXML objCamposXML = new CamposXML();
            objCamposXML.pCampo = "A";
            objCamposXML.pTabla = "BD";
            Encriptador objEncriptador = new Encriptador();
            String strValor = objEncriptador.desencriptar(objConsultor.leerDatosXML(objCamposXML));
            Error = objConsultor.Error;
            return strValor;
        }
    }
}
