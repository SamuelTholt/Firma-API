# Firma-API

### 1. Databáza

Spustiť skript ("Súbor: database_exported_from_MSSMS.sql") v SQL Server Management Studio (SSMS) alebo cez príkaz v cmd `sqlcmd`
```bash
sqlcmd -S nazov_sql_server -E -i nazov_subor.sql
```
V MOJOM PRÍPADE:
```bash
sqlcmd -S "(localdb)\mssqllocaldb" -i "C:\Users\spotk\Desktop\ASP.NET project\Firma-API\Firma-API\database_exported_from_MSSMS.sql"
```

alebo

nechať aplikáciu vytvoriť tabuľky automaticky cez EF Migrations pri prvom štarte.

### 2. Connection string

Upraviť `appsettings.json` podľa inštalácie SQL Server:

V MOJOM PRÍPADE:
```json
{
  "ConnectionStrings": {
	"CompanyApiDbContext": "Server=(localdb)\\mssqllocaldb;Database=CompanyApiDbContext;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

+ ešte som si spravil druhú databázu na testovanie cez `TeaPie` a tam
treba tiež upraviť `appsettings.json`

V MOJOM PRÍPADE:
```json
{
  "ConnectionStrings": {
    "CompanyApiDbContext": "Server=(localdb)\\mssqllocaldb;Database=CompanyApiDbContext_Test;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```