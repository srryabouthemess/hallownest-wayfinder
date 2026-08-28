# Contributing

Thank you for helping improve Hallownest Wayfinder.

## Reporting problems

When opening an issue, include the objective shown by the mod, the room or area where the problem occurred, what you expected to happen and what happened instead. Screenshots and the relevant save state are helpful when available.

## Building

Follow the build instructions in `README.md`. Game and Modding API references are restored automatically; never commit their binaries to this repository. Run the xUnit suite before opening a pull request.

## Route changes

Each route step should have a stable identifier and, whenever possible, an automatic completion condition. Add its concise objective and actionable hint to both `Assets/localization_pt.txt` and `Assets/localization_en.txt`; route definitions should contain only the stable identifier and gameplay data. Navigation targets must use Hollow Knight's internal scene names.

Keep route-source attribution up to date when adapting information from an external guide.

## Releases

`VersionPrefix` in `Directory.Build.props` is the single source of the mod version shown in game and written to the assembly. When preparing a release, update it and move the corresponding notes from `Unreleased` to a matching heading in `CHANGELOG.md`. Pushing the matching `v<version>` tag runs the tests and publishes the DLL and ZIP automatically.

## Pull requests

Keep each pull request focused on one feature or correction. Describe how the change was tested in-game and call out any step that still requires manual advancement.
