# Tailoring API

## Starting SQL server
```powerShell
$sa_password="[SA PASSWORD HERE]"
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=$sa_password" -p 5249:5249 -v sqlvolume:/var/opt/mssql -d --rm --name mssql mcr.microsoft.com/mssql/server:2022-preview-ubuntu-22.04

```

## Setting the connection string to secret manager
```powerShell
 dotnet user-secrets init
$sa_password="[SA PASSWORD HERE]"

dotnet user-secrets set "ConnectionStrings:ReceiptCreatorContext" "server=localhost; Database=ReceiptCreatorDatabase; User Id=sa1; Password=$sa_password;Trusted_Connection=True; TrustServerCertificate=True;"

```


## Generate migration

```poweershell
dotnet ef migrations add InitialCreate --output-dir Data\Migrations


```