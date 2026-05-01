# Copilot Instructions For This Repository

## Project Overview

- This repository contains a bilingual (English + Ukrainian) WinForms desktop app in `NitKotin/` and a WiX v4 MSI installer in `NitKotin.Installer/`.
- The solution file is `NitKotin.slnx`.
- The app targets `.NET 8` for Windows.
- The installer is a per-user MSI and installs into `%LocalAppData%\NitKotin`.

## Product Conventions

- Keep user-facing UI text aligned with the selected app or installer language. English is the default app language; Ukrainian must remain fully supported.
- Preserve the current product naming: `NitKotin`.
- Keep the main workflow around smoke-free tracking, savings, tray icon behavior, and overlay behavior intact unless the task explicitly changes it.

## Important Files

- `Directory.Build.props`: single source of truth for app and installer version metadata.
- `README.md`: public-facing documentation for installation, releases, tray, and overlay behavior.
- `NitKotin/MainForm.cs`: main window behavior, timers, autosave, tray interactions, first-run UX.
- `NitKotin/OverlayForm.cs`: floating overlay window behavior.
- `NitKotin.Installer/Product.wxs`: WiX installer authoring.

## Build And Release Rules

- Before creating a new installer release, increment the version in `Directory.Build.props`.
- Prefer cleaning build outputs before a release build so stale files are not carried into the installer.
- Build the app in `Release` before packaging a new MSI.
- Build the installer in `Release` for both `InstallerLanguage=en` and `InstallerLanguage=uk`, and verify both MSI files appear under `NitKotin.Installer\bin\x64\Release\`.
- If documentation changes affect installation, release flow, tray, overlay, or versioning, update `README.md` in the same task.

## Standard Commands

```powershell
dotnet clean .\NitKotin.slnx -c Debug
dotnet clean .\NitKotin.slnx -c Release
dotnet clean .\NitKotin.Installer\NitKotin.Installer.wixproj -c Release
dotnet build .\NitKotin\NitKotin.csproj -c Release
dotnet build .\NitKotin.Installer\NitKotin.Installer.wixproj -c Release -p:InstallerLanguage=en
dotnet build .\NitKotin.Installer\NitKotin.Installer.wixproj -c Release -p:InstallerLanguage=uk
```

## GitHub Release Expectations

- Create a Git tag that matches the version from `Directory.Build.props`.
- Create a GitHub Release with the same version number.
- Upload both generated MSI files named `NitKotin-en-<version>-x64.msi` and `NitKotin-uk-<version>-x64.msi` as release artifacts.

## Installer Skill

- For a repeatable installer-release workflow, use `.github/skills/create-installer/SKILL.md`.