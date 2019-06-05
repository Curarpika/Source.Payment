rd/s/q Migrations
dotnet ef migrations add InitDb --context BaseAuthDbContext -o Migrations/BaseAuth
dotnet ef migrations add InitDb --context ProductDbContext -o Migrations/Product
dotnet ef migrations add InitDb --context PaymentDbContext -o Migrations/Payment
