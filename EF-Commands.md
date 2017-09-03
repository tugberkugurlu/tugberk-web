`cd` under `./src/Tugberk.Web`:

## Add Migration

```
dotnet ef migrations add {NAME-MIGRATION} --project ../Tugberk.Persistance.SqlServer/Tugberk.Persistance.SqlServer.csproj
```

## Update the Database

```
dotnet ef database update --project ../Tugberk.Persistance.SqlServer/Tugberk.Persistance.SqlServer.csproj
```