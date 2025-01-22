// tests/Shorten.Tests/Models/ShortContextTests.cs
using Microsoft.EntityFrameworkCore;
using Xunit;
using Shorten.Models;

namespace Shorten.Tests.Models
{
    public class ShortContextTests
    {
        [Fact]
        public void ShortContext_CanInitialize()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ShortContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            // Act
            using (var context = new ShortContext(options))
            {
                context.UrlMappings.Add(new UrlMapping
                {
                    Id = 1,
                    OriginalUrl = "https://example.com",
                    ShortenedUrl = "abc123"
                });
                context.SaveChanges();
            }

            // Assert
            using (var context = new ShortContext(options))
            {
                var urlMapping = context.UrlMappings.Find(1);
                Assert.NotNull(urlMapping);
                Assert.Equal("https://example.com", urlMapping.OriginalUrl);
                Assert.Equal("abc123", urlMapping.ShortenedUrl);
            }
        }
    }
}