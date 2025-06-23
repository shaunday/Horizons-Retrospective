
## [0.9.503-s] - 2025-06-..
- Add IsAnyContentMissing prop on elementDTO
- Reworked contentEdit logic - always load trade for now, for uptodate ismissing data (to rethink)
- Fix CompositeFK name mismatch (entity and dto)
- Fix AutoMapping for TradeCompositeInfo
- Fix Mishmash on delete's return values
- Fix evaluate (need to properly clone entities - new ids and all)


## [0.9.502-s] - 2025-06-22
- Readjusted overview items, summary/closure lists
- Fixed summary to always recalc, fixed summary/closure mishmash

- Total lines of backend code: 3762, client 1800 = ~5500 tots

## [0.9.501-b] - 2025-06-17
### Changed
- IsPending : Removed prop from the entity, renamed to IsAnyContentMissing in the DTO + set in the mapper 
### Added
- Caching
- Pagination data into config

## [0.9.16] - 2025-06-05
reworked position lists templates

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