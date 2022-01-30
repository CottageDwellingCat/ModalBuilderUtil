This won't work on stable dnet, build https://github.com/CottageDwellingCat/Discord.Net-Labs/tree/feature/modal_interactions and add it to the `csproj`.

To setup the project:
```bash
$ wget https://gist.githubusercontent.com/CottageDwellingCat/9f754dc0c16f612dd76e29d11ce92ae2/raw/d69d905ddfaa9f5575f528feeafb45fc84b6d3e7/ClientSettings.json
$ nano ClientSettings.json # Insert your token and guild id.
$ dotnet ef database update
$ dotnet build --nologo --force-restore
```
