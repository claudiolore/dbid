using System.Threading.Tasks;

namespace Services
{
    public interface IDatabaseManagementService
    {
        /// <summary>
        /// Elimina todos los datos de la base de datos
        /// </summary>
        /// <returns>True si la operación fue exitosa, False en caso contrario</returns>
        Task<bool> ResetDatabaseAsync();
    }
}