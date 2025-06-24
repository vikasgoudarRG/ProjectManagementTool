using Microsoft.AspNetCore.Mvc;
using ProjectManagementTool.Domain.Entities;
using ProjectManagementTool.Infrastructure.Contexts;

namespace ProjectManagementTool.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TagsController : ControllerBase
{
    private readonly AppDbContext _context;

    public TagsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAll() => Ok(_context.Tags.ToList());

    [HttpPost]
    public IActionResult Create(Tag tag)
    {
        _context.Tags.Add(tag);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetAll), new { id = tag.Id }, tag);
    }
}
