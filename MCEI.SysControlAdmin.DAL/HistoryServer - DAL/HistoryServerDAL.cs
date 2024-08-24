#region REFERNCIAS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Referencias Necesarias Para El Correcto Funcionamiento
using MCEI.SysControlAdmin.EN.HistoryServer___EN;
using Microsoft.EntityFrameworkCore;


#endregion

namespace MCEI.SysControlAdmin.DAL.HistoryServer___DAL
{
    public class HistoryServerDAL
    {
        #region METODO PARA CREAR
        // Metodo Para Guardar Un Nuevo Registro En La Base De Datos
        public static async Task<int> CreateAsync(HistoryServer historyServer)
        {
            int result = 0;
            // Un bloque de conexion que mientras se permanezca en el bloque la base de datos permanecera abierta y al terminar se destruira
            using (var dbContext = new ContextDB())
            {
                dbContext.HistoryServer.Add(historyServer);
                result = await dbContext.SaveChangesAsync();
            }
            return result;
        }
        #endregion.

        #region METODO PARA MOSTRAR
        // Metodo Para Mostrar La Lista De Registros
        public static async Task<List<HistoryServer>> GetAllAsync()
        {
            var historyServers = new List<HistoryServer>();
            using (var dbContext = new ContextDB())
            {
                historyServers = await dbContext.HistoryServer.ToListAsync();
            }
            return historyServers;
        }
        #endregion

        #region METODO PARA OBTENER POR ID
        // Metodo Para Mostrar Un Registro En Base A Su Id
        public static async Task<HistoryServer> GetByIdAsync(HistoryServer historyServer)
        {
            var historyServerDB = new HistoryServer();
            using (var dbContext = new ContextDB())
            {
                historyServerDB = await dbContext.HistoryServer.FirstOrDefaultAsync(c => c.Id == historyServer.Id);
            }
            return historyServerDB!;
        }
        #endregion

        #region METODO PARA BUSCAR REGISTROS MEDIANTE EL USO DE FILTROS
        // Metodo Para Buscar Por Filtros
        // IQueryable es una interfaz que toma un coleccion a la cual se le pueden implementar multiples consultas (Filtros)
        internal static IQueryable<HistoryServer> QuerySelect(IQueryable<HistoryServer> query, HistoryServer historyServer)
        {
            // Por ID
            if (historyServer.Id > 0)
                query = query.Where(c => c.Id == historyServer.Id);

            // Por Miembro
            if (historyServer.IdMembership > 0)
                query = query.Where(c => c.IdMembership == historyServer.IdMembership);

            // Por Privilegio
            if (historyServer.IdPrivilege > 0)
                query = query.Where(c => c.IdPrivilege == historyServer.IdPrivilege);

            // Por Su Estatus
            if (historyServer.Status > 0)
                query = query.Where(c => c.Status == historyServer.Status);

            // Ordenamos de Manera Desendente
            query = query.OrderByDescending(c => c.Id).AsQueryable();

            return query;
        }
        #endregion

        #region METODO PARA BUSCAR
        // Metodo para Buscar Registros Existentes
        public static async Task<List<HistoryServer>> SearchAsync(HistoryServer historyServer)
        {
            var historyServers = new List<HistoryServer>();
            using (var dbContext = new ContextDB())
            {
                var select = dbContext.HistoryServer.AsQueryable();
                select = QuerySelect(select, historyServer);
                historyServers = await select.ToListAsync();
            }
            return historyServers;
        }
        #endregion

        #region METODO PARA INCLUIR MEMBRESIA Y PRIVILEGIOS
        // Método que incluye el membresia y el privilegio para la búsqueda
        public static async Task<List<HistoryServer>> SearchIncludeAsync(HistoryServer historyServer)
        {
            var historyServers = new List<HistoryServer>();
            using (var dbContext = new ContextDB())
            {
                var select = dbContext.HistoryServer.AsQueryable();
                select = QuerySelect(select, historyServer).Include(c => c.Membership).Include(c => c.Privilege).AsQueryable();
                historyServers = await select.ToListAsync();
            }
            return historyServers;
        }
        #endregion
    }
}
