using System;
using System.Threading.Tasks;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Services
{
    public class DatabaseManagementService : IDatabaseManagementService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DatabaseManagementService> _logger;

        public DatabaseManagementService(ApplicationDbContext context, ILogger<DatabaseManagementService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> ResetDatabaseAsync()
        {
            try
            {
                // Iniciar transacción para asegurar que la operación sea atómica
                using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    _logger.LogInformation("Iniciando proceso de reseteo de la base de datos");

                    // Eliminar registros de todas las tablas en orden inverso de dependencia
                    await _context.SyncRecords.ExecuteDeleteAsync();
                    await _context.FileRecords.ExecuteDeleteAsync();
                    await _context.SegnalazioneProblema.ExecuteDeleteAsync();
                    await _context.DocumentiGenerali.ExecuteDeleteAsync();
                    await _context.AltriImpianti.ExecuteDeleteAsync();
                    await _context.ImpiantoClimaAcs.ExecuteDeleteAsync();
                    await _context.ImpiantiElettrici.ExecuteDeleteAsync();
                    await _context.ScarichiIdriciFognari.ExecuteDeleteAsync();
                    await _context.IdraulicoAdduzione.ExecuteDeleteAsync();
                    await _context.Infissi.ExecuteDeleteAsync();
                    await _context.Strutture.ExecuteDeleteAsync();
                    await _context.UnitaImmobiliari.ExecuteDeleteAsync();
                    await _context.Edifici.ExecuteDeleteAsync();
                    await _context.Complessi.ExecuteDeleteAsync();

                    // Confirmar la transacción
                    await transaction.CommitAsync();

                    _logger.LogInformation("Base de datos reseteada exitosamente");

                    return true;
                }
                catch (Exception ex)
                {
                    // Revertir la transacción en caso de error
                    await transaction.RollbackAsync();
                    _logger.LogError(ex, "Error al resetear la base de datos");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al iniciar la transacción para resetear la base de datos");
                return false;
            }
        }
    }
}