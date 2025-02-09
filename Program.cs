var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);

builder.Services.AddTransient<NoteRepository>();

var app = builder.Build();

app.MapGet("/", () => "Hello World");
app.MapControllers();

app.Run();