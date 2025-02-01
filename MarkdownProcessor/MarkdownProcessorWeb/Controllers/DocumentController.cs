using System.Security.Claims;
using Markdown.MarkdownProcessor;
using MarkdownProcessorWeb.Models;
using MarkdownProcessorWeb.Services;
using MarkdownProcessorWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Data;
using Document = MarkdownProcessorWeb.Models.Document;

namespace MarkdownProcessorWeb.Controllers;

[Authorize]
public class DocumentController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly MinIOStorageService _minioStorageService;
    private readonly IMarkdownProcessor _markdownProcessor;

    public DocumentController(ApplicationDbContext context, MinIOStorageService minioStorageService, IMarkdownProcessor markdownProcessor)
    {
        _context = context;
        _minioStorageService = minioStorageService;
        _markdownProcessor = markdownProcessor;
    }

    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var documents = await _context.Documents
            .Include(d => d.DocumentAccesses)
            .Where(d => d.AuthorId == int.Parse(userId) || d.DocumentAccesses.Any(da => da.UserId == int.Parse(userId)))
            .ToListAsync();

        return View(documents);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(DocumentViewModel model)
    {
        if (ModelState.IsValid)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var document = new Document
            {
                Title = model.Title,
                Content = model.Content,
                IsPublic = model.IsPublic,
                AuthorId = int.Parse(userId),
                MinIOKey = Guid.NewGuid().ToString()
            };
            
            using (var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(model.Content)))
            {
                await _minioStorageService.UploadFileAsync(document.MinIOKey, stream);
            }
            

            _context.Documents.Add(document);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var document = await _context.Documents
            .Include(d => d.DocumentAccesses)
            .FirstOrDefaultAsync(d => 
                d.Id == id && (d.AuthorId == int.Parse(userId) || d.DocumentAccesses.Any(da => 
                    da.UserId == int.Parse(userId) && da.AccessLevel == AccessLevel.Editor)));

        if (document == null)
            return NotFound();

        var model = new DocumentViewModel
        {
            Id = document.Id,
            Title = document.Title,
            Content = document.Content,
            IsPublic = document.IsPublic
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, DocumentViewModel model)
    {
        if (ModelState.IsValid)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            var document = await _context.Documents
                .Include(d => d.DocumentAccesses)
                .FirstOrDefaultAsync(d => 
                    d.Id == id && (d.AuthorId == int.Parse(userId) || d.DocumentAccesses.Any(da => 
                        da.UserId == int.Parse(userId) && da.AccessLevel == AccessLevel.Editor)));
            
            if (document == null)
                return NotFound();
            
            
            using (var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(model.Content)))
            {
                await _minioStorageService.UploadFileAsync(document.MinIOKey, stream);
            }
            
            document.Title = model.Title;
            document.Content = model.Content;
            document.IsPublic = model.IsPublic;
            
            _context.Documents.Update(document);
            await _context.SaveChangesAsync();
            
            return View(model);
        }
        
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Read(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var document = await _context.Documents
            .Include(d => d.DocumentAccesses)
            .FirstOrDefaultAsync(d => d.Id == id);
        
        if (document == null)
            return NotFound();
        
        if (!document.IsPublic && document.AuthorId != int.Parse(userId) && !document.DocumentAccesses.Any(da => da.UserId == int.Parse(userId)))
            return Forbid();

        return View(document);
    }
    
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var document = await _context.Documents
            .FirstOrDefaultAsync(d => d.Id == id && d.AuthorId == int.Parse(userId));
        
        if (document == null)
            return NotFound();
        
        await _minioStorageService.DeleteFileAsync(document.MinIOKey);
        
        _context.Documents.Remove(document);
        await _context.SaveChangesAsync();
        
        return RedirectToAction("Index");
    }
    
    [HttpPost]
    public IActionResult ConvertToHtml([FromBody] string markdown)
    {
        markdown = markdown.Replace("\n", "\r\n");
        
        var html = _markdownProcessor.ConvertToHtml(markdown);
        return Content(html, "text/plain");
    }
    
}