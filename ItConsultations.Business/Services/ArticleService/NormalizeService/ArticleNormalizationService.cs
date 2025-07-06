using ItConsultations.Business.Dtos.ArticleDtos;
using ItConsultations.Utilities.Guards;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace ItConsultations.Business.Services.ArticleService.NormalizeService;

public class ArticleNormalizationService : IArticleNormalizationService
{
    public async Task<ArticleDto> NormalizeAsync(ArticleDto articleDto, string articleConsId)
    {
        Guard.NotNull(articleDto);
        Guard.NotNullOrWhiteSpace(articleConsId);

        var normalizedArticle = new ArticleDto
        {
            Id = articleDto.Id,
            ArticleConsId = articleConsId,
            Title = await NormalizeTitleAsync(articleDto.Title),
            Text = await NormalizeTextAsync(articleDto.Text),
            CreatedAt = articleDto.CreatedAt,
            UpdatedAt = DateTime.UtcNow,
            CreatedBy = articleDto.CreatedBy,
            CoachConsId = articleDto.CoachConsId,
            StudentConsId = articleDto.StudentConsId,
            Attachments = articleDto.Attachments
        };

        return normalizedArticle;
    }

    public async Task<ArticleDto> NormalizeResponseAsync(ArticleDto articleDto, string language = "en")
    {
        Guard.NotNull(articleDto);

        var normalizedArticle = new ArticleDto
        {
            Id = articleDto.Id,
            ArticleConsId = articleDto.ArticleConsId,
            Title = await NormalizeTitleForResponseAsync(articleDto.Title, language),
            Text = await NormalizeTextForResponseAsync(articleDto.Text, language),
            CreatedAt = articleDto.CreatedAt,
            UpdatedAt = articleDto.UpdatedAt,
            CreatedBy = articleDto.CreatedBy,
            CoachConsId = articleDto.CoachConsId,
            StudentConsId = articleDto.StudentConsId,
            Attachments = articleDto.Attachments
        };

        return normalizedArticle;
    }

    public async Task<ArticleDto> NormalizeForSearchAsync(ArticleDto articleDto)
    {
        Guard.NotNull(articleDto);

        var normalizedArticle = new ArticleDto
        {
            Id = articleDto.Id,
            ArticleConsId = articleDto.ArticleConsId,
            Title = await NormalizeTitleForSearchAsync(articleDto.Title),
            Text = await NormalizeTextForSearchAsync(articleDto.Text),
            CreatedAt = articleDto.CreatedAt,
            UpdatedAt = articleDto.UpdatedAt,
            CreatedBy = articleDto.CreatedBy,
            CoachConsId = articleDto.CoachConsId,
            StudentConsId = articleDto.StudentConsId,
            Attachments = articleDto.Attachments
        };

        return normalizedArticle;
    }

    public async Task<ArticleDto> NormalizeForDisplayAsync(ArticleDto articleDto, string language = "en")
    {
        Guard.NotNull(articleDto);

        var normalizedArticle = new ArticleDto
        {
            Id = articleDto.Id,
            ArticleConsId = articleDto.ArticleConsId,
            Title = await NormalizeTitleForDisplayAsync(articleDto.Title, language),
            Text = await NormalizeTextForDisplayAsync(articleDto.Text, language),
            CreatedAt = articleDto.CreatedAt,
            UpdatedAt = articleDto.UpdatedAt,
            CreatedBy = articleDto.CreatedBy,
            CoachConsId = articleDto.CoachConsId,
            StudentConsId = articleDto.StudentConsId,
            Attachments = articleDto.Attachments
        };

        return normalizedArticle;
    }

    private async Task<string> NormalizeTitleAsync(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            return string.Empty;

        return await Task.Run(() =>
        {
            // Remove extra whitespace
            var normalized = Regex.Replace(title.Trim(), @"\s+", " ");
            
            // Capitalize first letter of each word (Title Case)
            var textInfo = CultureInfo.CurrentCulture.TextInfo;
            normalized = textInfo.ToTitleCase(normalized.ToLower());
            
            // Ensure proper length
            if (normalized.Length > 500)
                normalized = normalized.Substring(0, 497) + "...";
                
            return normalized;
        });
    }

