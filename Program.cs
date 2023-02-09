using aspnetmvc_blog.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DataContext>(options => options.UseInMemoryDatabase(databaseName: "Blog"));
builder.Services.AddControllersWithViews(options => options.MaxModelValidationErrors = 50).AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => { options.LoginPath = "/Account/Login/"; });
builder.Services.AddScoped<AuthenticationFilter>();
builder.Services.AddScoped<AuthenticationFilterAPI>();
builder.Services.AddScoped<LoggingFilter>();
builder.Services.AddScoped<LibrarySocket>();
builder.Services.AddScoped<LibraryBlog>();
builder.Services.AddWebSocketServerConnectionManager();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options => { options.IdleTimeout = TimeSpan.FromMinutes(60); });
builder.Services.AddAuthorization(options => { options.AddPolicy("Auth", policy => policy.Requirements.Add(new AuthenticationPolicy())); });
builder.Services.Configure<ForwardedHeadersOptions>(options => { options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto; });
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<DataContext>();
    DataInitializer.Initialize(context);
}
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseForwardedHeaders();
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
    app.UseForwardedHeaders();
}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseStaticFiles();
app.UseWebSockets();
app.UseWebSocketServer();
app.UseCookiePolicy();
app.UseRouting();
app.UseAuthorization();
app.UseStatusCodePagesWithRedirects("/Home/Error?type={0}");
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();