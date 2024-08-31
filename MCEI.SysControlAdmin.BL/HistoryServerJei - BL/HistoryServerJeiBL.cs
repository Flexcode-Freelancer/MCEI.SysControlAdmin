#region REFERENCIAS
using MCEI.SysControlAdmin.DAL.HistoryServerJei___DAL;
using MCEI.SysControlAdmin.EN.HistoryServerJei___EN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Referencias Necesarias Para El Correcto Funcionamiento



#endregion

namespace MCEI.SysControlAdmin.BL.HistoryServerJei___BL
{
    public class HistoryServerJeiBL
    {
        #region METODO PARA CREAR
        // Metodo Para Guardar Un Nuevo Registro
        public async Task<int> CreateAsync(HistoryServerJei historyServerJei)
        {
            return await HistoryServerJeiDAL.CreateAsync(historyServerJei);
        }
        #endregion

        #region METODO PARA MOSTRAR
        // Metodo Para Mostrar Una Lista De Registros
        public async Task<List<HistoryServerJei>> GetAllAsync()
        {
            return await HistoryServerJeiDAL.GetAllAsync();
        }
        #endregion

        #region METODO PARA MOSTRAR POR ID
        // Metodo Para Mostrar Un Registro Especifico Bajo Un Id
        public async Task<HistoryServerJei> GetByIdAsync(HistoryServerJei historyServerJei)
        {
            return await HistoryServerJeiDAL.GetByIdAsync(historyServerJei);
        }
        #endregion

        #region METODO PARA BUSCAR
        // Metodo Para Buscar Registros Existentes
        public async Task<List<HistoryServerJei>> SearchAsync(HistoryServerJei historyServerJei)
        {
            return await HistoryServerJeiDAL.SearchAsync(historyServerJei);
        }
        #endregion

        #region METODO PARA INCLUIR PRIVILEGIO Y PRIVILEGIO
        public async Task<List<HistoryServerJei>> SearchIncludeAsync(HistoryServerJei historyServerJei)
        {
            return await HistoryServerJeiDAL.SearchIncludeAsync(historyServerJei);
        }
        #endregion
    }
}