    private async Task<string> NormalizeTextAsync(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return string.Empty;

        return await Task.Run(() =>
        {
            // Remove extra whitespace and normalize line breaks
            var normalized = Regex.Replace(text, @"\r\n|\r|\n", "\n");
            normalized = Regex.Replace(normalized, @"\n\s*\n", "\n\n");
            normalized = Regex.Replace(normalized, @"[ \t]+", " ");
            normalized = normalized.Trim();
            
            // Normalize quotes and dashes
            normalized = normalized.Replace("'", "'").Replace("'", "'");
            normalized = normalized.Replace("\"", """).Replace("\"", """);
            normalized = normalized.Replace("--", "—");
            
            return normalized;
        });
    }

    private async Task<string> NormalizeTitleForResponseAsync(string title, string language)
    {
        if (string.IsNullOrWhiteSpace(title))
            return string.Empty;

        return await Task.Run(() =>
        {
            var normalized = title.Trim();
            
            // Apply language-specific normalization
            switch (language.ToLower())
            {
                case "uk":
                    // Ukrainian-specific normalization
                    normalized = normalized.Replace("і", "і").Replace("ї", "ї");
                    break;
                case "de":
                    // German-specific normalization
                    normalized = normalized.Replace("ß", "ss");
                    break;
                default:
                    // Default English normalization
                    break;
            }
            
            return normalized;
        });
    }

    private async Task<string> NormalizeTextForResponseAsync(string text, string language)
    {
        if (string.IsNullOrWhiteSpace(text))
            return string.Empty;

        return await Task.Run(() =>
        {
            var normalized = text.Trim();
            
            // Apply language-specific text normalization
            switch (language.ToLower())
            {
                case "uk":
                    // Ukrainian-specific text normalization
                    normalized = normalized.Replace("і", "і").Replace("ї", "ї");
                    break;
                case "de":
                    // German-specific text normalization
                    normalized = normalized.Replace("ß", "ss");
                    break;
                default:
                    // Default English text normalization
                    break;
            }
            
            return normalized;
        });
    }

    private async Task<string> NormalizeTitleForSearchAsync(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            return string.Empty;

        return await Task.Run(() =>
        {
            // Convert to lowercase for search
            var normalized = title.ToLowerInvariant();
            
            // Remove special characters but keep spaces
            normalized = Regex.Replace(normalized, @"[^\w\s]", " ");
            
            // Remove extra whitespace
            normalized = Regex.Replace(normalized, @"\s+", " ");
            
            return normalized.Trim();
        });
    }

    private async Task<string> NormalizeTextForSearchAsync(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return string.Empty;

        return await Task.Run(() =>
        {
            // Convert to lowercase for search
            var normalized = text.ToLowerInvariant();
            
            // Remove HTML tags if present
            normalized = Regex.Replace(normalized, @"<[^>]*>", " ");
            
            // Remove special characters but keep spaces and basic punctuation
            normalized = Regex.Replace(normalized, @"[^\w\s\.\,\!\?]", " ");
            
            // Remove extra whitespace
            normalized = Regex.Replace(normalized, @"\s+", " ");
            
            return normalized.Trim();
        });
    }

    private async Task<string> NormalizeTitleForDisplayAsync(string title, string language)
    {
        if (string.IsNullOrWhiteSpace(title))
            return string.Empty;

        return await Task.Run(() =>
        {
            var normalized = title.Trim();
            
            // Apply display-specific formatting
            var textInfo = CultureInfo.CurrentCulture.TextInfo;
            normalized = textInfo.ToTitleCase(normalized.ToLower());
            
            // Ensure proper length for display
            if (normalized.Length > 100)
                normalized = normalized.Substring(0, 97) + "...";
                
            return normalized;
        });
    }

    private async Task<string> NormalizeTextForDisplayAsync(string text, string language)
    {
        if (string.IsNullOrWhiteSpace(text))
            return string.Empty;

        return await Task.Run(() =>
        {
            var normalized = text.Trim();
            
            // Format paragraphs for display
            normalized = Regex.Replace(normalized, @"\n\s*\n", "</p><p>");
            normalized = "<p>" + normalized + "</p>";
            
            // Format line breaks
            normalized = normalized.Replace("\n", "<br/>");
            
            // Ensure proper HTML encoding
            normalized = System.Web.HttpUtility.HtmlEncode(normalized);
            
            return normalized;
        });
    }
}
