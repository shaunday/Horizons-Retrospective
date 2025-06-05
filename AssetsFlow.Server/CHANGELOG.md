



## [0.9.15] - 2025-06-05
track string collection changes using a value comparer - fixing ef core error

## [0.9.14] - 2025-06-05
Fix nginx start script - dont copy/dup https-server, preventing double listenere on port 80

## [0.9.13] - 2025-06-05
- Error middleware and controller for production
- Certainty > list (low, mid, high) 

## [0.9.12] - 2025-06-03
load supabase id correctly from the .env

## [0.9.11] - 2025-06-03
typo fix

## [0.9.09] - 2025-06-03
supabase connection string variable - added 1 for the id

## [0.9.3-0.9.8] - 2025-05-27
Versioning/ Date of release work:  env variables pass through dockerfile > GH Actions> Docker compose> BE > FE

## [0.9.2] - 2025-05-27
### Fixed
- Version getters
- Environment check unification (dont use Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
- Warning levels raised for production

## [0.9.1] - 2025-05-26
### Added
- This changelog
- Get app versions, commit hash

### Changed
- to do.txt > md format
- Re-adjust position boundaries (no 'risk in origin)


---

Fixed, Added, Changed, Database (Migrations).