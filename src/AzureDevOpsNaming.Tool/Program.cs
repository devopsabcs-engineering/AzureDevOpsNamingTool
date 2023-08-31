using AzureNaming.Tool.Attributes;
using AzureNaming.Tool.Helpers;
using AzureNaming.Tool.Models;
using AzureNaming.Tool.Services;
using BlazorDownloadFile;
using Blazored.Modal;
using Blazored.Toast;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMvcCore().AddApiExplorer();
builder.Services.AddRazorPages();
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddServerSideBlazor().AddCircuitOptions(x => x.DetailedErrors = true).AddHubOptions(x => x.MaximumReceiveMessageSize = 102400000);
}
else
{
    builder.Services.AddServerSideBlazor().AddHubOptions(x => x.MaximumReceiveMessageSize = 102400000);
}
builder.Services.AddBlazorDownloadFile();
builder.Services.AddBlazoredToast();
builder.Services.AddBlazoredModal();
builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton<StateContainer>();

// 9 helpers + 17 services + 1 factory -- came from static -- 27 total
builder.Services.AddSingleton<IAdminLogService, AdminLogService>(); //services
builder.Services.AddSingleton<IAdminService, AdminService>();
builder.Services.AddSingleton<IAdminUserService, AdminUserService>();
builder.Services.AddSingleton<ICustomComponentService, CustomComponentService>();
builder.Services.AddSingleton<IGeneratedNamesService, GeneratedNamesService>();
builder.Services.AddSingleton<IPolicyService, PolicyService>();
builder.Services.AddSingleton<IResourceDelimiterService, ResourceDelimiterService>();
builder.Services.AddSingleton<IResourceEnvironmentService, ResourceEnvironmentService>();
builder.Services.AddSingleton<IResourceFunctionService, ResourceFunctionService>();
builder.Services.AddSingleton<IResourceLocationService, ResourceLocationService>();
builder.Services.AddSingleton<IResourceNamingRequestService, ResourceNamingRequestService>();
builder.Services.AddSingleton<IResourceOrgService, ResourceOrgService>();
builder.Services.AddSingleton<IResourceProjAppSvcService, ResourceProjAppSvcService>();
builder.Services.AddSingleton<IResourceUnitDeptService, ResourceUnitDeptService>();
builder.Services.AddSingleton<IResourceTypeService, ResourceTypeService>(); //circular dependency with resource component
builder.Services.AddSingleton<IResourceComponentService, ResourceComponentService>(); //circular dependency with resource type
builder.Services.AddSingleton<IImportExportService, ImportExportService>();
builder.Services.AddSingleton<IPolicyRuleFactory, PolicyRuleFactory>(); // factory
builder.Services.AddSingleton<CacheHelper>(); //helpers
builder.Services.AddSingleton<ConfigurationHelper>();
builder.Services.AddSingleton<FileSystemHelper>();
builder.Services.AddSingleton<GeneralHelper>();
builder.Services.AddSingleton<IdentityHelper>();
builder.Services.AddSingleton<LogHelper>();
builder.Services.AddSingleton<ModalHelper>();
builder.Services.AddSingleton<ServicesHelper>();
builder.Services.AddSingleton<ValidationHelper>();


builder.Services.AddSwaggerGen(c =>
{
    c.OperationFilter<CustomHeaderSwaggerAttribute>();
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v" + Assembly.GetEntryAssembly()!.GetCustomAttribute<AssemblyInformationalVersionAttribute>()!.InformationalVersion,
        Title = "Azure DevOps Naming Tool API",
        Description = "An ASP.NET Core Web API for managing the Azure DevOps Naming Tool configuration. All API requests require the configured API Key (found in the site Admin configuration). You can find more details in the <a href=\"https://github.com/devopsabcs-engineering/AzureDevOpsNamingTool/wiki/Using-the-API\" target=\"_new\">Azure DevOps Naming Tool API documentation</a>."
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.WithOrigins("http://localhost:44332")
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
});

builder.Services.AddMemoryCache();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials()); // allow credentials
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts. 
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AzureNamingToolAPI"));

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

//app.UseAuthentication();
//app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
