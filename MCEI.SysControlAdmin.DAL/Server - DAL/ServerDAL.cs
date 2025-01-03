﻿#region REFERENCIAS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Referencias Necesarias Para El Correcto Funcionamiento
using MCEI.SysControlAdmin.EN.Server___EN;
using Microsoft.EntityFrameworkCore;


#endregion

namespace MCEI.SysControlAdmin.DAL.Server___DAL
{
    public class ServerDAL
    {
        #region METODO PARA CREAR
        // Metodo Para Guardar Un Nuevo Registro En La Base De Datos
        public static async Task<int> CreateAsync(Server server)
        {
            int result = 0;
            // Un bloque de conexion que mientras se permanezca en el bloque la base de datos permanecera abierta y al terminar se destruira
            using (var dbContext = new ContextDB())
            {
                bool serverExists = await ExistServer(server, dbContext);
                if (serverExists == false)
                {
                    dbContext.Server.Add(server);
                    result = await dbContext.SaveChangesAsync();
                }
                else
                    throw new Exception("Servidor Ya Existente, Vuelve a Intentarlo Nuevamente");
            }
            return result;
        }
        #endregion

        #region METODO PARA MODIFICAR
        // Metodo Para Modificar Un Registro Existente De La Base De Datos
        public static async Task<int> UpdateAsync(Server server)
        {
            int result = 0;
            using (var dbContext = new ContextDB())
            {
                var serverDB = await dbContext.Server.FirstOrDefaultAsync(c => c.Id == server.Id);
                if (serverDB != null)
                {
                    bool serverExists = await ExistServer(server, dbContext);
                    if (serverExists == false)
                    {
                        serverDB.IdMembership = server.IdMembership;
                        serverDB.IdPrivilege = server.IdPrivilege;
                        serverDB.Status = server.Status;
                        serverDB.DateCreated = server.DateCreated;
                        serverDB.DateModification = server.DateModification;

                        dbContext.Update(serverDB);
                        result = await dbContext.SaveChangesAsync();
                    }
                    else
                    {
                        throw new Exception("Servidor Ya Existente, Vuelve a Intentarlo Nuevamente");
                    }
                }
                else
                {
                    throw new Exception("Servidor No Encontrado Para Actualizar.");
                }
            }
            return result;
        }
        #endregion

        #region METODO PARA ELIMINAR
        // Metodo Para Eliminar Un Registro Existente En La Base De Datos
        public static async Task<int> DeleteAsync(Server server)
        {
            int result = 0;
            // Un bloque de conexion que mientras se permanezca en el bloque la base de datos permanecera abierta y al terminar se destruira
            using (var dbContext = new ContextDB())
            {
                var serverDB = await dbContext.Server.FirstOrDefaultAsync(c => c.Id == server.Id);
                if (serverDB != null)
                {
                    dbContext.Server.Remove(serverDB);
                    result = await dbContext.SaveChangesAsync();
                }
            }
            return result;
        }
        #endregion

        #region METODO PARA MOSTRAR
        // Metodo Para Mostrar La Lista De Registros
        public static async Task<List<Server>> GetAllAsync()
        {
            var servers = new List<Server>();
            using (var dbContext = new ContextDB())
            {
                servers = await dbContext.Server.ToListAsync();
            }
            return servers;
        }
        #endregion

        #region METODO PARA OBTENER POR ID
        // Metodo Para Mostrar Un Registro En Base A Su Id
        public static async Task<Server> GetByIdAsync(Server server)
        {
            var serverDB = new Server();
            using (var dbContext = new ContextDB())
            {
                serverDB = await dbContext.Server.Include(m => m.Membership).Include(m => m.Privilege).FirstOrDefaultAsync(c => c.Id == server.Id);
            }
            return serverDB!;
        }

        // ******** Comunmente el metodo es asi, pero se cambio debido a la creacion de reporte y acceder a datos de Membership ******
        //public static async Task<Server> GetByIdAsync(Server server)
        //{
        //    var serverDB = new Server();
        //    using (var dbContext = new ContextDB())
        //    {
        //        serverDB = await dbContext.Server.FirstOrDefaultAsync(c => c.Id == server.Id);
        //    }
        //    return serverDB!;
        //}
        #endregion

        public static async Task<Dictionary<int, List<Server>>> GetActiveServersGroupedByPrivilegeAsync()
        {
            using (var dbContext = new ContextDB())
            {
                var activeServers = await dbContext.Server
                    .Where(s => s.Status == 1) // Filtrar por status = 1
                    .Include(s => s.Membership) // Incluir propiedades relacionadas si es necesario
                    .Include(s => s.Privilege)
                    .ToListAsync();

                // Agrupar por PrivilegeId
                var groupedServers = activeServers
                    .GroupBy(s => s.IdPrivilege) // Agrupar por IdPrivilege
                    .ToDictionary(g => g.Key, g => g.ToList()); // Convertir a un diccionario

                return groupedServers;
            }
        }

        #region METODO PARA BUSCAR REGISTROS MEDIANTE EL USO DE FILTROS
        // Metodo Para Buscar Por Filtros
        // IQueryable es una interfaz que toma un coleccion a la cual se le pueden implementar multiples consultas (Filtros)
        internal static IQueryable<Server> QuerySelect(IQueryable<Server> query, Server server)
        {
            // Por ID
            if (server.Id > 0)
                query = query.Where(c => c.Id == server.Id);

            // Por Miembro
            if (server.IdMembership > 0)
                query = query.Where(c => c.IdMembership == server.IdMembership);

            // Por Privilegio
            if (server.IdPrivilege > 0)
                query = query.Where(c => c.IdPrivilege == server.IdPrivilege);

            // Por Su Estatus
            if (server.Status > 0)
                query = query.Where(c => c.Status == server.Status);

            // Ordenamos de Manera Desendente
            query = query.OrderByDescending(c => c.Id).AsQueryable();

            return query;
        }
        #endregion

        #region METODO PARA BUSCAR
        // Metodo para Buscar Registros Existentes
        public static async Task<List<Server>> SearchAsync(Server server)
        {
            var servers = new List<Server>();
            using (var dbContext = new ContextDB())
            {
                var select = dbContext.Server.AsQueryable();
                select = QuerySelect(select, server);
                servers = await select.ToListAsync();
            }
            return servers;
        }
        #endregion

        #region METODO PARA INCLUIR MEMBRESIA Y PRIVILEGIOS
        // Método que incluye el membresia y el privilegio para la búsqueda
        public static async Task<List<Server>> SearchIncludeAsync(Server server)
        {
            var servers = new List<Server>();
            using (var dbContext = new ContextDB())
            {
                var select = dbContext.Server.AsQueryable();
                select = QuerySelect(select, server).Include(c => c.Membership).Include(c => c.Privilege).AsQueryable();
                servers = await select.ToListAsync();
            }
            return servers;
        }
        #endregion

        #region METODO PARA VALIDAR UNICA EXISTENCIA DEL REGISTRO
        // Metodo Para Validar La Unica Existencia De Un Registro y No Haber Duplicidad
        private static async Task<bool> ExistServer(Server server, ContextDB contextDB)
        {
            bool result = false;
            var servers = await contextDB.Server.FirstOrDefaultAsync(c => c.IdMembership == server.IdMembership && c.Id != server.Id);
            if (servers != null && servers.Id > 0 && servers.IdMembership == server.IdMembership)
                result = true;
            return result;
        }
        #endregion
    }
}
