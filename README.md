### Client: React SAA

## Fetch Flow and Cache Management

1. **Initial Data Prefetch in App.Js (`useFetchAndCacheTrades`):**
   - Fetches all trades on app start or refocus.
   - Caches trade IDs and individual trades.

1a. **Actual UseQuery Hook in JournalContainer (1 level below App.Js)
   - Same fetch function, should be using prefetch's data as it comes up

2. **Trade Data Fetching (`useTrade`):**
   - Retrieves trade data from the cache or fetches if missing.
   - Updates cache with new data if fetched.

3. **Adding Trades (`useAddTrade`):**
   - Handles new trade submissions.
   - Updates both the trade IDs list and individual trade caches.



### Server: PostgreSQL, EF, ASP.NET Core



Trade Position composed of elements  (origin,entry,exit,etc)
Each line composed of cells


TradeComposite: List<TradeElement>
TradeElement: List<InfoCell>

TradeComposite:  (Origin)    [ ] [ ] [ ] [ ]
                 (Entry)     [ ] [ ] [ ] [ ]
                 (Reduction) [ ] [ ] [ ] [ ]

                 (Summary)   [ ] [ ] [ ] [ ] 
