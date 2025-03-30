//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Security.Permissions;
//using System.Text;
//using System.Threading.Tasks;

//namespace ERPNext_PowerPlay.Models
//{
//    public class Frappe_Doc
//    {
//        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
//        public class Data
//        {
//            public string name { get; set; }
//            public string owner { get; set; }
//            public string creation { get; set; }
//            public string modified { get; set; }
//            public string modified_by { get; set; }
//            public int docstatus { get; set; }
//            public int idx { get; set; }
//            public string title { get; set; }
//            public string naming_series { get; set; }
//            public string customer { get; set; }
//            public string customer_name { get; set; }
//            public string tax_id { get; set; }
//            public string custom_cash_tax_id { get; set; }
//            public string taxes_and_charges { get; set; }
//            public string company { get; set; }
//            public string posting_date { get; set; }
//            public string posting_time { get; set; }
//            public string due_date { get; set; }
//            public string return_against { get; set; }
//            public int is_pos { get; set; }
//            public int posa_is_printed { get; set; }
//            public int is_consolidated { get; set; }
//            public int is_return { get; set; }
//            public int update_outstanding_for_self { get; set; }
//            public int update_billed_amount_in_sales_order { get; set; }
//            public int update_billed_amount_in_delivery_note { get; set; }
//            public int is_debit_note { get; set; }
//            public string currency { get; set; }
//            public double conversion_rate { get; set; }
//            public string selling_price_list { get; set; }
//            public string price_list_currency { get; set; }
//            public double plc_conversion_rate { get; set; }
//            public int ignore_pricing_rule { get; set; }
//            public int update_stock { get; set; }
//            public double total_qty { get; set; }
//            public double total_net_weight { get; set; }
//            public double base_total { get; set; }
//            public double base_net_total { get; set; }
//            public double total { get; set; }
//            public double net_total { get; set; }
//            public int exempt_from_sales_tax { get; set; }
//            public string tax_category { get; set; }
//            public double posa_delivery_charges_rate { get; set; }
//            public double base_total_taxes_and_charges { get; set; }
//            public double total_taxes_and_charges { get; set; }
//            public double base_grand_total { get; set; }
//            public double base_rounding_adjustment { get; set; }
//            public double base_rounded_total { get; set; }
//            public string base_in_words { get; set; }
//            public double grand_total { get; set; }
//            public double rounding_adjustment { get; set; }
//            public int use_company_roundoff_cost_center { get; set; }
//            public double rounded_total { get; set; }
//            public string in_words { get; set; }
//            public double total_advance { get; set; }
//            public double outstanding_amount { get; set; }
//            public int disable_rounded_total { get; set; }
//            public string apply_discount_on { get; set; }
//            public double base_discount_amount { get; set; }
//            public int is_cash_or_non_trade_discount { get; set; }
//            public double additional_discount_percentage { get; set; }
//            public double discount_amount { get; set; }
//            public string other_charges_calculation { get; set; }
//            public double total_billing_hours { get; set; }
//            public double total_billing_amount { get; set; }
//            public double base_paid_amount { get; set; }
//            public double paid_amount { get; set; }
//            public double base_change_amount { get; set; }
//            public double change_amount { get; set; }
//            public int allocate_advances_automatically { get; set; }
//            public int only_include_allocated_payments { get; set; }
//            public double write_off_amount { get; set; }
//            public double base_write_off_amount { get; set; }
//            public int write_off_outstanding_amount_automatically { get; set; }
//            public int redeem_loyalty_points { get; set; }
//            public int loyalty_points { get; set; }
//            public double loyalty_amount { get; set; }
//            public string loyalty_program { get; set; }
//            public string customer_address { get; set; }
//            public string address_display { get; set; }
//            public string contact_person { get; set; }
//            public string contact_display { get; set; }
//            public string contact_mobile { get; set; }
//            public string contact_email { get; set; }
//            public string territory { get; set; }
//            public string shipping_address_name { get; set; }
//            public string shipping_address { get; set; }
//            public string company_address { get; set; }
//            public string company_address_display { get; set; }
//            public int ignore_default_payment_terms_template { get; set; }
//            public string payment_terms_template { get; set; }
//            public string po_no { get; set; }
//            public string debit_to { get; set; }
//            public string party_account_currency { get; set; }
//            public string is_opening { get; set; }
//            public string against_income_account { get; set; }
//            public double amount_eligible_for_commission { get; set; }
//            public double commission_rate { get; set; }
//            public double total_commission { get; set; }
//            public string letter_head { get; set; }
//            public int group_same_items { get; set; }
//            public string language { get; set; }
//            public string status { get; set; }
//            public string customer_group { get; set; }
//            public int is_internal_customer { get; set; }
//            public int is_discounted { get; set; }
//            public string remarks { get; set; }
//            public string doctype { get; set; }
//            public List<object> payments { get; set; }
//            public List<object> pricing_rules { get; set; }
//            public List<object> packed_items { get; set; }
//            public List<PaymentSchedule> payment_schedule { get; set; }
//            public List<object> posa_coupons { get; set; }
//            public List<object> sales_team { get; set; }
//            public List<Item> items { get; set; }
//            public List<Taxis> taxes { get; set; }
//            public List<object> posa_offers { get; set; }
//            public List<object> advances { get; set; }
//            public List<object> timesheets { get; set; }

