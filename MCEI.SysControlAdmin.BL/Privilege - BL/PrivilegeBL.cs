#region REFERENCIAS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Referencias Necesarias Para El Correcto Funcionamiento
using MCEI.SysControlAdmin.EN.Privilege___EN;
using MCEI.SysControlAdmin.DAL.Privilege___DAL;

#endregion

namespace MCEI.SysControlAdmin.BL.Privilege___BL
{
    public class PrivilegeBL
    {
        #region METODO PARA CREAR
        // Metodo para Guardar Un Nuevo Registro
        public async Task<int> CreateAsync(Privilege privilege)
        {
            return await PrivilegeDAL.CreateAsync(privilege);
        }
        #endregion

        #region METODO PARA MODIFICAR
        // Metodo para Modificar Un Registro Existente
        public async Task<int> UpdateAsync(Privilege privilege)
        {
            return await PrivilegeDAL.UpdateAsync(privilege);
        }
        #endregion

        #region METODO PARA ELIMINAR
        // Metodo Para Eliminar Un Registro Existente
        public async Task<int> DeleteAsync(Privilege privilege)
        {
            return await PrivilegeDAL.DeleteAsync(privilege);
        }
        #endregion

        #region METODO PARA MOSTRAR
        // Metodo Para Mostrar Una Lista De Registros
        public async Task<List<Privilege>> GetAllAsync()
        {
            return await PrivilegeDAL.GetAllAsync();
        }
        #endregion

        #region METODO PARA MOSTRAR POR ID
        // Metodo Para Mostrar Un Registro Especifico Bajo Un Id
        public async Task<Privilege> GetByIdAsync(Privilege privilege)
        {
            return await PrivilegeDAL.GetByIdAsync(privilege);
        }
        #endregion

        #region METODO PARA BUSCAR
        // Metodo Para Buscar Registros Existentes
        public async Task<List<Privilege>> SearchAsync(Privilege privilege)
        {
            return await PrivilegeDAL.SearchAsync(privilege);
        }
        #endregion
    }
}
