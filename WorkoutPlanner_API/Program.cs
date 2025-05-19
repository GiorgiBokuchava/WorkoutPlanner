using System.Reflection;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var swagger = builder.Configuration.GetSection("Swagger");

builder.Services.AddSwaggerGen(c =>
{
    var version = swagger["Version"];
    var title = swagger["Title"];
    var description = swagger["Description"];

    var contactSection = swagger.GetSection("Contact");
    var licenseSection = swagger.GetSection("License");

    c.SwaggerDoc(version, new OpenApiInfo
    {
        Version = version,
        Title = title,
        Description = description,
        TermsOfService = UriOrNull(swagger["TermsOfService"]),
        Contact = new OpenApiContact
        {
            Name = contactSection["Name"],
            Url = UriOrNull(contactSection["Url"])
        },
        License = new OpenApiLicense
        {
            Name = licenseSection["Name"],
            Url = UriOrNull(licenseSection["Url"])
        }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath)) c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(ui =>
    {
        var version = swagger["Version"];
        var title = swagger["Title"];
        ui.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"{title} {version}");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

static Uri? UriOrNull(string? url)
{
    if (string.IsNullOrWhiteSpace(url))
    {
        return null;
    }

    return new Uri(url);
}