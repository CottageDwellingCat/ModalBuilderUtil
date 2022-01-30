This won't work on stable dnet, build https://github.com/CottageDwellingCat/Discord.Net-Labs/tree/feature/modal_interactions and add it to the `csproj`.

To setup the project:
```bash
wget https://gist.githubusercontent.com/CottageDwellingCat/9f754dc0c16f612dd76e29d11ce92ae2/raw/ClientSettings.json
nano ClientSettings.json # Insert your token and guild id.
dotnet ef database update
dotnet build --nologo --force-restore
```

You should get a file called `SuspiciouslySmelly.db`. You are obligated to mention it's stench, out loud, every time you interact with it, even when other people are around.

