
# TODO
- replace supa's password, reset db, update db -> name for user's service
- add version to user.s api

- Configure JWT refresh logic - auth/refresh

- fix user entity (remove stuff?), adjust login request to user/pass?

- add ignoreActivation flag to content edit
- add idea/origin date (not same as trade open)
- on remove, set condition where net is positive post, otherwise error
- rethink isContentMissing - either way dont need to reload all tradeComposite to get it, you always got the old one (in the fe at least)
- closed trade logic : deny edits/ re-open trade to allow edit?

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
- set full deployment (deploy on aws post version tag .. deploy all??)

## Expansion:
Suspence? RQ Suspence
Socket.IO