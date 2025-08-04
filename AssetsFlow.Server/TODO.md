
# TODO
- Journal to Journal.All, move to main
- HsR.Infrastructure to Hsr.Journal**..
- HsR.UserService < move to main and rename
- Caching should move to common, it's test(HsR.Web.API.Caching.Tests) renamed

- aspire packages

- update readme
- directory.packages.props managepackageversionscentrally
- parse elements from json templates
- production envs to GH secrets
- add Entity base class and valueObject. do i have aggregates? maybe trade/elements?

- error route doesnt work 

- add version to user api
- fix user entity (remove stuff?), adjust login request to user/pass?

- rethink isContentMissing - either way dont need to reload all tradeComposite to get it, you always got the old one (in the fe at least)
- closed trade logic : deny edits/ re-open trade to allow edit?

- filtering/sortby/search by

## security
 - bcrypt (encrypt passwords), firebase (more auth)

## testing
- API tests (seeder?)
- expand tests/integration tests

## dashboard + price alerts
- Indicator alerts, upcoming reports, trend watch

## ledger - populate brokers list with actual data
- position weight/ sector weight
- pies

## nice to have
- screenshots firebase (to store images?)
- Actual R:R in closure
- conluences check list = trends per tf, bb/ma distances
- filtering history
- Add rate limiting, monitoring, and alerting.

## Expansion:
Suspence? RQ Suspence
Socket.IO