# Changelog

## [0.9.123-c] - 2025-08-18

fix: a bunch of prop checks, removed unused vars
fix: mantines notifications

## [0.9.122-c] - 2025-08-18

fix: cotent edit popup open> disable tooltip
allow edit on 1/2 overview
fix: close trade not working
fix: close trade not refreshing - parse new- reduce value
add: close trade input validation
fix: trade by id cached twice
fix: invlidate also by key fortradeids after reseed
fix: add trade doesnt refresh
fix: content edit popup appears twice sometimes- jotai

### Fixed

- Error component for errror boundary states
- Get all trades / refresh after reseed
- Edit logic - no modal, popover instead.

## [0.9.120-c] - 2025-07-26

### Added

- Users Service, Demo user
- Update frontend to store and send JWT tokens + auth api
- Unified gateway - auth + fetch trades steps
- demo seeder
- mantine v7 > v8

## [0.9.019-c] - 2025-06-24

### Added

- Missing notification for collapsed trade elements
- HUGE work removing callbacks/ unifying status update logic with hooks instead

### Fixed

- All sorta of logic not working /refreshing - trade action buttons' state, summary state
- CompositeFK name mismatch (server issue tbf)
- Reducing position
- evalute wasnt updating

## [0.9.018-c] - 2025-06-23

### Fixed

- "Add a trade" button typo

## [0.9.017-c] - 2025-06-22

### Added

- Dev. indicators (browser and page headers)
- when adding a trade, expand it on reponse

### Changed

- reverted expand/collapse on background clicks
- Journal loading/error states : styles rework
- Action buttons unification - addtrade/change data/set closing price
- Some modals styling (close price, edit data)

### Fixed

- Edit button "dancing" (caused by translation in class name instead of in the style)

## [0.9.016-c] - 2025-06-22

### Changed

- Terser minification for production builds
- Console/debugger removal in production only
- Mantine dependency pre-bundling via optimizeDeps
- Chunk size warning limit (1000kb)
- styling: allow overflow on all objects, fix controls pos.
- header/icon
- min width for dataelements

## [0.9.015-c] - 2025-06-21

### Changed

- Gallery styling
- Click to expand/collapse: Trade
- Missing Data : use background tint instead of border
- Trade buttons rework

## [0.9.014-c] - 2025-06-21

### Changed

- Reworked all styles to tailwind utility classes

## [0.9.013-f] - 2025-06-20

### Changed

- IsPending identifier string due to change in the BE

### Added

- Trade notification control - show on missing content
- Trade admin controls - under dev for now.

## [0.9.12] - 2025-05-26

### Added

- Red border on missing data, when you open a trade

## [0.9.11] - 2025-06-05

### Fixed

no restriction on width for collapsed element

## [0.9.1] - 2025-05-26

### Added

- Versions info on the footer
- Help > About Drawer

---

Fixed, Added, Changed
