
# ERPNext PowerPlay

A windows based tool to **automate printing of ERPNext documents** based on custom filters.

Uses EFCore to create a SQLite database to store settings and print history.

Example use case: Company has 5 branches, with a central e-invoicing device, that is integrated to update a tax signature on all sales. Once a user submits a document, the e-invoicing signature is appended (this could take a few seconds), after which the user needs to click print preview > print > select a printer > print. Instead of this, the user only submits the document, and within a few seconds, the document is printed, in the correct warehouse, to the preset printer.

Example A5 and Thermal/Roll Paper Templates:
![A5](https://i.imgur.com/ukW9E4p.png)
![Roll Paper](https://i.imgur.com/SUkfuti.png)
## Features

- *4 Print Engines* (in case of compatibility issues with different printers, paper sizes, etc)
  - FrappePDF - Streams the PDF directly from ERPNext as you see on your ERP.
  - SumatraPDF - Uses SumatraPDF to print the streamed PDF.
  - Ghostscript - Uses Ghostscript to print the streamed PDF.
  - CustomTemplate - Uses DevExpress's Layout designer and print format (.repx files).
    - Easily create drag and drop professional print templates.
    - Create thermal receipts (for thermal printers, Kitchen Order Tickets (KOT), etc)
- **Reprinting**
  - Add a int field `custom_print_count` that will be incremented from this tool. Enable `Allow On Submit` and `No Copy`.
  - Tool saves print history, ensuring that the document does not get printed multiple times
- Create multiple jobs for the same doctype if you need multiple layouts (you could use one for the receipt and a modified one acting as a picking list)
- Use any print format (not limited to default)
- Windows only

## Custom Templates
 - The inbuilt Report Designer makes it easy to drag and drop and implement custom sorting or visibility rules!
 - Easily create a thermal receipt template (provided in the Layout-Samples folder) for Roll-Paper/thermal paper printer.
 - Easily implement Kitchen Order Tickets (KOT), picking slips, etc as you need.
 - HINT: Ensure your JSON DATA SOURCE is in the format `[ "data": { "name": "INV-007"...` as shown in the image below.
![Report Designer](https://i.imgur.com/dcXvEbn.png)
## Usage/Examples

**FOR CUSTOM TEMPLATES**
- Open an existing document > copy to clipboard
- Create (or edit the jsonDataSource) in the REPX file
- Paste your copied json data and save
- This will create your companies document structure in the designer

**SETTINGS FOR _SALES INVOICE_**
![Settings](https://i.imgur.com/mrsVrLS.png)
- You can add multiple settings for the same doctype, allowing you to print the same document in different formats
- `Copy Name` has no function currently.
- Field list
  - Sample: `["owner", "name", "customer", "posting_date", "docstatus", "status", "custom_print_count", "grand_total"]`
  - Must include name: `name`
  - Must include a date, any of: `date, posting_date, transaction_date`
  - Must include a title, any of:`customer, supplier, title`
  - Must include a total, any of: `grand_total, total`
  - Must include a print count: `custom_print_count`
  - Add `Company` if using multiple companies in a site
- Filter list
  - Sample: `["Sales Invoice","docstatus","=","1"], ["Sales Invoice","etr_invoice_number","!=",""], ["Sales Invoice","posting_date",">","2024-01-01"], ["Sales Invoice","total","!=","0"]`
    - Explaination of the filters;
     - "docstatus","=","1" - only submitted documents
     - "etr_invoice_number","!=","" - custom field (for Kenya integrations), etr_invoice_number is not empty
     - "posting_date",">","2024-01-01" - document only after this date
     - "total","!=","0" - where totals are not 0
  - Sample with User Filter: `[["Sales Invoice","docstatus","=","1"],["Sales Invoice","etr_invoice_number","!=",""],["Sales Invoice","posting_date",">","2024-09-25"],["Sales Invoice","total","!=","0"],["Sales Invoice", "owner", "IN", ["me@here.com", "you@here.com`"]]]`


  **SETTINGS FOR DOCTYPE REPORTS**
- The GRID system we use is just simply magic. Grouping, filtering, ordering, sorting and more. AND ability to SAVE your preferred view (of groups, ordering, etc)!
- Payment Entry Report Sample:
![Reports](https://i.imgur.com/ufHsaq9.png) (Config on the left, Report result on the right)
  - Field List: `["name", "owner", "posting_date", "status", "payment_type", "mode_of_payment", "reference_no", "paid_amount"]`
  - Filter List: `["Payment Entry", "posting_date", ">=", "2024/04/25"],["Payment Entry", "posting_date", "<=", "2025/04/25"]`


  **HOW TO LOAD REPORTS**
- Name: Payment Ledger Report
- DocType: <blank>
- End Point: api/method/frappe.desk.query_report.run
- Field List/Request Body: 
```
{
    "report_name": "Payment Ledger",
    "filters": {
        "company": "<companyname>","period_start_date": "2026-01-02","period_end_date": "2026-01-17","account": [],"party": []
    },
    "as_dict": 1
}
```

- Name: Sales Register
```
{
    "report_name": "Sales Register",
    "filters": {"from_date":"2025-12-01","to_date":"2026-01-24","company":"<company>","department":[]},
    "ignore_prepared_report": 1
}
```