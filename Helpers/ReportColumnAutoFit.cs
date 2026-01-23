using DevExpress.XtraReports.UI;
using System;
using System.Drawing;

namespace ERPNext_PowerPlay.Helpers
{
    /// <summary>
    /// Ensures all report content fits within the printable page width.
    /// Handles tables, controls, and band widths.
    /// </summary>
    public class ReportColumnAutoFit
    {
        private float _pageWidth;

        /// <summary>
        /// Gets the effective printable page width based on paper size and orientation.
        /// </summary>
        private float GetEffectivePageWidth(XtraReport report)
        {
            // In XtraReports:
            // - PageWidth/PageHeight are the paper dimensions in portrait orientation
            // - When Landscape=true, the paper is rotated, so effective width = PageHeight
            // - Margins.Left/Right are always applied to content sides
            float effectivePageDimension = report.Landscape ? report.PageHeight : report.PageWidth;
            return effectivePageDimension - report.Margins.Left - report.Margins.Right;
        }

        /// <summary>
        /// Applies constraints to ensure all content fits within the printable page width.
        /// </summary>
        public XtraReport ApplyAutoFitColumnWidths(XtraReport report)
        {
            // Get effective page width accounting for orientation
            _pageWidth = GetEffectivePageWidth(report);

            // Process all bands
            foreach (Band band in report.Bands)
            {
                ProcessBand(band);
            }

            return report;
        }

        private void ProcessBand(Band band)
        {
            // Process all controls in this band
            foreach (XRControl control in band.Controls)
            {
                ConstrainControlToPageWidth(control);
            }

            // Process sub-bands
            foreach (Band subBand in band.SubBands)
            {
                ProcessBand(subBand);
            }

            // Process detail report bands (for master-detail reports)
            if (band is DetailReportBand detailReport)
            {
                foreach (Band innerBand in detailReport.Bands)
                {
                    ProcessBand(innerBand);
                }
            }
        }

        /// <summary>
        /// Constrains any control to fit within page width.
        /// </summary>
        private void ConstrainControlToPageWidth(XRControl control)
        {
            // Handle tables specially - scale columns proportionally
            if (control is XRTable table)
            {
                ConstrainTableToPageWidth(table);
                return;
            }

            // Handle subreports - process their contents
            if (control is XRSubreport subreport && subreport.ReportSource != null)
            {
                var subAutoFit = new ReportColumnAutoFit();
                subAutoFit.ApplyAutoFitColumnWidths(subreport.ReportSource);
                return;
            }

            // Handle containers (panels, etc.) - process child controls
            if (control.Controls.Count > 0)
            {
                foreach (XRControl child in control.Controls)
                {
                    ConstrainControlToPageWidth(child);
                }
            }

            // For all controls: if control extends beyond page width, constrain it
            float controlRight = control.LocationF.X + control.WidthF;
            if (controlRight > _pageWidth)
            {
                // If control starts within page but extends beyond, shrink width
                if (control.LocationF.X < _pageWidth)
                {
                    control.WidthF = _pageWidth - control.LocationF.X;
                }
                else
                {
                    // Control starts beyond page - this shouldn't happen normally
                    // but handle it by moving and shrinking
                    control.LocationF = new PointF(0, control.LocationF.Y);
                    control.WidthF = Math.Min(control.WidthF, _pageWidth);
                }
            }
        }

        /// <summary>
        /// Constrains a table and all its columns to fit within the page width.
        /// </summary>
        private void ConstrainTableToPageWidth(XRTable table)
        {
            if (table.Rows.Count == 0) return;

            // Calculate actual table width from cells (more accurate than WidthF)
            float actualTableWidth = 0;
            if (table.Rows.Count > 0)
            {
                foreach (XRTableCell cell in table.Rows[0].Cells)
                {
                    actualTableWidth += cell.WidthF;
                }
            }

            // Use the larger of actual width or reported width
            float currentTableWidth = Math.Max(actualTableWidth, table.WidthF);

            // Account for table's X position
            float tableRight = table.LocationF.X + currentTableWidth;
            float availableWidth = _pageWidth - table.LocationF.X;

            // If table fits within available space, no scaling needed
            if (tableRight <= _pageWidth)
                return;

            // Calculate scale factor to fit within available width
            float scaleFactor = availableWidth / currentTableWidth;

            // Apply scale factor to all cells in all rows
            foreach (XRTableRow row in table.Rows)
            {
                foreach (XRTableCell cell in row.Cells)
                {
                    cell.WidthF = cell.WidthF * scaleFactor;
                }
            }

            // Set table width to fit exactly within available space
            table.WidthF = availableWidth;
        }
    }
}
