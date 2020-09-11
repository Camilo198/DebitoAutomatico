using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Data.SqlClient;
using System.Data.Odbc;
using System.Data;
using System.ServiceModel;

namespace DebitoAutomatico.AD.Servicios
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "IWcfData" en el código y en el archivo de configuración a la vez.
    [ServiceContract]
    public interface IWcfData
    {
        [OperationContract]
        SqlConnection ConectaSql(string key);

        [OperationContract]
        OdbcConnection ConectaOdbc(string key);

        [OperationContract]
        void CreaParametro(int tamaño);

        [OperationContract]
        void AdicionaParametro(String param, object valor, int posicion);

        [OperationContract]
        string Actualizar(string procedimiento, SqlParameter[] parametros, SqlConnection ActConexion);

        [OperationContract]
        List<string[,]> LlenarLista(string[, ,] Parametro, string sentenciaSql, string key, string TipoCons, string TipConeccion);

        [OperationContract]
        string sqlInsert(string keyOdbc, string sentenciaOdbc, string keySql, string TablaDestino);

        [OperationContract]
        string Ejecutar(string[, ,] Valor, string procedimiento, string strConn);

        [OperationContract]
        string ConsultaSqlDato(string procedimiento, string key);

        [OperationContract]
        DataSet ConsultaOdbc(string NomTabla, string sentenciaSql, string key);
    }
}
