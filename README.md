
# ERPNext PowerPlay

A windows based tool to **automate printing of ERPNext documents** based on custom filters.

Uses EFCore to create a SQLite database to store settings and print history.

Example use case: Company has 5 branches, with a central e-invoicing device, that is integrated to update a tax signature on all sales. Once a user submits a document, the e-invoicing signature is appended (this could take a few seconds), after which the user needs to click print preview > print > select a printer > print. Instead of this, the user only submits the document, and within a few seconds, the document is printed, in the correct warehouse, to the preset printer.
## Features

- *4 Print Engines* (in case of compatibility issues with different printers, paper sizes, etc)
  - FrappePDF - Streams the PDF directly from ERPNext as you see on your ERP.
  - SumatraPDF - Uses SumatraPDF to print the streamed PDF.
  - Ghostscript - Uses Ghostscript to print the streamed PDF.
  - CustomTemplate - Uses DevExpress's Layout designer and print format (.repx files).
    - Easily create drag and drop professional print templates.
    - Create thermal receipts (for thermal printers, Kitchen Order Tickets (KOT), etc)
- **Reprinting**
  - Add a int field `custom_print_count` that will be incremented from this tool
  - Tool saves print history, ensuring that the document does not get printed multiple times
- Use any print format (not limited to default)
- Windows only

## Custom Templates
 - The inbuilt Report Designer makes it easy to drag and drop and implement custom sorting or visibility rules!
 - Easily create a thermal receipt template (provided in the Layout-Samples folder) for Roll-Paper/thermal paper printer.
 - Easily implement Kitchen Order Tickets (KOT), picking slips, etc as you need.
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
- Filter list
  - Sample: `["Sales Invoice","docstatus","=","1"], ["Sales Invoice","etr_invoice_number","!=",""], ["Sales Invoice","posting_date",">","2024-01-01"], ["Sales Invoice","total","!=","0"]`
    - Explaination of the filters;
     - "docstatus","=","1" - only submitted documents
     - "etr_invoice_number","!=","" - custom field (for Kenya integrations), etr_invoice_number is not empty
     - "posting_date",">","2024-01-01" - document only after this date
     - "total","!=","0" - where totals are not 0
  - Sample with User Filter: `[["Sales Invoice","docstatus","=","1"],["Sales Invoice","etr_invoice_number","!=",""],["Sales Invoice","posting_date",">","2024-09-25"],["Sales Invoice","total","!=","0"],["Sales Invoice", "owner", "IN", ["me@here.com", "you@here.com`"]]]` 