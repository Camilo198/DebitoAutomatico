using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfServicioDebito
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "IService1" en el código y en el archivo de configuración a la vez.
    [ServiceContract]
    public interface IServicioDebito
    {
        [OperationContract]
        string IdBanco(string Usuario, string Password);

        [OperationContract]
        string TipoCuenta(string Usuario, string Password);

        [OperationContract]
        string FechaDebito(string Usuario, string Password);

        [OperationContract]
        string CanalIngreso(string Usuario, string Password);

        [OperationContract]
        string ConsultaCliente(int Contrato, int IdTitularCuenta, string Usuario, string Password);

        [OperationContract]
        string GuardarCliente(int Contrato, int IdBanco, int TipoCuenta, string NumeroCuenta, int CanalIngreso, bool Tercero, string NombreTercero,
                              int IdentificacionTercero, int TipoIdTercero, string DireccionIp, int FechaDebito, string Usuario, string Password);

        [OperationContract]
        string ModificarDatos(int Contrato, string NumeroCuenta, int TipoCuenta, int IdBanco, string DireccionIp, int FechaDebito, string Usuario, string Password);

        [OperationContract]
        string ConsultaClientePrenota(int Contrato);

        [OperationContract]
        string ConsultaClienteContratoDigital(int Contrato, string Usuario, string Password);
        [OperationContract]
        string ModificarDatosContratoDigital(int Contrato, string NumeroCuenta, int TipoCuenta, int IdBanco, string DireccionIp, int FechaDebito, string Usuario, string Password);
        [OperationContract]
        string GuardarClienteContratoDigital(int Contrato, int IdBanco, int TipoCuenta, string NumeroCuenta, int CanalIngreso, string DireccionIp, int FechaDebito, string Usuario, string Password);
    }
}
