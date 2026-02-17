FinSkew Website Audit — Findings

Generated: 2026-02-15T19:14:59Z

Overview

This document captures the audit findings from running and reviewing the FinSkew Blazor WebAssembly app (src\FinSkew.Ui). Issues are grouped by category and prioritized where relevant.

----

PERFORMANCE (7 issues)

1. `CalculateResult()` called multiple times per render
   - Location: All calculator pages (e.g., src\FinSkew.Ui\Components\Pages\SimpleInterestCalculator.razor:98,123,140,157,184)
   - Impact: Expensive calculations are repeated on each render; wastes CPU and increases latency.

2. `Data` property recalculates on every access
   - Location: All calculator pages (e.g., SIPCalculator.razor:178-185)
   - Impact: Chart data getter invokes calculations redundantly.

3. Conflicting debounce: `Immediate="true"` + `DebounceInterval="200"`
   - Location: All calculators (example: SimpleInterestCalculator.razor:75-76)
   - Impact: Computation fires immediately and again after debounce, causing extra work.

4. No trimming/AOT configuration in project
   - Location: FinSkew.Ui.csproj
   - Impact: Larger WASM download and slower startup; publish trimming/AOT not configured.

5. Missing preconnect for Google Fonts
   - Location: wwwroot\index.html
   - Impact: Font loading delayed by extra DNS/TCP handshake.

6. No response compression configured for static assets
   - Location: HTTP responses (MudBlazor CSS reported ~610KB uncompressed)
   - Impact: Large downloads; use gzip/Brotli in production server configuration.

----

SEO (8 issues)

1. Missing meta description
   - Location: wwwroot\index.html

2. Missing Open Graph tags (og:title, og:description, og:image, og:url)
   - Location: wwwroot\index.html

3. Missing canonical link
   - Location: wwwroot\index.html

4. robots.txt not present (404)
   - Location: wwwroot/robots.txt

5. sitemap.xml not present (404)
   - Location: wwwroot/sitemap.xml

6. No per-page HeadContent/Page-level meta
   - Location: All calculator .razor pages — no <HeadContent> blocks. Pages rely on base title only.

7. No JSON-LD structured data for calculators
   - Location: wwwroot\index.html

8. STPCalculator.razor is empty but routable
   - Location: Components/Pages/STPCalculator.razor

----

ACCESSIBILITY (5 issues)

1. Missing visible <h1> headings on calculator pages
   - Location: All calculator pages. App.razor uses FocusOnNavigate Selector="h1" but pages lack <h1>.
   - Impact: FocusOnNavigate fails; screen readers have no visible page heading.

2. Missing skip-to-content link
   - Location: Components/Layout/MainLayout.razor
   - Impact: Keyboard users must tab through navigation before accessing main content.

3. NotFound.razor uses <h3> instead of <h1>
   - Location: Components/Pages/NotFound.razor:4

4. Breadcrumb "Calculators" uses href="#" (dead anchor)
   - Location: Many PageHeader usages (e.g., SimpleInterestCalculator.razor:14)
   - Impact: Clicking the breadcrumb may scroll unexpectedly instead of navigating.

5. NotFound.razor missing PageTitle
   - Location: Components/Pages/NotFound.razor

----

RESPONSIVE DESIGN / MOBILE (5 issues)

1. Hardcoded charts sized 300px × 300px
   - Location: All calculator pages (e.g., MudChart Height="300px" Width="300px")
   - Impact: Charts dominate small viewports and do not scale responsively.

2. Results container uses max-width:420px
   - Location: Calculator pages (e.g., Style="width:100%;max-width:420px;")
   - Impact: Can cause horizontal overflow or crowding on small phones.

3. No CSS @media queries in app.css
   - Location: wwwroot\css\app.css
   - Impact: No mobile-specific style adjustments.

4. Layout uses Row + Wrap but lacks explicit responsive breakpoint attributes (xs=12, md=6)
   - Location: All calculator pages (MudStack/MudGrid usage) 

5. Sidebar default open on mobile
   - Location: MainLayout.razor (_sideBarOpen = true)
   - Impact: Drawer can cover content on small screens.

----

BEST PRACTICES / CODE QUALITY (6 issues)

1. TODO comments present in production code
   - Location: SimpleInterestCalculator.razor:33, CompoundInterestCalculator.razor:32, MapperProfile.cs

2. No favicon.ico fallback (favicon.svg only)
   - Location: wwwroot/ (favicon.ico returns 404)

3. Missing HTTP security headers (X-Content-Type-Options, X-Frame-Options, CSP, Referrer-Policy)
   - Observed via HTTP response headers

----

MINOR (2 issues)

1. Cache-Control set to `no-store` for HTML and assets
   - Location: HTTP response headers
   - Impact: Prevents client caching; reconsider long-lived caching for static assets in production.

2. No <noscript> fallback for JS-off users
   - Location: wwwroot\index.html

----

SUMMARY

Total issues found: 33 across Performance, SEO, Accessibility, Responsive Design, Best Practices, and Minor categories. The highest-priority fixes are:
- Deduplicate and cache calculations (CalculateResult) to avoid repeated computation per render.
- Add per-page <HeadContent> and meta descriptions / OG tags; add robots.txt and sitemap.xml.
- Add visible <h1> headings and a skip-to-content link; fix FocusOnNavigate selector or add h1s.
- Make charts responsive and remove fixed pixel dimensions; add media queries and responsive grid attributes.

If desired, next steps can be turned into tracked todos and small PRs (start with caching calculations and adding per-page HeadContent + meta tags).
