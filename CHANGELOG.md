# Changelog

All notable changes to Hallownest Wayfinder will be documented here.

## Unreleased

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
