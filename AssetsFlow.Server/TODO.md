
# TODO
- replace supa's password, reset db
- "userservice" 

Update User Service (gRPC) to Issue JWTs
[ ] Add JWT issuing to your gRPC login method
Update UserService.LoginAsync() to generate and return JWT token
Update UserGrpcService.Login() to include JWT in response
Update AuthResponse proto to include token field
Add JWT service to user service project

2. Add JWT Middleware to Main API
[ ] Add JWT validation middleware to your journal service
Add JWT Bearer authentication to Program.cs
Configure JWT validation parameters
Add [Authorize] attributes to protected endpoints

3. Update AuthController to Use JWT Service
[ ] Update AuthController to use IJwtService
Inject IJwtService into AuthController
Generate JWT tokens on successful login/register
Return tokens in API responses
Remove TODO comments

4. Update Controllers for JWT Validation
[ ] Update controllers to use JWT validation instead of user ID validation
Replace ValidateUserExists() calls with JWT token validation
Extract user ID from JWT claims instead of parameters
Add [Authorize] attributes to journal endpoints
Remove userId parameters from endpoints (get from JWT)

5. Update Proto Definitions
[ ] Update gRPC proto to include JWT tokens
Add token field to AuthResponse message
Regenerate proto files
Update client interfaces

6. Security & Configuration
[ ] Ensure JWTs include necessary claims
Add user roles to JWT claims (when roles are implemented)
Configure proper JWT expiration and refresh logic
Add JWT secret key to environment variables (not in appsettings)


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