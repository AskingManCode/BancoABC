var builder = WebApplication.CreateBuilder(args);
//hola
// Add services to the container.
builder.Services.AddRazorPages();

// Para leer la Session en otras páginas
builder.Services.AddHttpContextAccessor();

// Construit la Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(15);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Usar la Session
app.UseSession();

app.MapRazorPages();

app.Run();
