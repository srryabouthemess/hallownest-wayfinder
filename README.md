# Hallownest Wayfinder

An in-game route guide and navigation assistant for Hollow Knight.

[![CI](https://github.com/srryabouthemess/hallownest-wayfinder/actions/workflows/ci.yml/badge.svg)](https://github.com/srryabouthemess/hallownest-wayfinder/actions/workflows/ci.yml)

Hallownest Wayfinder displays the next objective of a guided route inside the game. It can automatically detect completed objectives, provide room-aware navigation and keep separate progress for each save file.

> The project is under active development. It currently includes a guided 112% playthrough and a safe, glitchless route for the five-hour speedrun achievement.

## Features

- Persistent objective HUD while playing.
- Guided 112% route with automatic progress tracking.
- Selectable `Speedrun 5h` route with ten segments and independent progress.
- `Larvas 46/46` diagnostic route that skips rescued grubs and stops at the next missing location.
- `Save Completion` mode that scans an existing save and recommends the next persistent, unfinished objective whose prerequisites are available.
- Intelligent room-to-room navigation with an approximate-direction fallback.
- Save-aware navigation that avoids transitions blocked by progression abilities.
- A compact 112% checklist for charms, masks, vessels, Nail upgrades and Essence.
- Embedded icons and a UI designed to fit Hollow Knight's visual style.
- Independent progress for every save file.
- Configurable interface size and navigation mode.
- Portuguese (Brazil) and English localization, with automatic game-language detection and a manual override.

In `Save Completion`, route-only travel instructions and consumable counters are excluded because they cannot reliably describe an existing save. The mode checks permanent progression records, includes all 46 grubs individually and lets you postpone or restore recommendations with the configured controls.

## Controls

| Default key | Action |
| --- | --- |
| `F6` | Show or hide the HUD |
| `F7` | Return to the previous objective |
| `F8` | Advance or manually complete an objective |

All three bindings can be changed in the mod menu. The menu also includes an action to reset the manual progress of the currently selected route.

## Installation

Download `HallownestWayfinder.dll` or `HallownestWayfinder.zip` from the [latest release](https://github.com/srryabouthemess/hallownest-wayfinder/releases/latest).

1. Install the Hollow Knight Modding API using a compatible mod installer.
2. Create a `HallownestWayfinder` folder inside `hollow_knight_Data/Managed/Mods`.
3. Copy `HallownestWayfinder.dll` into that folder.

## Building from source

Install the .NET 8 SDK, clone the repository and run:

```powershell
dotnet build -c Release
```

`HKBuildUtils` restores the Hollow Knight Modding API references automatically, so building does not require an installed copy of the game. Developers may still opt into their local installation:

```powershell
dotnet build -c Release -p:HollowKnightRefs="C:\path\to\hollow_knight_Data\Managed"
```

Release artifacts are generated at `bin/Publish/HallownestWayfinder.dll` and `bin/Publish/Publish.zip`.

Run the data-integrity and behavioral suite with:

```powershell
dotnet test tests/HallownestWayfinder.Tests/HallownestWayfinder.Tests.csproj -c Release
```

Route gameplay data is stored in `Assets/routes.json`. Objectives, hints and UI
copy are editable in `Assets/localization_pt.txt` and
`Assets/localization_en.txt`, so content corrections do not require changing C#.

## Route source and attribution

The current 112% route is adapted and summarized from the Steam guide *112% Completion Walkthrough with Maps* by Almech Alfarion. See [THIRD_PARTY_NOTICES.md](THIRD_PARTY_NOTICES.md) for attribution and third-party licensing details.

The five-hour speedrun route is adapted and summarized from fireb0rn's Steam guide [*Hollow Knight - 5 Hour Speedrun Achievement Guide by a Speedrunner*](https://steamcommunity.com/sharedfiles/filedetails/?id=1861523602). The in-game instructions are original summaries, available in Portuguese and English, rather than reproductions of the guide text.

The grub diagnostic reads Hollow Knight's individual rescued-scene records.
Its complete vanilla scene checklist was verified against the public location
data from [ItemChanger](https://github.com/homothetyhk/HollowKnight.ItemChanger).

## Contributing

Bug reports, corrections and route suggestions are welcome through GitHub Issues. Before contributing code, see [CONTRIBUTING.md](CONTRIBUTING.md).

## License

Hallownest Wayfinder is licensed under the [GNU General Public License v3.0](LICENSE).

This is an unofficial fan-made project and is not affiliated with Team Cherry.
