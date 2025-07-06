
# TODO

- users client
- replace supa's password, reset db

- Add JWT issuing to your gRPC login method.
- Add JWT validation middleware to your journal service (and user service if needed).
- Ensure JWTs include necessary claims (e.g., user ID, email, roles if used).
- Update clients to use JWTs for authenticated requests.

- AI textbox for tradeElement creation
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