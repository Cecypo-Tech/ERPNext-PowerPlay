using DevExpress.DataAccess.Native.Web;
using DevExpress.DocumentServices.ServiceModel.DataContracts;
using DevExpress.Pdf;
using DevExpress.XtraReports.Templates;
using DevExpress.XtraRichEdit.Import.EPub;
using ERPNext_PowerPlay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;


namespace ERPNext_PowerPlay.Helpers
{
    public class PrintActions
    {
        public async Task<bool> Frappe_GetDoc(string DocName, PrinterSetting PS)
        {
            try
            {
                string frappe_printfilter = "doctype=Sales Invoice&name=_docname_&format=_printformat_&no_letterhead=_noletterhead_&letterhead=_letterheadnname_&settings={\"compact_item_print\":_compact_,\"print_uom_after_quantity\":_uom_}";
                frappe_printfilter = frappe_printfilter.Replace("_printformat_", PS.FrappeTemplateName);
                frappe_printfilter = frappe_printfilter.Replace("_letterheadnname_", PS.LetterHead);
                if (String.IsNullOrEmpty(PS.LetterHead))
                    frappe_printfilter = frappe_printfilter.Replace("_noletterhead_", "0");
                else
                    frappe_printfilter = frappe_printfilter.Replace("_noletterhead_", "1");
                if (PS.Compact)
                    frappe_printfilter = frappe_printfilter.Replace("_compact_", "1");
                else
                    frappe_printfilter = frappe_printfilter.Replace("_compact_", "0");
                if (PS.UOM)
                    frappe_printfilter = frappe_printfilter.Replace("_uom_", "1");
                else
                    frappe_printfilter = frappe_printfilter.Replace("_uom_", "0");

                frappe_printfilter = frappe_printfilter.Replace("_docname_", DocName);

                string filepart = Path.GetRandomFileName() + "_" + DocName + ".pdf";
                string filename = Path.Combine(Path.GetTempPath(), filepart);

                FrappeAPI fapi = new FrappeAPI();
                HttpResponseMessage response_qr = await fapi.GetAsReponse("/api/method/frappe.utils.print_format.download_pdf", frappe_printfilter);
                response_qr.EnsureSuccessStatusCode();
                byte[] b = await response_qr.Content.ReadAsByteArrayAsync();
                PrintDoc(DocName, filename, b);
                //string m = System.Text.Encoding.UTF8.GetString(b);
                //using (MemoryStream ms = new MemoryStream(b))
                //{
                //    //Write a temp PDF file?
                //    // File.WriteAllBytes(filename, ms.ToArray());

                //    Log.Information(string.Format("[ERP] {0}: Fetch PDF for DocID {1} successful!", DocName, filepart));
                //    PrintDoc(DocName, filename, ms);
                //}


                return true;
            }
            catch (Exception exSQL)
            {
                Log.Error(exSQL, exSQL.Message);
                return false;
            }
        }


