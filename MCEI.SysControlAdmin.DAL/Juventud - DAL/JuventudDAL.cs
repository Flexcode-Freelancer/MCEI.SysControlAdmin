#region REFERENCIAS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Referencias Necesarias Para El Correcto Funcionamiento
using Microsoft.EntityFrameworkCore;
using MCEI.SysControlAdmin.EN.Juventud___EN;

#endregion

namespace MCEI.SysControlAdmin.DAL.Juventud___DAL
{
    public class JuventudDAL
    {
        #region METODO PARA CREAR
        // Metodo Para Guardar Un Nuevo Registro En La Base De Datos
        public static async Task<int> CreateAsync(Juventud juventud)
        {
            int result = 0;
            // Un bloque de conexion que mientras se permanezca en el bloque la base de datos permanecera abierta y al terminar se destruira
            using (var dbContext = new ContextDB())
            {
                bool juventudExists = await ExistJuventud(juventud, dbContext);
                if (juventudExists == false)
                {
                    dbContext.Juventud.Add(juventud);
                    result = await dbContext.SaveChangesAsync(); // Await sirve para esperar a terminar todos los procesos para devolverlos todos juntos
                }
                else
                    throw new Exception("Joven Ya Existente, Vuelve a Intentarlo Nuevamente.");
            }
            return result;  // Si se realizo con exito devuelve 1 sino devuelve 0
        }
        #endregion

        #region METODO PARA MODIFICAR
        // Metodo Para Modificar Un Registro Existente De La Base De Datos
        public static async Task<int> UpdateAsync(Juventud juventud)
        {
            int result = 0;
            using (var dbContext = new ContextDB())
            {
                var juventudDB = await dbContext.Juventud.FirstOrDefaultAsync(m => m.Id == juventud.Id);
                if (juventudDB != null)
                {
                    bool juventudExists = await ExistJuventud(juventud, dbContext);
                    if (juventudExists == false)
                    {
                        juventudDB.Name = juventud.Name;
                        juventudDB.LastName = juventud.LastName;
                        juventudDB.DateOfBirth = juventud.DateOfBirth;
                        juventudDB.Age = juventud.Age;
                        juventudDB.Gender = juventud.Gender;
                        juventudDB.CivilStatus = juventud.CivilStatus;
                        juventudDB.Phone = juventud.Phone;
                        juventudDB.IdProfessionOrStudy = juventud.IdProfessionOrStudy;
                        juventudDB.WaterBaptism = juventud.WaterBaptism;
                        juventudDB.BaptismOfTheHolySpirit = juventud.BaptismOfTheHolySpirit;
                        juventudDB.Status = juventud.Status;
                        juventudDB.CommentsOrObservations = juventud.CommentsOrObservations;
                        juventudDB.DateCreated = juventud.DateCreated;
                        juventudDB.DateModification = juventud.DateModification;
                        juventudDB.ImageData = juventud.ImageData;

                        dbContext.Update(juventudDB);
                        result = await dbContext.SaveChangesAsync();
                    }
                    else
                    {
                        throw new Exception("Joven ya existente, vuelve a intentarlo nuevamente.");
                    }
                }
                else
                {
                    throw new Exception("Joven no encontrado para actualizar.");
                }
            }
            return result;
        }
        #endregion

        #region METODO PARA ELIMINAR
        // Metodo Para Eliminar Un Registro Existente En La Base De Datos
        public static async Task<int> DeleteAsync(Juventud juventudDB)
        {
            int result = 0;
            // Un bloque de conexion que mientras se permanezca en el bloque la base de datos permanecera abierta y al terminar se destruira
            using (var dbContext = new ContextDB())
            {
                // Obtiene el primer registro con el Id específico de memberships desde la base de datos.
                var juventudDBDB = await dbContext.Juventud.FirstOrDefaultAsync(m => m.Id == juventudDB.Id);
                // Validamos que membershipDB sea diferente de NULL
                if (juventudDBDB != null)
                {
                    dbContext.Juventud.Remove(juventudDBDB);
                    result = await dbContext.SaveChangesAsync();
                }
            }
            return result;  // Si se realizo con exito devuelve 1 sino devuelve 0
        }
        #endregion

        #region METODO PARA MOSTRAR
        // Metodo Para Mostrar La Lista De Registros
        public static async Task<List<Juventud>> GetAllAsync()
        {
            var juventud = new List<Juventud>();
            // Un bloque de conexion que mientras se permanezca en el bloque la base de datos permanecera abierta y al terminar se destruira
            using (var dbContext = new ContextDB())
            {
                juventud = await dbContext.Juventud.ToListAsync();
            }
            return juventud;
        }
        #endregion

