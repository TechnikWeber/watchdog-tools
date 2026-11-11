Native builds for Windows on ARM and checksums. The tools themselves are unchanged since v1.0.1.

- **New: `…-arm64.exe`** for each tool — native programs for Windows on ARM (Snapdragon laptops). They need the Arm64 .NET 10 Desktop Runtime.
- **New: `SHA256SUMS`** lists the checksum of every file.

All three tools as single `.exe` files, built by GitHub Actions.

- They need the [.NET 10 Desktop Runtime](https://dotnet.microsoft.com/download/dotnet/10.0) (x64, or Arm64 for the `…-arm64.exe`).
- The files are not signed; SmartScreen may warn (*More info › Run anyway*).
