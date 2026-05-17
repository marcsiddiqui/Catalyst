# Phoenix nopCommerce admin integration

This folder is a safe integration layer for Phoenix-inspired admin styling.

Phoenix v1.24.0 is built on Bootstrap 5.3.8, while nopCommerce 4.90.3 admin uses AdminLTE 3 with Bootstrap 4.6. Loading Phoenix's full Bootstrap/CSS/JS bundle globally would risk conflicts with nopCommerce admin dropdowns, modals, DataTables, validation, and plugin pages.

For Phase 1 and Phase 2, only scoped CSS tokens are placed here. The visual overrides live in `wwwroot/css/admin/phoenix-nop-overrides.css` and are activated by the `phoenix-nop-admin` body class.
