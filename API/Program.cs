using API.Extensions;
using Business.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddApplicationServices(builder);
builder.Services.AddBusinessServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowFront");
app.MapApplicationEndpoints();
app.UseHttpsRedirection();
app.MapHub<RoomChatHub>("/roomchathub").RequireCors("AllowFront");

app.Run();
