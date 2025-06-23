
# TODO

- add demo DB
- redo seeder

- add opened/closedat somehow (to overview?) 
- opened = first activate date, close = closed date (remove props from trade make em getters / readonly ?)
- on remove, reset activation states or just re-check. open = at least 1 add exists

- rethink isContentMissing - either way dont need to reload all trade to get it, you always got the old one (in the fe at least)
- closed trade logic : deny edits/ re-open trade to allow edit?
- rethink activation/ summary (maybe summary all activated elements only?) 


## testing
- API tests (seeder?)

## dashboard + price alerts
- Indicator alerts, upcoming reports, trend watch

## ledger - populate brokers list with actual data
- position weight/ sector weight
- pies

## nice to have
- Actual R:R in closure
- conluences check list = trends per tf, bb/ma distances
- filtering history
- users + authentication

## Expansion:
Suspence? RQ Suspence
GraphQL instead of REST