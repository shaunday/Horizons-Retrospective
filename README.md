## Overview: 
Stage1: A Trading Journal, allows the user to add and manage trade ideas and ongoing trade positions.
(logic/ connection is mostly wrapped up, next on the list - styles work + MantineUI)

TBD1: Add Ledger: withdraw, deposit, convert (per broker) + time stamps           .....> expand to Asset / Trading Flow

TBD2: Positions/allocations pie charts

TBD Stage2: Trade alerts, based on current price action + indicators.

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

## Part2: Server; PostgreSQL, EF Core, ASP.NET Core, REST API
### Entities (Rough) Design Overview:

![image](https://github.com/user-attachments/assets/37b0def3-7901-4748-b2e8-2acb93e9d59e)


