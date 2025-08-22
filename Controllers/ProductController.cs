using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductApi.Contracts;
using ProductApi.Data;
using ProductApi.Models;

namespace ProductApi.Controllers;
[ApiController]
[Route("api/product")]
[Authorize] // all product APIs require Bearer token
public class ProductController : ControllerBase
{
    private readonly AppDbContext _db;
    public ProductController(AppDbContext db) { _db = db; }

    // POST /api/product
    [HttpPost]
    public async Task<IActionResult> Create(ProductCreateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var entity = new Product { ProductCode = dto.ProductCode, ProductName = dto.ProductName, Price = dto.Price };

        _db.Products.Add(entity);
        try { await _db.SaveChangesAsync(); }
        catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("Duplicate") == true)
        {
            return Conflict(new { message = "ProductCode must be unique." });
        }
        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
    }

    // GET helper (not in spec but useful for CreatedAtAction)
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var p = await _db.Products.FindAsync(id);
        return p is null ? NotFound() : Ok(p);
    }

    // PUT /api/product/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, ProductUpdateDto dto)
    {
        var p = await _db.Products.FindAsync(id);
        if (p is null) return NotFound();

        p.ProductCode = dto.ProductCode;
        p.ProductName = dto.ProductName;
        p.Price = dto.Price;

        try { await _db.SaveChangesAsync(); }
        catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("Duplicate") == true)
        {
            return Conflict(new { message = "ProductCode must be unique." });
        }
        return NoContent();
    }

    // GET /api/product/search?query=
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string query)
    {
        if (string.IsNullOrWhiteSpace(query)) return Ok(Array.Empty<Product>());
        query = query.Trim();
        var results = await _db.Products
            .Where(p => EF.Functions.Like(p.ProductCode, $"%{query}%")
                     || EF.Functions.Like(p.ProductName, $"%{query}%"))
            .OrderBy(p => p.ProductCode)
            .ToListAsync();
        return Ok(results);
    }
}
