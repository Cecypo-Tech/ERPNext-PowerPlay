# Frappe Scripts for PowerPlay Socket.IO Integration

## Overview

These scripts enable the PowerPlay desktop app to receive real-time print events from ERPNext via Socket.IO.

---

## Client Script (Sales Order)

**Setup Location:** Setup > Customization > Client Script
**DocType:** Sales Order
**Enabled:** Yes

### Option 1: Using Server API Method

```javascript
frappe.ui.form.on('Sales Order', {
    refresh: function(frm) {
        // Add button under Actions dropdown for submitted documents
        if (!frm.is_new() && frm.doc.docstatus === 1) {
            frm.add_custom_button(__('Send to PowerPlay'), function() {
                frappe.call({
                    method: 'your_app.api.publish_powerplay_event',
                    args: {
                        doctype: 'Sales Order',
                        docname: frm.doc.name,
                        print_count: frm.doc.custom_print_count || 0
                    },
                    callback: function(response) {
                        if (response.message && response.message.success) {
                            frappe.show_alert({
                                message: __('Sent to PowerPlay for printing'),
                                indicator: 'green'
                            });
                        }
                    },
                    error: function() {
                        frappe.show_alert({
                            message: __('Failed to send to PowerPlay'),
                            indicator: 'red'
                        });
                    }
                });
            }, __('Actions'));
        }
    }
});
```

### Option 2: Direct Realtime Publish (No Server Script Needed)

```javascript
frappe.ui.form.on('Sales Order', {
    refresh: function(frm) {
        if (!frm.is_new() && frm.doc.docstatus === 1) {
            frm.add_custom_button(__('Send to PowerPlay'), function() {
                // Direct realtime publish via socket
                frappe.realtime.publish('powerplay_print_invoice', {
                    name: frm.doc.name,
                    doctype: 'Sales Order',
                    custom_print_count: frm.doc.custom_print_count || 0
                });

                frappe.show_alert({
                    message: __('Sent to PowerPlay'),
                    indicator: 'green'
                });
            }, __('Actions'));
        }
    }
});
```

### Option 3: Auto-Send on Submit (Server Script)

**Setup Location:** Setup > Customization > Server Script
**DocType Event:** Sales Order - After Submit

```python
# Automatically publish event when Sales Order is submitted
frappe.publish_realtime(
    event='powerplay_print_invoice',
    message={
        'name': doc.name,
        'doctype': 'Sales Order',
        'custom_print_count': doc.custom_print_count or 0
    },
    user=frappe.session.user
)
```

---

## Server Script Options

### Option A: Whitelisted API Method (Custom App)

Add to your custom app's `api.py`:

```python
import frappe

@frappe.whitelist()
def publish_powerplay_event(doctype, docname, print_count=0):
    """
    Publish a realtime event to PowerPlay desktop app for printing

    Args:
        doctype: The document type (e.g., 'Sales Order', 'Sales Invoice')
        docname: The document name/ID
        print_count: Current print count (to avoid reprinting)
    """
    frappe.publish_realtime(
        event='powerplay_print_invoice',
        message={
            'name': docname,
            'doctype': doctype,
            'custom_print_count': int(print_count)
        },
        user=frappe.session.user
    )
    return {'success': True, 'message': f'Event published for {docname}'}
```

### Option B: Server Script via Frappe UI

**Setup Location:** Setup > Customization > Server Script
**Script Type:** API
**API Method:** `publish_powerplay_event`
**Allow Guest:** No

```python
# Server Script body
doctype = frappe.form_dict.get('doctype')
docname = frappe.form_dict.get('docname')
print_count = frappe.form_dict.get('print_count', 0)

if not doctype or not docname:
    frappe.throw('Missing required parameters: doctype and docname')

frappe.publish_realtime(
    event='powerplay_print_invoice',
    message={
        'name': docname,
        'doctype': doctype,
        'custom_print_count': int(print_count)
    },
    user=frappe.session.user
)

frappe.response['message'] = {'success': True, 'docname': docname}
```

---

## Prerequisites

### 1. Custom Field (Required)

Add custom field to track print count:

| Field | Value |
|-------|-------|
| DocType | Sales Order (and/or Sales Invoice) |
| Field Name | `custom_print_count` |
| Field Type | Int |
| Default | 0 |

