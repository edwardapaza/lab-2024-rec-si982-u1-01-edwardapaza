using Xunit;
using Shorten.Models;

public class UrlMappingTests
{
    [Fact]
    public void UrlMapping_Properties_SetCorrectly()
    {
        // Arrange
        var urlMapping = new UrlMapping
        {
            Id = 1,
            OriginalUrl = "https://example.com",
            ShortenedUrl = "abc123"
        };

        // Assert
        Assert.Equal(1, urlMapping.Id);
        Assert.Equal("https://example.com", urlMapping.OriginalUrl);
        Assert.Equal("abc123", urlMapping.ShortenedUrl);
    }
}