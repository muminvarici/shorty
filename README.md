# About The Project
Shorty is a link shortening service. It's 'free to use' for retail users.
You can access to the service on [Shorty](https://shorty.beerealm.com/)

For local usage with Sqlite you can add new migrations with this command
### Creating new migration: (no need for initial usage)
`dotnet ef migrations add InitialMigration -o Infrastructure\Data\SomeMigrationName`

Or use existing one:
### Updating database:
`dotnet ef database update`

### Adding to `systemctl` startup task for nginx
- `nano /etc/systemd/system/shorty.service`
>
> [Unit]<br>
> Description=Shorty .NET Application<br>
After=network.target
>
>[Service]<br>
WorkingDirectory=/home/kadir/shorty/Shorty.Api/bin/Release/net8.0/publish<br>
ExecStart=/usr/bin/dotnet Shorty.Api.dll --urls http://*:5001<br>
Restart=on-failure<br>
RestartSec=10<br>
KillSignal=SIGINT<br>
SyslogIdentifier=shorty-api<br>
User=**'sudo user name'**<br>
Environment=ASPNETCORE_ENVIRONMENT=Production<br>
>
> [Install]<br>
WantedBy=multi-user.target

Restart systemctl task:
- `sudo systemctl restart shorty.service`

Go to nginx path:
- ` cd /etc/nginx/sites-available/`
- 