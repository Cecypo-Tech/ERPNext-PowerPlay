using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPNext_PowerPlay.Models
{
    public enum DocType
    {
        [Description("Sales Invoice")]
        SalesInvoice = 1,
        [Description("Picking List")]
        PickingList = 2,
        [Description("Delivery Note")]
        DeliveryNote = 3,
        [Description("Purchase Order")]
        PurchaseOrder = 4,
        [Description("Purchase Receipt")]
        PurchaseReceipt = 5,
        [Description("Purchase Invoice")]
        PurchaseInvoice = 6,
        [Description("Stock Entry")]
        StockEntry = 7,
        [Description("Stock Reconciliation")]
        StockReconciliation = 8,
        [Description("Stock Adjustment")]
        StockAdjustment = 9,
        [Description("Material Request")]
        MaterialRequest = 10,
        [Description("Material Transfer")]
        MaterialTransfer = 11,
        [Description("Material Issue")]
        MaterialIssue = 12,
        [Description("Material Receipt")]
        MaterialReceipt = 13,
        [Description("Quotation")]
        Quotation = 14,
        [Description("Sales Order")]
        SalesOrder = 15,
        [Description("Payment Entry")]
        PaymentEntry = 16,
        [Description("Mpesa")]
        Mpesa = 17
    }
}
