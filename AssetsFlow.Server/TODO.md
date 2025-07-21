
# TODO
- replace supa's password, reset db, update db name for user's service


Use async EF methods with ConfigureAwait(false) for scalability
  await dbContext.SaveChangesAsync().ConfigureAwait(false);

2. Update AuthController to Use JWT Service
[ ] Update AuthController to use IJwtService
Inject IJwtService into AuthController
Generate JWT tokens on successful login/register
Return tokens in API responses
Remove TODO comments

3. Update Controllers for JWT Validation
[ ] Update controllers to use JWT validation instead of user ID validation
Replace ValidateUserExists() calls with JWT token validation
Extract user ID from JWT claims instead of parameters
Add [Authorize] attributes to journal endpoints
Remove userId parameters from endpoints (get from JWT)

4. Security & Configuration
[ ] Ensure JWTs include necessary claims
Add user roles to JWT claims (when roles are implemented)
Configure proper JWT expiration and refresh logic

- AI textbox for tradeElement creation
- add ignoreActivation flag to content edit
- add idea/origin date (not same as trade open)
- on remove, set condition where net is positive post, otherwise error
- rethink isContentMissing - either way dont need to reload all tradeComposite to get it, you always got the old one (in the fe at least)
- closed trade logic : deny edits/ re-open trade to allow edit?

- dup code under " // Determine the base URL based on environment"

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