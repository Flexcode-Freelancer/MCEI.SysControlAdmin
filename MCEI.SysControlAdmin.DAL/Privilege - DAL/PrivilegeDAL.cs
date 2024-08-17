#region REFERENCIAS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Referencias Necesarias Para El Correcto Funcionamiento
using Microsoft.EntityFrameworkCore;
using MCEI.SysControlAdmin.EN.Privilege___EN;

#endregion

namespace MCEI.SysControlAdmin.DAL.Privilege___DAL
{
    public class PrivilegeDAL
    {
        #region METODO PARA CREAR
        // Metodo Para Guardar Un Nuevo Registro En La Base De Datos
        public static async Task<int> CreateAsync(Privilege privilege)
        {
            int result = 0;
            using (var dbContext = new ContextDB())
            {
                bool privilegeExists = await ExistPrivilege(privilege, dbContext);
                if (privilegeExists == false)
                {
                    dbContext.Privilege.Add(privilege);
                    result = await dbContext.SaveChangesAsync(); // Await sirve para esperar a terminar todos los procesos para devolverlos todos juntos
                }
                else
                    throw new Exception("Privilegio Ya Existente, Vuelve a Intentarlo");
            }
            return result;  // Si se realizo con exito devuelve 1 sino devuelve 0
        }
        #endregion

        #region METODO PARA MODIFICAR
        // Metodo Para Modificar Un Registro Existente En La Base De Datos
        public static async Task<int> UpdateAsync(Privilege privilege)
        {
            int result = 0;
            using (var dbContext = new ContextDB())
            {
                // Obtiene el primer registro con el Id específico de ProfessionOrStudys desde la base de datos.
                var privilegeDB = await dbContext.Privilege.FirstOrDefaultAsync(c => c.Id == privilege.Id);
                // Validamos que professionOrStudyDB sea diferente de NULL
                if (privilegeDB != null)
                {
                    bool privilegeExists = await ExistPrivilege(privilege, dbContext);
                    if (privilegeExists == false)
                    {
                        privilegeDB.Name = privilege.Name;

                        dbContext.Update(privilegeDB);
                        result = await dbContext.SaveChangesAsync();
                    }
                    else
                        throw new Exception("Privilegio Ya Existente, Vuelve a Intentarlo");
                }
            }
            return result;  // Si se realizo con exito devuelve 1 sino devuelve 0
        }
        #endregion

        #region METODO PARA ELIMINAR
        // Metodo Para Eliminar Un Registro Existente En La Base De Datos
        public static async Task<int> DeleteAsync(Privilege privilege)
        {
            int result = 0;
            using (var dbContext = new ContextDB())
            {
                // Obtiene el primer registro con el Id específico de ProfessionOrStudys desde la base de datos.
                var privilegeDB = await dbContext.Privilege.FirstOrDefaultAsync(c => c.Id == privilege.Id);
                // Validamos que professionOrStudyDB sea diferente de NULL
                if (privilegeDB != null)
                {
                    dbContext.Privilege.Remove(privilegeDB);
                    result = await dbContext.SaveChangesAsync();
                }
            }
            return result;  // Si se realizo con exito devuelve 1 sino devuelve 0
        }
        #endregion

        #region METODO PARA MOSTRAR
        // Metodo Para Mostrar La Lista De Registros
        public static async Task<List<Privilege>> GetAllAsync()
        {
            var privilege = new List<Privilege>();
            using (var dbContext = new ContextDB())
            {
                privilege = await dbContext.Privilege.ToListAsync();
            }
            return privilege;
        }
        #endregion

        #region METODO PARA OBTENER POR ID
        // Metodo Para Mostrar Un Registro En Base A Su Id
        public static async Task<Privilege> GetByIdAsync(Privilege privilege)
        {
            var privilegeDB = new Privilege();
            using (var dbContext = new ContextDB())
            {
                privilegeDB = await dbContext.Privilege.FirstOrDefaultAsync(c => c.Id == privilege.Id);
            }
            return privilegeDB!; // Retornamos el Registro Encontrado
        }
        #endregion

        #region METODO PARA BUSCAR REGISTROS MEDIANTE EL USO DE FILTROS (Por Nombre)
        // Metodo Para Buscar Por Filtros
        // IQueryable es una interfaz que toma un coleccion a la cual se le pueden implementar multiples consultas (Filtros)
        internal static IQueryable<Privilege> QuerySelect(IQueryable<Privilege> query, Privilege privilege)
        {
            // Por ID
            if (privilege.Id > 0)
                query = query.Where(c => c.Id == privilege.Id);

            // Por Nomnbre, Si es verdadero lo vuelve falso y viceversa 
            if (!string.IsNullOrWhiteSpace(privilege.Name))
                query = query.Where(c => c.Name.Contains(privilege.Name));

            // Ordenamos de Manera Desendente
            query = query.OrderByDescending(c => c.Id).AsQueryable();

            return query;
        }
        #endregion

        #region METODO PARA BUSCAR
        // Metodo para Buscar Registros Existentes
        public static async Task<List<Privilege>> SearchAsync(Privilege privilege)
        {
            var privileges = new List<Privilege>();
            using (var dbContext = new ContextDB())
            {
                var select = dbContext.Privilege.AsQueryable();
                select = QuerySelect(select, privilege);
                privileges = await select.ToListAsync();
            }
            return privileges;
        }
        #endregion

        #region METODO PARA VALIDAR UNICA EXISTENCIA DEL REGISTRO
        // Metodo Para Validar La Unica Existencia De Un Registro y No Haber Duplicidads
        private static async Task<bool> ExistPrivilege(Privilege privilege, ContextDB dbContext)
        {
            bool result = false;
            var privileges = await dbContext.Privilege.FirstOrDefaultAsync(p => p.Name == privilege.Name && p.Id != privilege.Id);
            if (privileges != null && privileges.Id > 0 && privileges.Name == privilege.Name)
                result = true;

            return result;
        }
        #endregion
    }
}
