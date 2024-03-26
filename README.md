# About The Project
Shorty is a link shortening service. It's 'free to use' for retail users.
You can access to the service on [Shorty](https://shorty.beerealm.com/)

For local usage with Sqlite you can add new migrations with this command
### Creating new migration: (no need for initial usage)
`dotnet ef migrations add InitialMigration -o Infrastructure\Data\SomeMigrationName`

Or use existing one:
### Updating database:
`dotnet ef database update`