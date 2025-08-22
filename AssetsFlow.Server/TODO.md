
# TODO
- userdata: cache symbols, update symbol if it's edited, verify it's all sent back
- data migrations
- alltrades should be via filter method

- add Entity base class and valueObject. do i have aggregates? maybe trade/elements?
- fix user entity (remove stuff?), adjust login request to user/pass?
- GetUserIdFromToken/ValidateToken are not used.. 
- closed trade logic : deny edits/ re-open trade to allow edit? meanwhile its all open

- filtering: add mistakes, strengths, takeaways + dates, free search
- add sortby

- rethink isContentMissing - either way dont need to reload all tradeComposite to get it, you always got the old one (in the fe at least)

## testing
- API tests (seeder?) + expand tests/integration tests

## dashboard + price alerts
- Indicator alerts, upcoming reports, trend watch

## ledger - populate brokers list with actual data
- position weight/ sector weight
- pies

## nice to have
- saved sectors: get online? add to filters + filterId (saving logic commented out for now)
- screenshots firebase (to store images?)
- Actual R:R in closure
- conluences check list = trends per tf, bb/ma distances
- filtering history
- Add rate limiting, monitoring, and alerting.
- Microsoft.Extensions.Caching.Hybrid

## Expansion:
Suspence? RQ Suspence
Socket.IO
Full auto deploy ? Expand publish action to also auto deploy on ec2