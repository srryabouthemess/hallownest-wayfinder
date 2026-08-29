# Changelog

All notable changes to Hallownest Wayfinder will be documented here.

## Unreleased

## 0.12.0 - 2026-08-29

- Added save-aware free navigation to nine Hallownest landmarks, with a
  configurable destination, F10 toggle, HUD arrival and blocked-route states.
- Condensed the free-navigation menu into one shorter destination selector and
  shortened the five-hour route name to avoid menu text overlap.
- Added an optional in-game waypoint recorder with configurable controls,
  clipboard-ready JSON, accumulated paths and nearby-door detection.
- Expanded the 112% checklist into an exact category breakdown with detected
  percentage, compact/detailed/off display modes and PlayerData validation.
- Fixed the Seer's final objective completing at 1800 Essence instead of after
  claiming the 2400-Essence reward.
- Added 57 objective icons for bosses, Dreamers, abilities, charms and late-game
  progression across the 112%, speedrun and save-completion routes.
- Added Sprintmaster as its own automatically tracked objective in the 112% and
  save-completion routes.
- Fixed CI and release builds by provisioning pinned, checksum-verified Hollow
  Knight references with retry-capable downloads instead of the fragile
  HKBuildUtils HTTP downloader.
- Cached save analysis, scene collections, transition transforms, navigation
  results, localization selection and HUD layout to remove per-frame scans and
  allocations.
- Replaced fixed per-route progress fields with a versioned route-ID dictionary
  and automatic migration of existing saves.
- Added a testable game-state abstraction plus behavioral tests for completion,
  prerequisites, save analysis, gated BFS and progress migration.
- Moved route content to `Assets/routes.json`, interface text to both localization
  resources and source files into responsibility-based `src/` folders.
- Enabled .NET analyzers and warnings-as-errors in CI, bounded navigation caches
  by access state and made release tags the authoritative assembly version.
- Fixed Portuguese language detection, route recovery on advanced saves, bench
  completion checks and repeated automatic-advance error logging.
- Added usable in-room intelligent navigation and moved navigation calculation
  out of the HUD drawing loop.
- Replaced hardcoded save-completion prerequisite rules with route data and
  separated route-skipping from 112% completion requirements.
- Moved all 247 Portuguese route objectives to the editable
  `Assets/localization_pt.txt` resource.
- Consolidated route completion conditions, enabled nullable-reference analysis
  and added shared project style and build settings.
- Added dependency restoration without a local game installation, GitHub Actions
  build/test validation, automated tagged releases and route-data xUnit tests.
- Made route finding respect PlayerData-gated transitions and added authored
  waypoints for the opening rooms.
- Added configurable HUD controls, per-route progress reset, a persistent
  completion screen and a compact 112% checklist.

## 0.11.0

- Added `Save Completion`, which scans existing saves and recommends the next unfinished objective with detectable prerequisites.
- Integrated all 46 individual grub records into the save-completion analysis and added per-save postpone/restore controls.
- Added complete English localization for the menu, HUD, navigation and all 247 route objectives.
- Added automatic language detection plus manual Portuguese (Brazil) and English options.
- Added combined completion rules supporting all/any conditions, visited scenes,
  specific benches, cumulative item totals and Pantheon completion.
- Automated all 19 route steps that previously required manual advancement,
  while preserving F8 as a fallback.
- Track individual grubs by their rescued scene instead of relying only on the total count.
- Correctly handle grubs collected out of the suggested route order.
- Added a complete 46-grub diagnostic route for existing save files.
- Added a selectable five-hour, glitchless speedrun route adapted from fireb0rn's guide.
- Added independent per-save progress for the 112% and speedrun routes.
- Added a route selector to the mod menu.
- Renamed the project from RouteCompass to Hallownest Wayfinder.
- Added the initial public project documentation.
- Prepared the repository for future selectable routes.
