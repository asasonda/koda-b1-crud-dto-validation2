// using KodaStore.Api.DTOs;
// using KodaStore.Api.Services;
// using Microsoft.AspNetCore.Mvc;

// namespace KodaStore.Api.Controllers;

// public sealed class CatalogController : Controller
// {
//     private readonly IProductService _service;
//     public CatalogController(IProductService service)
//     {
//         _service = service;
//     }

//     [HttpGet("/catalog")]
//     public IActionResult Index()
//     {
//         var products = _service.GetAll();
//         return View(products);
//     }

//     [HttpPost("/catalog")]
//     public IActionResult Create(ProductRequest request)
//     {
//         var createProduct = _service.Create(request);

//         return View(createProduct);
//     }
// }