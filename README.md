# FileStorageProject (Angular SPA + ASP.NET Core Web API)

## Description
Solution for file storage.📁📁📁 Just for portfolio purposes.

### Preview Screen
![Alt text](https://github.com/Pashkett/FileStorageProject/blob/master/FileStorageApp.JPG)

## Features
 - Angular v9 & ASP.NET Core 3.1
 - JWT token & ASP.NET Core Identity
 - Role based Authorization
 - SQL data access via Entity Framework Core
 - Logging via NLog
 
## Prerequisites
 - Install
    - At least [.NET Core 3.1](https://www.microsoft.com/net/download/core)
    - [Node.js and npm](https://nodejs.org)
    - At least [Angular CLI 9](https://cli.angular.io/)
 - Command line & .NET Core CLI
    - In FileStorage-SPA folder run: `npm install`
    - In FileStorageBackend\FileStorage.API folder run: `dotnet build`

## Running
 - The SPA will be served on `http://localhost:4001`
 - Web API will be served on `https://localhost:5001`
 - Command line & .NET Core CLI
    - In FileStorage-SPA folder run: `ng serve`
    - In FileStorageBackend\FileStorage.API folder run: `dotnet run`

### Test Users
User with "Member" role:
 - Login: Tina 
 - Password: password

User with "Admin" role:
 - Login: Admin
 - Password: admin

## DB Connection
- Edit `ConnectionStrings` in _appsettings.json_:
```Json
"ConnectionStrings": {
  "sqlConnection": "Server=(localdb)\\.;database=FileStorageDb; Integrated Security=true"
}
```

## License
MIT
