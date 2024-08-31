#region REFERENCIAS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Referencias Necesarias Para El Correcto Funcionamiento
using Microsoft.EntityFrameworkCore;
using MCEI.SysControlAdmin.EN.HistoryServerJei___EN;


#endregion

namespace MCEI.SysControlAdmin.DAL.HistoryServerJei___DAL
{
    public class HistoryServerJeiDAL
    {
        #region METODO PARA CREAR
        // Metodo Para Guardar Un Nuevo Registro En La Base De Datos
        public static async Task<int> CreateAsync(HistoryServerJei historyServerJei)
        {
            int result = 0;
            // Un bloque de conexion que mientras se permanezca en el bloque la base de datos permanecera abierta y al terminar se destruira
            using (var dbContext = new ContextDB())
            {
                dbContext.HistoryServerJei.Add(historyServerJei);
                result = await dbContext.SaveChangesAsync();
            }
            return result;
        }
        #endregion

        #region METODO PARA MOSTRAR
        // Metodo Para Mostrar La Lista De Registros
        public static async Task<List<HistoryServerJei>> GetAllAsync()
        {
            var historyServerJei = new List<HistoryServerJei>();
            using (var dbContext = new ContextDB())
            {
                historyServerJei = await dbContext.HistoryServerJei.ToListAsync();
            }
            return historyServerJei;
        }
        #endregion

        #region METODO PARA OBTENER POR ID
        // Metodo Para Mostrar Un Registro En Base A Su Id
        public static async Task<HistoryServerJei> GetByIdAsync(HistoryServerJei historyServerJei)
        {
            var historyServerJeiDB = new HistoryServerJei();
            using (var dbContext = new ContextDB())
            {
                historyServerJeiDB = await dbContext.HistoryServerJei.FirstOrDefaultAsync(c => c.Id == historyServerJei.Id);
            }
            return historyServerJeiDB!;
        }
        #endregion

        #region METODO PARA BUSCAR REGISTROS MEDIANTE EL USO DE FILTROS
        // Metodo Para Buscar Por Filtros
        // IQueryable es una interfaz que toma un coleccion a la cual se le pueden implementar multiples consultas (Filtros)
        internal static IQueryable<HistoryServerJei> QuerySelect(IQueryable<HistoryServerJei> query, HistoryServerJei historyServerJei)
        {
            // Por ID
            if (historyServerJei.Id > 0)
                query = query.Where(c => c.Id == historyServerJei.Id);

            // Por Miembro
            if (historyServerJei.IdJuventud > 0)
                query = query.Where(c => c.IdJuventud == historyServerJei.IdJuventud);

            // Por Privilegio
            if (historyServerJei.IdPrivilege > 0)
                query = query.Where(c => c.IdPrivilege == historyServerJei.IdPrivilege);

            // Por Su Estatus
            if (historyServerJei.Status > 0)
                query = query.Where(c => c.Status == historyServerJei.Status);

            // Ordenamos de Manera Desendente
            query = query.OrderByDescending(c => c.Id).AsQueryable();

            return query;
        }
        #endregion

        #region METODO PARA BUSCAR
        // Metodo para Buscar Registros Existentes
        public static async Task<List<HistoryServerJei>> SearchAsync(HistoryServerJei historyServerJei)
        {
            var historyServersJei = new List<HistoryServerJei>();
            using (var dbContext = new ContextDB())
            {
                var select = dbContext.HistoryServerJei.AsQueryable();
                select = QuerySelect(select, historyServerJei);
                historyServersJei = await select.ToListAsync();
            }
            return historyServersJei;
        }
        #endregion

        #region METODO PARA INCLUIR MEMBRESIA Y PRIVILEGIOS
        // Método que incluye el Joven y el privilegio para la búsqueda
        public static async Task<List<HistoryServerJei>> SearchIncludeAsync(HistoryServerJei historyServerJei)
        {
            var historyServersJei = new List<HistoryServerJei>();
            using (var dbContext = new ContextDB())
            {
                var select = dbContext.HistoryServerJei.AsQueryable();
                select = QuerySelect(select, historyServerJei).Include(c => c.Juventud).Include(c => c.Privilege).AsQueryable();
                historyServersJei = await select.ToListAsync();
            }
            return historyServersJei;
        }
        #endregion
    }
}