        #region METODO PARA OBTENER POR ID
        // Metodo Para Mostrar Un Registro En Base A Su Id
        public static async Task<Juventud> GetByIdAsync(Juventud juventud)
        {
            var juventudDB = new Juventud();

            // Un bloque de conexión que mientras se permanezca en el bloque, la base de datos permanecerá abierta y al terminar se destruirá
            using (var dbContext = new ContextDB())
            {
                juventudDB = await dbContext.Juventud
                    .Include(m => m.ProfessionOrStudy) // Incluir la propiedad de navegación de Profesión u Oficio
                    .FirstOrDefaultAsync(m => m.Id == juventud.Id);
            }

            return juventudDB!; // Retornamos el registro encontrado
        }

        // Normalmente el metodo deberia ser asi como el siguiente pero, por modificaciones en
        // el proceso de servidores se a modificado pero se deja la forma normal para aprenderla.
        // *********** Metodo Para Mostrar Un Registro En Base A Su Id *************
        //public static async Task<Membership> GetByIdAsync(Membership memberships)
        //{
        //    var membershipDB = new Membership();
        //    // Un bloque de conexion que mientras se permanezca en el bloque la base de datos permanecera abierta y al terminar se destruira
        //    using (var dbContext = new ContextDB())
        //    {
        //        membershipDB = await dbContext.Membership.FirstOrDefaultAsync(m => m.Id == memberships.Id);
        //    }
        //    return membershipDB!; // Retornamos el Registro Encontrado
        //}
        #endregion

        #region METODO PARA BUSCAR REGISTROS MEDIANTE EL USO DE FILTROS
        // Metodo Para Buscar Por Filtros
        // IQueryable es una interfaz que toma un coleccion a la cual se le pueden implementar multiples consultas (Filtros)
        internal static IQueryable<Juventud> QuerySelect(IQueryable<Juventud> query, Juventud juventud)
        {
            // Por ID
            if (juventud.Id > 0)
                query = query.Where(m => m.Id == juventud.Id);

            // Por Su Profesion u Oficio
            if (juventud.IdProfessionOrStudy > 0)
                query = query.Where(m => m.IdProfessionOrStudy == juventud.IdProfessionOrStudy);

            // Por Nomnbre, Si es verdadero lo vuelve falso y viceversa 
            if (!string.IsNullOrWhiteSpace(juventud.Name))
                query = query.Where(m => m.Name.Contains(juventud.Name));

            // Por Apellido, Si es verdadero lo vuelve falso y viceversa 
            if (!string.IsNullOrWhiteSpace(juventud.LastName))
                query = query.Where(m => m.LastName.Contains(juventud.LastName));

            // Ordenamos de Manera Desendente
            query = query.OrderByDescending(c => c.Id).AsQueryable();

            return query;
        }
        #endregion

        #region METODO PARA BUSCAR
        // Metodo para Buscar Registros Existentes
        public static async Task<List<Juventud>> SearchAsync(Juventud juventud)
        {
            var juventuds = new List<Juventud>();
            // Un bloque de conexion que mientras se permanezca en el bloque la base de datos permanecera abierta y al terminar se destruira
            using (var dbContext = new ContextDB())
            {
                var select = dbContext.Juventud.AsQueryable();
                select = QuerySelect(select, juventud);
                juventuds = await select.ToListAsync();
            }
            return juventuds;
        }
        #endregion

        #region METODO PARA INCLUIR A PROFESION U OFICIO
        // Metodo Que Incluye la Profesion u Oficio Para La Busqueda
        public static async Task<List<Juventud>> SearchIncludeProfessionOrStudyAsync(Juventud juventud)
        {
            var juventuds = new List<Juventud>();
            using (var dbContext = new ContextDB())
            {
                var select = dbContext.Juventud.AsQueryable();
                select = QuerySelect(select, juventud).Include(m => m.ProfessionOrStudy).AsQueryable();
                juventuds = await select.ToListAsync();
            }
            return juventuds;
        }
        #endregion

        #region METODO PARA VALIDAR UNICA EXISTENCIA DEL REGISTRO
        // Metodo Para Validar La Unica Existencia De Un Registro y No Haber Duplicidad
        private static async Task<bool> ExistJuventud(Juventud juventud, ContextDB contextDB)
        {
            bool result = false;
            var juventuds = await contextDB.Juventud.FirstOrDefaultAsync(m => m.Name == juventud.Name && m.LastName == juventud.LastName && m.Id != juventud.Id);
            if (juventuds != null && juventuds.Id > 0 && juventuds.Name == juventud.Name && juventuds.LastName == juventud.LastName)
                result = true;
            return result;
        }
        #endregion
    }
}
