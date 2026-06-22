# Firma-API

### 1. Databáza

Spustiť skript ("Súbor: database_exported_from_MSSMS.sql") v SQL Server Management Studio (SSMS) alebo cez príkaz v cmd `sqlcmd`
```bash
sqlcmd -S nazov_sql_server -E -i nazov_subor.sql
```
PRÍKLAD:
```bash
sqlcmd -S "(localdb)\mssqllocaldb" -i "C:\Users\spotk\Desktop\ASP.NET project\Firma-API\Firma-API\database_exported_from_MSSMS.sql"
```

alebo

nechať aplikáciu vytvoriť tabuľky automaticky cez EF Migrations pri prvom štarte.

### 2. Connection string

Upraviť `appsettings.json` podľa inštalácie SQL Server:

PRÍKLAD:
```json
{
  "ConnectionStrings": {
	"CompanyApiDbContext": "Server=(localdb)\\mssqllocaldb;Database=CompanyApiDbContext;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

+ ešte som si spravil druhú databázu na testovanie cez `TeaPie` a tam
treba tiež upraviť `appsettings.Testing.json`

PRÍKLAD:
```json
{
  "ConnectionStrings": {
    "CompanyApiDbContext": "Server=(localdb)\\mssqllocaldb;Database=CompanyApiDbContext_Test;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

Pripojenie cez SQL Server Management Studio (SSMS):


<img width="481" height="591" alt="image" src="https://github.com/user-attachments/assets/9069f5d3-fbed-4370-9c38-7c9f42190205" />


### 3. Obnovenie balíčkov a spustenie
Potrebné byť v projekte, kde sa nachádza súbor s koncovkou `*.csproj` a následne v cmd (Terminal/Developer PowerShell):

```bash
cd umiestnenie
dotnet restore
```

PRÍKLAD:
```bash
cd "C:\Users\spotk\Desktop\ASP.NET project\Firma-API\Firma-API"
dotnet restore
```

Následne si je možné vybrať z profilov a spustiť ich:
```bash
dotnet run --launch-profile https
```
ALEBO:
```bash
dotnet run --launch-profile testing
```

Aplikácia počúva na `https://localhost:7066` alebo  `https://localhost:5286`.
<img width="837" height="203" alt="image" src="https://github.com/user-attachments/assets/689b95dc-b2c1-4c2b-bf10-1aa62fc13f24" />

## Scalar 

Po spustení otvoriť v prehliadači:

```
https://localhost:7066/scalar/v1
```

alebo dá sa tam dostať aj cez:
```
https://localhost:5286/scalar/v1
```

Tu si možno prezerať a vyskúšať všetky endpointy priamo v UI.

<img width="1853" height="914" alt="image" src="https://github.com/user-attachments/assets/22fa79a2-cb89-46f3-a113-49823a091881" />


## TeaPie – testovanie endpointov
[Všetko v CMD]
Najlepšie je mať tento profil:
```bash
dotnet run --launch-profile testing
```

Ak by sa objavila chyba pri spustení testov:
```bash
dotnet dev-certs https --trust
```

Teraz toto v druhom CMD/terminal okne:

```bash
# Inštalácia
dotnet tool install -g TeaPie.Tool
# Spustenie testov
teapie test ./tests/teapie -e local
```


<img width="903" height="245" alt="image" src="https://github.com/user-attachments/assets/0c70e8ed-893c-4f70-b0b8-a6199852f08b" />

<img width="1026" height="287" alt="image" src="https://github.com/user-attachments/assets/5d70b177-9991-4aee-b557-49081aa8b0f2" />

---

## Endpointy
### Firmy `/api/companies`
| Metóda | URL | Popis |
|--------|-----|-------|
| GET | `/api/companies` | Zoznam všetkých firiem |
| GET | `/api/companies/{id}` | Detail firmy (vrátane divízií) |
| POST | `/api/companies` | Vytvorenie firmy |
| PUT | `/api/companies/{id}` | Aktualizácia firmy |
| DELETE | `/api/companies/{id}` | Vymazanie firmy |

### Zamestnanci `/api/employees`
| Metóda | URL | Popis |
|--------|-----|-------|
| GET | `/api/employees` | Zoznam všetkých zamestnancov |
| GET | `/api/employees?companyId=` | Zoznam zamestnancov (filter podľa firmy) |
| GET | `/api/employees/{id}` | Detail zamestnanca |
| POST | `/api/employees` | Vytvorenie zamestnanca |
| PUT | `/api/employees/{id}` | Aktualizácia zamestnanca |
| DELETE | `/api/employees/{id}` | Vymazanie zamestnanca |

### Divízie `/api/divisions`
| Metóda | URL | Popis |
|--------|-----|-------|
| GET | `/api/divisions` | Zoznam všetkých divízií |
| GET | `/api/divisions?companyId=` | Zoznam divízií (filter podľa firmy) |
| GET | `/api/divisions/{id}` | Detail divízie (vrátane projektov) |
| POST | `/api/divisions` | Vytvorenie divízie |
| PUT | `/api/divisions/{id}` | Aktualizácia divízie |
| DELETE | `/api/divisions/{id}` | Vymazanie divízie |

### Projekty `/api/projects`
| Metóda | URL | Popis |
|--------|-----|-------|
| GET | `/api/projects?divisionId=` | Zoznam všetkých projektov |
| GET | `/api/projects?divisionId=` | Zoznam projektov (filter podľa divízie) |
| GET | `/api/projects/{id}` | Detail projektu (vrátane oddelení) |
| POST | `/api/projects` | Vytvorenie projektu |
| PUT | `/api/projects/{id}` | Aktualizácia projektu |
| DELETE | `/api/projects/{id}` | Vymazanie projektu |

### Oddelenia `/api/departments`
| Metóda | URL | Popis |
|--------|-----|-------|
| GET | `/api/departments` | Zoznam všetkých oddelení |
| GET | `/api/departments?projectId=` | Zoznam (filter podľa projektu) |
| GET | `/api/departments/{id}` | Detail oddelenia |
| POST | `/api/departments` | Vytvorenie oddelenia |
| PUT | `/api/departments/{id}` | Aktualizácia oddelenia |
| DELETE | `/api/departments/{id}` | Vymazanie oddelenia |

---
## Validácie a chybové kódy

| HTTP kód | Príčina |
|----------|---------|
| 400 | Chýbajúce/neplatné hodnoty, duplicitný kód, vedúci nie je z tejto firmy |
| 404 | Entita s daným ID neexistuje |
| 204 | Úspešné vymazanie (bez tela odpovede) |

### Povinné polia
- **Zamestnanec**: firstName, lastName, phone, email, companyId
- **Firma / Divízia / Projekt / Oddelenie**: name, code + príslušné parentId
