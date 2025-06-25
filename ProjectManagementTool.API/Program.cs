using Microsoft.EntityFrameworkCore;
using ProjectManagementTool.Infrastructure.Contexts;
using ProjectManagementTool.Domain.Interfaces.Repositories;
using ProjectManagementTool.Application.Services;
using ProjectManagementTool.Application.Interfaces.Services;
using ProjectManagementTool.Infrastructure.Repositories;
using ProjectManagementTool.Infrastructure.Common;
using ProjectManagementTool.Domain.Interfaces.Repositories.Common;
using ProjectManagementTool.Application.Interfaces.Mappers;
using ProjectManagementTool.Application.Mappers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserNotificationRepository, UserNotificationRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<ITaskItemRepository, TaskItemRepository>();
builder.Services.AddScoped<ITaskItemCommentRepository, TaskItemCommentRepository>();
builder.Services.AddScoped<ITaskItemChangeLogRepository, TaskItemChangeLogRepository>();
builder.Services.AddScoped<IProjectChangeLogRepository, ProjectChangeLogRepository>();
builder.Services.AddScoped<ITeamChangeLogRepository, TeamChangeLogRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IUserNotificationService, UserNotificationService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<ITaskItemService, TaskItemService>();
builder.Services.AddScoped<ITaskItemCommentService, TaskItemCommentService>();
builder.Services.AddScoped<IChangeLogService, ChangeLogService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IUserMapper, UserMapper>();
builder.Services.AddScoped<IProjectMapper, ProjectMapper>();
builder.Services.AddScoped<IUserNotificationMapper, UserNotificationMapper>();
builder.Services.AddScoped<ITaskItemCommentMapper, TaskItemCommentMapper>();
builder.Services.AddScoped<ITaskItemMapper, TaskItemMapper>();
builder.Services.AddScoped<ITeamMapper, TeamMapper>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.ContentType = "application/json";
        Exception? exception = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>()?.Error;

        context.Response.StatusCode = exception switch
        {
            UnauthorizedAccessException => StatusCodes.Status403Forbidden,
            KeyNotFoundException => StatusCodes.Status404NotFound,
            ArgumentException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        string? result = System.Text.Json.JsonSerializer.Serialize(new { error = exception?.Message });
        await context.Response.WriteAsync(result);
    });
});
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
