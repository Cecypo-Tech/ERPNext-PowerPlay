using DevExpress.XtraEditors;
using DevExpress.XtraReports.Security;
using Serilog;
using Serilog.Sinks.WinForms.Base;
using System.Diagnostics;
using System.IO;
using Velopack;

namespace ERPNext_PowerPlay
{
    internal static class Program
    {

        public static string FrappeURL = "";
        public static string FrappeUser = "";
        public static string MyAppDir = "";
        public static string ApiToken = "";

        private static bool _updateChecked = false;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // MUST be the very first thing called in Main
            VelopackApp.Build().Run();

            if (PriorProcess() != null)
            {
                XtraMessageBox.Show("Another instance of the app is already running.", "ERPNext PowerPlay", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            ConfigureSerilog();
            ScriptPermissionManager.GlobalInstance = new ScriptPermissionManager(ExecutionMode.Unrestricted);
            ApplicationConfiguration.Initialize();

            // Check for updates once the message loop is idle
            Application.Idle += OnFirstIdle;

            Application.Run(new frmMain());
        }

        private static async void OnFirstIdle(object? sender, EventArgs e)
        {
            if (_updateChecked) return;
            _updateChecked = true;
            Application.Idle -= OnFirstIdle;
            await CheckForUpdatesAsync();
        }

        public static async Task CheckForUpdatesAsync()
        {
            try
            {
                var mgr = new UpdateManager("https://cecypo.tech/updates/");
                var newVersion = await mgr.CheckForUpdatesAsync();
                if (newVersion == null) return;

                var result = XtraMessageBox.Show(
                    $"Version {newVersion.TargetFullRelease.Version} is available.\n\nWould you like to update now?",
                    "Update Available",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                {
                    await mgr.DownloadUpdatesAsync(newVersion);
                    mgr.ApplyUpdatesAndRestart(newVersion);
                }
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Update check failed");
            }
        }

        public static Process? PriorProcess()
        // Returns a System.Diagnostics.Process pointing to
        // a pre-existing process with the same name as the
        // current one, if any; or null if the current process
        // is unique.
        {
            Process curr = Process.GetCurrentProcess();
            Process[] procs = Process.GetProcessesByName(curr.ProcessName);
            foreach (Process p in procs)
            {
                if ((p.Id != curr.Id) &&
                    (p.MainModule!.FileName == curr.MainModule!.FileName))
                    return p;
            }
            return null;
        }

        private static void ConfigureSerilog()
        {
            MyAppDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + Path.DirectorySeparatorChar + Application.ProductName;
            Log.Logger = new LoggerConfiguration()
                        .Enrich.FromLogContext()
                        .WriteToGridView()
                        .WriteTo.File(path: Program.MyAppDir + @"\\log.txt", rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true, retainedFileCountLimit: 90)
                        .CreateLogger();
        }
    }
}
