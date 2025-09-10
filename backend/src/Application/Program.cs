using Common.Application;
using Auth.Application;
using User.Application;
using Profile.Application;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCommonServices();

builder.Services.AddAuthServices("Host=postgres;Port=5432;Database=auth_db;Username=postgres;Password=9tcXCjJ3Xb4rDQhy9sfVGKyv");

builder.Services.AddUserServices("Host=postgres;Port=5432;Database=user_db;Username=postgres;Password=9tcXCjJ3Xb4rDQhy9sfVGKyv");

builder.Services.AddProfileServices("Host=postgres;Port=5432;Database=profile_db;Username=postgres;Password=9tcXCjJ3Xb4rDQhy9sfVGKyv");

builder.Services.AddControllers();

var app = builder.Build();
app.MapControllers();
app.Run();