        public void PrintDoc(string DocName, string filename, byte[] b)
        {
            try
            {
                AppDbContext db = new AppDbContext();
                db.PrinterSetting.Load();
                List<PrinterSetting> ps = db.PrinterSetting.ToList();

                foreach (PrinterSetting printrow in db.PrinterSetting)
                {
                    if (PrinterExists(printrow.Printer))
                    {
                        switch (printrow.PrintEngine)
                        {
                            case PrintEngine.DX:
                                PrintDX(DocName, b, printrow);
                                break;
                            //case PrintEngine.SM:
                            //    PrintSumatra(tmpFile, doc, t2, cp);
                            //    break;
                            //case PrintEngine.GS:
                            //    p.PrintGhostScript(tmpFile, doc, t2, cp);
                            //    break;
                            //case PrintEngine.REPX:
                            //    p.PrintREPX(tmpFile, doc, t2, cp);
                            //    break;
                            default:
                                PrintDX(DocName, b, printrow);
                                break;
                        }

                    }
                    else
                    {
                        Log.Error("Printer not found: " + printrow.Printer.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in PrintDoc with DocID: " + DocName);
            }

        }

        //public async Task<ParsedDocumentPageList> ParsePDFDocument(byte[] fileBytes)
        //{
        //    var parsedPages = new ParsedDocumentPageList();
        //    await Task.Run(() =>
        //    {
        //        // fileBytes is the byte array of the PDF document
        //        using (var memoryStream = new MemoryStream(fileBytes))
        //        {
        //            using (PdfDocumentProcessor source = new PdfDocumentProcessor())
        //            {
        //                source.LoadDocument(memoryStream);
        //            }
        //        }
        //    });
        //}

        public static bool PrinterExists(string printerName)
        {
            if (String.IsNullOrEmpty(printerName)) { return false; } //throw new ArgumentNullException("printerName"); }
            return System.Drawing.Printing.PrinterSettings.InstalledPrinters.Cast<string>().Any(name => printerName.ToUpper().Trim() == name.ToUpper().Trim());
        }

        public void PrintDX(string DocName, byte[] b, PrinterSetting copyData)
        {
            try
            {
                // Create a PDF Document Processor instance and load a PDF into it.
                using (var memoryStream = new MemoryStream(b))
                {
                    using (PdfDocumentProcessor documentProcessor = new PdfDocumentProcessor())
                    {
                        documentProcessor.LoadDocument(memoryStream);

                        //  //  Embed a "CopyName" on the PDF
                        //if (copyData.CopyName != null)
                        //    if (copyData.CopyName.Length > 0 && copyData.FontSize > 0)
                        //    { //For DX, run without creating multiple temp files!
                        //        using (SolidBrush textBrush = new SolidBrush(Color.Black))
                        //            new SharedClasses().AddGraphics(documentProcessor, copyData.CopyName, textBrush, copyData.FontSize, copyData.Alignment);
                        //    }

                        // Declare the PDF printer settings.
                        PdfPrinterSettings settings = new PdfPrinterSettings();
                        settings.Settings.PrinterName = copyData.Printer;
                        //settings.ScaleMode = PdfPrintScaleMode.ActualSize;
                        settings.Scale = 100;
                        settings.Settings.DefaultPageSettings.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);

                        settings.Settings.Copies = (short)copyData.Copies;
                        int[] PrintPages = GetPageRange(copyData);
                        if (PrintPages != null) settings.PageNumbers = PrintPages;

                        //settings.PrintingDpi = 300;
                        switch (copyData.Orientation)
                        {
                            case Models.Orientation.Auto:
                                settings.PageOrientation = PdfPrintPageOrientation.Auto;
                                break;
                            case Models.Orientation.Portrait:
                                settings.PageOrientation = PdfPrintPageOrientation.Portrait;
                                break;
                            case Models.Orientation.Landscape:
                                settings.PageOrientation = PdfPrintPageOrientation.Landscape;
                                break;
                            default:
                                settings.PageOrientation = PdfPrintPageOrientation.Auto;
                                break;
                        }
                        switch (copyData.Scaling)
                        {
                            case Scaling.Fit:
                                settings.ScaleMode = PdfPrintScaleMode.Fit;
                                break;
                            case Scaling.ActualSize:
                                settings.ScaleMode = PdfPrintScaleMode.ActualSize;
                                break;
                            case Scaling.CustomFit:
                                settings.ScaleMode = PdfPrintScaleMode.CustomScale;
                                break;
                            default:
                                settings.ScaleMode = PdfPrintScaleMode.Fit;
                                break;
                        }

                        if (PrinterExists(copyData.Printer))
                        {
                            Log.Debug(String.Format("Extra Log Start: Printing {0} to {1}", DocName, copyData.Printer));

                            // Specify the page numbers to be printed.
                            //settings.PageNumbers = new int[] { 1, 2, 3, 4, 5, 6 };

                            // Handle the PrintPage event to specify print output.
                            documentProcessor.PrintPage += OnPrintPage;

                            // Handle the QueryPageSettings event to customize settings for a page to be printed.
                            documentProcessor.QueryPageSettings += OnQueryPageSettings;

                            // Print the document using the specified printer settings.
                            documentProcessor.Print(settings);

                            // Unsubscribe from PrintPage and QueryPageSettings events. 
                            documentProcessor.PrintPage -= OnPrintPage;
                            documentProcessor.QueryPageSettings -= OnQueryPageSettings;
                            Log.Debug(String.Format("Extra Log End: Printing {0} to {1}", DocName, copyData.Printer));
                        }
                    }
                }
                Log.Information("[Printed] {0} => {1}", copyData.PrintEngine.ToString(), DocName);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "[PrintError] {0}", ex.Message);
                if (ex.InnerException != null) Log.Error(ex, "[PrintError] {0}", ex.InnerException.Message);
            }
        }

        private static void OnQueryPageSettings(object sender, PdfQueryPageSettingsEventArgs e)
        {
            // Print the second page in landscape size.
            //if (e.PageNumber == 2)
            //    e.PageSettings.Landscape = true;
            //else e.PageSettings.Landscape = false;
        }

        // Specify what happens when the PrintPage event is raised.
        private static void OnPrintPage(object sender, PdfPrintPageEventArgs e)
        {
            // Draw a picture on each printed page.        
            //using (Bitmap image = new Bitmap(@"..\..\DevExpress.png"))
            //e.Graphics.DrawImage(image, new RectangleF(10, 30, image.Width / 2, image.Height / 2));
        }

        public int GetCopyAlignmentFromString(string str)
        {
            switch (str)
            {
                case "LEFT":
                    return 0;
                case "CENTER":
                    return 1;
                case "RIGHT":
                    return 2;
                case "JUSTIFIED":
                    return 3;
                case "TOP":
                    return 4;
                case "MIDDLE":
                    return 5;
                case "BOTTOM":
                    return 6;
                case "BASELINE":
                    return 7;
                case "JUSTIFIED_ALL":
                    return 8;
                default:
                    return 0;
            }
        }


        public int[] GetPageRange(PrinterSetting cp)
        {
            try
            {
                List<string> PrintPages = new List<string>();
                int[] PrintPagesINT = null;

                if (cp.PageRange != null)
                {
                    if (cp.PageRange.Length > 0)
                    {
                        char[] delimiterChars = { ',' };
                        PrintPages = cp.PageRange.Split(delimiterChars).ToList();
                    }
                }
                if (PrintPages.Count > 0)
                {
                    foreach (string s in PrintPages)
                        if (IsNumeric(s) == false)
                        {
                            PrintPages.Clear();
                            break;
                        }
                    PrintPagesINT = Array.ConvertAll(PrintPages.ToArray(), s => int.Parse(s));
                }
                if (PrintPagesINT == null)
                    return PrintPagesINT;
                if (PrintPagesINT.Length > 0)
                    return PrintPagesINT;

                return null;

            }
            catch (Exception exPrintPages)
            {
                Log.Error(exPrintPages, exPrintPages.Message);
                return null;
            }
        }
        public bool IsNumeric(string value)
        {
            return value.All(char.IsNumber);
        }
    }
}