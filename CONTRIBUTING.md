# Contributing

Thank you for helping improve Hallownest Wayfinder.

## Reporting problems

When opening an issue, include the objective shown by the mod, the room or area where the problem occurred, what you expected to happen and what happened instead. Screenshots and the relevant save state are helpful when available.

## Building

Follow the build instructions in `README.md`. Game and Modding API references are restored automatically; never commit their binaries to this repository. Run the xUnit suite before opening a pull request.

## Route changes

Each route step should have a stable identifier and, whenever possible, an automatic completion condition. Gameplay data lives in `Assets/routes.json`; add the concise objective and actionable hint to both `Assets/localization_pt.txt` and `Assets/localization_en.txt`. Interface lines in those files use the `@key|text` format. Navigation targets must use Hollow Knight's internal scene names.

Keep route-source attribution up to date when adapting information from an external guide.

## Releases

For local builds, `VersionPrefix` in `Directory.Build.props` supplies a development version. A release tag such as `v0.12.0` is authoritative: the release workflow validates it, injects that version into the assembly, runs the strict build/tests and publishes the DLL and ZIP. Move the corresponding notes from `Unreleased` to a matching heading in `CHANGELOG.md` before tagging.

## Pull requests

Keep each pull request focused on one feature or correction. Describe how the change was tested in-game and call out any step that still requires manual advancement.
