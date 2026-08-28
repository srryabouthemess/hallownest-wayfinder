# Third-party notices

Hallownest Wayfinder is distributed under GNU GPL version 3. See `LICENSE`.

## Vanilla transition facts

`Assets/vanilla_transitions.txt` is a reduced, mechanically transformed list
of factual vanilla scene/door connections derived from transition metadata in:

- RandomizerMod — https://github.com/homothetyhk/RandomizerMod
- Copyright its contributors.
- Source project license: GNU LGPL version 2.1.
- A copy is included at `THIRD_PARTY_LICENSES/RandomizerMod-LGPL-2.1.txt`.

The factual mapping between vanilla grub locations and scene identifiers used
by the `Larvas 46/46` route was checked against ItemChanger's public location
data:

- ItemChanger — https://github.com/homothetyhk/HollowKnight.ItemChanger
- Copyright its contributors.
- Source project license: GNU LGPL version 2.1.
- The LGPL 2.1 text is included at
  `THIRD_PARTY_LICENSES/RandomizerMod-LGPL-2.1.txt`.

Hallownest Wayfinder does not include or link the ItemChanger binary. Route
descriptions and tracking code were written independently.

The Hallownest Wayfinder graph loader and breadth-first search implementation were
written independently for this project. No RCPathfinder or RandoMapCore code is
included in the Hallownest Wayfinder binary.

## Five-hour speedrun route

The route order is adapted from fireb0rn's Steam guide, "Hollow Knight - 5 Hour
Speedrun Achievement Guide by a Speedrunner":

https://steamcommunity.com/sharedfiles/filedetails/?id=1861523602

Hallownest Wayfinder uses original, concise Portuguese instructions written for
the in-game HUD and does not redistribute the guide's route-map images.

The following GPL projects were studied for interoperability and architectural
reference, but their binaries are not bundled:

- RCPathfinder — https://github.com/syyePhenomenol/RCPathfinder
- MapChanger — https://github.com/syyePhenomenol/MapChanger
- VanillaMapMod — https://github.com/syyePhenomenol/HollowKnight.VanillaMapMod
- RandoMapMod — https://github.com/syyePhenomenol/RandoMapMod

ConnectionMetadataInjector (MIT) was also inspected:
https://github.com/BadMagic100/ConnectionMetadataInjector
