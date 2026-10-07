# Family Gift List

See [SPEC.md](SPEC.md) for the full product spec and tech decisions.

## Running locally

Requires the .NET 10 SDK, Node 22+, and Docker (or Podman) for Aspire's local SQL Server container.

```
dotnet run --project src/GiftList.AppHost
```

This opens the Aspire dashboard and starts the SQL container, the API, and the Vite dev server together.

## Building and testing without Docker

```
dotnet build GiftList.slnx
dotnet test GiftList.slnx

cd client
npm run build
npm run test
```

## Regenerating the frontend API client

The TypeScript client in `client/src/api/generated` is generated from the API's OpenAPI document and should not be edited by hand:

```
dotnet build src/GiftList.Api
cd client && npm run generate-api
```
