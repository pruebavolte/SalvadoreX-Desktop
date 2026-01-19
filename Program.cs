using SalvadoreXDesktop.Forms;
using SalvadoreXDesktop.Data;
using SalvadoreXDesktop.Services;

namespace SalvadoreXDesktop
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            
            // Inicializar base de datos local
            DatabaseManager.Initialize();
            
            // Iniciar servicio de sincronizacion en segundo plano
            SyncService.Instance.StartBackgroundSync();
            
            Application.Run(new MainForm());
            
            // Detener sincronizacion al cerrar
            SyncService.Instance.StopBackgroundSync();
        }
    }
}
