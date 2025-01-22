using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Shorten.Controllers;
using Shorten.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shorten.Tests.Controllers
{
    public class UrlMappingControllerTests
    {
        private DbContextOptions<ShortContext> GetInMemoryDbContextOptions(string databaseName)
        {
            return new DbContextOptionsBuilder<ShortContext>()
                .UseInMemoryDatabase(databaseName: databaseName)
                .Options;
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithListOfUrlMappings()
        {
            // Arrange
            var options = GetInMemoryDbContextOptions("Index_Test");
            using (var context = new ShortContext(options))
            {
                context.UrlMappings.Add(new UrlMapping { Id = 1, OriginalUrl = "https://example.com", ShortenedUrl = "abc123" });
                context.UrlMappings.Add(new UrlMapping { Id = 2, OriginalUrl = "https://anotherexample.com", ShortenedUrl = "def456" });
                context.SaveChanges();
            }

            using (var context = new ShortContext(options))
            {
                var controller = new UrlMappingController(context);

                // Act
                var result = await controller.Index();

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<IEnumerable<UrlMapping>>(viewResult.ViewData.Model);
                Assert.Equal(2, model.Count());
            }
        }

        [Fact]
        public async Task Details_ReturnsViewResult_WithUrlMapping()
        {
            // Arrange
            var options = GetInMemoryDbContextOptions("Details_Test");
            using (var context = new ShortContext(options))
            {
                context.UrlMappings.Add(new UrlMapping { Id = 1, OriginalUrl = "https://example.com", ShortenedUrl = "abc123" });
                context.SaveChanges();
            }

            using (var context = new ShortContext(options))
            {
                var controller = new UrlMappingController(context);

                // Act
                var result = await controller.Details(1);

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsType<UrlMapping>(viewResult.ViewData.Model);
                Assert.Equal(1, model.Id);
                Assert.Equal("https://example.com", model.OriginalUrl);
                Assert.Equal("abc123", model.ShortenedUrl);
            }
        }

        [Fact]
        public void Create_ReturnsViewResult()
        {
            // Arrange
            var options = GetInMemoryDbContextOptions("Create_Test");
            using (var context = new ShortContext(options))
            {
                var controller = new UrlMappingController(context);

                // Act
                var result = controller.Create();

                // Assert
                Assert.IsType<ViewResult>(result);
            }
        }

        [Fact]
        public async Task Create_RedirectsToIndex_WhenModelStateIsValid()
        {
            // Arrange
            var options = GetInMemoryDbContextOptions("Create_Redirect_Test");
            using (var context = new ShortContext(options))
            {
                var controller = new UrlMappingController(context);
                var urlMapping = new UrlMapping
                {
                    Id = 1,
                    OriginalUrl = "https://example.com",
                    ShortenedUrl = "abc123"
                };

                // Act
                var result = await controller.Create(urlMapping);

                // Assert
                var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal("Index", redirectToActionResult.ActionName);
            }
        }

        [Fact]
        public async Task Edit_ReturnsViewResult_WithUrlMapping()
        {
            // Arrange
            var options = GetInMemoryDbContextOptions("Edit_Test");
            using (var context = new ShortContext(options))
            {
                context.UrlMappings.Add(new UrlMapping { Id = 1, OriginalUrl = "https://example.com", ShortenedUrl = "abc123" });
                context.SaveChanges();
            }

            using (var context = new ShortContext(options))
            {
                var controller = new UrlMappingController(context);

                // Act
                var result = await controller.Edit(1);

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsType<UrlMapping>(viewResult.ViewData.Model);
                Assert.Equal(1, model.Id);
                Assert.Equal("https://example.com", model.OriginalUrl);
                Assert.Equal("abc123", model.ShortenedUrl);
            }
        }

        [Fact]
        public async Task Edit_RedirectsToIndex_WhenModelStateIsValid()
        {
            // Arrange
            var options = GetInMemoryDbContextOptions("Edit_Redirect_Test");
            using (var context = new ShortContext(options))
            {
                context.UrlMappings.Add(new UrlMapping { Id = 1, OriginalUrl = "https://example.com", ShortenedUrl = "abc123" });
                context.SaveChanges();
            }

            using (var context = new ShortContext(options))
            {
                var controller = new UrlMappingController(context);
                var urlMapping = new UrlMapping
                {
                    Id = 1,
                    OriginalUrl = "https://newexample.com",
                    ShortenedUrl = "new123"
                };

                // Act
                var result = await controller.Edit(1, urlMapping);

                // Assert
                var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal("Index", redirectToActionResult.ActionName);
            }
        }

        [Fact]
        public async Task Delete_ReturnsViewResult_WithUrlMapping()
        {
            // Arrange
            var options = GetInMemoryDbContextOptions("Delete_Test");
            using (var context = new ShortContext(options))
            {
                context.UrlMappings.Add(new UrlMapping { Id = 1, OriginalUrl = "https://example.com", ShortenedUrl = "abc123" });
                context.SaveChanges();
            }

            using (var context = new ShortContext(options))
            {
                var controller = new UrlMappingController(context);

                // Act
                var result = await controller.Delete(1);

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsType<UrlMapping>(viewResult.ViewData.Model);
                Assert.Equal(1, model.Id);
                Assert.Equal("https://example.com", model.OriginalUrl);
                Assert.Equal("abc123", model.ShortenedUrl);
            }
        }

        [Fact]
        public async Task DeleteConfirmed_RedirectsToIndex_WhenUrlMappingExists()
        {
            // Arrange
            var options = GetInMemoryDbContextOptions("Delete_Redirect_Test");
            using (var context = new ShortContext(options))
            {
                context.UrlMappings.Add(new UrlMapping { Id = 1, OriginalUrl = "https://example.com", ShortenedUrl = "abc123" });
                context.SaveChanges();
            }

            using (var context = new ShortContext(options))
            {
                var controller = new UrlMappingController(context);

                // Act
                var result = await controller.DeleteConfirmed(1);

                // Assert
                var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal("Index", redirectToActionResult.ActionName);
            }
        }
    }
}