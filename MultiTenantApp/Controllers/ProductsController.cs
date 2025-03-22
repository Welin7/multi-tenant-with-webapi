using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiTenantApp.Infrastructure;
using MultiTenantApp.Models;
using MultiTenantApp.ServiceTenant.Interface;

namespace MultiTenantApp.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ITenantService _tenantService;

        public ProductsController(ApplicationDbContext context, ITenantService tenantService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _tenantService = tenantService ?? throw new ArgumentNullException(nameof(tenantService));
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest("Product cannot be null.");
            }

            _context.Products?.Add(product);
            await _context.SaveChangesAsync();
            return Ok(product);
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            if (_context.Products == null)
            {
                return NotFound("Products not found.");
            }

            return Ok(await _context.Products.ToListAsync()); // O filtro de TenantId será aplicado automaticamente
        }
    }

}
