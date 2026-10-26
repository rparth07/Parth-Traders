# Parth Traders recovery

## Active implementation and existing hosting
- Repository: https://github.com/rparth07/Parth-Traders
- Frontend: Angular 17 in `Frontend/parth-traders`; build with Node 20 and `npm ci && npm run build`.
- Publish directory: `Frontend/parth-traders/dist/parth-traders`.
- Existing Netlify site: https://rparth07-parth-traders.netlify.app (ID `1f645678-04cb-47bb-af93-7902a730bbd9`). Its published deployment was a manual upload, not a Git build. The live logo and favicon match this repository byte-for-byte.
- Backend: ASP.NET Core / EF Core, upgraded from .NET 6 / SQL Server to .NET 10 / PostgreSQL for Render and Neon.
- Neon project: `parth-traders` / `broad-heart-17023638`, Singapore region, with `production` and isolated `migration-validation` branches.
- The old `publish` directory contains obsolete backend output and is not the frontend or new deployment source.

## Data recovery
The user approved importing only the existing `product-table-data.sql`: 50 products. `Backend/Parth Traders/Recovery/products-source.json` is a faithful materialization, including decimal prices, discounts, stock, ratings, SKUs, and source relationship IDs. All 150 referenced product images are present.

The SQL file has no supplier/category records, customer accounts, or order history. Missing relationships remain null; original IDs are preserved as SourceSupplierId/SourceCategoryId. No sample CSVs or generated business data are imported. Public product cards identify unavailable category details. Stock/price edits preserve source IDs and unresolved relationships.

The importer runs only with `--migrate --import-products`, uses a transaction, and skips existing SKUs. Repeating it added zero duplicates. It does not overwrite existing product data.

## Private configuration
`.netlify/` is ignored. The original backend files and the user's existing uncommitted files were snapshotted there before migration. Never publish that directory.

Server environment names:
- `DATABASE_URL`: production PostgreSQL URL (TLS certificate verification enabled).
- `JwtSettings__signingKey`: generated private signing key, at least 32 characters.
- `AllowedOrigin`: exact frontend origin; local validation adds explicit localhost origins.
- `PORT`: Render HTTP port, bound on 0.0.0.0.
- `PayPal__ClientId` and `PayPal__ClientSecret`: sandbox merchant credentials.

The manual migration additionally uses `MIGRATION_DATABASE_URL` (direct connection) and the generated `BootstrapAdmin__userName` / `BootstrapAdmin__password` from the ignored private settings file. The migration creates only the technical administrator when absent. Runtime does not automatically migrate or import products.

Old SQL Server credentials were removed from tracked appsettings; old signing material was removed from active code. Their historical exposure still requires rotation/revocation in any surviving old hosting account. Do not rewrite Git history or print their values.

Only the public API URL belongs in `src/assets/runtime-config.js`. No private configuration is built into the frontend. The preview overrides that response in memory without changing production assets.

## Migrations and validation
SQL Server migrations are preserved and excluded from the PostgreSQL build. PostgreSQL migrations and a distinctly named `PostgreSqlContextSnapshot` live under `Backend/Parth Traders.Data/PostgreSqlMigrations`; keep that distinct name to avoid EF overwriting the legacy snapshot.

Checks performed on the isolated branch:
- 50 source products match prices, discounts, stock, and ratings exactly.
- Registration/login, sanitized Identity responses, owned profile updates and retained tokens.
- Anonymous admin access denied, customer tokens denied admin access, customer order ownership enforced.
- Server rejects the old client-trusted paid-order path.
- Category/supplier/product create/update/delete on clearly marked temporary fixtures; cleanup preserved recovered products.
- PayPal sandbox OAuth and server-priced order creation (no capture yet).
- Frontend regression tests cover async auth, search during loading, error/retry, bounded requests, token destinations/roles, discounted totals, cart refresh and discounted filtering.
- Frontend production build and backend Release publish run. Explicit runtime database package references prevent published-artifact dependency pruning.

Run `scripts/verify-validation.py` and `scripts/verify-admin-validation.py` from the repository root ONLY while the local API uses the migration-validation database. Both scripts are deliberately fixed to localhost. They create isolated test accounts; the CRUD script cleans up only its own named fixtures.

## Implemented flows and limits
Catalog, infinite scrolling, combined text/type/price/rating filters, gallery, cart, customer authentication, order history, profile endpoints, and admin catalog/customer screens are implemented. No separate product-detail URL exists; details are shown in the card/gallery view.

Contact email/phone links are operational. Most footer informational links intentionally lead to the pre-existing work-in-progress screen. Newsletter signup has no backend and now states that clearly instead of POSTing/reloading the app. Forgot-password delivery and the customer admin editor are not implemented; they must not claim success.

Checkout is sandbox-only by construction and visibly labeled. The API calculates totals from stored decimal prices, persists a draft, checks customer ownership, validates PayPal capture amounts/currency, locks stock, and records a unique capture transaction. Repeat capture returns the saved result. No production PayPal endpoint is enabled. A real-money launch needs its own review/configuration and fulfillment policy.

Existing nullable/compiler warnings remain. AutoMapper 13.0.1 carries GHSA-rvv3-g6hj-g44x; mapping depth is bounded to 16 and JSON input depth to 32 as mitigation. A patched major version requires a separate compatibility/licensing decision.

## Publication status
Backend and frontend publication/live verification are pending. Update this section with confirmed service/deploy URLs and IDs after publication.
