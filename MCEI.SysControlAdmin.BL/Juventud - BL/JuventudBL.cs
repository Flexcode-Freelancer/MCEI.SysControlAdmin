#region REFERENCIAS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Referencias Necesarias Para El Correcto Funcionamiento
using MCEI.SysControlAdmin.EN.Juventud___EN;
using MCEI.SysControlAdmin.DAL.Juventud___DAL;

#endregion

namespace MCEI.SysControlAdmin.BL.Juventud___BL
{
    public class JuventudBL
    {
        #region METODO PARA CREAR
        // Metodo Para Guardar Un Nuevo Registro
        public async Task<int> CreateAsync(Juventud juventud)
        {
            return await JuventudDAL.CreateAsync(juventud);
        }
        #endregion

        #region METODO PARA MODIFICAR
        // Metodo Para Guardar Un Nuevo Registro
        public async Task<int> UpdateAsync(Juventud juventud)
        {
            return await JuventudDAL.UpdateAsync(juventud);
        }
        #endregion

        #region METODO PARA ELIMINAR
        // Metodo Para Eliminar Un Registro Existente En La Base De Datos
        public async Task<int> DeleteAsync(Juventud juventud)
        {
            return await JuventudDAL.DeleteAsync(juventud);
        }
        #endregion

        #region METODO PARA MOSTRAR
        // Metodo Para Mostrar Una Lista De Registros
        public async Task<List<Juventud>> GetAllAsync()
        {
            return await JuventudDAL.GetAllAsync();
        }
        #endregion

        #region METODO PARA MOSTRAR POR ID
        // Metodo Para Mostrar Un Registro Especifico Bajo Un Id
        public async Task<Juventud> GetByIdAsync(Juventud juventud)
        {
            return await JuventudDAL.GetByIdAsync(juventud);
        }
        #endregion

        // Metodo para que admita int al hacer uso del metodo antecesor para automatizacion en el proceso de Servidores.
        public async Task<Juventud> GetByIdAsync(int id)
        {
            // Crear una instancia de Membership y asignarle el ID
            var juventud = new Juventud { Id = id };

            // Llamar al método existente con el objeto Membership
            return await JuventudDAL.GetByIdAsync(juventud);
        }

        #region METODO PARA BUSCAR
        // Metodo Para Buscar Registros Existentes
        public async Task<List<Juventud>> SearchAsync(Juventud juventud)
        {
            return await JuventudDAL.SearchAsync(juventud);
        }
        #endregion

        #region METODO PARA INCLUIR A PROFESION U OFICIO
        public async Task<List<Juventud>> SearchIncludeProfessionOrStudyAsync(Juventud juventud)
        {
            return await JuventudDAL.SearchIncludeProfessionOrStudyAsync(juventud);
        }
        #endregion
    }
}
