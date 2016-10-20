@rem dotnet ef migrations add Initial
@rem dotnet ef database update
dotnet ef dbcontext scaffold "Server=(localdb)\ProjectsV13;Database=ABHIPricingDB;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -o Models -f
pause