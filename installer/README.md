# Instalador do Jellyfin MAUI Client com Squirrel.Windows

Este processo cria um Setup.exe (unpackaged) e habilita auto‑update usando GitHub Releases.

## Pré‑requisitos
- .NET 8 SDK
- Workload MAUI para Windows
- Ferramenta Squirrel (Clowd.Squirrel) instalada globalmente:
  - `dotnet tool install --global Clowd.Squirrel`

> Se a instalação falhar localmente, você pode rodar em um runner do GitHub Actions.

## Como gerar localmente
```powershell
# Raiz do repositório
powershell -ExecutionPolicy Bypass -File .\installer\squirrel-pack.ps1 -Configuration Release
```
Saída: `installer\dist\` com `Setup.exe`, `RELEASES` e pacotes `.nupkg`.

## Feed de atualizações (GitHub Releases)
- Publique os arquivos de `installer\dist\` em um Release do GitHub.
- O app verifica o feed e pode se auto‑atualizar (você pode adicionar essa lógica no app conforme necessidade).

## Próximos passos
- Se quiser, adiciono um workflow em `.github/workflows/windows-installer.yml` para automatizar a geração a cada tag.