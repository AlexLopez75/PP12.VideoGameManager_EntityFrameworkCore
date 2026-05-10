# Projecte d'Entity Framework Core - Gestor de Videojocs

Aquest projecte és una aplicació web desenvolupada amb **ASP.NET Core Razor Pages** i **Entity Framework Core**. L'aplicació permet gestionar una base de dades de videojocs i desenvolupadors, implementant operacions CRUD completes, gestió de relacions (Un a Molts) i consultes avançades amb LINQ.

## Estructura del projecte

L'estructura principal del codi segueix el patró estàndard de Razor Pages i EF Core:

- **/Models:** Conté les entitats que representen les taules a la base de dades (`Game.cs`, `Developer.cs`).
- **/Data:** Conté la classe `GameStoreContext.cs`, que hereta de `DbContext` i gestiona la connexió i les col·leccions (`DbSet`) d'Entity Framework.
- **/Pages:** Interfícies d'usuari i la seva lògica (PageModels).
  - **/Games:** Pàgines CRUD per a la gestió de videojocs.
  - **/Developers:** Pàgines CRUD per a la gestió de desenvolupadors. Inclou la gestió d'integritat referencial per no permetre esborrar un desenvolupador si té jocs associats.
  - **/Stats:** Pàgina dedicada a mostrar estadístiques mitjançant consultes LINQ bàsiques i avançades (Top 5, agrupació per dècades, mitjana de puntuacions, etc.).

## Instruccions d'execució

El projecte ha de compilar i executar correctament amb `dotnet run` des del directori principal. Segueix aquests passos per posar-lo en marxa:

1. Obre un terminal o línia de comandes.
2. Navega fins al directori arrel del projecte web:
   ```bash
   cd HeroEngine.Web
   ```
3. Assegura't de tenir les migracions aplicades a la base de dades:

  ```bash
  dotnet ef database update
  ```
4. Executa l'aplicació:
  ```bash
  dotnet run
  ```
## Exemple de dades a la base (Mock / Seed)

Un cop executat, la base de dades contindrà informació relacionada com:

```
Developer: { Id: 1, Name: "Nintendo", Country: "Japan", FoundedYear: 1889 }
Game: { Id: 1, Title: "The Legend of Zelda: TotK", Genre: "Adventure", Year: 2023, Score: 9.8, DeveloperId: 1 }
```
