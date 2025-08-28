using ItConsultations.Business.SharedTypes.Enums.General;

namespace ItConsultations.Business.Services.IdGeneratorService;

public static class IdGeneratorService
{
    public static string GenerateUserId() => $"0000{DateTime.UtcNow:yyyyMMddHHmmssfff}{Random.Shared.NextInt64(0, 1_000_000_000_000_000):D15}";

    public static string GenerateCoachId() => $"0001{DateTime.UtcNow:yyyyMMddHHmmssfff}{Random.Shared.NextInt64(0, 1_000_000_000_000_000):D15}";

    public static string GenerateConsultationId() => $"0002{DateTime.UtcNow:yyyyMMddHHmmssfff}{Random.Shared.NextInt64(0, 1_000_000_000_000_000):D15}";

    public static string GenerateStudentId() => $"0003{DateTime.UtcNow:yyyyMMddHHmmssfff}{Random.Shared.NextInt64(0, 1_000_000_000_000_000):D15}";

    public static string GenerateArticleId() => $"0004{DateTime.UtcNow:yyyyMMddHHmmssfff}{Random.Shared.NextInt64(0, 1_000_000_000_000_000):D15}";

    public static string GenerateNoteId() => $"0005{DateTime.UtcNow:yyyyMMddHHmmssfff}{Random.Shared.NextInt64(0, 1_000_000_000_000_000):D15}";

    public static string GenerateEventId() => $"0006{DateTime.UtcNow:yyyyMMddHHmmssfff}{Random.Shared.NextInt64(0, 1_000_000_000_000_000):D15}";

    public static string GenerateConferenceId() => $"0007{DateTime.UtcNow:yyyyMMddHHmmssfff}{Random.Shared.NextInt64(0, 1_000_000_000_000_000):D15}";

    public static string GenerateAttachmentId() => $"0008{DateTime.UtcNow:yyyyMMddHHmmssfff}{Random.Shared.NextInt64(0, 1_000_000_000_000_000):D15}";

    public static string GenerateReviewId() => $"0009{DateTime.UtcNow:yyyyMMddHHmmssfff}{Random.Shared.NextInt64(0, 1_000_000_000_000_000):D15}";

    public static string GenerateId(string prefix) => $"{prefix}{DateTime.UtcNow:yyyyMMddHHmmssfff}{Random.Shared.NextInt64(0, 1_000_000_000_000_000):D15}";

    public static bool IsValidId(string id)
    {
        if (string.IsNullOrEmpty(id) || id.Length != 36)
        {
            return false;
        }

        if (!id.Substring(0, 4).All(char.IsDigit))
        {
            return false;
        }

        if (!id.Substring(4).All(char.IsDigit))
        {
            return false;
        }

        return true;
    }

    public static EntityType? GetEntityType(string id)
    {
        if (!IsValidId(id))
        {
            return null;
        }

        var prefix = id.Substring(0, 4);
        return prefix switch
        {
            "0000" => EntityType.User,
            "0001" => EntityType.Coach,
            "0002" => EntityType.Consultation,
            "0003" => EntityType.Student,
            "0004" => EntityType.Article,
            "0005" => EntityType.Note,
            "0006" => EntityType.Event,
            "0007" => EntityType.Conference,
            "0008" => EntityType.Attachment,
            "0009" => EntityType.Review,
            _ => null
        };
    }
}