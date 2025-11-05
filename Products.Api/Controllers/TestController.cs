using Microsoft.AspNetCore.Mvc;
using Products.Application.DTOs;
using Products.Application.Services;

namespace Products.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly DocumentProcessingService _documentProcessingService;

    public TestController(DocumentProcessingService documentProcessingService)
    {
        _documentProcessingService = documentProcessingService;
    }

    [HttpPost("{documentProductsCountThreshold:int}")]
    public async Task<ActionResult<DocumentProcessingResult>> ProcessDocument(int documentProductsCountThreshold, IFormFile? file) =>
        Ok(await _documentProcessingService.ProcessDocumentFileAsync(file, documentProductsCountThreshold));
}