### 2. Socket.IO Configuration

Ensure Socket.IO is enabled in your Frappe site:

```bash
# Check site_config.json for socketio_port (default: 9000)
cat sites/your-site/site_config.json
```

### 3. Frappe Realtime Service

```bash
# Ensure socketio is running
bench --site your-site socketio
```

---

## Event Format

The PowerPlay desktop app expects events in this format:

```json
{
    "name": "SAL-ORD-2024-00001",
    "doctype": "Sales Order",
    "custom_print_count": 0
}
```

| Field | Type | Description |
|-------|------|-------------|
| `name` | string | Document name (required) |
| `doctype` | string | Document type |
| `custom_print_count` | int | Skip printing if > 0 |

---

## Troubleshooting

### Event Not Received

1. Check Socket.IO is running: `bench --site your-site socketio`
2. Verify port 9000 is accessible from desktop app
3. Check browser console for realtime connection status
4. Verify API token has correct permissions

### Button Not Appearing

1. Ensure document is submitted (`docstatus === 1`)
2. Clear browser cache and reload
3. Check Client Script is enabled
4. Verify no JavaScript errors in browser console

---

## Desktop App Socket.IO Debug Guide

### Common Connection Issues

The PowerPlay desktop app uses `SocketIOClient` (v3.1.2) to connect to Frappe's Socket.IO server.

#### 1. Port Configuration
- Default Frappe Socket.IO port: **9000**
- Check `site_config.json` for custom `socketio_port`
- Ensure firewall allows connections to this port

#### 2. Authentication Methods
The app attempts two authentication methods:
- **Cookie-based**: SID cookie from `/api/method/frappe.auth.get_logged_user`
- **API Token**: `Authorization: token {api_key}:{api_secret}`

#### 3. Log Messages to Watch

**Successful connection:**
```
Socket.IO CONNECTED successfully to server
Socket ID: {socket_id}
Subscribed to Sales Invoice doctype events
```

**Authentication issues:**
```
Could not obtain session cookie for Socket.IO auth
No SID cookie found in response
```

**Connection failures:**
```
Socket.IO ERROR: {error_details}
Socket.IO DISCONNECTED: {reason}
```

### Debug Steps

1. **Check logs** in the PowerPlay app for Socket.IO messages
2. **Verify Frappe Socket.IO is running**:
   ```bash
   bench --site your-site socketio
   # or check supervisor/systemd status
   ```
3. **Test connectivity** to port 9000 from the desktop machine
4. **Check API token permissions** - ensure it has read access to Sales Invoice

### Event Flow

1. **Client Script** calls `frappe.publish_realtime('powerplay_print_invoice', {...})`
2. **Frappe** sends event via Redis to Socket.IO server
3. **Socket.IO server** broadcasts to subscribed clients
4. **Desktop app** receives via:
   - Direct event: `powerplay_print_invoice`
   - Wrapped event: `publish` containing the event name and message

### Testing Events from Frappe Console

```python
# In bench console: bench --site your-site console
import frappe

# Test publishing an event
frappe.publish_realtime(
    event='powerplay_print_invoice',
    message={
        'name': 'SAL-ORD-2024-00001',
        'doctype': 'Sales Order',
        'custom_print_count': 0
    },
    user='Administrator'  # or specific user
)
```

### Frappe Socket.IO Architecture

```
+------------------+     Redis      +-------------------+
|  Frappe Web      | ------------> |  Socket.IO Server |
|  (Python/Gunicorn)|   pub/sub    |  (Node.js :9000)  |
+------------------+               +-------------------+
                                           |
                                           | WebSocket
                                           v
                                   +-------------------+
                                   |  PowerPlay App    |
                                   |  (C# Desktop)     |
                                   +-------------------+
```

### Room Types in Frappe

| Room | Description | Subscription |
|------|-------------|--------------|
| `all` | All connected users | Auto |
| `website` | Website visitors | Auto |
| `user:{email}` | Specific user | `frappe.subscribe` |
| `doctype:{doctype}` | DocType changes | `doctype_subscribe` |
| `doc:{doctype}/{name}` | Specific document | `doc_subscribe` |
