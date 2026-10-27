<h1 align="center">watchdog-tools</h1>

<p align="center">
  <strong>Three small Windows tools for long measurements: notice when a log file stops growing or a program hangs, and back up files while they are still being written.</strong>
</p>

<p align="center">
  <img alt="Status: stable" src="https://img.shields.io/badge/status-stable-brightgreen">
  <a href="https://github.com/TechnikWeber/watchdog-tools/actions/workflows/release.yml"><img alt="Build" src="https://github.com/TechnikWeber/watchdog-tools/actions/workflows/release.yml/badge.svg"></a>
  <img alt="Platform: Windows" src="https://img.shields.io/badge/platform-Windows-blue">
  <img alt="UI language: German" src="https://img.shields.io/badge/UI-Deutsch-informational">
  <a href="LICENSE"><img alt="License: MIT" src="https://img.shields.io/badge/license-MIT-green"></a>
</p>

<p align="center">
  ⬇️ <a href="https://github.com/TechnikWeber/watchdog-tools/releases/latest"><strong>Download the .exe files</strong></a>
</p>

---

Made for test benches that log for days: if the logging program crashes or the
log stops, you want to know right away, not after the weekend. The user
interface is in German.

## The tools

| Tool | What it does |
|---|---|
| [`document-watchdog`](document-watchdog) | Watches one file. If its size has not grown within the check interval, it raises an alarm: a sound on Windows and/or an e-mail via SMTP. |
| [`program-watchdog`](program-watchdog) | Watches a running program. If it exits or freezes (detected with `IsHungAppWindow`, `SendMessageTimeout` and the process state), it raises an alarm: sound and/or e-mail. |
| [`running-backup`](running-backup) | Copies a file or a folder at a fixed interval (1 minute to 12 hours) into timestamped copies, e.g. `measurement_24_02_2026_10_00.csv` — also while another program keeps the file open and writes to it. |

## Download and run

1. Get the `.exe` you need from the [latest release](https://github.com/TechnikWeber/watchdog-tools/releases/latest).
2. Install the [.NET 10 Desktop Runtime](https://dotnet.microsoft.com/download/dotnet/10.0) if it is not there yet.
3. Windows SmartScreen may warn because the files are not signed: *More info › Run anyway*.

For e-mail alarms you need an SMTP account; with Gmail and similar services use an
app password, not your normal password.

## Build from source

Open the `.slnx` file of a tool in Visual Studio 2022 (17.13 or newer) and build,
or run `dotnet build` in its folder. Release builds are made by GitHub Actions
([`.github/workflows/release.yml`](.github/workflows/release.yml)) whenever a
version tag is pushed.

## History

Until October 2026 each tool had its own repository (`DocumentWatchdog`,
`ProgramWatchdog`, `RunningBackup`). They were merged here with their git history.

## License

[MIT](LICENSE).
