using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPNext_PowerPlay.Models
{
    public class PrinterSetting : BaseEntity
    {
        public int ID { get; set; }

        public bool Enabled { get; set; }
        public DocType DocType { get; set; }
        public string? WarehouseFilter { get; set; }
        public string? UserFilter { get; set; }

        public PrintEngine PrintEngine { get; set; }
        public string? FieldList { get; set; }
        public string? FilterList { get; set; }
        public string? Printer { get; set; }
        public string? CopyName { get; set; }
        public int Copies { get; set; }
        public string? PageRange { get; set; }
        public int? FontSize { get; set; }
        public Orientation Orientation { get; set; }
        public Scaling Scaling { get; set; }
        public string? REPX_Template { get; set; }

        //Pure Frappe Doc Settings;
        public string? FrappeTemplateName { get; set; }
        public string? LetterHead { get; set; }
        public bool Compact { get; set; }
        public bool UOM { get; set; }

    }
    public enum PrintEngine
    {
        FrappePDF,
        SumatraPDF,
        Ghostscript,
        CustomTemplate
    }
    public enum Orientation
    {
        Auto,
        Portrait,
        Landscape
    }
    public enum Scaling
    {
        Fit,
        ActualSize,
        CustomFit
    }
}
