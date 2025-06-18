using DevExpress.Diagram.Core.Native;
using DevExpress.XtraEditors;
using DevExpress.XtraReports.Security;
using DevExpress.XtraWaitForm;
using Serilog;
using Serilog.Sinks.WinForms.Base;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows.Documents;

namespace ERPNext_PowerPlay
{
    internal static class Program
    {
     
        public static string FrappeURL = "";
        public static string FrappeUser = "";
        public static string MyAppDir = "";
        public static CookieContainer Cookies = new CookieContainer();
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (PriorProcess() != null)
            {
                XtraMessageBox.Show("Another instance of the app is already running.", "ERPNext PowerPlay", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ConfigureSerilog();
            ScriptPermissionManager.GlobalInstance = new ScriptPermissionManager(ExecutionMode.Unrestricted);
            ApplicationConfiguration.Initialize();
            Application.Run(new frmMain());


        }
        public static Process PriorProcess()
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
                    (p.MainModule.FileName == curr.MainModule.FileName))
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