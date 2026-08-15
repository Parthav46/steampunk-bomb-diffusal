# Contributing

Thanks for helping improve Steampunk Bomb Diffusal.

## Development Setup

1. Install the .NET SDK version pinned in global.json.
2. Restore dependencies:

```bash
dotnet restore
```

3. Build the solution:

```bash
dotnet build
```

4. Run tests:

```bash
dotnet test sbd.tests
```

## Pull Request Guidelines

1. Keep changes focused and small.
2. Add or update tests when behavior changes.
3. Update docs when needed.
4. Ensure local build and tests pass before opening a PR.

## Commit Guidance

Use clear commit messages that explain what changed and why.

## Security

Do not commit secrets, private keys, credentials, or machine-local config.
If you find a vulnerability, follow SECURITY.md.