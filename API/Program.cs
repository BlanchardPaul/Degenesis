using API.Extensions;
using Business.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices(builder);
builder.Services.AddBusinessServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(c => { c.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi3_1; });
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowFront");
app.MapApplicationEndpoints();
app.UseHttpsRedirection();
app.MapHub<RoomChatHub>("/roomchathub").RequireCors("AllowFront");

app.Run();
