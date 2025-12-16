using eCommerce.SharedLibrary.Response;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Application.DTOs;
using ProductApi.Application.Interfaces;
using ProductApi.Domain.Entities;
using ProductApi.Presentation.Controllers;

namespace UnitTest.ProductApi.Controllers
{
    public class ProductControllerTest
    {
        private readonly IProduct _product;
        private readonly ProductController _productsController;

        public ProductControllerTest()
        {
            _product = A.Fake<IProduct>();
            _productsController = new ProductController(_product);
        }

        [Fact]
        public async Task GetProduct_WhenProductExists_ReturnOkResponseWithProduct()
        {
            //Arrange
            var products = new List<Product>()
            {
                new(){Id=1,Name="Product 1",Quantity=10,Price=100.70m},
                new(){Id=1,Name="Product 2",Quantity=110,Price=1004.70m}
            };

            A.CallTo(() => _product.GetAllAsync()).Returns(products);

            //Act
            var result = await _productsController.GetProducts();

            //Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);

            var returnedProducts = okResult.Value as IEnumerable<ProductDto>;

            returnedProducts.Should().NotBeNull();
            returnedProducts.Count().Should().Be(2);
        }

        [Fact]
        public async Task GetProducts_WhenNoProductsExist_ReturnNotFoundResponse()
        {
            //Arrange
            var products = new List<Product>();

            A.CallTo(()=>_product.GetAllAsync()).Returns(products);

            //Assert
            var result = await _productsController.GetProducts();

            //Assert
            var notFoundResult = result.Result as NotFoundObjectResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);

            var message = notFoundResult.Value as string;
            message.Should().Be("No products detected in the database.");
        }

        [Fact]
        public async Task CreateProduct_WhenModelStateIsInvalid_ReturnBadRequest()
        {
            //Arrange
            var productDto = new ProductDto(1, "Product 1", 67.95m, 34);
            _productsController.ModelState.AddModelError("Name", "Required");

            //Act
            var result = await _productsController.CreateProduct(productDto);

            //Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task CreateProduct_WhenCreateIsSuccessfull_ReturnOkResponse()
        {
            //Arrange
            var productDto = new ProductDto(1, "Product 1", 67.95m, 34);
            var response = new Response(true, "Created");

            //Act
            A.CallTo(() => _product.CreateAsync(A<Product>.Ignored)).Returns(response);
            var result = await _productsController.CreateProduct(productDto);
            
            //Asssert
            var okResult = result.Result as OkObjectResult;

            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);

            var responseResult = okResult.Value as Response;

            responseResult!.Message.Should().Be("Created");
        }
    }
}