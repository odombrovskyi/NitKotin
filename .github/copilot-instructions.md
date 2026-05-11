# Copilot Instructions For This Repository

## Project Overview

- This repository contains a bilingual (English + Ukrainian) WinForms desktop app in `QuitMojo/` and a WiX v4 MSI installer in `QuitMojo.Installer/`.
- The solution file is `QuitMojo.slnx`.
- The app targets `.NET 8` for Windows.
- The installer is a per-user MSI and installs into `%LocalAppData%\QuitMojo`.

## Product Conventions

- Keep user-facing UI text aligned with the selected app or installer language. English is the default app language; Ukrainian must remain fully supported.
- Preserve the current product naming: `QuitMojo`.
- Keep the main workflow around smoke-free tracking, savings, tray icon behavior, and overlay behavior intact unless the task explicitly changes it.

## Important Files

- `Directory.Build.props`: single source of truth for app and installer version metadata.
- `README.md`: public-facing documentation for installation, releases, tray, and overlay behavior.
- `QuitMojo/MainForm.cs`: main window behavior, timers, autosave, tray interactions, first-run UX.
- `QuitMojo/OverlayForm.cs`: floating overlay window behavior.
- `QuitMojo.Installer/Product.wxs`: WiX installer authoring.

## Build And Release Rules

- Before creating a new installer release, increment the version in `Directory.Build.props`.
- Prefer cleaning build outputs before a release build so stale files are not carried into the installer.
- Build the app in `Release` before packaging a new MSI.
- Build the installer in `Release` for both `InstallerLanguage=en` and `InstallerLanguage=uk`, and verify both MSI files appear under `QuitMojo.Installer\bin\x64\Release\`.
- If documentation changes affect installation, release flow, tray, overlay, or versioning, update `README.md` in the same task.

## Standard Commands

```powershell
dotnet clean .\QuitMojo.slnx -c Debug
dotnet clean .\QuitMojo.slnx -c Release
dotnet clean .\QuitMojo.Installer\QuitMojo.Installer.wixproj -c Release
dotnet build .\QuitMojo\QuitMojo.csproj -c Release
dotnet build .\QuitMojo.Installer\QuitMojo.Installer.wixproj -c Release -p:InstallerLanguage=en
dotnet build .\QuitMojo.Installer\QuitMojo.Installer.wixproj -c Release -p:InstallerLanguage=uk
```

## GitHub Release Expectations

- Create a Git tag that matches the version from `Directory.Build.props`.
- Create a GitHub Release with the same version number.
- Upload both generated MSI files named `QuitMojo-en-<version>-x64.msi` and `QuitMojo-uk-<version>-x64.msi` as release artifacts.

## Installer Skill

- For a repeatable installer-release workflow, use `.github/skills/create-installer/SKILL.md`.