#region REFERENCIAS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Referencias Necesarias Para El Correcto Funcionamiento
using MCEI.SysControlAdmin.EN.ServerJei___EN;
using MCEI.SysControlAdmin.DAL.ServerJei___DAL;

#endregion

namespace MCEI.SysControlAdmin.BL.ServerJei___BL
{
    public class ServerJeiBL
    {
        #region METODO PARA CREAR
        // Metodo Para Guardar Un Nuevo Registro
        public async Task<int> CreateAsync(ServerJei serverJei)
        {
            return await ServerJeiDAL.CreateAsync(serverJei);
        }
        #endregion

        #region METODO PARA MODIFICAR
        // Metodo Para Guardar Un Nuevo Registro
        public async Task<int> UpdateAsync(ServerJei serverJei)
        {
            return await ServerJeiDAL.UpdateAsync(serverJei);
        }
        #endregion

        #region METODO PARA ELIMINAR
        // Metodo Para Eliminar Un Registro Existente En La Base De Datos
        public async Task<int> DeleteAsync(ServerJei serverJei)
        {
            return await ServerJeiDAL.DeleteAsync(serverJei);
        }
        #endregion

        #region METODO PARA MOSTRAR
        // Metodo Para Mostrar Una Lista De Registros
        public async Task<List<ServerJei>> GetAllAsync()
        {
            return await ServerJeiDAL.GetAllAsync();
        }
        #endregion

        #region METODO PARA MOSTRAR POR ID
        // Metodo Para Mostrar Un Registro Especifico Bajo Un Id
        public async Task<ServerJei> GetByIdAsync(ServerJei serverJei)
        {
            return await ServerJeiDAL.GetByIdAsync(serverJei);
        }
        #endregion

        #region METODO PARA BUSCAR
        // Metodo Para Buscar Registros Existentes
        public async Task<List<ServerJei>> SearchAsync(ServerJei serverJei)
        {
            return await ServerJeiDAL.SearchAsync(serverJei);
        }
        #endregion

        #region METODO PARA INCLUIR PRIVILEGIO Y PRIVILEGIO
        public async Task<List<ServerJei>> SearchIncludeAsync(ServerJei server)
        {
            return await ServerJeiDAL.SearchIncludeAsync(server);
        }
        #endregion
    }

}
