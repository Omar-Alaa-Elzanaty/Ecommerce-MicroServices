using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ProductApi.Domain.Entities;
using ProductApi.Infrastructure.Data;
using ProductApi.Infrastructure.Repositories;
using System.Linq.Expressions;

namespace UnitTest.ProductApi.Repositories
{
    public class ProductRepositoryTest
    {
        private readonly ProductDbContext _productDbCotnext;
        private readonly ProductRepository _productRepository;

        public ProductRepositoryTest()
        {
            var options = new DbContextOptionsBuilder<ProductDbContext>()
                .UseInMemoryDatabase(databaseName: "ProductDb").Options;

            _productDbCotnext = new ProductDbContext(options);
            _productRepository = new ProductRepository(_productDbCotnext);
        }

        [Fact]
        public async Task CreateAsync_WhenProductAlreadyExits_ReturnResponse()
        {
            //Arrange
            var existingProduct = new Product { Name = "ExistingProduct" };
            _productDbCotnext.Products.Add(existingProduct);
            await _productDbCotnext.SaveChangesAsync();

            //Act
            var result = await _productRepository.CreateAsync(existingProduct);

            //Assert
            result.Should().NotBeNull();
            result.Flag.Should().BeFalse();
            result.Message.Should().Be("ExistingProduct already exists.");
        }

        [Fact]
        public async Task CreateAsync_WhenProductDoesNotExist_AddProductAndReturnsResponse()
        {
            //Arrange
            var product = new Product { Name = "New Product" };

            //Act
            var result = await _productRepository.CreateAsync(product);

            //Assert
            result.Should().NotBeNull();
            result.Flag.Should().BeTrue();
            result.Message.Should().Be("New Product added successfully.");
        }

        [Fact]
        public async Task DeleteAsync_WhenProductIsFound_ReturnsSuccessResponse()
        {
            //Arrange
            var product = new Product { Id = 1, Name = "Existing Product", Price = 78.67m, Quantity = 5 };
            _productDbCotnext.Products.Add(product);

            //Act
            var result = await _productRepository.DeleteAsync(product);

            //Assert
            result.Should().NotBeNull();
            result.Flag.Should().BeTrue();
            result.Message.Should().Be("Existing Product deleted successfully.");
        }

        [Fact]
        public async Task DeleteAsync_WhenProductIsNotFound_ReturnsNotFoundResposne()
        {
            //Arrange
            var product = new Product { Id = 2, Name = "Not Existing Product", Price = 78.67m, Quantity = 5 };

            //Act
            var result = await _productRepository.DeleteAsync(product);

            //Assert
            result.Should().NotBeNull();
            result.Flag.Should().BeFalse();
            result.Message.Should().Be("Not Existing Product not found.");
        }

        [Fact]
        public async Task FindByIdAsync_WhenProductIsFound_ReturnProduct()
        {
            //Arrange
            var product = new Product { Id = 1, Name = "Existing Product", Price = 78.67m, Quantity = 5 };
            _productDbCotnext.Products.Add(product);

            //Act
            var result = await _productRepository.FindByIdAsync(product.Id);

            //Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Name.Should().Be("Existing Product");
        }


        [Fact]
        public async Task FindByIdAsync_WhenProductisNotFound_ReturnNull()
        {
            //Arrange

            //Act
            var result = await _productRepository.FindByIdAsync(99);


            //Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetAllAsync_WhenProductsAreFound_ReturnProducts()
        {
            //Arrange
            var products = new List<Product>()
            {
                new(){Id=1,Name="Product 1"},
                new(){Id=2,Name="Product 2"}
            };
            _productDbCotnext.Products.AddRange(products);
            await _productDbCotnext.SaveChangesAsync();

            //Act
            var result = await _productRepository.GetAllAsync();

            //Assert
            result.Should().NotBeNull();
            result.Count().Should().Be(2);
            result.Should().Contain(p => p.Name == "Product 1");
            result.Should().Contain(p => p.Name == "Product 2");
        }

        [Fact]
        public async Task GetAllAsync_WhenProductsAreNotFound_ReturnNull()
        {
            //Arrange

            //Act
            var result = await _productRepository.GetAllAsync();

            //Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByAsync_WhenProductIsFound_ReturnProduct()
        {
            //Arrange
            var product = new Product() { Id = 1, Name = "Product 1" };
            _productDbCotnext.Products.Add(product);
            await _productDbCotnext.SaveChangesAsync();
            Expression<Func<Product, bool>> predicate = p => p.Name == "Product 1";

            //Act
            var result =await _productRepository.GetByAsync(predicate);

            //Assert
            result.Should().NotBeNull();
            result.Name.Should().Be("Product 1");
        }

        [Fact]
        public async Task GetByAsync_WhenProductIsFound_ReturnNull()
        {
            //Arrange
            Expression<Func<Product, bool>> predicate = p => p.Name == "Product 2";

            //Act
            var result = await _productRepository.GetByAsync(predicate);

            //Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task UpdateProduct_WhenProductIsUpdatedSuccessfully_ReturnSuccessResponse()
        {
            //Arrange
            var product = new Product() { Id = 1, Name = "Product 1" };
            _productDbCotnext.Products.Add(product);
            await _productDbCotnext.SaveChangesAsync();

            //Act
            var result = await _productRepository.UpdateAsync(product);

            //Assert
            result.Should().NotBeNull();
            result.Flag.Should().BeTrue();
            result.Message.Should().Be("Product 1 updated successfully.");
        }

        [Fact]
        public async Task UPdateAsync_WhenProductIsNotFound_ReturnErrorResponse()
        {
            //Arrange
            var updatedProduct= new Product() { Id = 1, Name = "Product22" };

            //Act
            var result = await _productRepository.UpdateAsync(updatedProduct);

            //Assert
            result.Should().NotBeNull();
            result.Flag.Should().BeFalse();
            result.Message.Should().Be("Product22 not found.");
        }

    }
}
