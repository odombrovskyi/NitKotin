# Skill: Create Installer Release

Use this runbook whenever you need to produce a fresh MSI release for `NitKotin`.

## Goal

Create a clean, versioned installer build without reusing stale artifacts.

## Preconditions

- Work from the repository root.
- Ensure no running `NitKotin.exe` process is locking build output.
- Confirm the intended release version before building.

## Steps

1. Increment the version in `Directory.Build.props`.
2. Update `README.md` if the visible release version or release instructions changed.
3. Clean all relevant outputs:

```powershell
dotnet clean .\NitKotin.slnx -c Debug
dotnet clean .\NitKotin.slnx -c Release
dotnet clean .\NitKotin.Installer\NitKotin.Installer.wixproj -c Release
```

4. Build the app in `Release`:

```powershell
dotnet build .\NitKotin\NitKotin.csproj -c Release
```

5. Build the installer in `Release`:

```powershell
dotnet build .\NitKotin.Installer\NitKotin.Installer.wixproj -c Release
```

6. Confirm the new artifact exists at:

```text
NitKotin.Installer\bin\x64\Release\NitKotin-<version>-x64.msi
```

## Expected Result

- The app and MSI both use the same version from `Directory.Build.props`.
- The installer output is fresh and built after a clean.
- The MSI is ready to upload to a GitHub Release.

## Notes

- The WiX project currently emits non-blocking `ICE91` warnings during build.
- The installer project stages publish output automatically before packaging.