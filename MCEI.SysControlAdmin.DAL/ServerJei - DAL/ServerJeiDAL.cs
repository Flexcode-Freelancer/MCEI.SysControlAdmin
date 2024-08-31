#region REFERENCIAS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Referencias Necesarias Para El Correcto Funcionamiento
using Microsoft.EntityFrameworkCore;
using MCEI.SysControlAdmin.EN.ServerJei___EN;

#endregion

namespace MCEI.SysControlAdmin.DAL.ServerJei___DAL
{
    public class ServerJeiDAL
    {
        #region METODO PARA CREAR
        // Metodo Para Guardar Un Nuevo Registro En La Base De Datos
        public static async Task<int> CreateAsync(ServerJei serverJei)
        {
            int result = 0;
            // Un bloque de conexion que mientras se permanezca en el bloque la base de datos permanecera abierta y al terminar se destruira
            using (var dbContext = new ContextDB())
            {
                bool serverJeiExists = await ExistServerJei(serverJei, dbContext);
                if (serverJeiExists == false)
                {
                    dbContext.ServerJei.Add(serverJei);
                    result = await dbContext.SaveChangesAsync();
                }
                else
                    throw new Exception("Servidor Juvenil Ya Existente, Vuelve a Intentarlo Nuevamente");
            }
            return result;
        }
        #endregion

        #region METODO PARA MODIFICAR
        // Metodo Para Modificar Un Registro Existente De La Base De Datos
        public static async Task<int> UpdateAsync(ServerJei serverJei)
        {
            int result = 0;
            using (var dbContext = new ContextDB())
            {
                var serverJeiDB = await dbContext.ServerJei.FirstOrDefaultAsync(c => c.Id == serverJei.Id);
                if (serverJeiDB != null)
                {
                    bool serverJeiExists = await ExistServerJei(serverJei, dbContext);
                    if (serverJeiExists == false)
                    {
                        serverJeiDB.Id = serverJei.IdPrivilege;
                        serverJeiDB.IdPrivilege = serverJei.IdPrivilege;
                        serverJeiDB.Status = serverJei.Status;
                        serverJeiDB.DateCreated = serverJei.DateCreated;
                        serverJeiDB.DateModification = serverJei.DateModification;

                        dbContext.Update(serverJeiDB);
                        result = await dbContext.SaveChangesAsync();
                    }
                    else
                    {
                        throw new Exception("Servidor Juvenil Ya Existente, Vuelve a Intentarlo Nuevamente");
                    }
                }
                else
                {
                    throw new Exception("Servidor Juvenil No Encontrado Para Actualizar.");
                }
            }
            return result;
        }
        #endregion

        #region METODO PARA ELIMINAR
        // Metodo Para Eliminar Un Registro Existente En La Base De Datos
        public static async Task<int> DeleteAsync(ServerJei serverJei)
        {
            int result = 0;
            // Un bloque de conexion que mientras se permanezca en el bloque la base de datos permanecera abierta y al terminar se destruira
            using (var dbContext = new ContextDB())
            {
                var serverJeiDB = await dbContext.ServerJei.FirstOrDefaultAsync(c => c.Id == serverJei.Id);
                if (serverJeiDB != null)
                {
                    dbContext.ServerJei.Remove(serverJeiDB);
                    result = await dbContext.SaveChangesAsync();
                }
            }
            return result;
        }
        #endregion

        #region METODO PARA MOSTRAR
        // Metodo Para Mostrar La Lista De Registros
        public static async Task<List<ServerJei>> GetAllAsync()
        {
            var serverJei = new List<ServerJei>();
            using (var dbContext = new ContextDB())
            {
                serverJei = await dbContext.ServerJei.ToListAsync();
            }
            return serverJei;
        }
        #endregion

        #region METODO PARA OBTENER POR ID
        // Metodo Para Mostrar Un Registro En Base A Su Id
        public static async Task<ServerJei> GetByIdAsync(ServerJei serverJei)
        {
            var serverJeiDB = new ServerJei();
            using (var dbContext = new ContextDB())
            {
                serverJeiDB = await dbContext.ServerJei.FirstOrDefaultAsync(c => c.Id == serverJei.Id);
            }
            return serverJeiDB!;
        }
        #endregion

        #region METODO PARA BUSCAR REGISTROS MEDIANTE EL USO DE FILTROS
        // Metodo Para Buscar Por Filtros
        // IQueryable es una interfaz que toma un coleccion a la cual se le pueden implementar multiples consultas (Filtros)
        internal static IQueryable<ServerJei> QuerySelect(IQueryable<ServerJei> query, ServerJei serverJei)
        {
            // Por ID
            if (serverJei.Id > 0)
                query = query.Where(c => c.Id == serverJei.Id);

            // Por Miembro
            if (serverJei.IdJuventud > 0)
                query = query.Where(c => c.IdJuventud == serverJei.IdJuventud);

            // Por Privilegio
            if (serverJei.IdPrivilege > 0)
                query = query.Where(c => c.IdPrivilege == serverJei.IdPrivilege);

            // Por Su Estatus
            if (serverJei.Status > 0)
                query = query.Where(c => c.Status == serverJei.Status);

            // Ordenamos de Manera Desendente
            query = query.OrderByDescending(c => c.Id).AsQueryable();

            return query;
        }
        #endregion

        #region METODO PARA BUSCAR
        // Metodo para Buscar Registros Existentes
        public static async Task<List<ServerJei>> SearchAsync(ServerJei serverJei)
        {
            var serversJei = new List<ServerJei>();
            using (var dbContext = new ContextDB())
            {
                var select = dbContext.ServerJei.AsQueryable();
                select = QuerySelect(select, serverJei);
                serversJei = await select.ToListAsync();
            }
            return serversJei;
        }
        #endregion

        #region METODO PARA INCLUIR Joven Y PRIVILEGIOS
        // Método que incluye el Joven y el privilegio para la búsqueda
        public static async Task<List<ServerJei>> SearchIncludeAsync(ServerJei serverJei)
        {
            var serversJei = new List<ServerJei>();
            using (var dbContext = new ContextDB())
            {
                var select = dbContext.ServerJei.AsQueryable();
                select = QuerySelect(select, serverJei).Include(c => c.Juventud).Include(c => c.Privilege).AsQueryable();
                serversJei = await select.ToListAsync();
            }
            return serversJei;
        }
        #endregion

        #region METODO PARA VALIDAR UNICA EXISTENCIA DEL REGISTRO
        // Metodo Para Validar La Unica Existencia De Un Registro y No Haber Duplicidad
        private static async Task<bool> ExistServerJei(ServerJei serverJei, ContextDB contextDB)
        {
            bool result = false;
            var serversJei = await contextDB.ServerJei.FirstOrDefaultAsync(c => c.IdJuventud == serverJei.IdJuventud && c.Id != serverJei.Id);
            if (serversJei != null && serversJei.Id > 0 && serversJei.IdJuventud == serverJei.IdJuventud)
                result = true;
            return result;
        }
        #endregion
    }
}
