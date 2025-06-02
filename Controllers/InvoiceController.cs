using DataLayer;
using Entity;
using Entity.Entity;
using MapperExtension.Extension;
using Microsoft.AspNetCore.Mvc;

namespace MapperExtension.Controllers;

[ApiController]
[Route("[controller]")]
public class InvoiceController : ControllerBase
{
    private MasterDbContext _masterDbContext;
    public InvoiceController(MasterDbContext masterDbContext)
    {
        _masterDbContext = masterDbContext;
    }
    [HttpPost]
    public async Task<IActionResult> Post()
    {
        var invoices = new List<Invoice>();
        for (var i = 1; i < 5000; i++)
        {
            var payment = 1000;
            var price = i * 1000;
            var service = 0;
            var tax = i * 10;
            var discount = i;
            var totalPrice = price + service + tax - discount;

            var invoice = new Invoice()
            {
                DateTime = DateTime.Now,
                Discount = discount,
                Name = "Hasan Kashmiri",
                Payment = payment,
                SerialNo = i,
                Price = price,
                Service = service,
                Tax = tax,
                StateId = 1,
                TypeId = 1,
                TotalPrice = totalPrice,
                Remained = totalPrice - payment,
                UserId = 1
            };
            invoices.Add(invoice);
        }
        await _masterDbContext.AddRangeAsync(invoices);
        await _masterDbContext.SaveChangesAsync();
        return Ok();
    }
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var invoices = _masterDbContext.Invoice.ToList();
        var mappedInvoices = invoices.MapCollection<Invoice, InvoiceDTO>().ToList();
        return Ok(mappedInvoices);
    }
}
