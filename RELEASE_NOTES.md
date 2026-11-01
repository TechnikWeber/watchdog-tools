Layout fixes for scaled displays (125 %, 150 % …) and a visible event log.

- All three tools now scale with the Windows display scaling. Before, texts were cut off, e.g. "Ausgabeord" or an e-mail checkbox without its label.
- `program-watchdog`: the event log was below the bottom edge of the window and could not be seen at all.
- `document-watchdog`, `program-watchdog`: no more empty space for hidden panels. The window grows when you tick *E-Mail-Benachrichtigung* or when an alarm is shown.
- On screens that are too small, the window fits the screen and scrolls.

All three tools as single `.exe` files, built by GitHub Actions.

- They need the .NET 10 Desktop Runtime.
- The files are not signed; SmartScreen may warn (*More info › Run anyway*).
