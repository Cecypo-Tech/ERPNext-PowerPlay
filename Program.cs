using DevExpress.Diagram.Core.Native;
using Serilog;
using Serilog.Sinks.WinForms.Base;
using System.IO;
using System.Net;
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
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ConfigureSerilog();
            ApplicationConfiguration.Initialize();
            Application.Run(new frmMain());

        }
        private static void ConfigureSerilog()
        {
            MyAppDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + Path.DirectorySeparatorChar + Application.ProductName ;
            Log.Logger = new LoggerConfiguration()
                        .Enrich.FromLogContext()
                        .WriteToSimpleAndRichTextBox()
                        .WriteToGridView()
                        .WriteTo.File(path: Program.MyAppDir + @"\\log.txt", rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true, retainedFileCountLimit: 90)
                        .CreateLogger();
        }
    }
}