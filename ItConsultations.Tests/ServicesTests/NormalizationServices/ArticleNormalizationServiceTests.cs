using ItConsultations.Business.Dtos.ArticleDtos;
using ItConsultations.Business.Entities;
using ItConsultations.Business.Services.ArticleService.NormalizeService;
using Xunit;

namespace ItConsultations.Tests;

public class ArticleNormalizationServiceTests
{
    private readonly IArticleNormalizationService _normalizationService;

    public ArticleNormalizationServiceTests()
    {
        _normalizationService = new ArticleNormalizationService();
    }

    [Fact]
    public async Task NormalizeAsync_WithValidArticle_ShouldNormalizeContent()
    {
        // Arrange
        var articleDto = new ArticleDto
        {
            Id = 1,
            ArticleConsId = "original-id",
            Title = "  this is a test title with extra spaces  ",
            Text = "This is a test text.\n\nWith multiple lines.\n\nAnd extra spaces.",
            CreatedAt = DateTime.UtcNow.AddDays(-1),
            UpdatedAt = DateTime.UtcNow.AddDays(-1),
            CreatedBy = null,
            CoachConsId = null,
            StudentConsId = null,
            Attachments = new List<ItConsultations.Business.Entities.Attachments.Attachment>()
        };

        // Act
        var result = await _normalizationService.NormalizeAsync(articleDto, "new-cons-id");

        // Assert
        Assert.Equal("new-cons-id", result.ArticleConsId);
        Assert.Equal("This Is A Test Title With Extra Spaces", result.Title);
        Assert.Contains("This is a test text.", result.Text);
        Assert.True(result.UpdatedAt > articleDto.UpdatedAt);
    }

    [Fact]
    public async Task NormalizeResponseAsync_WithEnglishLanguage_ShouldNormalizeForResponse()
    {
        // Arrange
        var articleDto = new ArticleDto
        {
            Id = 1,
            ArticleConsId = "test-id",
            Title = "Test Title",
            Text = "Test content with quotes 'single' and \"double\" quotes.",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            CreatedBy = null,
            Attachments = new List<ItConsultations.Business.Entities.Attachments.Attachment>()
        };

        // Act
        var result = await _normalizationService.NormalizeResponseAsync(articleDto, "en");

        // Assert
        Assert.Equal(articleDto.Title, result.Title);
        Assert.Contains("'", result.Text); // Should contain normalized quotes
    }

    [Fact]
    public async Task NormalizeForSearchAsync_ShouldOptimizeForSearch()
    {
        // Arrange
        var articleDto = new ArticleDto
        {
            Id = 1,
            ArticleConsId = "test-id",
            Title = "This Is A TEST Title!",
            Text = "This is test content with special characters @#$%^&*()",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            CreatedBy = null,
            Attachments = new List<ItConsultations.Business.Entities.Attachments.Attachment>()
        };

        // Act
        var result = await _normalizationService.NormalizeForSearchAsync(articleDto);

        // Assert
        Assert.Equal("this is a test title", result.Title.ToLower());
        Assert.Contains("this is test content", result.Text.ToLower());
        Assert.DoesNotContain("@#$%^&*()", result.Text);
    }

    [Fact]
    public async Task NormalizeForDisplayAsync_ShouldFormatForDisplay()
    {
        // Arrange
        var articleDto = new ArticleDto
        {
            Id = 1,
            ArticleConsId = "test-id",
            Title = "test title for display",
            Text = "First paragraph.\n\nSecond paragraph.\n\nThird paragraph.",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            CreatedBy = null,
            Attachments = new List<ItConsultations.Business.Entities.Attachments.Attachment>()
        };

        // Act
        var result = await _normalizationService.NormalizeForDisplayAsync(articleDto, "en");

        // Assert
        Assert.Equal("Test Title For Display", result.Title);
        Assert.Contains("<p>", result.Text);
        Assert.Contains("</p>", result.Text);
        Assert.Contains("<br/>", result.Text);
    }

    [Fact]
    public async Task NormalizeAsync_WithNullArticle_ShouldThrowException()
    {
        // Arrange
        ArticleDto? articleDto = null;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => 
            _normalizationService.NormalizeAsync(articleDto!, "test-id"));
    }

    [Fact]
    public async Task NormalizeAsync_WithEmptyConsId_ShouldThrowException()
    {
        // Arrange
        var articleDto = new ArticleDto
        {
            Id = 1,
            ArticleConsId = "test-id",
            Title = "Test Title",
            Text = "Test content",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            CreatedBy = null,
            Attachments = new List<ItConsultations.Business.Entities.Attachments.Attachment>()
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => 
            _normalizationService.NormalizeAsync(articleDto, ""));
    }

    [Fact]
    public async Task NormalizeResponseAsync_WithUkrainianLanguage_ShouldApplyUkrainianNormalization()
    {
        // Arrange
        var articleDto = new ArticleDto
        {
            Id = 1,
            ArticleConsId = "test-id",
            Title = "Тестовий заголовок з літерами і та ї",
            Text = "Тестовий текст з літерами і та ї",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            CreatedBy = null,
            Attachments = new List<ItConsultations.Business.Entities.Attachments.Attachment>()
        };

        // Act
        var result = await _normalizationService.NormalizeResponseAsync(articleDto, "uk");

        // Assert
        Assert.Contains("і", result.Title);
        Assert.Contains("ї", result.Title);
        Assert.Contains("і", result.Text);
        Assert.Contains("ї", result.Text);
    }

    [Fact]
    public async Task NormalizeResponseAsync_WithGermanLanguage_ShouldApplyGermanNormalization()
    {
        // Arrange
        var articleDto = new ArticleDto
        {
            Id = 1,
            ArticleConsId = "test-id",
            Title = "Test title with ß character",
            Text = "Test text with ß character",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            CreatedBy = null,
            Attachments = new List<ItConsultations.Business.Entities.Attachments.Attachment>()
        };

        // Act
        var result = await _normalizationService.NormalizeResponseAsync(articleDto, "de");

        // Assert
        Assert.Contains("ss", result.Title);
        Assert.Contains("ss", result.Text);
    }

    [Fact]
    public async Task NormalizeTitleAsync_WithLongTitle_ShouldTruncate()
    {
        // Arrange
        var longTitle = new string('a', 600); // Title longer than 500 characters

        // Act
        var result = await _normalizationService.NormalizeAsync(new ArticleDto
        {
            Id = 1,
            ArticleConsId = "test-id",
            Title = longTitle,
            Text = "Test content",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            CreatedBy = null,
            Attachments = new List<ItConsultations.Business.Entities.Attachments.Attachment>()
        }, "test-cons-id");

        // Assert
        Assert.Equal(500, result.Title.Length);
        Assert.EndsWith("...", result.Title);
    }

    [Fact]
    public async Task NormalizeTitleForDisplayAsync_WithLongTitle_ShouldTruncateForDisplay()
    {
        // Arrange
        var longTitle = new string('a', 150); // Title longer than 100 characters

        // Act
        var result = await _normalizationService.NormalizeForDisplayAsync(new ArticleDto
        {
            Id = 1,
            ArticleConsId = "test-id",
            Title = longTitle,
            Text = "Test content",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            CreatedBy = null,
            Attachments = new List<ItConsultations.Business.Entities.Attachments.Attachment>()
        }, "en");

        // Assert
        Assert.Equal(100, result.Title.Length);
        Assert.EndsWith("...", result.Title);
    }
} 