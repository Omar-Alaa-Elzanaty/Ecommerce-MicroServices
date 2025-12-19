using eCommerce.SharedLibrary.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Application.DTOs;
using ProductApi.Application.DTOs.Conversions;
using ProductApi.Application.Interfaces;

namespace ProductApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IProduct productInterface) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            var products = await productInterface.GetAllAsync();

            if (products != null &&!products.Any())
                return NotFound("No products detected in the database.");

            var (_, list) = ProductConversions.FromEntity(null!, products);

            return list!.Any() ? Ok(list) : NotFound("No products detected in the database.");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var product = await productInterface.FindByIdAsync(id);

            if (product is null)
                return NotFound($"Product with Id {id} not found.");

            var (singleProduct, _) = ProductConversions.FromEntity(product, null!);

            return singleProduct is not null ? Ok(singleProduct) : NotFound($"Product with Id {id} not found.");
        }


        [HttpPost]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult<Response>> CreateProduct(ProductDto product)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var getEntity = ProductConversions.ToEntity(product);
            var response = await productInterface.CreateAsync(getEntity);

            return response.Flag is true ? Ok(response) : BadRequest(response);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response>>UpdateProduct(ProductDto product)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var getEntity = ProductConversions.ToEntity(product);
            var response = await productInterface.UpdateAsync(getEntity);

            return response.Flag is true ? Ok(response) : BadRequest(response);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response>> DeleteProduct(ProductDto product)
        {
            var getEntity = ProductConversions.ToEntity(product);

            var response = await productInterface.DeleteAsync(getEntity);
            return response.Flag is true ? Ok(response) : BadRequest(response);
        }
    }
}
