using KodaStore.Api.Data;
using KodaStore.Api.DTOs;
using KodaStore.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace KodaStore.Api.Controllers;

[ApiController]
[Route("api/v1/products")]

public sealed class ProductController : ControllerBase
{
    private readonly InMemoryProductCatalog _catalog;

    public ProductController(InMemoryProductCatalog catalog)
    {
        _catalog = catalog;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ProductResponse>), StatusCodes.Status200OK)]

    public ActionResult<IReadOnlyList<Products>> GetAll()
    {
        var products = _catalog.GetAll().Select(ProductResponse.FromModel).ToArray();

        return Ok(products);
    }


    [HttpGet("{productCode}")]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]

    public ActionResult<ProductResponse> GetByCode([FromRoute] string productCode)
    {
        var product = _catalog.GetByCode(productCode);

        if(product is null)
        {
            return Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: "Product Not found",
                detail: $"No product was found for code {productCode}"
            );
        }

        return Ok(ProductResponse.FromModel(product));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public ActionResult<ProductResponse> Create([FromBody] ProductRequest request)
    {
        var product = new Products(request.ProductCode, request.Name, request.Price, request.Stock, request.Category);

        _catalog.AddProduct(product);

        return CreatedAtAction(
            actionName: nameof(GetByCode),
            value: product,
            routeValues: new{productCode = product.ProductCode}
        );
    }

    [HttpPut("{productCode}")]
    [ProducesResponseType(typeof(ProductResponse),StatusCodes.Status200OK )]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public ActionResult<ProductResponse> Update([FromRoute] string productCode, [FromBody] ProductRequest request)
    {
        var product = new Products(request.ProductCode, request.Name, request.Price, request.Stock, request.Category);
        _catalog.UpdateProduct(product, productCode);

        return Ok(
            "sip"
        );

    }
}