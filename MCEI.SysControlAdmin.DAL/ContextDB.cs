#region REFERENCIAS
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Referencias Necesarias Para El Correcto Funcionamiento
using Microsoft.EntityFrameworkCore;
using MCEI.SysControlAdmin.EN.HistoryServer___EN;
using MCEI.SysControlAdmin.EN.Membership___EN;
using MCEI.SysControlAdmin.EN.Privilege___EN;
using MCEI.SysControlAdmin.EN.ProfessionOrStudy___EN;
using MCEI.SysControlAdmin.EN.Role___EN;
using MCEI.SysControlAdmin.EN.Server___EN;
using MCEI.SysControlAdmin.EN.User___EN;
using MCEI.SysControlAdmin.EN.Juventud___EN;
using MCEI.SysControlAdmin.EN.ServerJei___EN;
using MCEI.SysControlAdmin.EN.HistoryServerJei___EN;


#endregion

namespace MCEI.SysControlAdmin.DAL
{
    public class ContextDB : DbContext
    {
        #region REFERENCIAS DE TABLAS DE LA BD
        //Coleccion que hace referencia a la tabla de la base de datos
        public DbSet<Role> Role { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<ProfessionOrStudy> ProfessionOrStudy { get; set; }
        public DbSet<Membership> Membership { get; set; }
        public DbSet<Privilege> Privilege { get; set; }
        public DbSet<Server> Server { get; set; }
        public DbSet<HistoryServer> HistoryServer { get; set; }
        public DbSet<Juventud> Juventud { get; set; }
        public DbSet<ServerJei> ServerJei { get; set; }
        public DbSet<HistoryServerJei> HistoryServerJei { get; set; }
        #endregion

        // Metodo de Conexion a la Base de Datos
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=p1434.use1.mysecurecloudhost.com;Initial Catalog=testmcei;User ID=eliqsv_mceitest;Password=mcei.2024;Trust Server Certificate=True"); // String de Conexion
        }
    }
}