## Overview: 
An online Trading Journal, will allow the user to add and manage ongoing trade positions.

TBD1: Filtering of trades, Positions/allocations pie charts

TBD2: Trade alerts, based on current price action + indicators.

## Part 1: Client; React 18, SAA
### Fetch Flow and Cache Management:

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

## Part2: Server; PostgreSQL, EF, ASP.NET Core, REST API
### Data Structure:

Each Trade Position composed of Elements  (origin, entry, exit, etc.)

Each Element (line) is composed of Entry (cell) objects

TradeComposite:  

                 (Origin)    [ ] [ ] [ ] [ ]

                 (Increase)  [ ] [ ] [ ] [ ]
                 
                 (Reduction) [ ] [ ] [ ] [ ]

                 (Summary)   [ ] [ ] [ ] [ ] 
