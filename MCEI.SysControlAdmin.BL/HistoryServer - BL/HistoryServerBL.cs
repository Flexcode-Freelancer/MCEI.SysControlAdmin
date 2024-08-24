#region REFERENCIAS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Referencias Necesarias Para El Correcto Funcionamiento
using MCEI.SysControlAdmin.EN.HistoryServer___EN;
using MCEI.SysControlAdmin.DAL.HistoryServer___DAL;

#endregion

namespace MCEI.SysControlAdmin.BL.HistoryServer___BL
{
    public class HistoryServerBL
    {
        #region METODO PARA CREAR
        // Metodo Para Guardar Un Nuevo Registro
        public async Task<int> CreateAsync(HistoryServer historyServer)
        {
            return await HistoryServerDAL.CreateAsync(historyServer);
        }
        #endregion

        #region METODO PARA MOSTRAR
        // Metodo Para Mostrar Una Lista De Registros
        public async Task<List<HistoryServer>> GetAllAsync()
        {
            return await HistoryServerDAL.GetAllAsync();
        }
        #endregion

        #region METODO PARA MOSTRAR POR ID
        // Metodo Para Mostrar Un Registro Especifico Bajo Un Id
        public async Task<HistoryServer> GetByIdAsync(HistoryServer historyServer)
        {
            return await HistoryServerDAL.GetByIdAsync(historyServer);
        }
        #endregion

        #region METODO PARA BUSCAR
        // Metodo Para Buscar Registros Existentes
        public async Task<List<HistoryServer>> SearchAsync(HistoryServer historyServer)
        {
            return await HistoryServerDAL.SearchAsync(historyServer);
        }
        #endregion

        #region METODO PARA INCLUIR PRIVILEGIO Y PRIVILEGIO
        public async Task<List<HistoryServer>> SearchIncludeAsync(HistoryServer historyServer)
        {
            return await HistoryServerDAL.SearchIncludeAsync(historyServer);
        }
        #endregion
    }
}