//            //CUSTOM
//            public int custom_print_count { get; set; }
//        }

//        public class Item
//        {
//            public string name { get; set; }
//            public string owner { get; set; }
//            public string creation { get; set; }
//            public string modified { get; set; }
//            public string modified_by { get; set; }
//            public int docstatus { get; set; }
//            public int idx { get; set; }
//            public int has_item_scanned { get; set; }
//            public string item_code { get; set; }
//            public string item_name { get; set; }
//            public string description { get; set; }
//            public string item_group { get; set; }
//            public string image { get; set; }
//            public double qty { get; set; }
//            public string stock_uom { get; set; }
//            public string uom { get; set; }
//            public double conversion_factor { get; set; }
//            public double stock_qty { get; set; }
//            public double price_list_rate { get; set; }
//            public double base_price_list_rate { get; set; }
//            public string margin_type { get; set; }
//            public double margin_rate_or_amount { get; set; }
//            public double rate_with_margin { get; set; }
//            public double discount_percentage { get; set; }
//            public double discount_amount { get; set; }
//            public double base_rate_with_margin { get; set; }
//            public double rate { get; set; }
//            public double amount { get; set; }
//            public string item_tax_template { get; set; }
//            public double base_rate { get; set; }
//            public double base_amount { get; set; }
//            public int posa_offer_applied { get; set; }
//            public int posa_is_offer { get; set; }
//            public double stock_uom_rate { get; set; }
//            public int is_free_item { get; set; }
//            public int grant_commission { get; set; }
//            public double net_rate { get; set; }
//            public double net_amount { get; set; }
//            public double base_net_rate { get; set; }
//            public double base_net_amount { get; set; }
//            public int delivered_by_supplier { get; set; }
//            public string income_account { get; set; }
//            public int is_fixed_asset { get; set; }
//            public string expense_account { get; set; }
//            public int enable_deferred_revenue { get; set; }
//            public double weight_per_unit { get; set; }
//            public double total_weight { get; set; }
//            public string warehouse { get; set; }
//            public int use_serial_batch_fields { get; set; }
//            public int allow_zero_valuation_rate { get; set; }
//            public double incoming_rate { get; set; }
//            public string item_tax_rate { get; set; }
//            public double actual_batch_qty { get; set; }
//            public double actual_qty { get; set; }
//            public string sales_order { get; set; }
//            public string so_detail { get; set; }
//            public double delivered_qty { get; set; }
//            public string cost_center { get; set; }
//            public int page_break { get; set; }
//            public string parent { get; set; }
//            public string parentfield { get; set; }
//            public string parenttype { get; set; }
//            public string doctype { get; set; }
//            public List<Taxis> taxes { get; set; }
//        }

//        public class PaymentSchedule
//        {
//            public string name { get; set; }
//            public string owner { get; set; }
//            public string creation { get; set; }
//            public string modified { get; set; }
//            public string modified_by { get; set; }
//            public int docstatus { get; set; }
//            public int idx { get; set; }
//            public string payment_term { get; set; }
//            public string due_date { get; set; }
//            public string mode_of_payment { get; set; }
//            public double invoice_portion { get; set; }
//            public string discount_type { get; set; }
//            public string discount_date { get; set; }
//            public double discount { get; set; }
//            public double payment_amount { get; set; }
//            public double outstanding { get; set; }
//            public double paid_amount { get; set; }
//            public double discounted_amount { get; set; }
//            public double base_payment_amount { get; set; }
//            public string parent { get; set; }
//            public string parentfield { get; set; }
//            public string parenttype { get; set; }
//            public string doctype { get; set; }
//        }

//        public class Root
//        {
//            public Data data { get; set; }
//        }

//        public class Taxis
//        {
//            public string name { get; set; }
//            public string owner { get; set; }
//            public string creation { get; set; }
//            public string modified { get; set; }
//            public string modified_by { get; set; }
//            public int docstatus { get; set; }
//            public int idx { get; set; }
//            public string charge_type { get; set; }
//            public string account_head { get; set; }
//            public string description { get; set; }
//            public int included_in_print_rate { get; set; }
//            public int included_in_paid_amount { get; set; }
//            public string cost_center { get; set; }
//            public double rate { get; set; }
//            public string account_currency { get; set; }
//            public double tax_amount { get; set; }
//            public double total { get; set; }
//            public double tax_amount_after_discount_amount { get; set; }
//            public double base_tax_amount { get; set; }
//            public double base_total { get; set; }
//            public double base_tax_amount_after_discount_amount { get; set; }
//            public string item_wise_tax_detail { get; set; }
//            public int dont_recompute_tax { get; set; }
//            public string parent { get; set; }
//            public string parentfield { get; set; }
//            public string parenttype { get; set; }
//            public string doctype { get; set; }

//            //Only for items;
//            public string item_tax_template { get; set; }
//            public string tims_hscode { get; set; }//Kenya/Navari

//        }



//    }
//}
